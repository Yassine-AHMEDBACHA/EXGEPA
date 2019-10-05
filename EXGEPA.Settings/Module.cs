
namespace EXGEPA.Settings
{
    using CORESI.WPF.Core;
    using CORESI.WPF.Model;

    public sealed class Module : AModule
    {
        private const string settingGroupName = "settingGroup";

        public override int Priority => 7;

        public override void AddGroups()
        {
            var setting = UIService.GetGroupPageByName(settingGroupName);
            bool isSmallIcon = true;
            if (setting == null)
            {
                setting = new Group { Name = "settingGroup" };
                isSmallIcon = false;
            }

            setting.AddCommand("Options", IconProvider.Properties, this.ShowSettingPage, isSmallIcon);
            this.UIService.AddGroupToHomePage(setting);
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
