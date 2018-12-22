using System;
using System.Collections.ObjectModel;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Output.Controls
{
    public class CessionCertificateViewModel : OutputViewModel
    {
        IDataProvider<AnalyticalAccount> analyticalAccountService;

        IDataProvider<AnalyticalAccountType> analyticalAccountTypeService;

        public CessionCertificateViewModel(IExportable exportableView) : base(OutputType.Cession, exportableView)
        {
            this.Caption = "Liste de PV de cession";
            ServiceLocator.Resolve(out this.analyticalAccountService);
            ServiceLocator.Resolve(out this.analyticalAccountTypeService);
            this.TakerVisibility = System.Windows.Visibility.Visible;
            this.TakerFieldName = "Unité réceptrice";
            this.TakerOptionVisibilty = System.Windows.Visibility.Collapsed;
            var types = this.analyticalAccountTypeService.SelectAll();
            var internalType = types.FirstOrDefault(x => x.Key.ToLowerInvariant() == "external");
            var list = this.analyticalAccountService.SelectAll(types);
            this.ListOfTakers = new ObservableCollection<NamedKeyRow>(list.Where(x => x.AnalyticalAccountType == internalType));
            this.AddNewGroup().AddCommand("Contenu du PV", IconProvider.GreaterThan, DisplayPvContent);
        }

        private void DisplayPvContent()
        {
            Predicate<Item> filter = x => x.OutputCertificate?.Id == this.SelectedRow.Id;
            this.UIItemService.DisplayItems(filter, $"Contenu du PV de cession {this.SelectedRow?.Key}", (items) =>
            {
                var reports = ServiceLocator.Resolve<IImmobilisationSheetProvider>();
                reports.PrintOutputSheet(items, true, "Fiche de cession");
            });
        }






    }
}
