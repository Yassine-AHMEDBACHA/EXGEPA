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
            IUIService uIService = ServiceLocator.Resolve<IUIService>();
            Group homeGroup = new Group()
            {
                Caption = "Hyproc"
            };

            homeGroup.Commands.Add(new RibbonButton()
            {
                Caption = "Sync",
                LargeGlyph = IconProvider.ManageDatasource,
                Action = () =>
                {
                    DbSyncViewModel viewModel = new DbSyncViewModel();
                    DbSyncView view = new DbSyncView
                    {
                        DataContext = viewModel
                    };
                    Page page = new Page(viewModel, view);
                    uIService.AddPage(page);
                }
            });

            uIService.AddGroupToHomePage(homeGroup);
        }
    }
}
