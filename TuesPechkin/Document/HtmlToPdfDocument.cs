using System;
using System.Collections.Generic;
namespace TuesPechkin
{
    [Serializable]
    public class HtmlToPdfDocument : IDocument
    {
        public HtmlToPdfDocument()
        {
            this.Objects = new List<ObjectSettings>();
        }

        public HtmlToPdfDocument(string html) : this()
        {
            this.Objects.Add(new ObjectSettings { HtmlText = html });
        }

        public List<ObjectSettings> Objects { get; private set; }

        public IEnumerable<IObject> GetObjects()
        {
            return Objects.ToArray();
        }

        private GlobalSettings global = new GlobalSettings();

        public GlobalSettings GlobalSettings
        {
            get
            {
                return this.global;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                this.global = value;
            }
        }

        public static implicit operator HtmlToPdfDocument(string html)
        {
            return new HtmlToPdfDocument(html);
        }
    }
}