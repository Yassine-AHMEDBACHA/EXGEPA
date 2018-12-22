using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORESI.DataAccess
{
    public class CommandFactory
    {

        public static IDbCommand GetDBCommand(string commandText, bool isStoredProcedure = false)
        {
            IDbCommand dBCommand;
            dBCommand = new SqlCommand();
            //if (DBConnectionFactory.ProviderType == DataProviderType.MSSQLSERVER)
            //{
               
            //}
            //else
            //{
            //    dBCommand = new OracleCommand();
            //}
            dBCommand.CommandType = isStoredProcedure ? CommandType.StoredProcedure : CommandType.Text;
            dBCommand.CommandText = commandText;
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
