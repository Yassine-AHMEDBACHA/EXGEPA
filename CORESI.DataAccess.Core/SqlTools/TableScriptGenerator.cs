using CORESI.Data;
using CORESI.IoC;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CORESI.DataAccess.Core.SqlTools
{
    public class TableScriptGenerator : ScriptGenerator
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TableScriptGenerator(Type type) : base(type)
        {
            this.DbFacade = ServiceLocator.Resolve<IDbFacade>();
        }

        public string GetTableCreationScript()
        {
            string script = ScriptGenerator.GetScriptHeader("Table : " + this.TableName);
            script += this.GetScriptToDeleteFK();
            script += this.GetScriptToCreateTable();
            CreatTable(this.TableName, script);
            if (this.IsArchivable)
            {
                string query = ScriptGenerator.GetScriptHeader("Table : " + this.HistoTableName);
                query += this.GetScriptToCreateHistoTable();
                this.CreatTable(this.HistoTableName, query);
                DbFacade.ExecuteNonQuery(query);
                script += query;
                ArchivableTableScriptGeneratorcs archivabletableScripGenerator = new ArchivableTableScriptGeneratorcs(this.TableName, this.HistoTableName);
                script += archivabletableScripGenerator.GetTriggerScripts();
            }
            return script;
        }

        void CreatTable(string tableName, string sqlScript)
        {
            logger.InfoFormat("Creating table : {0}", tableName);
            DbFacade.ExecuteNonQuery(sqlScript);
        }


        protected string GetScripteToDropTable(string table)
        {

            return "\nIF OBJECT_ID('[dbo].[" + table + "]') IS NOT NULL \nBEGIN \n\tDROP TABLE [dbo].[" + table + "]\nEND\n\n";
        }


        protected string GetScriptToCreateTable()
        {
            string fieldsSeparator = ",\n";

            string script = this.GetScripteToDropTable(this.TableName);
            script += "SET ANSI_NULLS ON\nSET QUOTED_IDENTIFIER ON\n";
            script += "CREATE TABLE [dbo].[" + this.TableName + "](\n";
            List<string> subQuery = new List<string>();

            subQuery.Add(string.Join(fieldsSeparator, this.Fields.Select(f => f.GetColumnDefinition())));
            subQuery.Add("[VersionDate] [DateTime]  DEFAULT(getdate()) NOT NULL");

            // subQuery.Add(this.GetForeignKey());
            subQuery.Add(GetConstraint());
            script += string.Join(fieldsSeparator, subQuery.Where(s => !string.IsNullOrEmpty(s)));
            script += ")";
            return script;
        }

        public string GetConstraint()
        {
            List<string> subqueries = new List<string>();
            subqueries.Add(this.GetUniqueContraint());
            subqueries.Add(this.GetPrimaryKey());
            return string.Join(",\n", subqueries.Where(x => x != null));
        }

        protected string GetPrimaryKey()
        {
            List<Field> primaryKeyFields = this.Fields.Where(f => f.IsPrimeryKey).ToList();
            if (primaryKeyFields.Count == 0)
                return null;
            string query = "CONSTRAINT[PK_" + this.TableName + "] PRIMARY KEY CLUSTERED(" + string.Join(",", primaryKeyFields.Select(f => f.GetSqlColumnName()));
            query += " ASC )\nWITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) \nON[PRIMARY]";
            return query;
        }

        public string GetUniqueContraint()
        {
            List<Field> uniqueFields = this.Fields.Where(f => f.IsUnique && !f.IsPrimeryKey).ToList();
            if (uniqueFields.Count == 0)
                return null;
            IEnumerable<string> subqueries = uniqueFields.Select(f => "CONSTRAINT UK_" + this.TableName + "_" + f.Name + " UNIQUE(" + f.GetSqlColumnName() + ")");
            return string.Join(",\n", subqueries);
        }
        protected string GetScriptToCreateHistoTable()
        {
            string fieldsSeparator = ",\n";
            string script = this.GetScripteToDropTable(this.HistoTableName);
            script += "SET ANSI_NULLS ON\nSET QUOTED_IDENTIFIER ON\n";
            script += "CREATE TABLE [dbo].[" + this.HistoTableName + "](\n";
            script += string.Join(fieldsSeparator, this.Fields.Select(f => f.GetColumnDefForHistoTable()));
            script += fieldsSeparator + "[VersionDate] [DateTime] NOT NULL) \n";
            return script;
        }

    }
}
