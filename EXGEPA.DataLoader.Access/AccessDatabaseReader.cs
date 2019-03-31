using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace EXGEPA.DataLoader.Access
{
    public class AccessDatabaseReader 
    {
        public static IList<T> SelectAll<T>(string filePath,string command,Func<IDataReader,T> mapper = null)
        {
            var listOfRows = new List<T>();
            using (var conn = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + "; Jet OLEDB:Database ")) 
            using (var cmd = new OleDbCommand(command, conn))
            {
                conn.Open();
                var Reader = cmd.ExecuteReader();
                if(Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        var instance = mapper(Reader);
                        listOfRows.Add(instance);
                    }
                }
            }

            return listOfRows;
        }
    }
}
