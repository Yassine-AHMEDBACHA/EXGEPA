using System;
using System.Collections.Generic;
using System.Linq;
using CORESI.Data;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using CORESI.Security;
using EXGEPA.Model;

namespace EXGEPA.Core.Database
{
    public class DbInitializer : IDisposable
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public DbInitializer()
        {
            GenericDALBase.OpenSession("EXGEPA.Core.Database", "DataBasebuilder");
        }

        public static void JustInsertData<T>(IEnumerable<T> dataToInsert) where T : Row
        {
            SqlHelper.InsertBulk(dataToInsert);
            logger.DebugFormat("{0} Row inserted.", dataToInsert.Count());
        }

        public static List<T> InsertData<T>(IEnumerable<T> dataToInsert) where T : Row
        {
            JustInsertData(dataToInsert);
            return LoadAllTable<T>();
        }

        public static T InsertInstance<T>(T instance) where T : Row
        {
            IDataProvider<T> service = ServiceLocator.Resolve<IDataProvider<T>>();
            service.Add(instance);
            return instance;
        }

        public static IEnumerable<T> InsertInstances<T>(IEnumerable<T> instances) where T : Row
        {
            IDataProvider<T> service = ServiceLocator.Resolve<IDataProvider<T>>();
            foreach (T instance in instances)
            {
                service.Add(instance);
            }

            return instances;
        }

        public static List<T> LoadAllTable<T>()
        {
            IDataProvider<T> service = ServiceLocator.Resolve<IDataProvider<T>>();
            return service.SelectAll().ToList();
        }

        public void AddSettings()
        {
            IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            parameterProvider.TrySetOrAdd("ItemLabelType", "RegionCode_ItemCode_LocalCode");
            parameterProvider.TrySetOrAdd("LocalCurrency", "DZD");
            parameterProvider.TrySetOrAdd("Theme", "Office2010Silver");
            parameterProvider.TrySetOrAdd("AllowInventFileLoading", false);
            parameterProvider.TrySetOrAdd("InventFileArchive", @"C:\SQLIMMO\Archive fichiers inventaires");
            parameterProvider.TrySetOrAdd("UseAndroidDevice", true);
            parameterProvider.TrySetOrAdd("ItemKeyStyle", "6C");
            parameterProvider.TrySetOrAdd("UseWinUIMessageStyle", true);
            parameterProvider.TrySetOrAdd("IsItemKeyReadOnlyAtCreation", false);
            parameterProvider.TrySetOrAdd($"{typeof(Currency).Name}KeyLength", 3);
            parameterProvider.TrySetOrAdd($"{typeof(GeneralAccount).Name}KeyLength", 6);
            parameterProvider.TrySetOrAdd($"{typeof(Reference).Name}KeyLength", 6);
            parameterProvider.TrySetOrAdd($"{typeof(ReferenceType).Name}KeyLength", 2);
            parameterProvider.TrySetOrAdd($"{typeof(AnalyticalAccount).Name}KeyLength", 6);
            parameterProvider.TrySetOrAdd($"{typeof(AnalyticalAccountType).Name}KeyLength", 2);
            parameterProvider.TrySetOrAdd($"{typeof(Provider).Name}KeyLength", 6);
            parameterProvider.TrySetOrAdd($"{typeof(InputSheet).Name}KeyLength", 6);
            parameterProvider.TrySetOrAdd($"{typeof(TransferOrder).Name}KeyLength", 6);
            parameterProvider.TrySetOrAdd($"{typeof(Project).Name}KeyLength", 4);
            parameterProvider.TrySetOrAdd($"{typeof(Invoice).Name}KeyLength", 4);
            parameterProvider.TrySetOrAdd("ItemInvestismentMinAmount", 30000);
            parameterProvider.TrySetOrAdd("PicturesDirectory", @"C:\SQLIMMO\Images");
            parameterProvider.TrySetOrAdd("LogoFileName", "logo.jpg");
            parameterProvider.TrySetOrAdd("DepartmentName", "MOYEN GENEREAUX");
            parameterProvider.TrySetOrAdd("DirectionName", "Direction générale");
        }

        public void SetInitialRights()
        {
            List<Type> tables = QueryBuilder.GetMappedTypes();
            IDataProvider<Role> roleService = ServiceLocator.Resolve<IDataProvider<Role>>();
            Role role = new Role()
            {
                Key = "Administrateur"
            };
            InsertInstance(role);
            List<Operation> operations = InsertData(RightManager.Transcoder.Values.Distinct().Select(v => new Operation { Key = v }));
            List<Resource> resources = InsertData(tables.OrderBy(x => x.Name).Select(x => new Resource { Key = x.Name }));
            IEnumerable<Ability> ablilities = resources.SelectMany(resource => operations.Select(operation => new Ability
            {
                HasAccess = true,
                Resource = resource,
                Operation = operation,
                Role = role
            }));

            InsertData(ablilities);
            InsertInstance(new Operator()
            {
                Key = "$",
                Password = new StringCryptor().Crypte("$"),
                Name = "Administrator",
                Role = role
            });
        }

        public void Dispose()
        {
            GenericDALBase.CloseSession();
        }


    }
}
