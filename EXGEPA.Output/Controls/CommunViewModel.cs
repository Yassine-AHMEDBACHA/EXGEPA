using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System.Collections.ObjectModel;
using System.Linq;
using CORESI.WPF.Model;
using System.Windows.Media;

namespace EXGEPA.Output.Controls
{
    public abstract class CommunOutputViewModel : GenericEditableViewModel<OutputCertificate>
    {
        protected IUIItemService UIItemService { get; set; }

        public virtual void SetItemAttribute()
        {
            var outputCertificate = this.SelectedRow;
            if (outputCertificate == null)
                return;
            ItemAttributionOptions options = GetItemAttributionOptions(outputCertificate);
            this.UIItemService.ShowItemAttribution(options);
        }

        public virtual ItemAttributionOptions GetItemAttributionOptions(OutputCertificate outputCertificate)
        {
            var options = new ItemAttributionOptions();
            options.PageCaption = "PV N°:" + outputCertificate.Key;
            options.SetConfirmationMessage = "Etes vous sûr de vouloir rajouter ces articles au PV N° " + outputCertificate.Key;
            options.ResetConfirmationMessage = "Etes vous sûr de vouloir retirer ces articles du PV N° " + outputCertificate.Key;
            options.RightPanelCaption = "Contenu du PV N° " + outputCertificate.Key;
            options.Tester = (item) => item.OutputCertificate?.Id == outputCertificate.Id;
            options.Setter = (item) => item.OutputCertificate = outputCertificate;
            options.Resetter = (item) => item.OutputCertificate = null;
            options.Categorie = new Categorie("Les Sorties", Colors.IndianRed);
            return options;
        }

        public CommunOutputViewModel(OutputType outputType, IExportableGrid exportableView) : base(exportableView)
        {
            UIItemService = ServiceLocator.Resolve<IUIItemService>();
            this.OutputType = outputType;
            EnableTotalSumary = true;
            this.DoubleClicAction = this.SetItemAttribute;
        }

        public OutputType OutputType { get; private set; }
        public override void InitData()
        {
            StartBackGroundAction(() =>
            {
                ShowLoadingPanel = true;
                var list = DBservice.SelectAll().Where(x => x.OutputType == this.OutputType).ToList();
                this.ListOfRows = new ObservableCollection<OutputCertificate>(list);
                ShowLoadingPanel = false;
            });
        }
        public override void AddItem()
        {
            base.AddItem();
            RaisePropertyChanged("ConcernedRow");
        }

        public override void AddItem(OutputCertificate outputCertificate)
        {
            outputCertificate.OutputType = this.OutputType;
            base.AddItem(outputCertificate);
        }

    }
}
