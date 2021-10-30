// <copyright file="ReformeCertificateViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using System;
    using System.Windows.Media;
    using CORESI.IoC;
    using CORESI.Tools;
    using CORESI.WPF.Core.Interfaces;
    using CORESI.WPF.Model;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;

    public class ReformeCertificateViewModel : FieldVisibilityBase<ReformeCertificate>
    {

        public ReformeCertificateViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.UIItemService = ServiceLocator.Resolve<IUIItemService>();
            this.DoubleClicAction = this.SetItemAttribute;
            this.Caption = "Liste de PV de Reforme";
            this.EnableTotalSumary = true;
            var processName = this.ParameterProvider.TryGet("ReformReportingTool", "mise_rfm.exe");
            if (ExternalProcess.Exists(processName))
            {
                var buttonCaption = this.ParameterProvider.TryGet("ReformReportingToolButtonCaption", "Mise en reforme");
                this.AddNewGroup().AddCommand(buttonCaption, () => this.UIMessage.TryDoAction(this.Logger, () => ExternalProcess.StartProcess(processName)));
            }
        }

        protected IUIItemService UIItemService { get; set; }

        public override void AddItem()
        {
            base.AddItem();
            this.ConcernedRow.Date = DateTime.Today;
            this.RaisePropertyChanged("ConcernedRow");
        }

        public void SetItemAttribute()
        {
            ReformeCertificate reformeCertificate = this.SelectedRow;
            if (reformeCertificate == null)
            {
                return;
            }

            ItemAttributionOptions options = new ItemAttributionOptions
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
            this.UIItemService.ShowItemAttribution(options);
        }
    }
}
