// <copyright file="ExternalProcess.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System;
    using System.Diagnostics;
    using System.IO;

    public class ExternalProcess
    {
        public static void StartProcess(string appName)
        {
            string path = Path.Combine(Environment.CurrentDirectory, appName);
            if (File.Exists(path))
            {
                Process p = new Process
                {
                    StartInfo = new ProcessStartInfo(path)
                };
                p.Start();
                return;
            }

            throw new FileNotFoundException($"impossible de trouver le fichier : {path}");
        }
    }
}
