using CORESI.Data;
using CORESI.IoC;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;

namespace CORESI.DataAccess.Core
{
    [Export(typeof(ISessionManager))]
    public class SessionManager : ISessionManager
    {

        private IDbFacade _DbFacade;


        public SessionManager()
        {
            this._DbFacade = ServiceLocator.Resolve<IDbFacade>();
        }

        Session currentSession;
        public Session CurrentSession => this.currentSession;
        

        public void CloseSession()
        {
            var query = "Update Sessions set [CloseDate] = GetDate() where id =" + this.currentSession.Id.ToString();
            _DbFacade.ExecuteNonQuery(query);
        }

        public Session OpenSession(string applicationLogin = "Unknown", string applicationName = null)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                applicationName = AppDomain.CurrentDomain.FriendlyName;
            }
            this.currentSession = new Session()
            {
                ApplicationName = applicationName,
                ApplicationLogin = applicationLogin,
                HostName = Environment.MachineName,
                UserName = Environment.UserName
            };
            var fields = PropertiesExtractor.ExtractFields(typeof(Session)).Where(f => !(f.IsIdentity || f.IsAutomatique)).ToList();
            var query = QueryBuilder.GetInsertQuery(typeof(Session), fields);
            var parameters = DbParameterFactory.BuildParametersFromTypeOfInstance(currentSession, fields);

            var dBCommand = CommandFactory.GetDBCommand(query, false);

            parameters.ForEach(parameter => dBCommand.Parameters.Add(parameter));


            currentSession.Id = _DbFacade.ExecuteScalaire<int>(dBCommand);
            GenericDALBase.Session = currentSession;

            return currentSession;

        }

        public void SetApplicationName(string name)
        {
          
            var query = "Update Sessions set [ApplicationName] = '" + name + "' where id =" + this.currentSession.Id.ToString();
            _DbFacade.ExecuteNonQuery(query);
        }
    }
}
