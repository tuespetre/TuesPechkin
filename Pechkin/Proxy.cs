using System;
using Pechkin.EventHandlers;
using Pechkin.Util;

namespace Pechkin
{
    internal class Proxy : MarshalByRefObject, IPechkin
    {
        private readonly Delegate invoker = null;

        private readonly IPechkin remoteInstance;

        internal Proxy(IPechkin remote, Delegate invoker)
        {
            this.remoteInstance = remote;
            this.invoker = invoker;

            // For all these event handlers, making sure to re-signal
            // using the PROXY as arg A, not the Remote, otherwise
            // synchronization could break

            remote.Begin += (a, b) =>
            {
                if (this.Begin != null)
                {
                    this.Begin(this, b);
                }
            };

            remote.Error += (a, b) =>
            {
                if (this.Error != null)
                {
                    this.Error(this, b);
                }
            };

            remote.Finished += (a, b) =>
            {
                if (this.Finished != null)
                {
                    this.Finished(this, b);
                }
            };

            remote.PhaseChanged += (a, b, c) =>
            {
                if (this.PhaseChanged != null)
                {
                    this.PhaseChanged(this, b, c);
                }
            };

            remote.ProgressChanged += (a, b, c) =>
            {
                if (this.ProgressChanged != null)
                {
                    this.ProgressChanged(this, b, c);
                }
            };

            remote.Warning += (a, b) =>
            {
                if (this.Warning != null)
                {
                    this.Warning(this, b);
                }
            };
        }

        private TReturn Invoke<TReturn>(Func<TReturn> del)
        {
            return (TReturn)this.invoker.DynamicInvoke(del);
        }

        public byte[] Convert(ObjectConfig doc, string html)
        {
            return this.Invoke(() => this.remoteInstance.Convert(doc, html));
        }

        public byte[] Convert(ObjectConfig doc, byte[] html)
        {
            return this.Invoke(() => this.remoteInstance.Convert(doc, html));
        }

        public byte[] Convert(ObjectConfig doc)
        {
            return this.Invoke(() => this.remoteInstance.Convert(doc));
        }

        public byte[] Convert(string html)
        {
            return this.Invoke(() => this.remoteInstance.Convert(html));
        }

        public byte[] Convert(byte[] html)
        {
            return this.Invoke(() => this.remoteInstance.Convert(html));
        }

        public byte[] Convert(Uri url)
        {
            return this.Invoke(() => this.remoteInstance.Convert(url));
        }

        public event BeginEventHandler Begin;

        public event WarningEventHandler Warning;

        public event ErrorEventHandler Error;

        public event PhaseChangedEventHandler PhaseChanged;

        public event ProgressChangedEventHandler ProgressChanged;

        public event FinishEventHandler Finished;

        public int CurrentPhase
        {
            get
            {
                return this.Invoke(() => this.remoteInstance.CurrentPhase);
            }
        }

        public int PhaseCount
        {
            get
            {
                return this.Invoke(() => this.remoteInstance.PhaseCount);
            }
        }

        public string PhaseDescription
        {
            get
            {
                return this.Invoke(() => this.remoteInstance.PhaseDescription);
            }
        }

        public string ProgressString
        {
            get
            {
                return this.Invoke(() => this.remoteInstance.ProgressString);
            }
        }

        public int HttpErrorCode
        {
            get
            {
                return this.Invoke(() => this.remoteInstance.HttpErrorCode);
            }
        }
    }
}