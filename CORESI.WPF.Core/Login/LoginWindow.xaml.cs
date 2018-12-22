using DevExpress.Xpf.Core;
using System.Windows;


namespace CORESI.WPF.Core.Login
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class LoginWindow : DXWindow
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public LoginWindow()
        {
            this.Loaded += Shell_Loaded;
        //    DXSplashScreen.Show<CORESI.WPF.Core.SplashScreen>();
            InitializeComponent();
        }

        void Shell_Loaded(object sender, RoutedEventArgs e)
        {
            logger.Info("Login window loaded");
        //    DXSplashScreen.Close();
            this.Loaded -= Shell_Loaded;
            ApplicationInitializer.SetTheme();
        }
    }
}
