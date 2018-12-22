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
            var setting = new Group();
            setting.AddCommand("Options", IconProvider.Properties, this.ShowSettingPage);
            uIService.AddGroupToHomePage(setting);
        }

        private void ShowSettingPage()
        {
            var viewModel = new Controls.SettingViewModel();
            var view = new Controls.SettingView();
            Page page = new Page(viewModel, view);
            uIService.AddPage(page);
        }

        
    }
}
