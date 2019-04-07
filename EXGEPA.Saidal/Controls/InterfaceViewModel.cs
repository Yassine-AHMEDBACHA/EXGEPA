// <copyright file="InterfaceViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Saidal.Controls
{
    using System.Linq;
    using System.Threading.Tasks;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.Tools;
    using CORESI.Tools.Collections;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public class InterfaceViewModel : GenericEditableViewModel<Item>
    {
        private readonly IRepositoryDataProvider repositoryDataProvider;

        public InterfaceViewModel(IExportableGrid exportableView)
            : base(exportableView, false)
        {
            this.AutoWidth = false;
            this.Caption = "Module d'interface";
            this.LoadButtons();
            ServiceLocator.Resolve(out this.repositoryDataProvider);
            this.InitData();
        }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                using (var scoopLogger = new ScoopLogger("Loading Data", this.Logger, true))
                {
                    var list = this.DBservice.All;
                    scoopLogger.Snap("Loading Data ");
                    this.repositoryDataProvider.BindItemFields(list);
                    this.ListOfRows = list.Where(this.IsToDisplay).ToObservable();
                }
            });
        }

        private bool IsToDisplay(Item item)
        {
            if (item.Invoice is null)
            {
                return false;
            }

            if (item.Invoice.Tag is bool value)
            {
                return !value;
            }

            return true;
        }

        private void LoadButtons()
        {
            var aquisitionSeriliazer = new AquisitionSerializer();
            ServiceLocator.Resolve(out IDataProvider<Invoice> invocieSevice);
            this.AddNewGroup().AddCommand("Aquisitions", () =>
            {
                var invoices = aquisitionSeriliazer.Serialize(this.Selection);
                invoices.ForEach(x => invocieSevice.Update(x));
                this.InitData();
                this.UIMessage.Notify("Fichier généré avec succès");
            });
        }
    }
}