using CORESI.Data;
using System;
using System.Reflection;
using CORESI.IoC;

namespace CORESI.DataAccess.Core
{
    public static class GenericDALBase
    {
        public static Session Session { get; set; }

        public static Session CreateSession()
        {
            Session session = new Session()
            {
                ApplicationName = Assembly.GetExecutingAssembly().FullName,
                HostName = Environment.MachineName,
                UserName = Environment.UserName
            };
            return session;
        }

        public static void OpenSession(string applicationName, string applicationLogin)
        {
            ISessionManager sessionManager = ServiceLocator.Resolve<ISessionManager>();
            Session = sessionManager.OpenSession(applicationName, applicationLogin);
        }

        public static void CloseSession()
        {
            ISessionManager sessionManager = ServiceLocator.Resolve<ISessionManager>();
            sessionManager.CloseSession();
        }


    }
}