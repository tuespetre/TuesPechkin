using System;
using System.Collections.Generic;
using System.Threading;
using Pechkin.Util;

namespace Pechkin
{
    /// <summary>
    /// This class runs the thread and lets users to run delegates synchronously on that thread while obtaining results of the execution.
    /// 
    /// It's like <code>ISynchronizedInvoke</code>, but with only synchronous methods (because we don't need more).
    /// </summary>
    internal static class SynchronizedDispatcher
    {
        private static readonly object queueLock = new object();

        private static readonly List<Action> taskQueue = new List<Action>();

        static SynchronizedDispatcher()
        {
            SynchronizedDispatcher.Thread = new Thread(Run)
            {
                IsBackground = true
            };

            SynchronizedDispatcher.Thread.Start();
        }

        private delegate void Action();

        private static bool Abort { get; set; }

        private static Thread Thread { get; set; }

        /// <summary>
        /// Invokes specified delegate with parameters on the dispatcher thread synchronously.
        /// </summary>
        /// <param name="task">delegate to run on the thread</param>
        /// <param name="args">arguments to supply to the delegate</param>
        /// <returns>result of an action</returns>
        public static TResult Invoke<TResult>(Func<TResult> @delegate)
        {
            // create the task
            var task = new DispatcherTask<TResult> { Task = @delegate };
            var execute = new Action(task.Execute);

            // we don't want the task to be completed before we start waiting for that, so the outer lock
            lock (execute)
            {
                lock (queueLock)
                {
                    taskQueue.Add(execute);

                    Monitor.Pulse(queueLock);
                }

                // until this point, evaluation could not start
                Monitor.Wait(execute);

                // and when we're done waiting, we know that the result was already set
                return task.Result;
            }
        }

        /// <summary>
        /// Tells the dispatcher to shutdown its worker thread.
        /// </summary>
        public static void Terminate()
        {
            SynchronizedDispatcher.Abort = true;
        }

        /// <summary>
        /// This method is used as a Thread.Run for the delegate hosting thread.
        /// </summary>
        private static void Run()
        {
            try
            {
                while (!SynchronizedDispatcher.Abort)
                {
                    Delegate task;

                    lock (queueLock)
                    {
                        if (taskQueue.Count > 0)
                        {
                            task = taskQueue[0];
                            taskQueue.RemoveAt(0);
                        }
                        else
                        {
                            Monitor.Wait(queueLock);
                            continue;
                        }
                    }

                    // if there's a task, process it asynchronously
                    lock (task)
                    {
                        try
                        {
                            task.DynamicInvoke();
                        }
                        catch (Exception e)
                        {
                            Tracer.Critical(string.Format("Exception in SynchronizedDispatcherThread \"{0}\"", Thread.CurrentThread.Name), e);
                        }

                        // notify waiting thread about completeion
                        Monitor.Pulse(task);
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        /// <summary>
        /// Task object that's pushed to the queue.
        /// </summary>
        private class DispatcherTask<TResult>
        {
            // result, filled out after it's executed
            public TResult Result { get; set; }

            // task code
            public Func<TResult> Task { get; set; }

            public void Execute()
            {
                this.Result = this.Task();
            }
        }
    }
}