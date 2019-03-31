// <copyright file="RunTimeHelper.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System.Reflection;

    public class RunTimeHelper
    {
        public static string GetApplicationName()
        {
            return Assembly.GetEntryAssembly().GetName().Name;
        }
    }
}
