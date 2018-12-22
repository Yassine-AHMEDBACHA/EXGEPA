using CORESI.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Data;

namespace CORESI.DataAccess
{
    [Export(typeof(IDbFacade)), PartCreationPolicy(CreationPolicy.Shared)]
    public class DBFacade : IDbFacade
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public string ConnectionString { get; private set; }

        public DBFacade()
        {
            ConnectionString = DBConnectionFactory.ConnectionString;
        }

        public DBFacade(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public T ExecuteScalaire<T>(IDbCommand dBCommand)
        {
            var result = default(T);
            using (var dBconnection = DBConnectionFactory.GetDbConnection(this.ConnectionString))
            {
                dBCommand.Connection = dBconnection;
                dBconnection.Open();
                var returnedObject = dBCommand.ExecuteScalar();
                dBconnection.Close();
                result = (T)returnedObject;
            }

            return result;
        }

        public int ExecuteNonQuery(IDbCommand dBCommand, string dataBaseName = null)
        {
            int count = 0;
            using (var dBconnection = DBConnectionFactory.GetDbConnection(this.ConnectionString))
            {
                dBCommand.Connection = dBconnection;
                dBconnection.Open();
                if (dataBaseName != null)
                    dBconnection.ChangeDatabase(dataBaseName);
                count = dBCommand.ExecuteNonQuery();
            }
            return count;
        }

        public void Fill<T>(IDbCommand dBCommand, IList<T> ListOfInstances, Func<IDataReader, T> mapper)
        {
            using (var dBconnection = DBConnectionFactory.GetDbConnection(this.ConnectionString))
            {
                dBCommand.Connection = dBconnection;
                dBconnection.Open();
                using (var dataReader = dBCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var instance = mapper(dataReader);
                        ListOfInstances.Add(instance);
                    }
                }
            }
        }

        public void Fill<T>(IDbCommand dBCommand, T instance, Action<IDataReader, T> mapper)
        {
            using (var dBconnection = DBConnectionFactory.GetDbConnection(this.ConnectionString))
            {
                dBCommand.Connection = dBconnection;
                dBconnection.Open();
                using (IDataReader dataReader = dBCommand.ExecuteReader())
                {
                    if (dataReader.Read())
                    {
                        mapper(dataReader, instance);
                    }
                }
            }
        }

        public List<T> ExecuteReader<T>(string command, Func<IDataReader, T> mapper)
        {
            List<T> list = new List<T>();
            var dBCommand = CommandFactory.GetDBCommand(command, false);
            Fill<T>(dBCommand, list, mapper);
            return list;
        }

        public T ExecuteScalaire<T>(string command)
        {
            IDbCommand dBcommand = CommandFactory.GetDBCommand(command, false);
            return this.ExecuteScalaire<T>(dBcommand);
        }

        public int ExecuteNonQuery(string command, bool isStoredProcedure = false, string dataBaseName = null)
        {
            var dBCommand = CommandFactory.GetDBCommand(command, false);
            return this.ExecuteNonQuery(dBCommand, dataBaseName);
        }

        public void ExecuteAction(string command, Action<IDataReader> action)
        {
            using (var dBconnection = DBConnectionFactory.GetDbConnection(this.ConnectionString))
            {
                var dBCommand = CommandFactory.GetDBCommand(dBconnection, command, false);
                dBconnection.Open();
                using (IDataReader dataReader = dBCommand.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        action(dataReader);
                    }
                }
            }
        }

        public bool TestConnection(string connectionString = null)
        {
            bool result = false;
            if (string.IsNullOrEmpty(connectionString))
                throw new Exception("La chaine de connexion est vide");
            try
            {
                using (var dBconnection = DBConnectionFactory.GetDbConnection(connectionString))
                {
                    dBconnection.Open();
                    result = dBconnection.State == ConnectionState.Open;
                    dBconnection.Close();
                }

            }
            catch (Exception ex)
            {
                logger.Error(ex);
            }
            return result;
        }

        public IDbFacade ChangeDB(string connectionString)
        {
            return new DBFacade(connectionString);
        }

        public string GetCurrentDatabase()
        {
            return DBConnectionFactory.GetDbConnection(this.ConnectionString).Database;
        }
    }
}