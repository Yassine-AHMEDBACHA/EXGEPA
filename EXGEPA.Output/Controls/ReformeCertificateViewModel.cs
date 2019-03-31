using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System;
using System.Windows.Media;

namespace EXGEPA.Output.Controls
{
    public class ReformeCertificateViewModel : GenericEditableViewModel<ReformeCertificate>
    {
        IUIItemService UIItemService { get; set; }

        public ReformeCertificateViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            this.UIItemService = ServiceLocator.Resolve<IUIItemService>();
            this.DoubleClicAction = this.SetItemAttribute;
            this.Caption = "Liste de PV de Reforme";
            base.EnableTotalSumary = true;
        }

        public override void AddItem()
        {
            base.AddItem();
            this.ConcernedRow.Date = DateTime.Today;
            RaisePropertyChanged("ConcernedRow");
        }

        public void SetItemAttribute()
        {
            var reformeCertificate = this.SelectedRow;
            if (reformeCertificate == null)
                return;
            var options = new ItemAttributionOptions
            {
                PageCaption = "PV N°:" + reformeCertificate.Key,
                SetConfirmationMessage = "Etes vous sûr de vouloir reformer ces articles et les inclure dans le PV N° " + reformeCertificate.Key,
                ResetConfirmationMessage = "Etes vous sûr de vouloir retirer ces articles du PV N° " + reformeCertificate.Key,
                RightPanelCaption = "Contenu du PV de reforme N° " + reformeCertificate.Key,
                Tester = (item) => item.ReformeCertificate?.Id == reformeCertificate.Id,
                Setter = (item) => item.ReformeCertificate = reformeCertificate,
                Resetter = (item) => item.ReformeCertificate = null,
                Categorie = new Categorie("PV de Reforme", Colors.Tomato)
            };
            UIItemService.ShowItemAttribution(options);
        }
    }
}
