using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;

namespace EXGEPA.DataLoader.Access
{
    public class AccessDatabaseReader
    {
        public static IList<T> SelectAll<T>(string filePath, string command, Func<IDataReader, T> mapper = null)
        {
            List<T> listOfRows = new List<T>();
            using (OleDbConnection conn = new OleDbConnection(@"Provider = Microsoft.ACE.OLEDB.12.0; Data Source = " + filePath + "; Jet OLEDB:Database "))
            using (OleDbCommand cmd = new OleDbCommand(command, conn))
            {
                conn.Open();
                OleDbDataReader Reader = cmd.ExecuteReader();
                if (Reader.HasRows)
                {
                    while (Reader.Read())
                    {
                        T instance = mapper(Reader);
                        listOfRows.Add(instance);
                    }
                }
            }

            return listOfRows;
        }
    }
}
