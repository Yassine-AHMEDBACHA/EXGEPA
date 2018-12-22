using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            var setting = new Group();
            setting.AddCommand("Utilisateur", IconProvider.BOUserSmall, this.ShowApplicationUserPage, true);
            setting.AddCommand("Roles", IconProvider.BOUserSmall, this.ShowRolePage, true);
            uIService.AddGroupToHomePage(setting);
        }

        private void ShowRolePage()
        {
            var view = new RoleView();
            var viewModel = new RoleViewModel(view);
            var page = new Page(viewModel, view, true);
            uIService.AddPage(page);
            viewModel.InitData();
        }

        private void ShowApplicationUserPage()
        {
            var view = new OperatorView();
            var viewModel = new OperatorViewModel(view);
            var page = new Page(viewModel, view, true);
            uIService.AddPage(page);
            viewModel.InitData();
        }
    }
}
