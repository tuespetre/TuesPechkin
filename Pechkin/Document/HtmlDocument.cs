using System;
using System.Collections.Generic;
namespace TuesPechkin
{
    [Serializable]
    public class HtmlDocument
    {
        public HtmlDocument()
        {
            this.Objects = new List<ObjectSettings>();
        }

        public HtmlDocument(string html) : this()
        {
            this.Objects.Add(new ObjectSettings { HtmlText = html });
        }

        private GlobalSettings global = new GlobalSettings();

        public List<ObjectSettings> Objects { get; private set; }

        //public TableOfContentsSettings TableOfContents { get; set; }

        public GlobalSettings GlobalSettings
        {
            get
            {
                return this.global;
            }
            set
            {
                DEPRECAAAAATED.BestAintBeNull(value); 
                this.global = value;
            }
        }

        public static implicit operator HtmlDocument(string html)
        {
            return new HtmlDocument(html);
        }
    }
}