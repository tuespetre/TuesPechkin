using System;
using System.Runtime.InteropServices;

namespace TuesPechkin.Util
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void IntCallback(IntPtr converter, int str);
}