// <copyright file="DependancyManager.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace CORESI.Tools
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Runtime.CompilerServices;

    public static class DependancyManager
    {
        private static readonly log4net.ILog Logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private static readonly object Locker = new object();

        static DependancyManager()
        {
            AlreadyLoadedAssemblies = new Dictionary<string, Assembly>();
        }

        public static Dictionary<string, Assembly> AlreadyLoadedAssemblies { get; set; }

        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Logger.Debug("Resolving : " + args.RequestingAssembly);

            // var strTempAssmbPath = @"C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\" + args.Name + ".dll";
            string strTempAssmbPath = args.Name + ".dll";
            Assembly resolvedAssembly = null;
            if (File.Exists(strTempAssmbPath))
            {
                Logger.Info(strTempAssmbPath);
                resolvedAssembly = Assembly.LoadFrom(strTempAssmbPath);
            }
            else
            {
                Logger.Error("Failed to resolve : " + args.Name + "Requested by" + args.RequestingAssembly.FullName);
            }

            return resolvedAssembly;
        }

        public static void RunTypeInitializers(Assembly assembly)
        {
            lock (Locker)
            {
                Logger.Debug("preloading " + assembly.FullName);
                if (AlreadyLoadedAssemblies.ContainsKey(assembly.FullName))
                {
                    return;
                }

                AlreadyLoadedAssemblies[assembly.FullName] = assembly;
            }

            using (ScoopLogger scooplogger = new ScoopLogger("Loading " + assembly.FullName, Logger))
            {
                foreach (Type type in assembly.GetExportedTypes())
                {
                    RuntimeHelpers.RunClassConstructor(type.TypeHandle);
                }
            }
        }

        public static void RunTypeInitializers(Type type)
        {
            RunTypeInitializers(Assembly.GetAssembly(type));
        }

        public static void RunTypeInitializers<T>()
        {
            RunTypeInitializers(typeof(T));
        }
    }
}
