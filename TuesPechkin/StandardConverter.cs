using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using ErrorEventArgs = TuesPechkin.ErrorEventArgs;

namespace TuesPechkin
{
    public class StandardConverter : MarshalByRefObject, IConverter
    {
        protected IToolset Toolset { get; private set; }

        protected IDocument ProcessingDocument { get; private set; }

        public StandardConverter(IToolset toolset)
        {
            if (toolset == null)
            {
                throw new ArgumentNullException("toolset");
            }

            Toolset = toolset;

            Tracer.Trace(string.Format("T:{0} Created StandardConverter", Thread.CurrentThread.Name));
        }

        public event EventHandler<BeginEventArgs> Begin;

        public event EventHandler<ErrorEventArgs> Error;

        public event EventHandler<FinishEventArgs> Finish;

        public event EventHandler<PhaseChangeEventArgs> PhaseChange;

        public event EventHandler<ProgressChangeEventArgs> ProgressChange;

        public event EventHandler<WarningEventArgs> Warning;

        public virtual byte[] Convert(IDocument document)
        {
            Toolset.Load();

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
            ProcessingDocument = null;
            return result;
        }

        private void OnBegin(IntPtr converter)
        {
            int expectedPhaseCount = Toolset.GetPhaseCount(converter);

            Tracer.Trace(string.Format("T:{0} Conversion started, {1} phases awaiting", Thread.CurrentThread.Name, expectedPhaseCount));

            try
            {
                if (Begin != null)
                {
                    var args = new BeginEventArgs
                    {
                        Document = ProcessingDocument,
                        ExpectedPhaseCount = expectedPhaseCount
                    };

                    Begin(this, args);
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

            try
            {
                if (Error != null)
                {
                    var args = new ErrorEventArgs
                    {
                        Document = ProcessingDocument,
                        ErrorMessage = errorText
                    };

                    Error(this, args);
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

            try
            {
                if (Finish != null)
                {
                    var args = new FinishEventArgs
                    {
                        Document = ProcessingDocument,
                        Success = success != 0
                    };

                    Finish(this, args);
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

            try
            {
                if (PhaseChange != null)
                {
                    var args = new PhaseChangeEventArgs
                    {
                        Document = ProcessingDocument,
                        PhaseNumber = phaseNumber,
                        PhaseDescription = phaseDescription
                    };

                    PhaseChange(this, args);
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

            try
            {
                var args = new ProgressChangeEventArgs
                {
                    Document = ProcessingDocument,
                    Progress = progress,
                    ProgressDescription = progressDescription
                };

                if (ProgressChange != null)
                {
                    ProgressChange(this, args);
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

            try
            {
                if (Warning != null)
                {
                    var args = new WarningEventArgs
                    {
                        Document = ProcessingDocument,
                        WarningMessage = warningText
                    };

                    Warning(this, args);
                }
            }
            catch (Exception e)
            {
                Tracer.Warn(string.Format("T:{0} Exception in Warning event handler", Thread.CurrentThread.Name), e);
            }
        }

        private IntPtr CreateConverter(IDocument document)
        {
            var converter = IntPtr.Zero;

            {
                var config = Toolset.CreateGlobalSettings();

                ApplySettingsToConfig(config, document, true);

                converter = Toolset.CreateConverter(config);
            }

            foreach (var setting in document.GetObjects())
            {
                if (setting != null)
                {
                    var config = Toolset.CreateObjectSettings();

                    ApplySettingsToConfig(config, setting, false);

                    Toolset.AddObject(converter, config, setting.GetData());
                }
            }

            return converter;
        }

        private void ApplySettingsToConfig(IntPtr config, ISettings settings, bool isGlobal)
        {
            if (settings == null)
            {
                return;
            }

            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

            foreach (var property in settings.GetType().GetProperties(bindingFlags))
            {
                var attributes = property.GetCustomAttributes(true);
                var rawValue = property.GetValue(settings, null);

                if (rawValue == null)
                {
                    continue;
                }
                else if (attributes.Length > 0 && attributes[0] is WkhtmltoxSettingAttribute)
                {
                    var attribute = attributes[0] as WkhtmltoxSettingAttribute;

                    Apply(config, attribute.Name, rawValue, isGlobal);
                }
                else if (rawValue is ISettings)
                {
                    ApplySettingsToConfig(config, rawValue as ISettings, isGlobal);
                }
            }
        }
        
        private void Apply(IntPtr config, string name, object value, bool isGlobal)
        {
            var type = value.GetType();
            var apply = isGlobal 
                ? (FuncShim<string, string, int>)((k, v) => Toolset.SetGlobalSetting(config, k, v))
                : (FuncShim<string, string, int>)((k, v) => Toolset.SetObjectSetting(config, k, v));

            if (type == typeof(double))
            {
                apply(name, ((double)value).ToString("0.##", CultureInfo.InvariantCulture));
            }
            else if (type == typeof(bool))
            {
                apply(name, ((bool)value) ? "true" : "false");
            }
            else if (typeof(IEnumerable<KeyValuePair<string, string>>).IsAssignableFrom(type))
            {
                var dictionary = (IEnumerable<KeyValuePair<string, string>>)value;
                var counter = 0;

                foreach (var entry in dictionary)
                {
                    if (entry.Key == null || entry.Value == null)
                    {
                        continue;
                    }

                    apply(name + ".append", null);
                    apply(string.Format("{0}[{1}]", name, counter), entry.Key + "\n" + entry.Value);

                    counter++;
                }
            }
            else if (typeof(IEnumerable<PostItem>).IsAssignableFrom(type))
            {
                var list = (IEnumerable<PostItem>)value;
                var counter = 0;

                foreach (var item in list)
                {
                    if (string.IsNullOrEmpty(item.Name) || string.IsNullOrEmpty(item.Value))
                    {
                        continue;
                    }

                    apply(name + ".append", null);
                    apply(string.Format("{0}[{1}].name", name, counter), item.Name);
                    apply(string.Format("{0}[{1}].value", name, counter), item.Value);
                    apply(string.Format("{0}[{1}].file", name, counter), item.IsFile ? "true" : "false");

                    counter++;
                }
            }
            else
            {
                apply(name, value.ToString());
            }
        }
    }
}