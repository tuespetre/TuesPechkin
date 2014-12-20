using System;
using System.Runtime.InteropServices;

namespace TuesPechkin
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    public delegate void VoidCallback(IntPtr converter);
}