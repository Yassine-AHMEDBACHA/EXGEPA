using CORESI.IoC;
using CORESI.WPF;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Depreciations.Core;
using System.ComponentModel.Composition;

namespace EXGEPA.Depreciations
{
    public sealed class Module : AModule
    {
        public override int Priority
        {
            get { return 4; }
        }

        public override void AddGroups()
        {
            var depreciationGroup = new Group();
            depreciationGroup.Commands.Add(GetHomePageRibbonButton());
            uIService.AddGroupToHomePage(depreciationGroup);
        }

        private RibbonButton GetHomePageRibbonButton()
        {
            return new RibbonButton()
            {

                Caption = "Dotations",
                LargeGlyph = IconProvider.AddCalculatedField,

                Action = () =>
                {
                    var accountingHepler = new AccountingPeriodHelper();
                    var calculator = new DailyCalculator(accountingHepler);
                    var view = new Depreciations.Contorls.DepreciationView();
                    var viewModelm = new Depreciations.Contorls.DepreciationViewModel(view, "Dotations");
                    var page = new Page(viewModelm, view, true);
                    uIService.AddPage(page);
                }
            };
        }
    }
}
