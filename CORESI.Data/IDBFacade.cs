using System;
using System.Collections.Generic;
using System.Data;
namespace CORESI.Data
{
    public interface IDbFacade
    {
        int ExecuteNonQuery(IDbCommand dBCommand, string dataBaseName = null);
        int ExecuteNonQuery(string command, bool isStoredProcedure = false, string dataBaseName = null);
        void ExecuteAction(string command, Action<System.Data.IDataReader> action);
        List<T> ExecuteReader<T>(string command, Func<System.Data.IDataReader, T> mapper);
        T ExecuteScalaire<T>(System.Data.IDbCommand dBCommand);
        T ExecuteScalaire<T>(string command);
        void Fill<T>(IDbCommand dBCommand, IList<T> ListOfInstances, Func<System.Data.IDataReader, T> mapper);
        void Fill<T>(IDbCommand dBCommand, T instance, Action<System.Data.IDataReader, T> mapper);
        bool TestConnection(string connectionString = null);
        IDbFacade ChangeDB(string connectionString);
        string GetCurrentDatabase();
    }
}
