using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.WPF.Core;
using CORESI.WPF.Model;

namespace EXGEPA.Securtiy
{
    public sealed class Module : AModule
    {
        public override int Priority
        {
            get { return 8; }
        }

        public override void AddGroups()
        {
            var setting = new Group();
            setting.AddCommand("Utilisateur", IconProvider.BOUser, this.ShowApplicationUserPage);
            UIService.AddGroupToHomePage(setting);
        }
        private void ShowApplicationUserPage()
        {
            var viewModel = new OperatorViewModel();
            var view = new OperatorView();
            var page = new Page(viewModel, view, true);
            UIService.AddPage(page);
            viewModel.InitData();
        }
    }
}
