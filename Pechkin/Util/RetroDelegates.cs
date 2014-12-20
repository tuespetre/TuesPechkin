using System;

namespace TuesPechkin.Util
{
    public delegate TResult FuncShim<TResult>();
    internal delegate TResult FuncShim<T1, T2, TResult>(T1 t1, T2 t2);
    public delegate void ActionShim();
}
