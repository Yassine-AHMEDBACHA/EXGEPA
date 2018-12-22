using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DevExpress.Xpf.Ribbon;
using DevExpress.Xpf.Core;
using CORESI.IoC;


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
        }
        
        void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            DXSplashScreen.Close();
            this.Loaded -= Shell_Loaded;
            logger.Info("Shell loading Done");
        }
    }
}
