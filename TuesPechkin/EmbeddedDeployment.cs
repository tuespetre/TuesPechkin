using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TuesPechkin
{
    [Serializable]
    public abstract class EmbeddedDeployment : IDeployment
    {
        public virtual string Path
        {
            get
            {
                var path = System.IO.Path.Combine(physical.Path, PathModifier ?? string.Empty);

                if (!deployed)
                {
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }

                    foreach (var nameAndContents in GetContents())
                    {
                        var filename = System.IO.Path.Combine(path, nameAndContents.Key);

                        if (!File.Exists(filename))
                        {
                            WriteStreamToFile(filename, nameAndContents.Value);
                        }
                    }

                    deployed = true;
                }

                // In 2.2.0 needs to return path
                return physical.Path;
            }
        }

        // Embedded deployments need to override this instead in 2.2.0
        protected virtual string PathModifier
        {
            get
            {
                return this.GetType().Assembly.GetName().Version.ToString();
            }
        }

        public EmbeddedDeployment(IDeployment physical)
        {
            if (physical == null)
            {
                throw new ArgumentException("physical");
            }

            this.physical = physical;
        }

        protected IDeployment physical;

        protected abstract IEnumerable<KeyValuePair<string, Stream>> GetContents();

        private bool deployed;

        private void WriteStreamToFile(string fileName, Stream stream)
        {
            if (!File.Exists(fileName))
            {
                var writeBuffer = new byte[8192];
                var writeLength = 0;

                using (var newFile = File.Open(fileName, FileMode.Create))
                {
                    while ((writeLength = stream.Read(writeBuffer, 0, writeBuffer.Length)) > 0)
                    {
                        newFile.Write(writeBuffer, 0, writeLength);
                    }
                }
            }
        }
    }
}
