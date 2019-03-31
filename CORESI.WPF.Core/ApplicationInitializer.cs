using CORESI.Data;
using CORESI.IoC;
using DevExpress.Xpf.Core;
using DevExpress.Xpf.Editors;
using System;
using System.Windows;
using System.Windows.Input;

namespace CORESI.WPF.Core
{
    internal static class ApplicationInitializer
    {
        static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static string CurrentTheme { get; private set; }

        static object locker = new object();
        internal static void SetTheme(string themeName)
        {
            Action setThemeAction = () =>
            {
                lock (locker)
                {
                    ApplicationThemeHelper.ApplicationThemeName = themeName;
                    CurrentTheme = themeName;
                    logger.DebugFormat("Application Theme : {0}", ApplicationThemeHelper.ApplicationThemeName);
                }
            };

            if (Application.Current != null)
            {
                Application.Current.Dispatcher.BeginInvoke(setThemeAction);
            }
            else
            {
                setThemeAction();
            }
        }

        [Obsolete]
        internal static void Initialize()
        {
            DXGridDataController.DisableThreadingProblemsDetection = true;
            EventManager.RegisterClassHandler(typeof(TextEdit), TextEdit.KeyDownEvent, new KeyEventHandler(TextBox_KeyDown));
            EventManager.RegisterClassHandler(typeof(CheckEdit), CheckEdit.KeyDownEvent, new KeyEventHandler(CheckBox_KeyDown));
        }

        internal static void SetTheme()
        {
            var parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            SetTheme(parameterProvider.GetValue<string>("Theme", Theme.Office2010SilverName));
        }

        static void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            var acceptReturn = (bool)sender.GetType().GetProperty("AcceptsReturn").GetValue(sender, null);
            if (e.Key == Key.Enter & acceptReturn == false) MoveToNextUIElement(e);
        }

        static void CheckBox_KeyDown(object sender, KeyEventArgs e)
        {
            MoveToNextUIElement(e);
            if (e.Handled == true)
            {
                CheckEdit cb = (CheckEdit)sender;
                cb.IsChecked = !cb.IsChecked;
            }
        }

        static void MoveToNextUIElement(KeyEventArgs e)
        {
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next;
            TraversalRequest request = new TraversalRequest(focusDirection);
            UIElement elementWithFocus = Keyboard.FocusedElement as UIElement;
            if (elementWithFocus != null)
            {
                if (elementWithFocus.MoveFocus(request)) e.Handled = true;
            }
        }
    }
}
