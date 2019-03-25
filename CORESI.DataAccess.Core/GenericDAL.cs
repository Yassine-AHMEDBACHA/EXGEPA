﻿
using CORESI.Data;
using CORESI.IoC;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using CORESI.Tools;

namespace CORESI.DataAccess.Core
{
    public class GenericDAL<T> : IGenericDAL<T> where T : Row
    {
        private static string SelectCommand;
        private static string LoadAllCommand;
        private static string DeleteCommand;
        private static string DeleteAllCommand;
        private static string UpdateCommand;
        private static string InsertCommand;
        private static string LoadHistoCommand;

        private static string RowCountCommand;
        public IDbFacade DbFacade { get; set; }
        public List<Expression> Setters { get; set; }
        public List<Field> Fields { get; set; }
        public List<Field> ReferenceField { get; set; }
        public List<Field> BasicField { get; set; }

        public GenericDAL()
        {
            Fields = PropertiesExtractor.ExtractAllFields(typeof(T)).ToList();
            ReferenceField = Fields.Where(f => f.IsReference).ToList();
            BasicField = Fields.Except(ReferenceField).ToList();
            LoadHistoCommand = QueryBuilder.GetAllHistoricalSelectQuery<T>(this.Fields);
            SelectCommand = QueryBuilder.GetSelectQuery(typeof(T), Fields);
            UpdateCommand = QueryBuilder.GetUpdateQuery(typeof(T), Fields);
            LoadAllCommand = QueryBuilder.GetSelectQuery(typeof(T), Fields, false);
            DeleteCommand = QueryBuilder.GetDeleteQuery(typeof(T));
            DeleteAllCommand = QueryBuilder.GetDeleteAllQuery(typeof(T));
            InsertCommand = QueryBuilder.GetInsertQuery(typeof(T), Fields);
            RowCountCommand = QueryBuilder.GetRowCountQuery(typeof(T));
            this.DbFacade = ServiceLocator.Resolve<IDbFacade>();
        }

        public void FillInstance<V>(V instance) where V : T
        {
            var sqlCommand = CommandFactory.GetDBCommand(SelectCommand, false);
            sqlCommand.Parameters.Add(DbParameterFactory.BuildIdParameter(instance.Id));
            DbFacade.Fill<V>(sqlCommand, instance, this.GenericMapper);
        }

        public V GetById<V>(int id) where V : T
        {
            V result = Activator.CreateInstance<V>();
            result.Id = id;
            FillInstance(result);
            return result;
        }

        public T GetById(int id)
        {
            return this.GetById<T>(id);
        }

        public IList<T> SelectAll()
        {
            var listOfInstances = new List<T>();
            Fill<T>(listOfInstances);
            return listOfInstances;
        }



        public void Fill<V>(IList<V> ListOfInstances) where V : T
        {
            var dBCommand = CommandFactory.GetDBCommand(SelectCommand, false);
            var sqlParameter = DbParameterFactory.BuildIdParameter();
            dBCommand.Parameters.Add(sqlParameter);
            DbFacade.Fill<V>(dBCommand, ListOfInstances, GenericMapper<V>);
        }

        public int Add(T instance)
        {
            instance.Session = GenericDALBase.Session;
            var dBCommand = CommandFactory.GetDBCommand(InsertCommand, false);
            var parameters = DbParameterFactory.BuildParametersFromTypeOfInstance(instance, this.Fields);

            parameters.ForEach(parameter =>
                {
                    dBCommand.Parameters.Add(parameter);
                });

            instance.Id = DbFacade.ExecuteScalaire<int>(dBCommand);
            return instance.Id;
        }

        public int Delete(T instance)
        {
            instance.Session = GenericDALBase.Session;
            var dBCommand = CommandFactory.GetDBCommand(DeleteCommand, false);
            var parameter = DbParameterFactory.BuildIdParameter(instance.Id);
            dBCommand.Parameters.Add(parameter);
            parameter = DbParameterFactory.BuildIdParameter(instance.Session.Id, "@Session_Id");
            dBCommand.Parameters.Add(parameter);
            int result = DbFacade.ExecuteNonQuery(dBCommand);
            return result;
        }

        public int DeleteAll()
        {
            int result = -1;
            result = DbFacade.ExecuteNonQuery(DeleteAllCommand);
            return result;
        }

        public int Update(T instance)
        {
            instance.Session = GenericDALBase.Session;
            var dBCommand = CommandFactory.GetDBCommand(UpdateCommand);
            dBCommand.Parameters.Add(DbParameterFactory.BuildIdParameter(instance.Id));
            var parameters = DbParameterFactory.BuildParametersFromTypeOfInstance<T>(instance, this.Fields);
            parameters.ForEach(parameter =>
            {
                dBCommand.Parameters.Add(parameter);
            });
            return DbFacade.ExecuteNonQuery(dBCommand);
        }



        public bool HasRows()
        {
            return GetRowsCount() > 0;
        }

        public int GetRowsCount()
        {
            return DbFacade.ExecuteScalaire<Int32>(RowCountCommand);
        }

        private V GenericMapper<V>(IDataReader dataReader) where V : T
        {
            V instance = Activator.CreateInstance<V>();
            GenericMapper(dataReader, instance);
            return instance;
        }

        private void GenericMapper(IDataReader dataReader, T instance)
        {
            var row = new object[dataReader.FieldCount];
            dataReader.GetValues(row);
            GenericMapper(instance, this.Fields, row);
        }

        public static void GenericMapper(T instance, List<Field> fields, object[] tableRow)
        {
            instance.Id = (int)tableRow[0];
            foreach (var field in fields)
            {
                int index = field.Ordinal;
                object value;
                if (tableRow[index] == DBNull.Value)
                {
                    if (field.IsNullable)
                    {
                        value = null;
                    }
                    else
                    {
                        throw new ArgumentNullException(field.PropertyInfo.Name + " must have value or column is null in the dataReader");
                    }
                }
                else
                {
                    value = tableRow[index];
                    if (field.IsReference)
                    {
                        var newInstance = Activator.CreateInstance(field.PropertyInfo.PropertyType, null) as IRowId;
                        newInstance.Id = (int)value;
                        value = newInstance;
                    }
                }
                field.PropertyInfo.SetValue(instance, value, null);
            }
        }

        public IList<T> LoadAllTable()
        {
            var listOfInstances = new List<T>();
            var dBCommand = CommandFactory.GetDBCommand(LoadAllCommand, false);
            var sqlParameter = DbParameterFactory.BuildIdParameter();
            dBCommand.Parameters.Add(sqlParameter);
            DbFacade.Fill<T>(dBCommand, listOfInstances, GenericMapper<T>);
            return listOfInstances;
        }

        public IList<T> GetHistoric(int id)
        {
            var listOfInstances = new List<T>();
            var dbCommand = CommandFactory.GetDBCommand(LoadHistoCommand);
            var sqlParameter = DbParameterFactory.BuildIdParameter(id);
            dbCommand.Parameters.Add(sqlParameter);
            DbFacade.Fill<T>(dbCommand, listOfInstances, GenericMapper<T>);

            foreach (var referenceField in this.ReferenceField)
            {
                dynamic dataService = ServiceLocator.Resolve(typeof(IDataProvider<>).MakeGenericType(referenceField.Type));
                foreach (var instance in listOfInstances)
                {
                    var idToFind = ((IRowId)referenceField.GetValue(instance))?.Id;
                    if (idToFind != null)
                    {
                        var result = dataService?.GetById(idToFind.Value);
                        referenceField.SetValue(instance, (object)result);
                    }
                }
            }

            return listOfInstances;
        }
    }
}