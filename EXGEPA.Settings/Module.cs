using CORESI.WPF.Core;
using CORESI.WPF.Model;

namespace EXGEPA.Settings
{
    public sealed class Module : AModule
    {
        public override int Priority
        {
            get { return 7; }
        }
        public override void AddGroups()
        {
            Group setting = new Group();
            setting.AddCommand("Options", IconProvider.Properties, this.ShowSettingPage);
            UIService.AddGroupToHomePage(setting);
        }

        private void ShowSettingPage()
        {
            Controls.SettingViewModel viewModel = new Controls.SettingViewModel();
            Controls.SettingView view = new Controls.SettingView();
            Page page = new Page(viewModel, view);
            UIService.AddPage(page);
        }


    }
}
