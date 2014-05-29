using System;
using System.Text;
using System.Threading;
using Pechkin.EventHandlers;
using Pechkin.Util;

namespace Pechkin
{
    /// <summary>
    /// Covers the necessary converter functionality, for internal
    /// use behind a remote proxy implementing the same interface.
    /// </summary>
    [Serializable]
    internal class SimplePechkin : MarshalByRefObject, IPechkin
    {
        private readonly GlobalSettings globalConfig;
        private readonly StringCallback onErrorDelegate;
        private readonly IntCallback onFinishedDelegate;
        private readonly VoidCallback onPhaseChangedDelegate;
        private readonly IntCallback onProgressChangedDelegate;
        private readonly StringCallback onWarningDelegate;
        private IntPtr converter;
        private IntPtr globalConfigUnmanaged;

        /// <summary>
        /// Constructs HTML to PDF converter instance from <code>GlobalSettings</code>.
        /// </summary>
        /// <param name="config">global configuration object</param>
        public SimplePechkin(GlobalSettings config)
        {
            this.onErrorDelegate = new StringCallback(this.OnError);
            this.onFinishedDelegate = new IntCallback(this.OnFinished);
            this.onPhaseChangedDelegate = new VoidCallback(this.OnPhaseChanged);
            this.onProgressChangedDelegate = new IntCallback(this.OnProgressChanged);
            this.onWarningDelegate = new StringCallback(this.OnWarning);

            Tracer.Trace(string.Format("T:{0} Creating SimplePechkin", Thread.CurrentThread.Name));

            this.globalConfig = config;
            
            Tracer.Trace(string.Format("T:{0} Created global config", Thread.CurrentThread.Name));
        }

        /// <summary>
        /// This event happens every time the conversion starts
        /// </summary>
        public event BeginEventHandler Begin;

        /// <summary>
        /// Event handler is called whenever error happens during conversion process.
        /// 
        /// Error typically means that conversion will be terminated.
        /// </summary>
        public event ErrorEventHandler Error;

        /// <summary>
        /// This event handler is fired when conversion is finished.
        /// </summary>
        public event FinishEventHandler Finished;

        /// <summary>
        /// This event handler signals phase change of the conversion process.
        /// </summary>
        public event PhaseChangedEventHandler PhaseChanged;

        /// <summary>
        /// This event handler signals progress change of the conversion process.
        /// 
        /// Number of percents is included in text description.
        /// </summary>
        public event ProgressChangedEventHandler ProgressChanged;

        /// <summary>
        /// This event handler is called whenever warning happens during conversion process.
        /// 
        /// You can also see javascript errors and warnings if you enable <code>SetJavascriptDebugMode</code> in <code>ObjectSettings</code>
        /// </summary>
        public event WarningEventHandler Warning;

        /// <summary>
        /// Current phase number for the converter.
        /// 
        /// We recommend to use this property only inside the event handlers.
        /// </summary>
        public int CurrentPhase
        {
            get
            {
                return PechkinStatic.GetPhaseNumber(this.converter);
            }
        }

        /// <summary>
        /// Error code returned by server when converter tried to request the page or the resource. Should be available after failed conversion attempt.
        /// </summary>
        public int HttpErrorCode
        {
            get
            {
                return PechkinStatic.GetHttpErrorCode(this.converter);
            }
        }

        /// <summary>
        /// Phase count for the current conversion process.
        /// 
        /// We recommend to use this property only inside the event handlers.
        /// </summary>
        public int PhaseCount
        {
            get
            {
                return PechkinStatic.GetPhaseCount(this.converter);
            }
        }

        /// <summary>
        /// Current phase string description for the converter.
        /// 
        /// We recommend to use this property only inside the event handlers.
        /// </summary>
        public string PhaseDescription
        {
            get
            {
                return PechkinStatic.GetPhaseDescription(this.converter, this.CurrentPhase);
            }
        }

