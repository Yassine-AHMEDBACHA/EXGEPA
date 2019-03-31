using CORESI.WPF.Core.Framework;
using EXGEPA.Report.Aquisitions;
using EXGEPA.Report.InvestismentRecap;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace EXGEPA.Report.Controls
{
    public class ReportViewModel : PageViewModel
    {
        public ICommand PeriodAquisitionCommand { get; private set; }
        public ICommand InvestismentRecapCommand { get; private set; }
        public ObservableCollection<ReportWrapperViewModel> AvailableReport { get; set; }

        public ReportViewModel(string caption) : base()
        {
            this.Caption = caption;
            this.AvailableReport = new ObservableCollection<ReportWrapperViewModel>();
            var newAquisition = this.AddNewGroup("Aquisitions de l'exercice");
            newAquisition.AddCommand("Investissements", this.PeriodNewInvestisment, isSmall: true);
            newAquisition.AddCommand("Charges", this.PeriodNewCharge, isSmall: true);
            this.AddNewGroup().AddCommand("Recap des investissments", this.InvestismentRecap);
            var inventoryGroup = this.AddNewGroup("Inventaire physique");
            inventoryGroup.AddCommand("Par compte", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            var immoGroup = this.AddNewGroup("Les immobilisations");
            immoGroup.AddCommand("Par compte", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            var outputGroup = this.AddNewGroup("Les sorties");
            outputGroup.AddCommand("Reformes", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            outputGroup.AddCommand("Cessions", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            outputGroup.AddCommand("Destruction", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            outputGroup.AddCommand("Disparition", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            var others = this.AddNewGroup("Exercice");
        }

        private void showItemsByCompte()
        {
            var preparator = new PeriodAquisitionPreparator();
            var wrapper = preparator.GetReportWrapper();
            AvailableReport.Add(wrapper);
        }

        private void PeriodNewInvestisment()
        {
            var preparator = new PeriodAquisitionPreparator();
            var wrapper = preparator.GetReportWrapper();
            AvailableReport.Add(wrapper);
        }

        private void PeriodNewCharge()
        {
            var preparator = new PeriodAquisitionPreparator();
            var wrapper = preparator.GetReportWrapper(false);
            AvailableReport.Add(wrapper);
        }

        public void InvestismentRecap()
        {
            var preparator = new RecapByGeneralAccountPreparator();
            var wrapper = preparator.GetReportWrapper();
            AvailableReport.Add(wrapper);
        }
    }
}