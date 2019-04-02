using CORESI.Data;
using CORESI.IoC;
using System;
using System.ComponentModel.Composition;
using System.Linq;

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

        public Session CurrentSession { get; private set; }


        public void CloseSession()
        {
            string query = "Update Sessions set [CloseDate] = GetDate() where id =" + this.CurrentSession.Id.ToString();
            _DbFacade.ExecuteNonQuery(query);
        }

        public Session OpenSession(string applicationLogin = "Unknown", string applicationName = null)
        {
            if (string.IsNullOrEmpty(applicationName))
            {
                applicationName = AppDomain.CurrentDomain.FriendlyName;
            }
            this.CurrentSession = new Session()
            {
                ApplicationName = applicationName,
                ApplicationLogin = applicationLogin,
                HostName = Environment.MachineName,
                UserName = Environment.UserName
            };
            System.Collections.Generic.List<Field> fields = PropertiesExtractor.ExtractFields(typeof(Session)).Where(f => !(f.IsIdentity || f.IsAutomatique)).ToList();
            string query = QueryBuilder.GetInsertQuery(typeof(Session), fields);
            System.Collections.Generic.List<System.Data.IDataParameter> parameters = DbParameterFactory.BuildParametersFromTypeOfInstance(CurrentSession, fields);

            System.Data.IDbCommand dBCommand = CommandFactory.GetDBCommand(query, false);

            parameters.ForEach(parameter => dBCommand.Parameters.Add(parameter));


            CurrentSession.Id = _DbFacade.ExecuteScalaire<int>(dBCommand);
            GenericDALBase.Session = CurrentSession;

            return CurrentSession;

        }

        public void SetApplicationName(string name)
        {

            string query = "Update Sessions set [ApplicationName] = '" + name + "' where id =" + this.CurrentSession.Id.ToString();
            _DbFacade.ExecuteNonQuery(query);
        }
    }
}
