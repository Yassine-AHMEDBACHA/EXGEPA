using CORESI.Data;
using CORESI.DataAccess.Core;
using System.Linq;
using EXGEPA.Model;
using System.Collections.Generic;
using System;

namespace EXGEPA.DataBaseCopy
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            _ = new Item();
            _ = GetSelectQuery<GeneralAccount>();

            //var types = QueryBuilder.GetMappedTypes();
            //var baseProperties = typeof(Row).GetProperties().Select(p=>p.Name).ToList();
            //var tables = types.ToDictionary(x => x, x => x.GetProperties().Where(p => !baseProperties.Contains(p.Name)).ToList());






        }


        public static string GetSelectQuery<T>() where T : KeyRow
        {
            List<Field> fields = GetFields<T>().Where(x => x != null).ToList();
            return GetSelectQuery<T>(fields);
        }

        public static string GetSelectQuery<T>(List<Field> fields)
        {
            Type type = typeof(T);
            string tableName = GetTableName(type);
            string query = "Insert into " + tableName + " (" + string.Join(",", fields.Select(f => f.Name)) + ",session_id)";
            query += " SELECT ";
            int i = 1;
            foreach (Field field in fields)
            {
                query = query + "[" + field.Name + "] ,";
                field.Ordinal = i;
                i++;
            }

            query = query.TrimEnd(',');
            query = query + ",1 FROM GIMMO..[" + tableName + "]";




            query += " ";
            return query;
        }

        public static string GetTableName(Type type)
        {
            string tableName = type.Name;
            if (!tableName.EndsWith("s"))
            {
                tableName += "s";
            }
            return tableName;
        }

        public static List<Field> GetFields<T>() where T : KeyRow
        {
            List<Type> types = QueryBuilder.GetMappedTypes();
            List<string> baseProperties = typeof(KeyRow).GetProperties().Select(p => p.Name.ToLower()).ToList();
            baseProperties.Add("SmallDescription".ToLower());
            baseProperties.Add("ProposeToReformCertificate".ToLower());
            return typeof(T).GetProperties().Where(p => !baseProperties.Contains(p.Name.ToLower())).Select(f => f.PropertyToField()).ToList();
        }
    }
}
