using DevExpress.Mvvm.UI;
using DevExpress.Xpf.Ribbon;

namespace CORESI.WPF.Core.Shell
{
    public class RibbonPropertyChangedEventArgsConverter : EventArgsConverterBase<RibbonPropertyChangedEventArgs>
    {

        protected override object Convert(object sender, RibbonPropertyChangedEventArgs args)
        {
            object page = null;
            if (args != null)
            {
                var ribbonPage = args.NewValue as RibbonPage;
                {
                    if (ribbonPage != null)
                    {
                        page = ribbonPage.DataContext;
                    }
                }
            }
            return page;
        }

      
    }

}
