// <copyright file="CommunViewModel.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
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
        protected IUIItemService UIItemService { get; set; }

        public virtual void SetItemAttribute()
        {
            var outputCertificate = this.SelectedRow;
            if (outputCertificate == null)
            {
                return;
            }

            ItemAttributionOptions options = this.GetItemAttributionOptions(outputCertificate);
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

        public CommunOutputViewModel(OutputType outputType, IExportableGrid exportableView)
            : base(exportableView)
        {
            this.UIItemService = ServiceLocator.Resolve<IUIItemService>();
            this.OutputType = outputType;
            this.EnableTotalSumary = true;
            this.DoubleClicAction = this.SetItemAttribute;
        }

        public OutputType OutputType { get; private set; }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                this.ShowLoadingPanel = true;
                var list = this.DBservice.SelectAll().Where(x => x.OutputType == this.OutputType).ToList();
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
