using System;

namespace FAD3
{
    internal static class extension
    {
        public static void With<T>(this T obj, Action<T> a)
        {
            a(obj);
        }
    }
}