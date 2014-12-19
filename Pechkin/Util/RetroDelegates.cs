using System;

namespace TuesPechkin.Util
{
    internal delegate TResult Func<TResult>();
    internal delegate TResult Func<T, TResult>(T t);
    internal delegate TResult Func<T1, T2, TResult>(T1 t1, T2 t2);
    internal delegate TResult Func<T1, T2, T3, TResult>(T1 t1, T2 t2, T3 t3);
    internal delegate void Action();
    internal delegate void Action<T1, T2>(T1 t1, T2 t2);
}
