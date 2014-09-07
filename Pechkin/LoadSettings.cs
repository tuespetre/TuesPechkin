using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace TuesPechkin
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LoadSettings
    {
        public LoadSettings()
        {
            this.Cookies = new Dictionary<string, string>();
            this.CustomHeaders = new Dictionary<string, string>();
        }

        public enum ContentErrorHandling
        {
            Abort,
            Skip,
            Ignore
        }

        [WkhtmltopdfSetting("load.blockLocalFileAccess")]
        public bool? BlockLocalFileAccess { get; set; }

        [WkhtmltopdfSetting("load.cookies")]
        public Dictionary<string, string> Cookies { get; private set; }

        [WkhtmltopdfSetting("load.customHeaders")]
        public Dictionary<string, string> CustomHeaders { get; private set; }

        [WkhtmltopdfSetting("load.debugJavascript")]
        public bool? DebugJavascript { get; set; }

        [WkhtmltopdfSetting("load.loadErrorHandling")]
        public ContentErrorHandling? ErrorHandling { get; set; }

        [WkhtmltopdfSetting("load.password")]
        public string Password { get; set; }

        [WkhtmltopdfSetting("load.proxy")]
        public string Proxy { get; set; }

        [WkhtmltopdfSetting("load.jsdelay")]
        public int? RenderDelay { get; set; }

        [WkhtmltopdfSetting("load.repeatCustomHeaders")]
        public bool? RepeatCustomHeaders { get; set; }

        [WkhtmltopdfSetting("load.stopSlowScript")]
        public bool? StopSlowScript { get; set; }

        [WkhtmltopdfSetting("load.username")]
        public string Username { get; set; }

        [WkhtmltopdfSetting("load.zoomFactor")]
        public double? ZoomFactor { get; set; }
    }
}