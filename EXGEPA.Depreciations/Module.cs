using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Depreciations.Core;

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
            Group depreciationGroup = new Group();
            depreciationGroup.Commands.Add(GetHomePageRibbonButton());
            UIService.AddGroupToHomePage(depreciationGroup);
        }

        private RibbonButton GetHomePageRibbonButton()
        {
            return new RibbonButton()
            {

                Caption = "Dotations",
                LargeGlyph = IconProvider.AddCalculatedField,

                Action = () =>
                {
                    AccountingPeriodHelper accountingHepler = new AccountingPeriodHelper();
                    DailyCalculator calculator = new DailyCalculator(accountingHepler);
                    Contorls.DepreciationView view = new Depreciations.Contorls.DepreciationView();
                    Contorls.DepreciationViewModel viewModelm = new Depreciations.Contorls.DepreciationViewModel(view, "Dotations");
                    Page page = new Page(viewModelm, view, true);
                    UIService.AddPage(page);
                }
            };
        }
    }
}
