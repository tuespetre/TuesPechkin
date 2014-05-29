using System;

namespace Pechkin
{
    [Serializable]
    public class LoadSettings
    {
        public LoadSettings()
        {
            this.ErrorHandling = ContentErrorHandling.Ignore;
            this.StopSlowScript = true;
        }

        public enum ContentErrorHandling
        {
            Abort,
            Skip,
            Ignore
        }

        [WkhtmltopdfSetting("load.blockLocalFileAccess")]
        public bool BlockLocalFileAccess { get; set; }

        [WkhtmltopdfSetting("load.debugJavascript")]
        public bool DebugJavascript { get; set; }

        [WkhtmltopdfSetting("load.loadErrorHandling")]
        public ContentErrorHandling ErrorHandling { get; set; }

        [WkhtmltopdfSetting("load.password")]
        public string Password { get; set; }

        [WkhtmltopdfSetting("load.proxy")]
        public string Proxy { get; set; }

        [WkhtmltopdfSetting("load.jsdelay")]
        public int? RenderDelay { get; set; }

        [WkhtmltopdfSetting("load.stopSlowScript")]
        public bool StopSlowScript { get; set; }

        [WkhtmltopdfSetting("load.username")]
        public string Username { get; set; }

        [WkhtmltopdfSetting("load.zoomFactor")]
        public double? ZoomFactor { get; set; }
    }
}