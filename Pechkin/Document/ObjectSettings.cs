using System;
using System.ComponentModel;
using TuesPechkin.Attributes;

namespace TuesPechkin
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class ObjectSettings
    {
        [ObjectSetting("includeInOutline")]
        public bool? IncludeInOutline { get; set; }

        [ObjectSetting("pagesCount")]
        public bool? CountPages { get; set; }

        [ObjectSetting("page")]
        public string PageUrl { get; set; }

        [ObjectSetting("produceForms")]
        public bool? ProduceForms { get; set; }

        [ObjectSetting("useExternalLinks")]
        public bool? ProduceExternalLinks { get; set; }

        [ObjectSetting("useLocalLinks")]
        public bool? ProduceLocalLinks { get; set; }

        [SettingBag]
        public FooterSettings FooterSettings
        {
            get
            {
                return this.footer;
            }
            set
            {
                DEPRECAAAAATED.BestAintBeNull(value);
                this.footer = value;
            }
        }

        [SettingBag]
        public HeaderSettings HeaderSettings
        {
            get
            {
                return this.header;
            }
            set
            {
                DEPRECAAAAATED.BestAintBeNull(value);
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
                DEPRECAAAAATED.BestAintBeNull(value);
                this.data = System.Text.Encoding.UTF8.GetBytes(value);
            }
        }

        [SettingBag]
        public LoadSettings LoadSettings
        {
            get
            {
                return this.load;
            }
            set
            {
                DEPRECAAAAATED.BestAintBeNull(value);
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
                DEPRECAAAAATED.BestAintBeNull(value);
                this.data = value;
            }
        }

        [SettingBag]
        public WebSettings WebSettings
        {
            get
            {
                return this.web;
            }
            set
            {
                DEPRECAAAAATED.BestAintBeNull(value);
                this.web = value;
            }
        }

        private byte[] data = new byte[0];

        private FooterSettings footer = new FooterSettings();

        private HeaderSettings header = new HeaderSettings();

        private LoadSettings load = new LoadSettings();

        private WebSettings web = new WebSettings();
    }
}