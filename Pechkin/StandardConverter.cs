using System;
using System.Runtime.InteropServices;
using System.Threading;
using TuesPechkin.EventHandlers;
using TuesPechkin.Util;

namespace TuesPechkin
{
    public class StandardConverter : MarshalByRefObject, IConverter
    {
        protected IToolset Toolset { get; private set; }

        protected HtmlDocument ProcessingDocument { get; private set; }

        public StandardConverter(IToolset assembly)
        {
            if (assembly == null)
            {
                throw new ArgumentNullException("assembly");
            }

            Toolset = assembly;

            Tracer.Trace(string.Format("T:{0} Created StandardConverter", Thread.CurrentThread.Name));
        }

        public event BeginEventHandler Begin;

        public event ErrorEventHandler Error;

        public event FinishEventHandler Finished;

        public event PhaseChangedEventHandler PhaseChanged;

        public event ProgressChangedEventHandler ProgressChanged;

        public event WarningEventHandler Warning;

        public virtual byte[] Convert(HtmlDocument document)
        {
            Toolset.Load();
            Toolset.SetUp();

            ProcessingDocument = document;
            var converter = CreateConverter(document);

            Tracer.Trace(string.Format("T:{0} Created converter", Thread.CurrentThread.Name));
            
            Toolset.SetErrorCallback(converter, OnError);
            Toolset.SetWarningCallback(converter, OnWarning);
            Toolset.SetPhaseChangedCallback(converter, OnPhaseChanged);
            Toolset.SetProgressChangedCallback(converter, OnProgressChanged);
            Toolset.SetFinishedCallback(converter, OnFinished);

            Tracer.Trace(string.Format("T:{0} Added callbacks to converter", Thread.CurrentThread.Name));

            // run OnBegin
            OnBegin(converter);

            byte[] result = null;

            // run conversion process
            if (!Toolset.PerformConversion(converter))
            {
                Tracer.Trace(string.Format("T:{0} Conversion failed, null returned", Thread.CurrentThread.Name));
            }
            else
            {
                // get output
                result = Toolset.GetConverterResult(converter);
            }

            Tracer.Trace(string.Format("T:{0} Releasing unmanaged converter", Thread.CurrentThread.Name));
            Toolset.DestroyConverter(converter);
            Toolset.TearDown();
            ProcessingDocument = null;
            return result;
        }

        private void OnBegin(IntPtr converter)
        {
            int expectedPhaseCount = Toolset.GetPhaseCount(converter);

            Tracer.Trace(string.Format("T:{0} Conversion started, {1} phases awaiting", Thread.CurrentThread.Name, expectedPhaseCount));

            BeginEventHandler handler = this.Begin;
            try
            {
                if (handler != null)
                {
                    handler(this, ProcessingDocument, expectedPhaseCount);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(String.Format("T:{1} Exception in Begin event handler {0}", e, Thread.CurrentThread.Name));
            }
        }

        private void OnError(IntPtr converter, string errorText)
        {
            Tracer.Warn(string.Format("T:{0} Conversion Error: {1}", Thread.CurrentThread.Name, errorText));

            ErrorEventHandler handler = this.Error;
            try
            {
                if (handler != null)
                {
                    handler(this, ProcessingDocument, errorText);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Error event handler", Thread.CurrentThread.Name), e);
            }
        }

        private void OnFinished(IntPtr converter, int success)
        {
            Tracer.Trace(string.Format("T:{0} Conversion Finished: {1}", Thread.CurrentThread.Name, success != 0 ? "Succeeded" : "Failed"));

            FinishEventHandler handler = this.Finished;
            try
            {
                if (handler != null)
                {
                    handler(this, ProcessingDocument, success != 0);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Finish event handler", Thread.CurrentThread.Name), e);
            }
        }

        private void OnPhaseChanged(IntPtr converter)
        {
            int phaseNumber = Toolset.GetPhaseNumber(converter);
            string phaseDescription = Toolset.GetPhaseDescription(converter, phaseNumber);

            Tracer.Trace(string.Format("T:{0} Conversion Phase Changed: #{1} {2}", Thread.CurrentThread.Name, phaseNumber, phaseDescription));

            PhaseChangedEventHandler handler = this.PhaseChanged;
            try
            {
                if (handler != null)
                {
                    handler(this, ProcessingDocument, phaseNumber, phaseDescription);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in PhaseChange event handler", Thread.CurrentThread.Name), e);
            }
        }

        private void OnProgressChanged(IntPtr converter, int progress)
        {
            string progressDescription = Toolset.GetProgressDescription(converter);

            Tracer.Trace(string.Format("T:{0} Conversion Progress Changed: ({1}) {2}", Thread.CurrentThread.Name, progress, progressDescription));

            ProgressChangedEventHandler handler = this.ProgressChanged;
            try
            {
                if (handler != null)
                {
                    handler(this, ProcessingDocument, progress, progressDescription);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Progress event handler", Thread.CurrentThread.Name), e);
            }
        }

        private void OnWarning(IntPtr converter, string warningText)
        {
            Tracer.Warn(string.Format("T:{0} Conversion Warning: {1}", Thread.CurrentThread.Name, warningText));

            WarningEventHandler handler = this.Warning;
            try
            {
                if (handler != null)
                {
                    handler(this, ProcessingDocument, warningText);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Warning event handler", Thread.CurrentThread.Name), e);
            }
        }

        private IntPtr CreateConverter(HtmlDocument document)
        {
            if (document.Objects.Count == 0)
            {
                throw new InvalidOperationException("No objects defined for document; cannot convert");
            }

            var converter = IntPtr.Zero;

            {
                var config = Toolset.CreateGlobalSettings();

                SettingApplicator.ApplySettings(Toolset, config, document.GlobalSettings);

                converter = Toolset.CreateConverter(config);
            }

            //if (this.TableOfContents != null)
            //{
            //    this.TableOfContents.ApplyToConverter(converter);
            //}

            foreach (var setting in document.Objects)
            {
                if (setting != null)
                {
                    var config = Toolset.CreateObjectSettings();

                    SettingApplicator.ApplySettings(Toolset, config, setting);

                    Toolset.AddObject(converter, config, setting.RawData);
                }
            }

            return converter;
        }
    }
}