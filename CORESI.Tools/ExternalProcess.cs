using System;
using System.Diagnostics;
using System.IO;

namespace CORESI.Tools
{
    public class ExternalProcess 
    {
        public static void StartProcess(string appName)
        {
            var path = Path.Combine(Environment.CurrentDirectory, appName);
            if (File.Exists(path))
            {
                var p = new Process();
                p.StartInfo = new ProcessStartInfo(path);
                p.Start();
                return;
            }

            throw new FileNotFoundException($"impossible de trouver le fichier : {path}");
        }
    }
}
