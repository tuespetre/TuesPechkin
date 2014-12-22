using System;
using System.Runtime.InteropServices;

namespace TuesPechkin
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void IntCallback(IntPtr converter, int str);
}