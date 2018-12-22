﻿using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;

namespace EXGEPA.Administration
{
    public sealed class Module : AModule
    {
        public override int Priority
        {
            get
            {
                return 100;
            }
        }

        public override void AddGroups()
        {
            var reportGroup = new Group("CORESI");
            //reportGroup.AddCommand("", IconProvider.Article, () =>
            //{
            //    var editionViewModel = new Controls.EditionViewModel("Editions charges");
            //    var editionView = new Controls.EditionView();
            //    var page = new Page(editionViewModel, editionView, true);
            //    UIService.AddPage(page);
            //});
            uIService.AddGroupToHomePage(reportGroup);
        }
    }
}
