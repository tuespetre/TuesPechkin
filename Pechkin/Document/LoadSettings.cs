using System;
using System.Collections.Generic;
using System.ComponentModel;
using TuesPechkin.Attributes;

namespace TuesPechkin
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class LoadSettings
    {
        public LoadSettings()
        {
            //this.Cookies = new Dictionary<string, string>();
            //this.CustomHeaders = new Dictionary<string, string>();
        }

        public enum ContentErrorHandling
        {
            Abort,
            Skip,
            Ignore
        }

        [ObjectSetting("load.blockLocalFileAccess")]
        public bool? BlockLocalFileAccess { get; set; }

        /*[ObjectSetting("load.cookies")]
        public Dictionary<string, string> Cookies { get; private set; }*/

        /*[ObjectSetting("load.customHeaders")]
        public Dictionary<string, string> CustomHeaders { get; private set; }*/

        [ObjectSetting("load.debugJavascript")]
        public bool? DebugJavascript { get; set; }

        [ObjectSetting("load.loadErrorHandling")]
        public ContentErrorHandling? ErrorHandling { get; set; }

        [ObjectSetting("load.password")]
        public string Password { get; set; }

        [ObjectSetting("load.proxy")]
        public string Proxy { get; set; }

        [ObjectSetting("load.jsdelay")]
        public int? RenderDelay { get; set; }

        [ObjectSetting("load.repeatCustomHeaders")]
        public bool? RepeatCustomHeaders { get; set; }

        [ObjectSetting("load.stopSlowScript")]
        public bool? StopSlowScript { get; set; }

        [ObjectSetting("load.username")]
        public string Username { get; set; }

        [ObjectSetting("load.zoomFactor")]
        public double? ZoomFactor { get; set; }
    }
}