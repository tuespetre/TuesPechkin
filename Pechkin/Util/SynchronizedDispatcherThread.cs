using System;
using System.Collections.Generic;
using System.Threading;

namespace Pechkin.Util
{
    /// <summary>
    /// This class runs the thread and lets users to run delegates synchronously on that thread while obtaining results of the execution.
    /// 
    /// It's like <code>ISynchronizedInvoke</code>, but with only synchronous methods (because we don't need more).
    /// </summary>
    public class SynchronizedDispatcherThread
    {
        private readonly object queueLock = new object();

        private readonly List<DispatcherTask> taskQueue = new List<DispatcherTask>();

        private readonly Thread thread;

        private bool shutdown;

        /// <summary>
        /// Creates new <code>SynchronizedDispatcherThread</code>, the object is initialized and thread is started here.
        /// </summary>
        public SynchronizedDispatcherThread()
        {
            this.thread = new Thread(this.Run)
            {
                IsBackground = true
            };

            this.thread.Start();
        }

        /// <summary>
        /// Invokes specified delegate with parameters on the dispatcher thread synchronously.
        /// </summary>
        /// <param name="method">delegate to run on the thread</param>
        /// <param name="args">arguments to supply to the delegate</param>
        /// <returns>result of an action</returns>
        public object Invoke(Delegate method, object[] args)
        {
            // create the task
            var task = new DispatcherTask { Task = method, Params = args };

            // we don't want the task to be completed before we start waiting for that, so the outer lock
            lock (task)
            {
                lock (this.queueLock)
                {
                    this.taskQueue.Add(task);

                    Monitor.Pulse(this.queueLock);
                }

                // until this point, evaluation could not start
                Monitor.Wait(task);

                // and when we're done waiting, we know that the result was already set
                return task.Result;
            }
        }

        /// <summary>
        /// Tells the dispatcher to shutdown its worker thread.
        /// </summary>
        public void Terminate()
        {
            this.shutdown = true;
        }

        /// <summary>
        /// This method is used as a Thread.Run for the delegate hosting thread.
        /// </summary>
        protected void Run()
        {
            while (!this.shutdown)
            {
                try
                {
                    DispatcherTask task;

                    lock (this.queueLock)
                    {
                        if (this.taskQueue.Count > 0)
                        {
                            task = this.taskQueue[0];
                            this.taskQueue.RemoveAt(0);
                        }
                        else
                        {
                            Monitor.Wait(this.queueLock);
                            continue;
                        }
                    }

                    // if there's a task, process it asynchronously
                    lock (task)
                    {
                        try
                        {
                            task.Result = task.Task.DynamicInvoke(task.Params);
                        }
                        catch (Exception e)
                        {
                            Tracer.Critical(string.Format("Exception in SynchronizedDispatcherThread \"{0}\"", Thread.CurrentThread.Name), e);
                        }

                        // notify waiting thread about completeion
                        Monitor.Pulse(task);
                    }
                }
                catch (ThreadAbortException)
                {
                }
            }
        }

        /// <summary>
        /// Task object that's pushed to the queue.
        /// </summary>
        private class DispatcherTask
        {
            // task parameters
            public object[] Params;

            // result, filled out after it's executed
            public object Result;

            // task code
            public Delegate Task;
        }
    }
}