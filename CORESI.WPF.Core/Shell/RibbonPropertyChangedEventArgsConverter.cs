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
                {
                    if (args.NewValue is RibbonPage ribbonPage)
                    {
                        page = ribbonPage.DataContext;
                    }
                }
            }
            return page;
        }


    }

}
