// <copyright file="CommunOutputViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Media;
    using CORESI.IoC;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using CORESI.WPF.Model;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;

    public abstract class CommunOutputViewModel : GenericEditableViewModel<OutputCertificate>
    {
        public CommunOutputViewModel(OutputType outputType, IExportableGrid exportableView)
            : base(exportableView)
        {
            this.UIItemService = ServiceLocator.Resolve<IUIItemService>();
            this.OutputType = outputType;
            this.EnableTotalSumary = true;
            this.DoubleClicAction = this.SetItemAttribute;
        }

        public OutputType OutputType { get; private set; }

        protected IUIItemService UIItemService { get; set; }

        public virtual void SetItemAttribute()
        {
            OutputCertificate outputCertificate = this.SelectedRow;
            if (outputCertificate == null)
            {
                return;
            }

            ItemAttributionOptions options = this.GetItemAttributionOptions(outputCertificate);
            this.UIItemService.ShowItemAttribution(options);
        }

        public virtual ItemAttributionOptions GetItemAttributionOptions(OutputCertificate outputCertificate)
        {
            ItemAttributionOptions options = new ItemAttributionOptions
            {
                PageCaption = "PV N°:" + outputCertificate.Key,
                SetConfirmationMessage = "Etes vous sûr de vouloir rajouter ces articles au PV N° " + outputCertificate.Key,
                ResetConfirmationMessage = "Etes vous sûr de vouloir retirer ces articles du PV N° " + outputCertificate.Key,
                RightPanelCaption = "Contenu du PV N° " + outputCertificate.Key,
                Tester = (item) => item.OutputCertificate?.Id == outputCertificate.Id,
                Setter = (item) => item.OutputCertificate = outputCertificate,
                Resetter = (item) => item.OutputCertificate = null,
                Categorie = new Categorie("Les Sorties", Colors.IndianRed)
            };
            return options;
        }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                this.ShowLoadingPanel = true;
                System.Collections.Generic.List<OutputCertificate> list = this.DBservice.SelectAll().Where(x => x.OutputType == this.OutputType).ToList();
                this.ListOfRows = new ObservableCollection<OutputCertificate>(list);
                this.ShowLoadingPanel = false;
            });
        }

        public override void AddItem()
        {
            base.AddItem();
            this.RaisePropertyChanged("ConcernedRow");
        }

        public override void AddItem(OutputCertificate outputCertificate)
        {
            outputCertificate.OutputType = this.OutputType;
            base.AddItem(outputCertificate);
        }
    }
}
