using System;
using System.ComponentModel;
using TuesPechkin.Attributes;

namespace TuesPechkin
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class WebSettings
    {
        /// <summary>
        /// What encoding should we guess content is using if they do not specify it properly? E.g. "utf-8"
        /// </summary>
        [ObjectSetting("web.defaultEncoding")]
        public string DefaultEncoding { get; set; }

        /// <summary>
        /// Whether or not to enable intelligent compression of content to fit in the page
        /// </summary>
        [ObjectSetting("web.enableIntelligentShrinking")]
        public bool? EnableIntelligentShrinking { get; set; }

        /// <summary>
        /// Whether or not to enable JavaScript
        /// </summary>
        [ObjectSetting("web.enableJavascript")]
        public bool? EnableJavascript { get; set; }

        /// <summary>
        /// Whether to enable plugins (maybe like Flash? unsure)
        /// </summary>
        [ObjectSetting("web.enablePlugins")]
        public bool? EnablePlugins { get; set; }

        /// <summary>
        /// Whether or not to load images
        /// </summary>
        [ObjectSetting("web.loadImages")]
        public bool? LoadImages { get; set; }

        /// <summary>
        /// The minimum font size to use
        /// </summary>
        [ObjectSetting("web.minimumFontSize")]
        public double? MinimumFontSize { get; set; }

        /// <summary>
        /// Whether or not to print the background on elements
        /// </summary>
        [ObjectSetting("web.background")]
        public bool? PrintBackground { get; set; }

        /// <summary>
        /// Whether to print the content using the print media type instead of the screen media type
        /// </summary>
        [ObjectSetting("web.printMediaType")]
        public bool? PrintMediaType { get; set; }

        /// <summary>
        /// Path to a user specified style sheet
        /// </summary>
        [ObjectSetting("web.userStyleSheet")]
        public string UserStyleSheet { get; set; }
    }
}