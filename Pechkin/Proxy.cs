using System;
using Pechkin.EventHandlers;

namespace Pechkin
{
    internal class Proxy : MarshalByRefObject, IPechkin
    {
        private readonly IPechkin remoteInstance;

        internal Proxy(IPechkin remote)
        {
            this.remoteInstance = remote;

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

        public event BeginEventHandler Begin;

        public event ErrorEventHandler Error;

        public event FinishEventHandler Finished;

        public event PhaseChangedEventHandler PhaseChanged;

        public event ProgressChangedEventHandler ProgressChanged;

        public event WarningEventHandler Warning;

        public int CurrentPhase
        {
            get
            {
                return SynchronizedDispatcher.Invoke(() => this.remoteInstance.CurrentPhase);
            }
        }

        public int HttpErrorCode
        {
            get
            {
                return SynchronizedDispatcher.Invoke(() => this.remoteInstance.HttpErrorCode);
            }
        }

        public int PhaseCount
        {
            get
            {
                return SynchronizedDispatcher.Invoke(() => this.remoteInstance.PhaseCount);
            }
        }

        public string PhaseDescription
        {
            get
            {
                return SynchronizedDispatcher.Invoke(() => this.remoteInstance.PhaseDescription);
            }
        }

        public string ProgressString
        {
            get
            {
                return SynchronizedDispatcher.Invoke(() => this.remoteInstance.ProgressString);
            }
        }

        public byte[] Convert(HtmlToPdfDocument document)
        {
            return SynchronizedDispatcher.Invoke(() => this.remoteInstance.Convert(document));
        }

        public byte[] Convert(ObjectSettings settings)
        {
            return SynchronizedDispatcher.Invoke(() => this.remoteInstance.Convert(settings));
        }

        public byte[] Convert(string html)
        {
            return SynchronizedDispatcher.Invoke(() => this.remoteInstance.Convert(html));
        }

        public byte[] Convert(byte[] html)
        {
            return SynchronizedDispatcher.Invoke(() => this.remoteInstance.Convert(html));
        }

        public byte[] Convert(Uri url)
        {
            return SynchronizedDispatcher.Invoke(() => this.remoteInstance.Convert(url));
        }
    }
}