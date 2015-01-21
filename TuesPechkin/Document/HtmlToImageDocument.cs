using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    public class HtmlToImageDocument : IDocument
    {
        [WkhtmltoxSetting("screenHeight")]
        public double ScreenHeight { get; set; }

        [WkhtmltoxSetting("screenWidth")]
        public double? ScreenWidth { get; set; }

        [WkhtmltoxSetting("quality")]
        public double? Quality { get; set; }

        [WkhtmltoxSetting("fmt")]
        public string Format { get; set; }

        [WkhtmltoxSetting("out")]
        public string Out { get; set; }

        [WkhtmltoxSetting("in")]
        public string In { get; set; }

        [WkhtmltoxSetting("transparent")]
        public bool? Transparent { get; set; }

        public CropSettings CropSettings
        {
            get
            {
                return this.crop;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.crop = value;
            }
        }

        public LoadSettings LoadSettings
        {
            get
            {
                return this.load;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.load = value;
            }
        }

        public WebSettings WebSettings
        {
            get
            {
                return this.web;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.web = value;
            }
        }

        public IEnumerable<IObject> GetObjects()
        {
            return new IObject[0];
        }

        private CropSettings crop = new CropSettings();

        private LoadSettings load = new LoadSettings();

        private WebSettings web = new WebSettings();
    }
}
