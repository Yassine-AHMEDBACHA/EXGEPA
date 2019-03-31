using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Permissions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using CORESI.Data;
using CORESI.IoC;
using CORESI.Security;
using CORESI.Tools;
using CORESI.WPF;
using log4net.Config;

namespace EXGEPA
{
    public static class Program
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [STAThread]
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public static void Main()
        {
            if (!CanStartNewApplicationInstance("EXGEPA"))
            {
                return;
            }

            AppDomain.CurrentDomain.UnhandledException += UnhandledException;
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
            XmlConfigurator.Configure();
            logger.Info("************************************************** Starting Application *********************************************************************");

            var uIService = ServiceLocator.Resolve<IUIService>();
            if (uIService == null)
                throw new ApplicationException("Mef Resolver was unable to resolve type IUIService, application must shut down");
            var result = uIService.ShowLoginWindow();
            if (result != null)
            {
                logger.Info("Loggin : " + result.Login + " ID : " + result.Id);
                var app = new Application
                {
                    //app.Startup += App_Startup;
                    MainWindow = uIService.CreateShell()
                };
                uIService.InitShellInformation(result);
                var sessionManager = ServiceLocator.Resolve<ISessionManager>();
                if (sessionManager.CurrentSession != null)
                {
                    sessionManager.SetApplicationName("EXGEPA");
                }
                else
                {
                    sessionManager.OpenSession(result.Login, "EXGEPA");
                }

                app.Exit += (s, e) => { sessionManager.CloseSession(); };
                var role = ServiceLocator.Resolve<IDataProvider<Role>>().SelectAll().First(x => x.Id == result.Role.Id);
                ServiceLocator.Resolve<RightManager>().Initialize(role);
                Task.Factory.StartNew(() =>
                {
                    var dbFacade = ServiceLocator.Resolve<IDbFacade>();
                    var CompanyName = ServiceLocator.Resolve<IParameterProvider>().GetAndSetIfMissing("CompanyName", "CORESI");
                    var exercice = dbFacade.ExecuteScalaire<string>("SELECT [KEY] FROM [AccountingPeriods] where Approved=0");
                    uIService.SetApplicationTitle(CompanyName + " - " + exercice);
                });
                var modules = ServiceLocator.ResolveMany<IModule>().OrderByDescending(module => module.Priority).ToList();
                modules.ForEach(module => module.LoadModule());
                app.Run(app.MainWindow);
                logger.Info("********************************************** Exiting Application **********************************************************************");
            }
        }

        private static bool CanStartNewApplicationInstance(string applicationName)
        {
            var mutex = new Mutex(true, applicationName, out bool isNewMutex);
            if (!isNewMutex)
            {
                MessageBox.Show("Another instance is already running.");
                return false;
            }
            GC.KeepAlive(mutex);

            return isNewMutex;
        }

        private static Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            string strTempAssmbPath = "DXAssemblies\\" + args.Name.Substring(0, args.Name.IndexOf(",")) + ".dll";
            if (File.Exists(strTempAssmbPath))
            {
                logger.Debug("Resolving assembly in " + strTempAssmbPath);
                var assembly = Assembly.LoadFrom(strTempAssmbPath);
                logger.Debug("Success for = " + strTempAssmbPath + " - State = " + assembly);
                return assembly;
            }

            logger.Error("Failed to resolve : " + args.Name + "Requested by" + args.RequestingAssembly.FullName);
            return null;
        }

        private static void UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {

            if (e.IsTerminating)
            {

                logger.Fatal("Fatal error", (Exception)e.ExceptionObject);
                CurrentEnvirenement.LogEnvirenementInfo();
            }
            else
            {
                logger.Error(e.ExceptionObject);
            }
        }
    }
}
