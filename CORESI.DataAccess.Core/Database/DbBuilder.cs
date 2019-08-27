using System.Collections.Generic;
using System.Linq;
using CORESI.Data;
using CORESI.Data.Tools;
using CORESI.DataAccess.Core.SqlTools;
using CORESI.IoC;
using CORESI.Tools;

namespace CORESI.DataAccess.Core.Database
{
    public static class DbBuilder
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static List<string> GetAllDataBase(IDbFacade dbFacade)
        {
            string query = "SELECT [name],[dbid],[sid] ,[mode] ,[status] ,[status2] ,[crdate] ,[reserved] ,[category] ,[cmptlevel],[filename],[version]FROM [master].[sys].[sysdatabases]";

            return dbFacade.ExecuteReader(query, (dr) => $"{dr["name"]}");
        }

        public static bool CheckIfExist(string dbName, IDbFacade dbFacade = null)
        {
            if (dbFacade == null)
            {
                dbFacade = ServiceLocator.Resolve<IDbFacade>();
            }

            return GetAllDataBase(dbFacade).Any(db => db.Equals(dbName));
        }

        public static bool CreateDatabase(string dbName, IDbFacade dbFacade = null)
        {
            if (dbFacade == null)
            {
                dbFacade = ServiceLocator.Resolve<IDbFacade>();
            }

            string query = $"create database {dbName}";
            return dbFacade.ExecuteNonQuery(query, false, "Master") == 1;
        }

        public static void BuildNewDatabase()
        {
            IDbFacade dbFacade = ServiceLocator.Resolve<IDbFacade>();
            using (ScoopLogger scoop = new ScoopLogger("Creating DataBase", logger))
            {
                logger.Info("Checking Database");
                string target = dbFacade.GetCurrentDatabase();
                if (!CheckIfExist(target, dbFacade))
                {
                    CreateDatabase(target, dbFacade);
                }

                logger.Info("Loading Model");

                List<System.Type> tables = QueryBuilder.GetMappedTypes();
                List<string> triggersCleaner = new List<string>();
                List<string> triggersQuery = new List<string>();
                List<TableScriptGenerator> list = tables.Select(t => new TableScriptGenerator(t)).ToList();
                list.ForEach(t => t.GetTableCreationScript());

                list.ForEach(t =>
                {
                    string script = t.GetForeignKey();
                    if (script.IsValid())
                    {
                        logger.InfoFormat("Adding foreign keys for table : {0}", t.TableName);
                        dbFacade.ExecuteNonQuery(script);
                    }
                });
            }
        }
    }
}
