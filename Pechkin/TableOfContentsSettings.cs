using System;

namespace TuesPechkin
{
  [Serializable]
    public class TableOfContentsSettings : ObjectSettings
    {
        public TableOfContentsSettings()
        {
            this.ProduceTableOfContents = true;
        }

        private string xslStyleSheet;
 
        [WkhtmltopdfSetting("isTableOfContent")]
        internal bool ProduceTableOfContents { get; set; } 

        [WkhtmltopdfSetting("toc.captionText")]
        public string CaptionText { get; set; } 

        [WkhtmltopdfSetting("toc.fontScale")]
        public double? FontScale { get; set; }

        [WkhtmltopdfSetting("toc.indentation")]
        public string Indentation { get; set; }

        [WkhtmltopdfSetting("toc.backLinks")]
        public bool ProduceBackLinks { get; set; }

        [WkhtmltopdfSetting("toc.forwardLinks")]
        public bool ProduceForwardLinks { get; set; }

        [WkhtmltopdfSetting("toc.useDottedLines")]
        public bool UseDottedLines { get; set; }

        [WkhtmltopdfSetting("tocXsl")]
        public string XslStyleSheet
        {
            get
            {
              return this.xslStyleSheet ?? new DefaultTOCStyleSheetDumper( this ).MakeDefaultTOCStyleSheetFile();
            }
            set
            {
                this.xslStyleSheet = value;
            }
        } 
    }
}