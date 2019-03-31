using System.Windows;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Core;
using CORESI.IoC;
using CORESI.Tools;
using DevExpress.Data;

namespace CORESI.WPF.Core.Shell
{
    public partial class Shell : DXRibbonWindow
    {
        protected readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public Shell()
        {
            logger.Info("Start loading Shell");
            DXSplashScreen.Show<CORESI.WPF.Core.SplashScreen>();
            this.Loaded += Shell_Loaded;
            InitializeComponent();
            logger.Debug("Resolving Shell View Model");
            this.DataContext = ServiceLocator.Resolve<ShellViewModel>();
            var applicationID = CurrentEnvirenement.ApplicationName;
            ShellHelper.TryCreateShortcut(applicationID, applicationID);
        }

        void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            DXSplashScreen.Close();
            this.Loaded -= Shell_Loaded;
            logger.Info("Shell loading Done");
        }
    }
}
