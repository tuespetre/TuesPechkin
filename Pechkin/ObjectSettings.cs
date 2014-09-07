using System;
using System.ComponentModel;

namespace TuesPechkin
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ObjectSettings
    {
        [WkhtmltopdfSetting("includeInOutline")]
        public bool? IncludeInOutline { get; set; }

        [WkhtmltopdfSetting("pagesCount")]
        public bool? CountPages { get; set; }

        [WkhtmltopdfSetting("page")]
        public string PageUrl { get; set; }

        [WkhtmltopdfSetting("produceForms")]
        public bool? ProduceForms { get; set; }

        [WkhtmltopdfSetting("useExternalLinks")]
        public bool? ProduceExternalLinks { get; set; }

        [WkhtmltopdfSetting("useLocalLinks")]
        public bool? ProduceLocalLinks { get; set; }

        private byte[] data = new byte[0];

        private FooterSettings footer = new FooterSettings();

        private HeaderSettings header = new HeaderSettings();

        private LoadSettings load = new LoadSettings();

        private WebSettings web = new WebSettings();

        public FooterSettings FooterSettings
        {
            get
            {
                return this.footer;
            }
            set
            {
                Assert.BestAintBeNull(value);
                this.footer = value;
            }
        }

        public HeaderSettings HeaderSettings
        {
            get
            {
                return this.header;
            }
            set
            {
                Assert.BestAintBeNull(value);
                this.header = value;
            }
        }

        public string HtmlText
        {
            get
            {
                return System.Text.Encoding.UTF8.GetString(this.data);
            }
            set
            {
                Assert.BestAintBeNull(value);
                this.data = System.Text.Encoding.UTF8.GetBytes(value);
            }
        }

        public LoadSettings LoadSettings
        {
            get
            {
                return this.load;
            }
            set
            {
                Assert.BestAintBeNull(value);
                this.load = value;
            }
        }

        [Browsable(false)]
        public byte[] RawData
        {
            get
            {
                return this.data;
            }
            set
            {
                Assert.BestAintBeNull(value);
                this.data = value;
            }
        }

        public WebSettings WebSettings
        {
            get
            {
                return this.web;
            }
            set
            {
                Assert.BestAintBeNull(value);
                this.web = value;
            }
        }

        internal void ApplyToConverter(IntPtr converter)
        {
            var config = PechkinStatic.CreateObjectSettings();

            SettingApplicator.ApplySettings(config, this);
            SettingApplicator.ApplySettings(config, this.HeaderSettings);
            SettingApplicator.ApplySettings(config, this.FooterSettings);
            SettingApplicator.ApplySettings(config, this.WebSettings);
            SettingApplicator.ApplySettings(config, this.LoadSettings);

            PechkinStatic.AddObject(converter, config, this.data);
        }
    }
}