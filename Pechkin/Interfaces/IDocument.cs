using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    public interface IDocument : ISettings
    {
        IEnumerable<IObject> GetObjects();
    }
}
