using CORESI.Tools;
using DevExpress.Xpf.Core;

namespace CORESI.WPF.Core
{
    public class DXControlInitializer
    {
        public static void LoadComponent<T>() where T : System.Windows.DependencyObject, new()
        {
            DependancyManager.RunTypeInitializers<T>();
            ThemeManager.SetThemeName(new T(), ApplicationInitializer.CurrentTheme);
        }
    }
}
