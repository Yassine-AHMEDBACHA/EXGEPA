// <copyright file="StringExtensions.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System;

    public static class StringExtensions
    {
        public static string Align(this string str, int lenght, string ch, AdditionnalCharPosition additionnalCharPosition = AdditionnalCharPosition.Left)
        {
            if (str.Length > lenght)
            {
                return str.Substring(0, lenght);
            }

            Func<string> func;
            if (additionnalCharPosition == AdditionnalCharPosition.Left)
            {
                func = () => ch + str;
            }
            else
            {
                func = () => str + ch;
            }

            while (str.Length < lenght)
            {
                str = func();
            }

            return str;
        }

        public static string ToAlignedString(this object obj, int lenght, string ch, AdditionnalCharPosition additionnalCharPosition = AdditionnalCharPosition.Left)
        {
            return obj.ToString().Align(lenght, ch, additionnalCharPosition);
        }

        public static bool ContainsString(this string str, string value)
        {
            return str.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
