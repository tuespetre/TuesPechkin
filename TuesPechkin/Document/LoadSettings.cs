using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TuesPechkin
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LoadSettings : ISettings
    {
        public LoadSettings()
        {
            this.Cookies = new Dictionary<string, string>();
            this.CustomHeaders = new Dictionary<string, string>();
            this.PostItems = new List<PostItem>();
        }

        public enum ContentErrorHandling
        {
            Abort,
            Skip,
            Ignore
        }

        [WkhtmltoxSetting("load.blockLocalFileAccess")]
        public bool? BlockLocalFileAccess { get; set; }

        [WkhtmltoxSetting("load.cookies")]
        public Dictionary<string, string> Cookies { get; private set; }

        [WkhtmltoxSetting("load.customHeaders")]
        public Dictionary<string, string> CustomHeaders { get; private set; }

        [WkhtmltoxSetting("load.debugJavascript")]
        public bool? DebugJavascript { get; set; }

        [WkhtmltoxSetting("load.loadErrorHandling")]
        public ContentErrorHandling? ErrorHandling { get; set; }

        [WkhtmltoxSetting("load.password")]
        public string Password { get; set; }

        [WkhtmltoxSetting("load.post")]
        public IList<PostItem> PostItems { get; private set; }

        [WkhtmltoxSetting("load.proxy")]
        public string Proxy { get; set; }

        [WkhtmltoxSetting("load.jsdelay")]
        public int? RenderDelay { get; set; }

        [WkhtmltoxSetting("load.repeatCustomHeaders")]
        public bool? RepeatCustomHeaders { get; set; }

        [WkhtmltoxSetting("load.stopSlowScript")]
        public bool? StopSlowScript { get; set; }

        [WkhtmltoxSetting("load.username")]
        public string Username { get; set; }

        [WkhtmltoxSetting("load.windowStatus")]
        public string WindowStatus { get; set; }

        [WkhtmltoxSetting("load.zoomFactor")]
        public double? ZoomFactor { get; set; }
    }
}