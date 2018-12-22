using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Output.Controls
{
    public class SaleCertificateViewModel : OutputViewModel
    {
        IDataProvider<Provider> providerService;
        public SaleCertificateViewModel(IExportable exportableView) : base(OutputType.Vente, exportableView)
        {
            this.Caption = "Liste de PV de vente";
            this.TakerVisibility = System.Windows.Visibility.Visible;
            this.TakerOptionVisibilty = System.Windows.Visibility.Visible;
            this.TakerFieldName = "Client";
            ServiceLocator.Resolve(out this.providerService);
            this.ListOfTakers = new ObservableCollection<NamedKeyRow>(this.providerService.SelectAll());
        }


        
    }
}
