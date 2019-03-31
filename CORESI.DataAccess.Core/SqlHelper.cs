using CORESI.Data;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CORESI.DataAccess.Core
{
    public class SqlHelper
    {

        public static bool insertRawRows<T>(IEnumerable<T> instances, string tableName = null) where T : RowId
        {
            bool succes = false;

            if (tableName == null)
            {
                tableName = QueryBuilder.GetTableName(typeof(T));
            }
            tableName = $"[{tableName}]";
            if ((instances == null) || (!instances.Any()))
                return succes;

            DataTable dataTable = new DataTable(tableName);

            List<Field> fields = PropertiesExtractor.ExtractFields(typeof(T));
            fields.ForEach(field =>
            {
                DataColumn column = new DataColumn()
                {
                    ColumnName = field.Name,
                    DataType = field.IsReference ? typeof(int) : field.PropertyInfo.PropertyType
                };
                dataTable.Columns.Add(column);
            });

            List<Field> referenceFields = fields.Where(x => x.IsReference).ToList();
            List<Field> simpleFields = fields.Except(referenceFields).ToList();
            foreach (T instance in instances)
            {
                DataRow row = dataTable.NewRow();
                referenceFields.ForEach(referenceField =>
                {
                    if (referenceField.GetValue(instance) is RowId value)
                        row[referenceField.Name] = value.Id;
                });
                simpleFields.ForEach(simpleField =>
                {
                    row[simpleField.Name] = simpleField.GetValue(instance);
                });
                dataTable.Rows.Add(row);
            };
            dataTable.AcceptChanges();
            using (IDbConnection connection = DBConnectionFactory.GetDbConnection(DBConnectionFactory.ConnectionString))
            {
                SqlConnection sqconnection = new SqlConnection(connection.ConnectionString);
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqconnection))
                {
                    bulkCopy.BatchSize = 100;
                    bulkCopy.DestinationTableName = tableName;
                    sqconnection.Open();
                    fields.ForEach(field =>
                    {
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(field.Name, field.Name));
                    });
                    connection.Open();
                    bulkCopy.WriteToServer(dataTable);
                    succes = true;
                }
            }

            return succes;
        }


        public static bool InsertBulk<T>(IEnumerable<T> instances, string tableName = null) where T : Row
        {
            bool succes = false;

            if (tableName == null)
            {
                tableName = QueryBuilder.GetTableName(typeof(T));
            }
            tableName = $"[{tableName}]";
            if ((instances == null) || (!instances.Any()))
                return succes;

            DataTable dataTable = new DataTable(tableName);

            List<Field> fields = PropertiesExtractor.ExtractFields(typeof(T));
            fields.ForEach(field =>
                {
                    DataColumn column = new DataColumn()
                    {
                        ColumnName = field.Name,
                        DataType = field.IsReference ? typeof(int) : field.PropertyInfo.PropertyType
                    };
                    dataTable.Columns.Add(column);
                });

            List<Field> referenceFields = fields.Where(x => x.IsReference).ToList();
            List<Field> simpleFields = fields.Except(referenceFields).ToList();
            foreach (T instance in instances)
            {
                instance.Session = GenericDALBase.Session;
                DataRow row = dataTable.NewRow();
                referenceFields.ForEach(referenceField =>
                    {
                        if (referenceField.GetValue(instance) is RowId value)
                            row[referenceField.Name] = value.Id;
                    });
                simpleFields.ForEach(simpleField =>
                {
                    row[simpleField.Name] = simpleField.GetValue(instance);
                });
                dataTable.Rows.Add(row);
            };
            dataTable.AcceptChanges();
            using (IDbConnection connection = DBConnectionFactory.GetDbConnection(DBConnectionFactory.ConnectionString))
            {
                SqlConnection sqconnection = new SqlConnection(connection.ConnectionString);
                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(sqconnection))
                {
                    bulkCopy.BatchSize = 100;
                    bulkCopy.DestinationTableName = tableName;
                    sqconnection.Open();
                    fields.ForEach(field =>
                    {
                        bulkCopy.ColumnMappings.Add(new SqlBulkCopyColumnMapping(field.Name, field.Name));
                    });
                    connection.Open();
                    bulkCopy.WriteToServer(dataTable);
                    succes = true;
                }
            }

            return succes;

        }
    }
}
