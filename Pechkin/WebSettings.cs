using System;

namespace Pechkin
{
    [Serializable]
    public class WebSettings
    {
        public WebSettings()
        {
            this.DefaultEncoding = "utf-8";
            this.EnableIntelligentShrinking = true;
            this.EnableJavascript = true;
            this.LoadImages = true;
            this.PrintBackground = true;
            this.PrintMediaType = true;
        }

        /// <summary>
        /// What encoding should we guess content is using if they do not specify it properly? E.g. "utf-8"
        /// </summary>
        [WkhtmltopdfSetting("web.defaultEncoding")]
        public string DefaultEncoding { get; set; }

        /// <summary>
        /// Whether or not to enable intelligent compression of content to fit in the page
        /// </summary>
        [WkhtmltopdfSetting("web.enableIntelligentShrinking")]
        public bool EnableIntelligentShrinking { get; set; }

        /// <summary>
        /// Whether or not to enable JavaScript
        /// </summary>
        [WkhtmltopdfSetting("web.enableJavascript")]
        public bool EnableJavascript { get; set; }

        /// <summary>
        /// Whether to enable plugins (maybe like Flash? unsure)
        /// </summary>
        [WkhtmltopdfSetting("web.enablePlugins")]
        public bool EnablePlugins { get; set; }

        /// <summary>
        /// Whether or not to load images
        /// </summary>
        [WkhtmltopdfSetting("web.loadImages")]
        public bool LoadImages { get; set; }

        /// <summary>
        /// The minimum font size to use
        /// </summary>
        [WkhtmltopdfSetting("web.minimumFontSize")]
        public double? MinimumFontSize { get; set; }

        /// <summary>
        /// Whether or not to print the background on elements
        /// </summary>
        [WkhtmltopdfSetting("web.background")]
        public bool? PrintBackground { get; set; }

        /// <summary>
        /// Whether to print the content using the print media type instead of the screen media type
        /// </summary>
        [WkhtmltopdfSetting("web.printMediaType")]
        public bool? PrintMediaType { get; set; }

        /// <summary>
        /// Path to a user specified style sheet
        /// </summary>
        [WkhtmltopdfSetting("web.userStyleSheet")]
        public string UserStyleSheet { get; set; }
    }
}