        /// <summary>
        /// Current progress string description. It includes percent count, btw.
        /// 
        /// We recommend to use this property only inside the event handlers.
        /// </summary>
        public string ProgressString
        {
            get
            {
                return PechkinStatic.GetProgressDescription(this.converter);
            }
        }

        /// <summary>
        /// Runs conversion process.
        /// 
        /// Allows to convert both external HTML resource and HTML string.
        /// 
        /// Takes html source as a byte array for when you don't know the encoding.
        /// </summary>
        /// <param name="doc">document parameters</param>
        /// <param name="html">document body, ignored if <code>ObjectSettings.SetPageUri</code> is set</param>
        /// <returns>PDF document body</returns>
        public byte[] Convert(ObjectSettings doc, byte[] html)
        {
            this.CreateConverter();

            // create unmanaged object config
            IntPtr objConf = doc.CreateObjectConfig();

            Tracer.Trace(string.Format("T:{0} Created object config", Thread.CurrentThread.Name));

            // add object to converter
            PechkinStatic.AddObject(this.converter, objConf, html);

            Tracer.Trace(string.Format("T:{0} Added object to converter", Thread.CurrentThread.Name));

            // run OnBegin
            this.OnBegin(this.converter);

            // run conversion process
            if (!PechkinStatic.PerformConversion(this.converter))
            {
                Tracer.Trace(string.Format("T:{0} Conversion failed, null returned", Thread.CurrentThread.Name));

                return null;
            }

            // get output
            var result = PechkinStatic.GetConverterResult(this.converter);

            if (!this.converter.Equals(IntPtr.Zero))
            {
                Tracer.Trace(string.Format("T:{0} Releasing unmanaged converter", Thread.CurrentThread.Name));

                PechkinStatic.DestroyConverter(this.converter);
            }

            return result;
        }

        /// <summary>
        /// Runs conversion process.
        /// 
        /// Allows to convert both external HTML resource and HTML string.
        /// </summary>
        /// <param name="doc">document parameters</param>
        /// <param name="html">document body, ignored if <code>ObjectSettings.SetPageUri</code> is set</param>
        /// <returns>PDF document body</returns>
        public byte[] Convert(ObjectSettings doc, string html)
        {
            return this.Convert(doc, Encoding.UTF8.GetBytes(html));
        }

        /// <summary>
        /// Converts external HTML resource into PDF.
        /// </summary>
        /// <param name="doc">document parameters, <code>ObjectSettings.SetPageUri</code> should be set</param>
        /// <returns>PDF document body</returns>
        public byte[] Convert(ObjectSettings doc)
        {
            return this.Convert(doc, (byte[])null);
        }

        /// <summary>
        /// Converts HTML string to PDF with default settings.
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <returns>PDF document body</returns>
        public byte[] Convert(string html)
        {
            return this.Convert(new ObjectSettings(), html);
        }

        /// <summary>
        /// Converts HTML string to PDF with default settings.
        /// 
        /// Takes html source as a byte array for when you don't know the encoding.
        /// </summary>
        /// <param name="html">HTML string</param>
        /// <returns>PDF document body</returns>
        public byte[] Convert(byte[] html)
        {
            return this.Convert(new ObjectSettings(), html);
        }

        /// <summary>
        /// Converts HTML page at specified URL to PDF with default settings.
        /// </summary>
        /// <param name="url">url of page, can be either http/https or file link</param>
        /// <returns>PDF document body</returns>
        public byte[] Convert(Uri url)
        {
            return this.Convert(new ObjectSettings().SetPageUri(url.AbsoluteUri));
        }

