// <copyright file="InterfaceViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Saidal.Controls
{
    using System;
    using System.Linq;
    using CORESI.Data.Tools;
    using CORESI.IoC;
    using CORESI.Tools;
    using CORESI.Tools.Collections;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using CORESI.WPF.Model;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public class InterfaceViewModel : GenericEditableViewModel<Invoice>
    {
        private readonly IRepositoryDataProvider repositoryDataProvider;

        private readonly bool displayOnlyValidatedInvoice;

        public InterfaceViewModel(IExportableGrid exportableView)
        {
            this.AutoWidth = false;
            this.SetToolGroup();
            this.SetExportGroup(exportableView);
            this.HideAddButton = true;
            this.HideEditButton = true;
            this.HideDeleteButton = true;
            this.Caption = "Module d'interface";
            this.LoadButtons();
            ServiceLocator.Resolve(out this.repositoryDataProvider);
            this.StartDateEditRibbon.Date = DateTime.Today.AddMonths(-1);
            this.EndDateEditRibbon.Date = DateTime.Today;
            var sttingsGroup = this.AddNewGroup("Parametres", InsertPosition.Left);
            sttingsGroup.Commands.Add(this.EndDateEditRibbon);
            sttingsGroup.Commands.Add(this.StartDateEditRibbon);
            this.displayOnlyValidatedInvoice = this.ParameterProvider.TryGet("InterfaceDisplayOnlyValidatedInvoice", true);
        }

        public DateEditRibbon StartDateEditRibbon { get; } = new DateEditRibbon("Date début");

        public DateEditRibbon EndDateEditRibbon { get; } = new DateEditRibbon("Date fin");

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                using (var scoopLogger = new ScoopLogger("Loading Data", this.Logger, true))
                {
                    scoopLogger.Snap("Loading Data ");
                    this.repositoryDataProvider.Refresh();
                    this.repositoryDataProvider.MapAllItems();
                    this.ListOfRows = this.repositoryDataProvider.AllInvoices
                    .Where(this.IsToDisplay)
                    .ToObservable();
                }
            });
        }

        private bool IsToDisplay(Invoice invoice)
        {
            if (!invoice.Date.IsBetween(this.StartDateEditRibbon.Date, this.EndDateEditRibbon.Date))
            {
                return false;
            }

            if (invoice is null)
            {
                return false;
            }

            if (this.displayOnlyValidatedInvoice && !invoice.IsValidated)
            {
                return false;
            }

            if (invoice.Tag is bool value)
            {
                return !value;
            }

            if (invoice.Tag?.ToString().EqualsTo("1") == true)
            {
                return false;
            }

            return invoice.Items.Any();
        }

        private void LoadButtons()
        {
            var aquisitionSeriliazer = new AquisitionSerializer();
            this.AddNewGroup().AddCommand("Aquisitions", () =>
            {
                var invoices = aquisitionSeriliazer.Serialize(this.Selection);
                if (invoices?.Count > 0)
                {
                    invoices.ForEach(x => this.DBservice.Update(x));
                    this.InitData();
                    this.UIMessage.Notify("Fichier généré avec succès");
                }
            });
        }
    }
}