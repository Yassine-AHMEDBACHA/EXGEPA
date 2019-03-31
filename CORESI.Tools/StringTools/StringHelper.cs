// <copyright file="StringHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools.StringTools
{
    using System;

    public static class StringHelper
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
    }
}
