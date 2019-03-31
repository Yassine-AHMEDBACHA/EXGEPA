using CORESI.Data;
using CORESI.IoC;
using System.Collections.Generic;

namespace CORESI.DataAccess.Core.SqlTools
{
    public class ArchivableTableScriptGeneratorcs
    {
        string HistoTableName { get; set; }
        string TableName { get; set; }
        private string UpdateTriggerName { get; set; }
        private string DeleteTriggerName { get; set; }
        public IDbFacade DBFacade { get; private set; }

        public ArchivableTableScriptGeneratorcs(string tableName, string histoTableName)
        {
            this.TableName = tableName;
            this.HistoTableName = histoTableName;
            this.UpdateTriggerName = this.TableName + "_UpdateTrigger";
            this.DeleteTriggerName = this.TableName + "_DeleteTrigger";
            this.DBFacade = ServiceLocator.Resolve<IDbFacade>();
        }

        public string GetTriggerScripts()
        {
            var scriptParts = new List<string>();

            var query = this.GetUpdateTriggerDropper();
            DBFacade.ExecuteNonQuery(query);
            scriptParts.Add(query);

            query = this.GetDeleteTriggerDropper();
            DBFacade.ExecuteNonQuery(query);
            scriptParts.Add(query);

            query = this.GetScriptForUpdateTrigger();
            DBFacade.ExecuteNonQuery(query);
            scriptParts.Add(query);

            query = this.GetScriptForDeleteTrigger();
            DBFacade.ExecuteNonQuery(query);
            scriptParts.Add(query);
            var script = string.Join("\nGo\n", scriptParts);

            return script;
        }

        private string GetScriptForUpdateTrigger()
        {

            var script = "Create trigger [" + UpdateTriggerName + "] on [dbo].[" + this.TableName + "]  after Update \nas \nbegin \nSET NOCOUNT ON;";
            script += "insert into [dbo].[" + this.HistoTableName + "] select * from deleted; ";
            script += "update [dbo].[" + this.TableName + "]  set VersionDate = GetDate() from deleted where [dbo].[" + this.TableName + "].Id = deleted.Id end";
            return script;
        }

        private string GetUpdateTriggerDropper()
        {
            var script = ScriptGenerator.GetScriptHeader("Trigger : " + this.UpdateTriggerName);
            script += this.GetScriptToDropTrigger(this.UpdateTriggerName);
            return script;
        }

        private string GetScriptForDeleteTrigger()
        {

            var script = "Create trigger [" + DeleteTriggerName + "] on [dbo].[" + this.TableName + "]  after delete \nas \nBEGIN \nSET NOCOUNT ON;\n";
            script += "insert into [dbo].[" + this.HistoTableName + "] select * from deleted END";
            return script;
        }

        private string GetDeleteTriggerDropper()
        {
            var script = ScriptGenerator.GetScriptHeader("Trigger : " + this.DeleteTriggerName);
            script += this.GetScriptToDropTrigger(this.DeleteTriggerName);
            return script;
        }

        private string GetScriptToDropTrigger(string triggerName)
        {
            var query = "IF OBJECT_ID ('" + triggerName + "', 'TR') IS NOT NULL\nBEGIN \n\tDROP TRIGGER " + triggerName + " \nEND\n";
            return query;
        }

    }
}

