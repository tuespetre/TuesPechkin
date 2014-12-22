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
                if (!deployed) 
                { 
                    if (!Directory.Exists(physical.Path))
                    {
                        Directory.CreateDirectory(physical.Path);
                    }

                    foreach (var nameAndContents in GetContents())
                    {
                        var filename = System.IO.Path.Combine(
                            physical.Path,
                            nameAndContents.Key);

                        if (!File.Exists(filename))
                        {
                            WriteStreamToFile(filename, nameAndContents.Value);
                        }
                    }

                    deployed = true;
                }

                return physical.Path;
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
