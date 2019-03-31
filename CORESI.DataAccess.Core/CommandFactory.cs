using System.Data;
using System.Data.SqlClient;

namespace CORESI.DataAccess
{
    public class CommandFactory
    {

        public static IDbCommand GetDBCommand(string commandText, bool isStoredProcedure = false)
        {
            IDbCommand dBCommand;
            dBCommand = new SqlCommand
            {
                //if (DBConnectionFactory.ProviderType == DataProviderType.MSSQLSERVER)
                //{

                //}
                //else
                //{
                //    dBCommand = new OracleCommand();
                //}
                CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text,
                CommandText = commandText
            };
            return dBCommand;
        }

        public static IDbCommand GetDBCommand(IDbConnection dBConnection, string commandText, bool isStoredProcedure = false)
        {
            IDbCommand dBCommand = GetDBCommand(commandText, isStoredProcedure);
            dBCommand.Connection = dBConnection;
            return dBCommand;
        }

    }
}
