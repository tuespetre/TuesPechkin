using System;
using System.ComponentModel;

namespace TuesPechkin
{
    [Serializable]
    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class FooterSettings : ISettings
    {
        /// <summary>
        /// Sets center text for the header/footer. Following replaces occur in this text:
        /// * [page]       Replaced by the number of the pages currently being printed
        /// * [frompage]   Replaced by the number of the first page to be printed
        /// * [topage]     Replaced by the number of the last page to be printed
        /// * [webpage]    Replaced by the URL of the page being printed
        /// * [section]    Replaced by the name of the current section
        /// * [subsection] Replaced by the name of the current subsection
        /// * [date]       Replaced by the current date in system local format
        /// * [time]       Replaced by the current time in system local format
        /// </summary>
        [WkhtmltoxSetting("footer.center")]
        public string CenterText { get; set; }

        /// <summary>
        /// Amount of space between header/footer and content in millimeters
        /// </summary>
        [WkhtmltoxSetting("footer.spacing")]
        public double? ContentSpacing { get; set; }

        /// <summary>
        /// Font name for the header, e.g. "Courier New"
        /// </summary>
        [WkhtmltoxSetting("footer.fontName")]
        public string FontName { get; set; }

        /// <summary>
        /// The font size in pt
        /// </summary>
        [WkhtmltoxSetting("footer.fontSize")]
        public double? FontSize { get; set; }

        /// <summary>
        /// URL for the HTML document to use as a header
        /// </summary>
        [WkhtmltoxSetting("footer.htmlUrl")]
        public string HtmlUrl { get; set; }

        /// <summary>
        /// Sets left text for the header/footer. Following replaces occur in this text:
        /// * [page]       Replaced by the number of the pages currently being printed
        /// * [frompage]   Replaced by the number of the first page to be printed
        /// * [topage]     Replaced by the number of the last page to be printed
        /// * [webpage]    Replaced by the URL of the page being printed
        /// * [section]    Replaced by the name of the current section
        /// * [subsection] Replaced by the name of the current subsection
        /// * [date]       Replaced by the current date in system local format
        /// * [time]       Replaced by the current time in system local format
        /// </summary>
        [WkhtmltoxSetting("footer.left")]
        public string LeftText { get; set; }

        /// <summary>
        /// Sets right text for the header/footer. Following replaces occur in this text:
        /// * [page]       Replaced by the number of the pages currently being printed
        /// * [frompage]   Replaced by the number of the first page to be printed
        /// * [topage]     Replaced by the number of the last page to be printed
        /// * [webpage]    Replaced by the URL of the page being printed
        /// * [section]    Replaced by the name of the current section
        /// * [subsection] Replaced by the name of the current subsection
        /// * [date]       Replaced by the current date in system local format
        /// * [time]       Replaced by the current time in system local format
        /// </summary>
        [WkhtmltoxSetting("footer.right")]
        public string RightText { get; set; }

        /// <summary>
        /// Whether or not to print a line between the header/footer and content (Default: false)
        /// </summary>
        [WkhtmltoxSetting("footer.line")]
        public bool? UseLineSeparator { get; set; }
    }
}