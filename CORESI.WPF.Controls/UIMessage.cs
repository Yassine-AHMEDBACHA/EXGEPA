using CORESI.Data;
using CORESI.IoC;
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
        public UIMessage()
        {
            var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            var useWinUIMessageStyle = parameterProvider.GetValue("UseWinUIMessageStyle", true);
            if(useWinUIMessageStyle)
            {
                this.ShowMessage = WinUIMessageBox.Show;
            }
            else
            {
                this.ShowMessage = DXMessageBox.Show;
            }
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

    }
}
