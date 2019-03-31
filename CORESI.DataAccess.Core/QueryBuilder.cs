using CORESI.Data;
using CORESI.DataAccess.Core.SqlTools;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CORESI.DataAccess.Core
{
    public class QueryBuilder
    {
        public static string GetRowCountQuery(Type type)
        {
            string tableName = GetTableName(type);
            var query = "Select Count(1) From " + tableName;
            return query;
        }

        public static List<Type> GetMappedTypes()
        {
            var type = typeof(IRowId);
            var types = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => type.IsAssignableFrom(p) && !p.IsAbstract && !p.IsInterface).ToList();
            return types;
        }

        public static string GetInsertQuery(Type type, List<Field> allFields)
        {
            var fields = allFields.Where(x => !x.IsIdentity);
            string tableName = GetTableName(type);
            string query = "BEGIN TRANSACTION INSERT INTO [" + tableName + "] (";
            string values = ") VALUES (";
            foreach (var field in fields)
            {
                query = query + field.GetSqlColumnName() + ",";
                values = values + field.GetSqlParameterName() + ",";
            }

            query = query.TrimEnd(',');
            values = values.TrimEnd(',');
            query = query + values + ") DECLARE @Identity INT set @Identity = SCOPE_IDENTITY() select @Identity COMMIT TRANSACTION";
            return query;
        }

        public static string GetUpdateQuery(Type type, List<Field> allFields)
        {
            var fields = allFields.Where(x => !x.IsIdentity);
            string tableName = GetTableName(type);
            string query = "UPDATE [" + tableName + "] SET ";
            foreach (var field in fields)
            {
                query = query + field.GetSqlColumnName() + " = " + field.GetSqlParameterName() + " ,";
            }
            query = query.TrimEnd(',');
            query = query + "WHERE  [Id] = @Id";
            return $"BEGIN TRANSACTION  {query}  COMMIT TRANSACTION";
        }

        public static string GetSelectQuery(Type type, List<Field> fields, bool withoutId = false)
        {
            string tableName = GetTableName(type);
            var filterCreteria = withoutId ? "WHERE  ([Id] = @Id OR @Id IS NULL)" : string.Empty;
            string query = $"{GetSelectQuery(fields, tableName)} {filterCreteria}";
            return $"BEGIN TRANSACTION  {query}  COMMIT TRANSACTION";
        }

        private static string GetSelectQuery(List<Field> fields, string tableName)
        {
            var query = $" SELECT [Id],{string.Join(",", fields.Select(x => x.GetSqlColumnName()))} FROM [{tableName}]";
            return query;
        }

        public static string GetAllHistoricalSelectQuery<T>(List<Field> fields)
        {
            var activeTableName = GetTableName(typeof(T));
            var histoTableName = $"{activeTableName}_Histo";
            var filterCreteria = "WHERE  ([Id] = @Id)";
            var query = $"{GetSelectQuery(fields, activeTableName)} {filterCreteria} UNION {GetSelectQuery(fields, histoTableName)}{filterCreteria}";
            return $"BEGIN TRANSACTION  {query}  COMMIT TRANSACTION";
        }

        public static string GetSelectQuery(Type type)
        {
            var fields = PropertiesExtractor.ExtractFields(type);
            return GetSelectQuery(type, fields);
        }

        public static string GetDeleteQuery(Type type)
        {
            string tableName = GetTableName(type);
            string query = "BEGIN TRANSACTION Update [" + tableName + "] set IsDeleted = 1, Session_Id = @Session_Id WHERE ([Id] = @Id);  DELETE FROM [" + tableName + "] WHERE ([Id] = @Id); COMMIT TRANSACTION";
            return query;
        }

        public static string GetDeleteAllQuery(Type type)
        {
            string tableName = GetTableName(type);
            string query = "BEGIN TRANSACTION  Update [" + tableName + "] set IsDeleted = 1; DELETE from [" + tableName + "] COMMIT TRANSACTION";
            return query;
        }

        public static string GetTableName(Type type)
        {
            var tableName = type.Name;
            if (!tableName.EndsWith("s"))
            {
                tableName = tableName + "s";
            }
            return tableName;
        }

        public static string GetCustomSession()
        {
            return string.Empty;
        }
    }
}
