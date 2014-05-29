using System;
using System.Drawing;
using System.Globalization;

namespace Pechkin
{
    [Serializable]
    public class HeaderSettings
    {
        protected readonly virtual string settingPrefix = "header";

        public string FontSize; // font size for the header in pt, e.g. "13"
        public string FontName; // font name for the header, e.g. "Courier New"    
        public string LeftText;
        public string CenterText;
        public string RightText;
        public string LineSeparator = "false"; // if "true", line is printed under the header (and on top of the footer) separating header from content
        public string ContentSpacing; // space between the header and content in pt, e.g. "1.8"
        public string HtmlUrl; // URL for the HTML document to use as a header

        public HeaderSettings SetFontSize(double sizeInPt)
        {
            this.FontSize = sizeInPt.ToString("0.##", CultureInfo.InvariantCulture);

            return this;
        }

        public HeaderSettings SetFontName(string fontName)
        {
            this.FontName = fontName;

            return this;
        }

        public HeaderSettings SetFont(Font font)
        {
            return this.SetFontName(font.Name).SetFontSize(font.SizeInPoints);
        }

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
        /// <param name="leftText">text for the left part</param>
        /// <returns>config object</returns>
        public HeaderSettings SetLeftText(string leftText)
        {
            this.LeftText = leftText;

            return this;
        }

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
        /// <param name="centerText">text for the center part</param>
        /// <returns>config object</returns>
        public HeaderSettings SetCenterText(string centerText)
        {
            this.CenterText = centerText;

            return this;
        }

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
        /// <param name="rightText">text for the right part</param>
        /// <returns>config object</returns>
        public HeaderSettings SetRightText(string rightText)
        {
            this.RightText = rightText;

            return this;
        }

        /// <summary>
        /// Sets the texts for the header/footer. Following replaces occur in each of texts:
        /// * [page]       Replaced by the number of the pages currently being printed
        /// * [frompage]   Replaced by the number of the first page to be printed
        /// * [topage]     Replaced by the number of the last page to be printed
        /// * [webpage]    Replaced by the URL of the page being printed
        /// * [section]    Replaced by the name of the current section
        /// * [subsection] Replaced by the name of the current subsection
        /// * [date]       Replaced by the current date in system local format
        /// * [time]       Replaced by the current time in system local format
        /// </summary>
        /// <param name="leftText">text for the left part</param>
        /// <param name="centerText">text for the center part</param>
        /// <param name="rightText">text for the right part</param>
        /// <returns>config object</returns>
        public HeaderSettings SetTexts(string leftText, string centerText, string rightText)
        {
            return this.SetLeftText(leftText).SetCenterText(centerText).SetRightText(rightText);
        }

        public HeaderSettings SetLineSeparator(bool useLineSeparator)
        {
            this.LineSeparator = useLineSeparator ? "true" : "false";

            return this;
        }

        /// <summary>
        /// Sets the amount of space between header/footer and the page content.
        /// </summary>
        /// <param name="distanceFromContent">amount of space in pt</param>
        /// <returns>config object</returns>
        public HeaderSettings SetContentSpacing(double distanceFromContent)
        {
            this.ContentSpacing = distanceFromContent.ToString("0.##", CultureInfo.InvariantCulture);

            return this;
        }

        /// <summary>
        /// Sets the URL for the HTML document to use as a header/footer. The text are ignored in this case.
        /// </summary>
        /// <param name="htmlUri">URI for the document</param>
        /// <returns>config object</returns>
        public HeaderSettings SetHtmlContent(string htmlUri)
        {
            this.HtmlUrl = htmlUri;

            return this;
        }

        /// <summary>
        /// Sets up the supplied object config.
        /// </summary>
        /// <param name="config">config object pointer</param>
        internal void SetUpObjectConfig(IntPtr config)
        {
            if (this.FontSize != null)
            {
                PechkinStatic.SetObjectSetting(config, String.Format("{0}.fontSize", this.settingPrefix), this.FontSize);
            }
            if (this.FontName != null)
            {
                PechkinStatic.SetObjectSetting(config, String.Format("{0}.fontName", this.settingPrefix), this.FontName);
            }
            if (this.LeftText != null)
            {
                PechkinStatic.SetObjectSetting(config, String.Format("{0}.left", this.settingPrefix), this.LeftText);
            }
            if (this.CenterText != null)
            {
                PechkinStatic.SetObjectSetting(config, String.Format("{0}.center", this.settingPrefix), this.CenterText);
            }
            if (this.RightText != null)
            {
                PechkinStatic.SetObjectSetting(config, String.Format("{0}.right", this.settingPrefix), this.RightText);
            }
            if (this.LineSeparator != null)
            {
                PechkinStatic.SetObjectSetting(config, String.Format("{0}.line", this.settingPrefix), this.LineSeparator);
            }
            if (this.ContentSpacing != null)
            {
                PechkinStatic.SetObjectSetting(config, String.Format("{0}.space", this.settingPrefix), this.ContentSpacing);
            }
            if (this.HtmlUrl != null)
            {
                PechkinStatic.SetObjectSetting(config, String.Format("{0}.htmlUrl", this.settingPrefix), this.HtmlUrl);
            }
        }
    }
}