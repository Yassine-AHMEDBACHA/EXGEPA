using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using Hyproc.Controls;

namespace Hyproc
{
    //: IModule
    public class Module
    {
        public int Priority
        {
            get { return 15; }
        }

        public void LoadModule()
        {
            var uIService = ServiceLocator.Resolve<IUIService>();
            var homeGroup = new Group()
            {
                Caption = "Hyproc"
            };

            homeGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Sync",
                LargeGlyph = IconProvider.ManageDatasource,
                Action = () =>
                {
                    var viewModel = new DbSyncViewModel();
                    var view = new DbSyncView();
                    view.DataContext = viewModel;
                    var page = new Page(viewModel, view);
                    uIService.AddPage(page);
                }
            });

            uIService.AddGroupToHomePage(homeGroup);
        }
    }
}
