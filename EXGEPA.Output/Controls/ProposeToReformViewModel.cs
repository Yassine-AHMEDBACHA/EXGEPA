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
    public class ProposeToReformViewModel : GenericEditableViewModel<ProposeToReformCertificate>
    {
        IUIItemService UIItemService { get; set; }
        public ProposeToReformViewModel(IExportableGrid exportableView) : base(exportableView)
        {
            this.UIItemService = ServiceLocator.Resolve<IUIItemService>();
            this.DoubleClicAction = this.SetItemAttribute;
            this.Caption = "Liste de PV de proposition à la reforme";
        }

        public override void AddItem()
        {
            base.AddItem();
            this.ConcernedRow.Date = DateTime.Today;
            RaisePropertyChanged("ConcernedRow");
        }

        public void SetItemAttribute()
        {
            var proposeToReformCertificate = this.SelectedRow;
            if (proposeToReformCertificate == null)
                return;
            var options = new ItemAttributionOptions
            {
                PageCaption = "PV N°:" + proposeToReformCertificate.Key,
                SetConfirmationMessage = "Etes vous sûr de vouloir affecter ces articles au PV N° " + proposeToReformCertificate.Key,
                ResetConfirmationMessage = "Etes vous sûr de vouloir retirer ces articles du PV N° " + proposeToReformCertificate.Key,
                RightPanelCaption = "Contenu du PV de proposition à la reforme N° " + proposeToReformCertificate.Key,
                Tester = (item) => item.ProposeToReformCertificate?.Id == proposeToReformCertificate.Id,
                Setter = (item) => item.ProposeToReformCertificate = proposeToReformCertificate,
                Resetter = (item) => item.ProposeToReformCertificate = null,
                Categorie = new Categorie("PV de Proposition à la Reforme", Colors.Tomato)
            };
            UIItemService.ShowItemAttribution(options);
        }
    }
}