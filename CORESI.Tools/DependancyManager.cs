using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CORESI.Tools
{
    public static class DependancyManager
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static Dictionary<string, Assembly> AlreadyLoadedAssemblies { get; set; }
        static DependancyManager()
        {
            AlreadyLoadedAssemblies = new Dictionary<string, Assembly>();
        }


        public static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {

            logger.Debug("Resolving : " + args.RequestingAssembly);
            // var strTempAssmbPath = @"C:\Program Files (x86)\DevExpress 15.2\Components\Bin\Framework\" + args.Name + ".dll";
            var strTempAssmbPath = args.Name + ".dll";
            Assembly resolvedAssembly = null;
            if (File.Exists(strTempAssmbPath))
            {

                logger.Info(strTempAssmbPath);
                resolvedAssembly = Assembly.LoadFrom(strTempAssmbPath);

            }
            else
            {
                logger.Error("Failed to resolve : " + args.Name + "Requested by" + args.RequestingAssembly.FullName);
            }
            return resolvedAssembly;
        }
        static object locker = new object();
        public static void RunTypeInitializers(Assembly assembly)
        {
            lock (locker)
            {
                logger.Debug("preloading " + assembly.FullName);
                if (AlreadyLoadedAssemblies.ContainsKey(assembly.FullName))
                {
                    return;
                }
                AlreadyLoadedAssemblies[assembly.FullName] = assembly;
            }

            using (var scooplogger = new ScoopLogger("Loading " + assembly.FullName, logger))
            {
                foreach (var type in assembly.GetExportedTypes())
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
