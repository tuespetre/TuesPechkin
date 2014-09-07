using System;
using System.Collections.Generic;
using System.Text;

namespace TuesPechkin
{
    internal static class Assert
    {
        public static void BestAintBeNull(object value)
        {
            if (value == null)
            {
                throw new ArgumentNullException("value");
            }
        }
    }
}
