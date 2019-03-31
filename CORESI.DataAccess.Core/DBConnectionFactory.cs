using System.Configuration;
using System.Data;

namespace CORESI.DataAccess
{
    public class DBConnectionFactory
    {
        static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static DataProviderType ProviderType = DataProviderType.MSSQLSERVER;

        public static string GetConnectionStrings(string config = "DataBaseContext")
        {
            var connectionStrings = ConfigurationManager.ConnectionStrings;
            log.Debug("ConnectionString = " + connectionStrings[config].ConnectionString);
            return connectionStrings[config].ConnectionString;
        }

        public static string ConnectionString = GetConnectionStrings();


        public static IDbConnection GetDbConnection(string connectionString)
        {
            IDbConnection dBConnection;
            dBConnection = new System.Data.SqlClient.SqlConnection(connectionString);
            return dBConnection;
        }

    }

    public enum DataProviderType
    {
        MSSQLSERVER = 1,
        ORACLE
    }
}
