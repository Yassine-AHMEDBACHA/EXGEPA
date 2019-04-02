﻿using CORESI.IoC;
using CORESI.Tools;
using CORESI.WPF.Controls;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Media;

namespace EXGEPA.Items.Controls
{
    public class ItemGridViewModel : GenericEditableViewModel<Item>
    {
        IUIItemService UIItemService { get; set; }

        IRepositoryDataProvider RepositoryDataProvider { get; set; }

        public ItemGridViewModel(IExportableGrid exportableView) : base(exportableView, false)
        {
            this.Logger.Info("Initiating ItemGridViewModel ...");
            this.Logger.Debug("Start composing ItemGridViewModel ...");
            this.Caption = "Liste des immobilisations";
            this.AutoWidth = false;
            this.TryAddSummaryButton();
            RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
            UIItemService = ServiceLocator.Resolve<IUIItemService>();
            this.Logger.Debug("ItemGridViewModel Composition Done");
            Depreciations.Core.MonthelyCalculator MenthlyCalculator = new Depreciations.Core.MonthelyCalculator(new Depreciations.Core.AccountingPeriodHelper());
            Group immoSheetGroup = this.AddNewGroup("Fiches immo");
            immoSheetGroup.AddCommand("Mensuelle", () => this.StartUIBackGroundAction(() =>
            {
                System.Collections.Generic.List<Item> result = this.Selection.ToList();
                MenthlyCalculator.GetDepriciation(result, DateTime.MinValue, DateTime.MaxValue);
                ServiceLocator.Resolve<IImmobilisationSheetProvider>().PrintImmobilisationSheet(result.SelectMany(x => x.Depreciations).ToList(), "Fiche immo mensuelle");
            }), true);

            Depreciations.Core.DailyCalculator dailyCalculator = new Depreciations.Core.DailyCalculator(new Depreciations.Core.AccountingPeriodHelper());
            immoSheetGroup.AddCommand("Journaliere", () => this.StartUIBackGroundAction(
                 () =>
                 {
                     System.Collections.Generic.List<Item> result = this.Selection.ToList();
                     MenthlyCalculator.GetDepriciation(result, DateTime.MinValue, DateTime.MaxValue);
                     ServiceLocator.Resolve<IImmobilisationSheetProvider>().PrintImmobilisationSheet(result.SelectMany(x => x.Depreciations).ToList(), "Fiche immo journaliere");
                 }), true);

            immoSheetGroup.AddCommand("F.exploitation", () => this.StartUIBackGroundAction(
                () =>
                {
                    System.Collections.Generic.List<Item> result = this.Selection.ToList();
                    ServiceLocator.Resolve<IImmobilisationSheetProvider>().PrintExploitationStartupSheet(result, "Fiche de mise en exploitation");
                }), true);

            Group viewGroup = this.AddNewGroup("Filtres");
            this.Filter = "[OutputCertificate.OutputType] is null";
            viewGroup.AddCommand("Tous", () => this.Filter = string.Empty, true);
            viewGroup.AddCommand("Actifs", () => this.Filter = "[OutputCertificate.OutputType] is null", true);
            viewGroup.AddCommand("Reformés actifs", () => this.Filter = "[ReformeCertificate] is not null and [OutputCertificate.OutputType] is null", true);
            viewGroup.AddCommand("Cédés", () => this.Filter = "[OutputCertificate.OutputType] ='Cession'", true);
            viewGroup.AddCommand("Disparition", () => this.Filter = "[OutputCertificate.OutputType] ='Disparition'", true);
            viewGroup.AddCommand("Déstruction", () => this.Filter = "[OutputCertificate.OutputType] ='Destruction'", true);
            this.AddNewGroup().AddCommand("Etiquettes", IconProvider.BarCode, this.UIItemService.ShowPrintLabelPanel);

            IItemByCompteProvider itemByCompteProvider = ServiceLocator.Resolve<IItemByCompteProvider>();
            Group group = GetReportGroup(itemByCompteProvider);
            this.AddGroup(group);
            this.AddNewGroup().AddCommand("Historique des mouvements", () =>
            {
                if (this.SelectedRow != null)
                {
                    ItemHistoView view = new ItemHistoView();
                    ItemHistoViewModel vm = new ItemHistoViewModel(view, this.SelectedRow.Id);
                    Page page = new Page(vm, view, true);
                    this.UIService.AddPage(page);
                }
            });
        }



        private Group GetReportGroup(IItemByCompteProvider itemByCompteProvider)
        {

            if (itemByCompteProvider != null)
            {

                Group group = new Group("Immobilisations");
                //group.AddCommand("Fiche immobilisation", () =>
                //{
                //    var items = this.ListOfRows.Where(x => x.InvestmentAccount != null).ToList();
                //    MenthlyCalculator.GetDepriciation(items, DateTime.MinValue, DateTime.MaxValue);
                //    itemByCompteProvider.PrintImmobilisationSheet(this.ListOfRows, "Fiche immobilisation");
                //});

                //group.AddCommand("Par Compte", () =>
                //{
                //    var itemToDisplay = this.ListOfRows.Where(x => x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment).ToList();
                //    itemByCompteProvider.PrintImmobilisationByAccount(itemToDisplay, "Etat des investissements par compte.");
                //});

                //group.AddCommand("Selection par Compte", () =>
                //{
                //    itemByCompteProvider.PrintImmobilisationByAccount(this.ListOfRows.Where(x => x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment).ToList(), "Etat des investissements par compte - filtré.", this.DisplayedFilter);
                //});

                group.AddCommand("Récap", () =>
                {
                    System.Collections.Generic.List<Item> items = this.ListOfRows.Where(x => x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment).ToList();
                    System.Collections.Generic.IEnumerable<GeneralAccount> others = RepositoryDataProvider.ListOfGeneralAccount.Where(x => x.GeneralAccountType.Id == 3);
                    System.Collections.Generic.IEnumerable<int> availableaccounts = items.GroupBy(g => g.GeneralAccount.Id).Select(g => g.First().Id);
                    System.Collections.Generic.List<Item> otherItems = others.Where(x => availableaccounts.Any(a => a == x.Id)).Select(t => new Item() { GeneralAccount = t }).ToList();
                    itemByCompteProvider.PrintRecapByAccount(items.Union(otherItems).ToList(), "Etat récapitulatif des investissements par compte.");
                });


                group.AddCommand("Details", () => this.UIMessage.TryDoAction(this.Logger, () => ExternalProcess.StartProcess("EQUIPCOMPTE.exe")));

                return group;
            }

            return null;
        }

        public override void EditItem()
        {
            UIItemService.EditItem(SelectedRow);
        }

        public override void AddItem()
        {
            UIItemService.AddNewItem();
        }
        public override void InitData()
        {
            StartBackGroundAction(() =>
            {
                using (ScoopLogger scoopLogger = new ScoopLogger("Loading Data", this.Logger, true))
                {
                    System.Collections.Generic.IList<Item> list = DBservice.SelectAll();
                    scoopLogger.Snap("Loading Data ");
                    Parallel.ForEach(list, (item) =>
                    {
                        RepositoryDataProvider.BindItemFields(item);
                        if (JsonHelper.TryDeserialize(item.Json, out ItemExtendedProperties itemExtendedProperties))
                        {
                            item.ItemExtendedProperties = itemExtendedProperties;
                        }
                    });
                    ListOfRows = new ObservableCollection<Item>(list);
                }
            });
        }
    }
}