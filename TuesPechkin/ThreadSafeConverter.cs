using System;
using System.Collections.Generic;
using System.Reflection;
using System.Security.Principal;
using System.Threading;

namespace TuesPechkin
{
    public sealed class ThreadSafeConverter : StandardConverter, IConverter
    {
        public ThreadSafeConverter(IToolset toolset)
            : base(toolset)
        {
            toolset.Unloaded += (sender, args) =>
            {
                new Thread(() => StopThread()).Start();
            };

            if (toolset is NestingToolset)
            {
                (toolset as NestingToolset).BeforeUnload += (sender, args) =>
                {
                    Invoke(sender as ActionShim);
                };
            }
        }

        public override byte[] Convert(IDocument document)
        {
            return Invoke(() => base.Convert(document));
        }

        public TResult Invoke<TResult>(FuncShim<TResult> @delegate)
        {
            StartThread();

            // create the task
            var task = new Task<TResult>(@delegate);

            // we don't want the task to be completed before we start waiting for that, so the outer lock
            lock (task)
            {
                lock (queueLock)
                {
                    taskQueue.Add(task);

                    Monitor.Pulse(queueLock);
                }

                // until this point, evaluation could not start
                Monitor.Wait(task);

                if (task.Exception != null)
                {
                    throw task.Exception;
                }

                // and when we're done waiting, we know that the result was already set
                return task.Result;
            }
        }
        
        public void Invoke(ActionShim @delegate)
        {
            StartThread();

            // create the task
            var task = new Task(@delegate);

            // we don't want the task to be completed before we start waiting for that, so the outer lock
            lock (task)
            {
                lock (queueLock)
                {
                    taskQueue.Add(task);

                    Monitor.Pulse(queueLock);
                }

                // until this point, evaluation could not start
                Monitor.Wait(task);

                if (task.Exception != null)
                {
                    throw task.Exception;
                }
            }
        }

        private Thread innerThread;

        private readonly object queueLock = new object();

        private readonly object startLock = new object();

        private bool stopRequested = false;

        private readonly List<Task> taskQueue = new List<Task>();

        private void StartThread()
        {
            lock (startLock)
            {
                if (innerThread == null)
                {
                    innerThread = new Thread(Run)
                    {
                        IsBackground = true
                    };

                    stopRequested = false;

                    innerThread.Start();
                }
            }
        }

        private void StopThread()
        {
            lock (startLock)
            {
                if (innerThread != null)
                {
                    stopRequested = true;

                    while (innerThread.ThreadState != ThreadState.Stopped) { }

                    innerThread = null;
                }
            }
        }

        private void Run()
        {
            using (WindowsIdentity.Impersonate(IntPtr.Zero))
            {
                Thread.CurrentPrincipal = new WindowsPrincipal(WindowsIdentity.GetCurrent());
            }

            while (!stopRequested)
            {
                Task task;

                lock (queueLock)
                {
                    if (taskQueue.Count > 0)
                    {
                        task = taskQueue[0];
                        taskQueue.RemoveAt(0);
                    }
                    else
                    {
                        Monitor.Wait(queueLock, 100);
                        continue;
                    }
                }

                // if there's a task, process it asynchronously
                lock (task)
                {
                    try
                    {
                        task.Action.DynamicInvoke();
                    }
                    catch (TargetInvocationException e)
                    {
                        Tracer.Critical(string.Format("Exception in SynchronizedDispatcherThread \"{0}\"", Thread.CurrentThread.Name), e);
                        task.Exception = e.InnerException;
                    }

                    // notify waiting thread about completeion
                    Monitor.Pulse(task);
                }
            }
        }

        private class Task
        {
            public Task(ActionShim action)
            {
                this.Action = action;
            }

            public virtual ActionShim Action { get; protected set; }

            public Exception Exception { get; set; }
        }

        private class Task<TResult> : Task
        {
            public Task(FuncShim<TResult> @delegate)
                : base(null)
            {
                this.Delegate = @delegate;
                this.Action = () => this.Result = this.Delegate();
            }

            // task code
            public FuncShim<TResult> Delegate { get; private set; }

            // result, filled out after it's executed
            public TResult Result { get; private set; }
        }
    }
}