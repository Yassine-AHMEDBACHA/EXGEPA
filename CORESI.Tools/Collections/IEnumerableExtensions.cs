// <copyright file="IEnumerableExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools.Collections
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
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
    }
}