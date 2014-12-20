using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    public interface IObject : ISettings
    {
        byte[] GetData();
    }
}
