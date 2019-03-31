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
            CORESI.WPF.Model.Group newAquisition = this.AddNewGroup("Aquisitions de l'exercice");
            newAquisition.AddCommand("Investissements", this.PeriodNewInvestisment, isSmall: true);
            newAquisition.AddCommand("Charges", this.PeriodNewCharge, isSmall: true);
            this.AddNewGroup().AddCommand("Recap des investissments", this.InvestismentRecap);
            CORESI.WPF.Model.Group inventoryGroup = this.AddNewGroup("Inventaire physique");
            inventoryGroup.AddCommand("Par compte", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            CORESI.WPF.Model.Group immoGroup = this.AddNewGroup("Les immobilisations");
            immoGroup.AddCommand("Par compte", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            CORESI.WPF.Model.Group outputGroup = this.AddNewGroup("Les sorties");
            outputGroup.AddCommand("Reformes", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            outputGroup.AddCommand("Cessions", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            outputGroup.AddCommand("Destruction", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            outputGroup.AddCommand("Disparition", null, () => this.UIMessage.Information("Etat en cours de developpement"), true);
            CORESI.WPF.Model.Group others = this.AddNewGroup("Exercice");
        }

        private void showItemsByCompte()
        {
            PeriodAquisitionPreparator preparator = new PeriodAquisitionPreparator();
            ReportWrapperViewModel wrapper = preparator.GetReportWrapper();
            AvailableReport.Add(wrapper);
        }

        private void PeriodNewInvestisment()
        {
            PeriodAquisitionPreparator preparator = new PeriodAquisitionPreparator();
            ReportWrapperViewModel wrapper = preparator.GetReportWrapper();
            AvailableReport.Add(wrapper);
        }

        private void PeriodNewCharge()
        {
            PeriodAquisitionPreparator preparator = new PeriodAquisitionPreparator();
            ReportWrapperViewModel wrapper = preparator.GetReportWrapper(false);
            AvailableReport.Add(wrapper);
        }

        public void InvestismentRecap()
        {
            RecapByGeneralAccountPreparator preparator = new RecapByGeneralAccountPreparator();
            ReportWrapperViewModel wrapper = preparator.GetReportWrapper();
            AvailableReport.Add(wrapper);
        }
    }
}