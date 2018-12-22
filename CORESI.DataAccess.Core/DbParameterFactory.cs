using CORESI.Data;
using CORESI.DataAccess.Core.SqlTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CORESI.DataAccess.Core
{
    public class DbParameterFactory
    {

        public static IDataParameter BuildIdParameter(int? value = null, string fieldName = null)
        {
            SqlParameter sqlParameter = new SqlParameter()
            {
                IsNullable = true,
                ParameterName = fieldName ?? "@Id",
                DbType = DbType.Int32,
                SqlValue = DBNull.Value,
            };
            if (value != null)
            {
                sqlParameter.Value = value;
            }
            return sqlParameter;
        }



        public static List<IDataParameter> BuildParametersFromTypeOfInstance<T>(T instance, List<Field> fields)
        {
            var ListOfSqlParamsters = new List<IDataParameter>();
            var targetFiels = fields.Where(x => !x.IsAutomatique).ToList();
            foreach (var field in targetFiels)
            {
                SqlParameter sqlParameter = new SqlParameter();
                sqlParameter.ParameterName = field.GetSqlParameterName();
                if (field.Type == typeof(DateTime))
                    sqlParameter.SqlDbType = SqlDbType.Date;
                var value = field.GetValue(instance);
                if (value == null)
                {
                    if (field.IsNullable)
                    {
                        sqlParameter.SqlValue = DBNull.Value;
                    }
                    else
                    {
                        throw new ArgumentNullException("The " + field.PropertyInfo.Name + " in type " + typeof(T).Name + " must have value");
                    }
                }
                else
                {
                    if (field.IsReference)
                    {
                        sqlParameter.DbType = DbType.Int32;
                        sqlParameter.Value = ((RowId)value).Id;
                    }
                    else
                    {
                        sqlParameter.Value = value;
                    }
                }
                ListOfSqlParamsters.Add(sqlParameter);
            }
            return ListOfSqlParamsters;
        }
    }
}
