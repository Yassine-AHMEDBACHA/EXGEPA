using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CORESI.IoC
{
    public static class ServiceLocator
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static CompositionContainer CompositionContainer { get; set; }

        private static AggregateCatalog aggregateCatalog = new AggregateCatalog();

        static ServiceLocator()
        {
            try
            {
                string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
                aggregateCatalog.Catalogs.Add(new DirectoryCatalog(path, "CORESI*.dll"));
                Assembly entryAssembly = Assembly.GetEntryAssembly();
                MethodInfo entryPoint = entryAssembly?.EntryPoint;
                string nameSpace = entryPoint?.DeclaringType.Namespace ?? "EXGEPA";
                string filter = nameSpace.Split('.').First() + "*.dll";
                DirectoryCatalog directoryCatalog = new DirectoryCatalog(path, filter);
                aggregateCatalog.Catalogs.Add(directoryCatalog);
                CompositionContainer = new CompositionContainer(aggregateCatalog, true);
                logger.Debug("MefResolver is ready to use");
            }
            catch (Exception ex)
            {
                ManageExceptionLoader(ex);
                throw ex;
            }
        }

        public static void AddCatalogue(string path, string filter = "*.dll")
        {
            DirectoryCatalog directoryCatalog = new DirectoryCatalog(path, filter);
            aggregateCatalog.Catalogs.Add(directoryCatalog);
        }

        public static void Resolve<T>(out T instance) where T : class
        {
            instance = Resolve<T>();
        }

        public static object Resolve(Type type)
        {
            return CompositionContainer.GetExports(type, null, null).FirstOrDefault()?.Value;
        }

        public static T Resolve<T>()
        {
            T result = default(T);
            try
            {
                result = CompositionContainer.GetExportedValue<T>();
            }
            catch (Exception ex)
            {
                logger.Debug($"Resolve Faild : [{typeof(T).FullName}]");
                ManageExceptionLoader(ex);
            }
            return result;
        }

        public static void ComposeParts(params object[] attributedParts)
        {
            try
            {
                CompositionContainer.ComposeParts(attributedParts);
            }
            catch (Exception ex)
            {
                logger.Fatal(ex);
            }
        }

        public static IEnumerable<T> ResolveMany<T>()
        {
            IEnumerable<T> result = default(IEnumerable<T>);
            try
            {
                result = CompositionContainer.GetExportedValues<T>();
            }
            catch (Exception ex)
            {
                logger.Debug("Resolve many Faild : " + typeof(T).FullName);
                ManageExceptionLoader(ex);
            }

            return result;
        }

        public static T GetDefault<T>() where T : IPriority
        {
            return GetPriorizedInstance<T>();
        }

        public static void GetDefault<T>(out T instance) where T : IPriority
        {
            instance = GetDefault<T>();
        }

        public static T GetPriorizedInstance<T>() where T : IPriority
        {
            IEnumerable<T> availableInstances = ResolveMany<T>();
            T instance = availableInstances.OrderBy(x => x.Priority).Last();
            return instance;
        }

        private static void ManageExceptionLoader(Exception ex)
        {
            if (ex is ReflectionTypeLoadException)
            {
                ReflectionTypeLoadException typeLoadException = ex as ReflectionTypeLoadException;
                Exception[] loaderExceptions = typeLoadException.LoaderExceptions;

                loaderExceptions.GroupBy(e => e.Message).Select(x => x.First()).ToList().ForEach(exception =>
                {
                    logger.Error(exception);

                });
            }
            else
            {
                logger.Error(ex);
            }
        }
    }
}
