using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    public interface IDeployment
    {
        /// <summary>
        /// Represent a path to a folder that contains the wkhtmltox.dll 
        /// library and any dependencies it may have.
        /// </summary>
        string Path { get; }
    }
}
