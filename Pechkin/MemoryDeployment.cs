using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TuesPechkin
{
    [Serializable]
    public abstract class MemoryDeployment : IDeployment
    {
        public virtual string Path
        {
            get 
            {
                if (!deployed) 
                { 
                    path = physical.Path;

                    var basePath = System.IO.Path.GetDirectoryName(path);

                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }

                    if (!File.Exists(path)) 
                    { 
                        WriteStreamToFile(path, GetStream());
                    }

                    deployed = true;
                }

                return path;
            }
        }

        public MemoryDeployment(IDeployment physical)
        {
            if (physical == null)
            {
                throw new ArgumentException("physical");
            }

            this.physical = physical;
        }

        protected IDeployment physical;

        protected abstract Stream GetStream();

        private bool deployed;

        private string path;

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
