using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Tools
{
    public static class EnumerableHelper
    {
        public static IEnumerable<T> ApplyOnAll<T>(this IEnumerable<T> enumerbale, Action<T> actionToDo)
        {
            foreach (var item in enumerbale)
            {
                actionToDo(item);
                yield return item;
            }
        }

        public static void ForEach<T>(this IEnumerable<T> enumerbale, Action<T> actionToDo)
        {
            foreach (var item in enumerbale)
            {
                actionToDo(item);
            }
        }
    }
}
