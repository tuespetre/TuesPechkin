using System;

namespace Pechkin
{
    [Serializable]
    public class TableOfContentsSettings
    {
        public TableOfContentsSettings()
        {
            this.ProduceTableOfContents = true;
        }

        private readonly ObjectSettings objectSettings = new ObjectSettings();

        private string xslStyleSheet;

        [WkhtmltopdfSetting("toc.fontScale")]
        public double? FontScale { get; set; }

        [WkhtmltopdfSetting("toc.indentation")]
        public string Indentation { get; set; }

        [WkhtmltopdfSetting("toc.backLinks")]
        public bool ProduceBackLinks { get; set; }

        [WkhtmltopdfSetting("toc.forwardLinks")]
        public bool ProduceForwardLinks { get; set; }

        [WkhtmltopdfSetting("toc.useDottedLines")]
        public bool UseDottedLines { get; set; }

        [WkhtmltopdfSetting("tocXsl")]
        public string XslStyleSheet
        {
            get
            {
                return this.xslStyleSheet ?? PechkinBindings.TocXslFilename;
            }
            set
            {
                this.xslStyleSheet = value;
            }
        }

        [WkhtmltopdfSetting("isTableOfContent")]
        internal bool ProduceTableOfContents { get; set; }

        internal void ApplyToConverter(IntPtr converter)
        {
            var config = PechkinStatic.CreateObjectSettings();

            SettingApplicator.ApplySettings(config, this);
            SettingApplicator.ApplySettings(config, this.objectSettings);
            SettingApplicator.ApplySettings(config, this.objectSettings.HeaderSettings);
            SettingApplicator.ApplySettings(config, this.objectSettings.FooterSettings);
            SettingApplicator.ApplySettings(config, this.objectSettings.WebSettings);
            SettingApplicator.ApplySettings(config, this.objectSettings.LoadSettings);

            PechkinStatic.AddObject(converter, config, (byte[])null);
        }
    }
}