using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Hyproc.Core
{
    public class DataBaseContent
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static void test()
        {
            var service = ServiceLocator.Resolve<IDataProvider<GeneralAccount>>();
            var slave = service.SelectAll();
            var master = service.SelectAll();

            master.Take(5).ToList().ForEach(x =>
            {
                x.Caption = "master update";
                //  ((int)x.SysVersion)++;
            });

            //master.Skip(7).Take(2).ForEach(x => { x.IsDeleted = true; x.SysVersion++; }
            //);
            //master.Take(1).ForEach(x =>
            //{
            //    x.Account = "mas";
            //    x.Caption = "master Add";
            //    x.Id = master.Count;
            //});
            //   var result1 = Comparator<string, GeneralAccount>.Compare(master, slave, (x) => x.Account);
        }

        public static string BackupDataBase(IDbFacade dbfacade = null)
        {
            var path = ServiceLocator.Resolve<IParameterProvider>().GetValue<string>("SyncShare", Environment.CurrentDirectory) + @"\";
            if (dbfacade == null)
            {
                dbfacade = ServiceLocator.Resolve<IDbFacade>();
                path += "local_";
            }
            else
            {
                path += "Remote_";
            }
            var dbName = dbfacade.ExecuteScalaire<string>("SELECT db_name()");
            path += dbName + "_" + DateTime.Now.ToString("dd-MM-yyyy_HH-mm-ss") + ".bak";
            string query = "BACKUP DATABASE " + dbName + " TO DISK = '" + path + "' WITH FORMAT, NAME ='" + dbName + "'";
            dbfacade.ExecuteNonQuery(query);
            return path;
        }

        public static int RestorDataBase(string path, IDbFacade dBFacade = null)
        {
            if (dBFacade == null)
            {
                dBFacade = ServiceLocator.Resolve<IDbFacade>();
            }
            var dbName = dBFacade.ExecuteScalaire<string>("SELECT db_name()");


            var query = " ALTER DATABASE " + dbName + " SET SINGLE_USER WITH ROLLBACK IMMEDIATE ";

            query += " restore database " + dbName + " from disk = '";
            query += path;
            query += "' with replace ";

            query += " ALTER DATABASE " + dbName + " SET MULTI_USER";
            logger.Debug("Running query : " + query);
            var tempfile = @"Temp.bak";
            File.Copy(path, tempfile, true);
            return dBFacade.ExecuteNonQuery(query, false, "master");
        }



        internal static List<IAnlayzer> GetListOfAnalyzer()
        {
            List<IAnlayzer> list = new List<IAnlayzer>
            {
                new TableAnalyzer<string, Operator>((instance) => instance.Key),
                new TableAnalyzer<string, Person>((instance) => instance.Key),
                new TableAnalyzer<string, GeneralAccount>((instance) => instance.Key),
                new TableAnalyzer<string, AnalyticalAccount>((instance) => instance.Key),
                new TableAnalyzer<string, Site>((instance) => instance.Code),
                new TableAnalyzer<int, Building>((instance) => instance.Id),
                new TableAnalyzer<int, Level>((instance) => instance.Id),
                new TableAnalyzer<string, Office>((instance) => instance.Code),
                new TableAnalyzer<string, Reference>((instance) => instance.Key),
                new TableAnalyzer<string, Invoice>((instance) => instance.Key),
                new TableAnalyzer<string, Item>((instance) => instance.Key),
                new TableAnalyzer<string, InventoryRow>((instance) => instance.Key)
            };

            return list;
        }


        public IDbFacade DbFacade { get; set; }
        public DataBaseContent(string connectionString)
        {
            var dbFacade = ServiceLocator.Resolve<IDbFacade>();
            DbFacade = dbFacade.ChangeDB(connectionString);

        }
        public Dictionary<string, Type> GetDbTables()
        {
            var dictionary = new Dictionary<string, Type>();
            dictionary.Add("items", typeof(Item));
            return dictionary;
        }



    }
}
