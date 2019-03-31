using CORESI.Data;
using CORESI.IoC;
using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.WindowsUI;
using System;
using System.ComponentModel.Composition;
using System.Windows;

namespace CORESI.WPF.Controls
{
    [Export(typeof(IUIMessage))]
    public class UIMessage : UIMessageBase
    {
        private NotificationService notificationService;
        public UIMessage()
        {
            IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            bool useWinUIMessageStyle = parameterProvider.GetValue("UseWinUIMessageStyle", true);
            if (useWinUIMessageStyle)
            {
                this.ShowMessage = WinUIMessageBox.Show;
            }
            else
            {
                this.ShowMessage = DXMessageBox.Show;
            }

            this.notificationService = new NotificationService
            {
                CustomNotificationPosition = NotificationPosition.BottomRight,
                CustomNotificationVisibleMaxCount = 3,
                CustomNotificationScreen = NotificationScreen.Primary,
                UseWin8NotificationsIfAvailable = true
            };
        }

        Func<string, string, MessageBoxButton, MessageBoxImage, MessageBoxResult> ShowMessage { get; set; }

        public override int Priority
        {
            get { return 10; }
        }
        protected override MessageBoxResult ShowUiMessageBox(string message, string title, MessageBoxButton buttons, MessageBoxImage image)
        {
            return ShowMessage(message, title, buttons, image);
        }

        public override void Notify(string message)
        {
            //this.notificationService.ApplicationId = Application.Current.MainWindow.Title;
            this.notificationService.CreateCustomNotification(message).ShowAsync();
        }

    }
}
