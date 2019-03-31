using CORESI.Data;
using System.Collections.Generic;

namespace CORESI.DataAccess.Core.SqlTools
{
    internal static class FieldSqlConverter
    {
        public static string GetSqlColumnName(this Field field)
        {
            return "[" + field.Name + "]";
        }



        public static string GetSqlParameterName(this Field field)
        {
            return "@" + field.Name;
        }

        public static string GetSqlColumnDefaultValue(this Field field)
        {
            if (string.IsNullOrEmpty(field.SqldefaultColumValue))
                return null;
            if (field.IsIdentity)
            {
                return field.SqldefaultColumValue;
            }
            return " default " + field.SqldefaultColumValue;
        }

        private static string IsSqlNullable(this Field field)
        {
            if (!field.IsNullable || field.IsIdentity || field.IsPrimeryKey)
                return "NOT NULL";
            return "NULL";
        }



        public static string GetColumnDefinition(this Field field)
        {
            List<string> scriptParts = new List<string>();
            scriptParts.Add(field.GetSqlColumnName());
            scriptParts.Add(field.Type.GetDBType(field.IsLong));
            scriptParts.Add(field.GetSqlColumnDefaultValue());
            scriptParts.Add(field.IsSqlNullable());
            string script = string.Join(" ", scriptParts);
            return script;
        }

        public static string GetColumnDefForHistoTable(this Field field)
        {
            List<string> scriptParts = new List<string>();
            scriptParts.Add(field.GetSqlColumnName());
            scriptParts.Add(field.Type.GetDBType(field.IsLong));
            scriptParts.Add(field.IsSqlNullable());
            string script = string.Join(" ", scriptParts);
            return script;
        }
    }
}