        protected virtual void OnBegin(IntPtr converter)
        {
            int expectedPhaseCount = PechkinStatic.GetPhaseCount(converter);

            Tracer.Trace(string.Format("T:{0} Conversion started, {1} phases awaiting", Thread.CurrentThread.Name, expectedPhaseCount));

            BeginEventHandler handler = this.Begin;
            try
            {
                if (handler != null)
                {
                    handler(this, expectedPhaseCount);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(String.Format("T:{1} Exception in Begin event handler {0}", e, Thread.CurrentThread.Name));
            }
        }

        protected virtual void OnError(IntPtr converter, string errorText)
        {
            Tracer.Warn(string.Format("T:{0} Conversion Error: {1}", Thread.CurrentThread.Name, errorText));

            ErrorEventHandler handler = this.Error;
            try
            {
                if (handler != null)
                {
                    handler(this, errorText);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Error event handler", Thread.CurrentThread.Name), e);
            }
        }

        protected virtual void OnFinished(IntPtr converter, int success)
        {
            Tracer.Trace(string.Format("T:{0} Conversion Finished: {1}", Thread.CurrentThread.Name, success != 0 ? "Succeede" : "Failed"));

            FinishEventHandler handler = this.Finished;
            try
            {
                if (handler != null)
                {
                    handler(this, success != 0);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Finish event handler", Thread.CurrentThread.Name), e);
            }
        }

        protected virtual void OnPhaseChanged(IntPtr converter)
        {
            int phaseNumber = PechkinStatic.GetPhaseNumber(converter);
            string phaseDescription = PechkinStatic.GetPhaseDescription(converter, phaseNumber);

            Tracer.Trace(string.Format("T:{0} Conversion Phase Changed: #{1} {2}", Thread.CurrentThread.Name, phaseNumber, phaseDescription));

            PhaseChangedEventHandler handler = this.PhaseChanged;
            try
            {
                if (handler != null)
                {
                    handler(this, phaseNumber, phaseDescription);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in PhaseChange event handler", Thread.CurrentThread.Name), e);
            }
        }

        protected virtual void OnProgressChanged(IntPtr converter, int progress)
        {
            string progressDescription = PechkinStatic.GetProgressDescription(converter);

            Tracer.Trace(string.Format("T:{0} Conversion Progress Changed: ({1}) {2}", Thread.CurrentThread.Name, progress, progressDescription));

            ProgressChangedEventHandler handler = this.ProgressChanged;
            try
            {
                if (handler != null)
                {
                    handler(this, progress, progressDescription);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Progress event handler", Thread.CurrentThread.Name), e);
            }
        }

        protected virtual void OnWarning(IntPtr converter, string warningText)
        {
            Tracer.Warn(string.Format("T:{0} Conversion Warning: {1}", Thread.CurrentThread.Name, warningText));

            WarningEventHandler handler = this.Warning;
            try
            {
                if (handler != null)
                {
                    handler(this, warningText);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Warning event handler", Thread.CurrentThread.Name), e);
            }
        }

        private void CreateConverter()
        {
            if (!this.converter.Equals(IntPtr.Zero))
            {
                PechkinStatic.DestroyConverter(this.converter);

                Tracer.Trace(string.Format("T:{0} Destroyed previous converter", Thread.CurrentThread.Name));
            }

            // the damn lib... we can't reuse anything
            this.globalConfigUnmanaged = this.globalConfig.CreateGlobalConfig();
            this.converter = PechkinStatic.CreateConverter(this.globalConfigUnmanaged);

            Tracer.Trace(string.Format("T:{0} Created converter", Thread.CurrentThread.Name));

            PechkinStatic.SetErrorCallback(this.converter, this.onErrorDelegate);
            PechkinStatic.SetWarningCallback(this.converter, this.onWarningDelegate);
            PechkinStatic.SetPhaseChangedCallback(this.converter, this.onPhaseChangedDelegate);
            PechkinStatic.SetProgressChangedCallback(this.converter, this.onProgressChangedDelegate);
            PechkinStatic.SetFinishedCallback(this.converter, this.onFinishedDelegate);

            Tracer.Trace(string.Format("T:{0} Added callbacks to converter", Thread.CurrentThread.Name));
        }
    }
}