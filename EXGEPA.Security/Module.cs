using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Security.Controls;

namespace EXGEPA.Security
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
            setting.AddCommand("Utilisateur", IconProvider.BOUserSmall, this.ShowApplicationUserPage, true);
            setting.AddCommand("Roles", IconProvider.BOUserSmall, this.ShowRolePage, true);
            UIService.AddGroupToHomePage(setting);
        }

        private void ShowRolePage()
        {
            RoleView view = new RoleView();
            RoleViewModel viewModel = new RoleViewModel(view);
            Page page = new Page(viewModel, view, true);
            UIService.AddPage(page);
            viewModel.InitData();
        }

        private void ShowApplicationUserPage()
        {
            OperatorView view = new OperatorView();
            OperatorViewModel viewModel = new OperatorViewModel(view);
            Page page = new Page(viewModel, view, true);
            UIService.AddPage(page);
            viewModel.InitData();
        }
    }
}
