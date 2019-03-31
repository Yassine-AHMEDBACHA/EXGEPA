using CORESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CORESI.DataAccess.Core.SqlTools
{
    public abstract class ScriptGenerator
    {
        protected readonly Type _type;
        public string TableName { get; private set; }
        public bool IsArchivable { get; private set; }
        public string HistoTableName { get; private set; }
        public List<Field> Fields { get; set; }
        public List<Field> ReferenceField { get; set; }
        public List<Field> BasicField { get; set; }
        public IDbFacade DbFacade { get; protected set; }
        public string GetForeignKey()
        {
            var foreignKey = Fields.Where(f => f.IsReference).ToList();
            var constaints = foreignKey.Select(x => "CONSTRAINT[FK_" + this.TableName + "_" + x.Name + "_" + x.Type.GetTableName() + "] FOREIGN KEY(" + x.GetSqlColumnName() + ") REFERENCES [" + x.Type.GetTableName() + "] ([Id])");
            var script = string.Join(";", constaints.Where(x => x != null).Select(c => " Alter Table [" + this.TableName + "] Add " + c));
            return script;
        }

        public string GetScriptToDeleteFK()
        {
            var query = "SELECT  RC.CONSTRAINT_NAME , KF.TABLE_NAME FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS RC JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KF ON RC.CONSTRAINT_NAME = KF.CONSTRAINT_NAME JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE KP ON RC.UNIQUE_CONSTRAINT_NAME = KP.CONSTRAINT_NAME where KP.TABLE_NAME = '" + this.TableName + "'";
            var subQueries = DbFacade.ExecuteReader(query, (r) => "ALTER TABLE [" + r["TABLE_NAME"].ToString() + "] DROP CONSTRAINT [" + r["CONSTRAINT_NAME"].ToString() + "];");
            return string.Join("\n", subQueries);
        }

        public ScriptGenerator(Type type)
        {
            this._type = type;
            this.TableName = this._type.GetTableName();
            this.Fields = type.GetAllFields();
            this.ReferenceField = Fields.Where(f => f.IsReference).ToList();
            this.BasicField = Fields.Except(ReferenceField).ToList();
            IsArchivable = typeof(ArchivableRow).IsAssignableFrom(type);
            this.HistoTableName = this.GetArchiveTableName();
        }

        private string GetArchiveTableName()
        {
            return this.TableName + "_Histo";
        }
        internal static string GetScriptHeader(string title)
        {
            var script = "\n/*---------------------------------------------------------------------------------------------*/\n";
            script += "--\t" + title + "\n";
            script += "/*---------------------------------------------------------------------------------------------*/\n";
            return script;
        }


    }
}