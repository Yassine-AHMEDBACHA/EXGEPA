// <copyright file="IEnumerableExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
            }
        }

        public static void ParallelForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            Parallel.ForEach(enumerable, item => action(item));
        }

        public static IEnumerable<T> ApplyOnAll<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action(item);
                yield return item;
            }
        }

        public static ObservableCollection<T> ToObservable<T>(this IEnumerable<T> enumerable)
        {
            return new ObservableCollection<T>(enumerable);
        }

        public static LinkedList<T> ToLinkedList<T>(this IEnumerable<T> enumerable)
        {
            return new LinkedList<T>(enumerable);
        }

        public static Queue<T> ToQueue<T>(this IEnumerable<T> enumerable)
        {
            return new Queue<T>(enumerable);
        }

        public static IEnumerable<T> ElementsBetween<T>(this IEnumerable<T> enumerable, T first, T last)
        {
            bool startOut = false;
            if (enumerable.Any())
            {
                foreach (var item in enumerable)
                {
                    if (!startOut && item.Equals(first))
                    {
                        startOut = true;
                    }

                    if (startOut)
                    {
                        yield return item;
                    }

                    if (item.Equals(last))
                    {
                        break;
                    }
                }
            }
        }
    }
}