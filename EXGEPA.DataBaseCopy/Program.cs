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
            Item i = new Item();

            var select = GetSelectQuery<GeneralAccount>();

            //var types = QueryBuilder.GetMappedTypes();
            //var baseProperties = typeof(Row).GetProperties().Select(p=>p.Name).ToList();
            //var tables = types.ToDictionary(x => x, x => x.GetProperties().Where(p => !baseProperties.Contains(p.Name)).ToList());


           



        }

      
        public static string GetSelectQuery<T>()where T:KeyRow
        {
            var fields = GetFields<T>().Where(x => x != null).ToList() ;
            return GetSelectQuery<T>(fields);
        }

        public static string GetSelectQuery<T>(List<Field> fields)
        {
            var type = typeof(T);
            string tableName = GetTableName(type);
            string query = "Insert into "+tableName+" ("+string.Join(",",fields.Select(f=>f.Name))+",session_id)";
            query += " SELECT ";
            int i = 1;
            foreach (var field in fields)
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
            var tableName = type.Name;
            if (!tableName.EndsWith("s"))
            {
                tableName = tableName + "s";
            }
            return tableName;
        }

        public static List<Field> GetFields<T>() where T : KeyRow
        {
            var types = QueryBuilder.GetMappedTypes();
            var baseProperties = typeof(KeyRow).GetProperties().Select(p => p.Name.ToLower()).ToList();
            baseProperties.Add("SmallDescription".ToLower());
            baseProperties.Add("ProposeToReformCertificate".ToLower());
            return typeof(T).GetProperties().Where(p => !baseProperties.Contains(p.Name.ToLower())).Select(f => f.PropertyToField()).ToList();
        }
    }
}
