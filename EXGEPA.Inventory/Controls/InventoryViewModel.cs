using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using CORESI.Data;
using CORESI.Data.Tools;
using CORESI.DataAccess.Core;
using CORESI.IoC;
using CORESI.Tools;
using CORESI.Tools.Collections;
using CORESI.WPF.Controls;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Inventory.Core;
using EXGEPA.Model;

namespace EXGEPA.Inventory.Controls
{
    public class InventoryViewModel : GenericEditableViewModel<InventoryData>
    {
        private readonly IParameterProvider parameterProvider;

        private readonly IDataProvider<Item> itemService;

        private readonly IDataProvider<InventoryRow> inventoryService;

        private readonly IRepositoryDataProvider repositoryDataProvider;

        private readonly IUIItemService uIItemService;

        private string oldCodeCaption;

        Dictionary<string, Office> AllOffices { get; set; }

        private ADeviceFileManager DeviceFileManager { get; set; }

        public InventoryViewModel(IExportableGrid view)
        {
            ServiceLocator.Resolve(out this.repositoryDataProvider);

            ServiceLocator.Resolve(out this.itemService);

            ServiceLocator.Resolve(out this.inventoryService);

            ServiceLocator.Resolve(out uIItemService);

            ServiceLocator.Resolve(out this.parameterProvider);

            SetDeviceManager();

            this.AutoWidth = false;
            HideAddButton = true;
            HideEditButton = true;
            HideDeleteButton = true;
            this.EnableTotalSumary = true;
            this.DoubleClicAction = this.EditItem;
            this.Caption = "Inventaire physique";
            InitilizeRibbonGroup(view);
            this.DisplayScannedItemOnly();
            IItemByCompteProvider itemByCompteProvider = ServiceLocator.Resolve<IItemByCompteProvider>();
            Group group = GetReportGroup(itemByCompteProvider);
            this.AddGroup(group);
            this.OldCodeCaption = this.ParameterProvider.TryGet("OldCodeCaption", "IMMO");
        }

        public string OldCodeCaption
        {
            get => this.oldCodeCaption;
            set
            {
                this.oldCodeCaption = value;
                RaisePropertyChanged(nameof(this.OldCodeCaption));
            }
        }

        private Group GetReportGroup(IItemByCompteProvider itemByCompteProvider)
        {

            if (itemByCompteProvider != null)
            {

                Group group = new Group("Ecarts");
                //group.AddCommand("Fiche immobilisation", () =>
                //{
                //    var items = this.ListOfRows.Where(x => x.InvestmentAccount != null).ToList();
                //    MenthlyCalculator.GetDepriciation(items, DateTime.MinValue, DateTime.MaxValue);
                //    itemByCompteProvider.PrintImmobilisationSheet(this.ListOfRows, "Fiche immobilisation");
                //});

                group.AddCommand("Non scannés", () =>
                {
                    var select = this.ExcludItemAccordingToExcludedAccountingPersiods(this.ListOfRows)
                    .Where(x => x.Item != null && x.Office == null)
                    .Select(x => x.Item)
                    .Where(x => x.OutputCertificate == null && x.GeneralAccount.GeneralAccountType.Type == EGeneralAccountType.Investment)
                    .ToList();
                    itemByCompteProvider.PrintImmobilisationByAccount(select, "Etat des articles non scannés par compte");
                });

                group.AddCommand("Negatif par compte", () =>
                {
                    var allItems = this.itemService.SelectAll().GroupBy(x => x.Office);
                    var scannedItem = this.ListOfRows.Select(x => x.Item);
                    var scannedOffices = this.ListOfRows.Where(x => x.Office != null).GroupBy(c => c.Office.Id).Select(g => g.Key).ToList();
                    var select = this.ExcludItemAccordingToExcludedAccountingPersiods(this.ListOfRows)
                    .Where(x => x.Item != null && x.Office == null && scannedOffices.Contains(x.Item.Office.Id))
                    .Select(x => x.Item)
                    .ToList();
                    itemByCompteProvider.PrintImmobilisationByAccount(select, "Etat des ecarts negatifs par compte");
                });

                //group.AddCommand("Inventaire par compte", () =>
                //{
                //    var select = this.ListOfRows.Where(x => x.Item != null && x.Office != null).Select(x => x.Item).ToList();

                //    itemByCompteProvider.PrintImmobilisationByAccount(select, "Etat des inventaire physique par compte");
                //});

                group.AddCommand("Details", () => this.UIMessage.TryDoAction(this.Logger, () => ExternalProcess.StartProcess("INVCOMPTE.exe")));

                //group.AddCommand("Récap", () =>
                //{
                //    var items = this.ListOfRows.Where(x => x.Item.InvestmentAccount != null).ToList();
                //    var others = this.repositoryDataProvider.ListOfGeneralAccount.Where(x => x.GeneralAccountType.Id == 3);
                //    var availableaccounts = items.GroupBy(g => g.Item.InvestmentAccount.Id).Select(g => g.First().Id);
                //    var otherItems = others.Where(x => availableaccounts.Any(a => a == x.Id)).Select(t => new InventoryData() { InvestmentAccount = t }).ToList();
                //    itemByCompteProvider.PrintRecapByAccount(items.Union(otherItems).ToList(), "Etat récapitulatif des investissements par compte.");
                //});
                return group;
            }

            return null;
        }



        private void SetDeviceManager()
        {

            if (this.parameterProvider.GetValue<bool>("UseAndroidDevice", false))
            {
                this.Logger.Info("Loading android device manager");
                this.DeviceFileManager = new AndroidFileManager();
            }
            else
            {
                this.Logger.Info("Loading WinCE device manager");
                this.DeviceFileManager = new WindowsCEFileManager();
            }
        }

        private new void EditItem()
        {
            if (this.SelectedRow != null)
                if (this.SelectedRow.Item != null)
                {
                    uIItemService.EditItem(SelectedRow.Item);
                }
                else
                {
                    this.UIMessage.Information("Aucun item correspandant a ce code n'a été trouvé !");
                }
        }

        public override void InitData()
        {
            StartBackGroundAction(() =>
            {
                using (var scoopLogger = new ScoopLogger("Initializing inventory module", this.Logger))
                {
                    var inventoryRows = this.inventoryService.SelectAll();
                    scoopLogger.Snap("To load inventorys");
                    var items = this.itemService.SelectAll();
                    scoopLogger.Snap("To load Items");
                    this.repositoryDataProvider.BindPropertyAndSetExtended(items);
                    scoopLogger.Snap("To bind Items");
                    var allItems = items.ToDictionary(x => x.Key);
                    this.AllOffices = this.repositoryDataProvider.ListOfOffice.ToDictionary(x => x.Key);
                    var result = new ConcurrentBag<InventoryData>();
                    Parallel.ForEach(inventoryRows, row => result.Add(this.ConvertToInventoryDataAndKeepOnlyNotScanned(row, allItems)));
                    List<Item> NotscannedItem = allItems.Values.Where(x => x.OutputCertificate == null).ToList();
                    Parallel.ForEach(result, x => x.GapType = this.ComputeGap(x));
                    Parallel.ForEach(NotscannedItem, x => { this.CheckIfItemIsExcludedFromInventory(result, x); });
                    ListOfRows = new ObservableCollection<InventoryData>(ExcludItemAccordingToExcludedAccountingPersiods(result));
                }
            });
        }

        private IEnumerable<InventoryData> ExcludItemAccordingToExcludedAccountingPersiods(IEnumerable<InventoryData> result)
        {
            List<int> excludedAccountingPeriod = this.GetExcludedAccountingPeriods();
            return result.Where(x => !excludedAccountingPeriod.Contains(x?.Item?.AccountingPeriod?.Id ?? -1));
        }

        private List<int> GetExcludedAccountingPeriods()
        {
            return this.repositoryDataProvider
            .ListOfAccountingPeriod
            .Where(ap => !ap.Approved)
            .OrderBy(x => x.StartDate)
            .Skip(1)
            .Select(x => x.Id)
            .ToList();
        }

        private string ComputeGap(InventoryData x)
        {
            if (x.Item == null && x.Office == null)
                return "Item et local non identifiés";
            if (x.Item == null && x.Office != null)
                return "Item non identifié";
            if (x.Item != null && x.Office == null)
                return "Local non identifié";
            if (x.Item.OutputCertificate != null)
                return "Positif";
            if (x.Office.Id == x.Item.Office.Id)
                return "Aucun";
            if (x.Office.Id != x.Item.Office.Id)
                return "Déplacement";
            return "NOT SUPPORTED";
        }

        private InventoryData ConvertToInventoryDataAndKeepOnlyNotScanned(InventoryRow inventoryRow, Dictionary<string, Item> allItems)
        {
            InventoryData inventory = new InventoryData()
            {
                Id = inventoryRow.Id,
                Key = inventoryRow.Key,
                InventoryRow = inventoryRow,
                Localization = inventoryRow.Localization,
                ItemState = repositoryDataProvider.AllStats.FirstOrDefault(x => x.Id == inventoryRow.ItemState?.Id)
            };

            if (inventoryRow.OpertationDate > DateTime.MinValue)
            {
                inventory.ImportDate = inventoryRow.OpertationDate;
            }
            else
            {
                inventory.ImportDate = null;
            }

            AllOffices.TryGetValue(inventoryRow.Localization, out Office office);
            inventory.Office = office;
            if (inventoryRow.Key.IsValid() && allItems.TryGetValue(inventoryRow.Key, out Item item))
            {
                allItems.Remove(inventoryRow.Key);
                inventory.Item = item;
            }

            inventory.GapType = ComputeGap(inventory);
            return inventory;
        }

        private void CheckIfItemIsExcludedFromInventory(ConcurrentBag<InventoryData> result, Item x)
        {
            if (x.ExcludedFromInventory)
            {
                result.Add(new InventoryData()
                {
                    GapType = "Inventorié par defaut",
                    Item = x,
                    Office = x.Office,
                    Key = x.Key,
                    Localization = x.Office.Key
                });
            }
            else
            {
                result.Add(new InventoryData()
                {
                    GapType = "Item non scanné",
                    Item = x,
                    Office = null,
                    Key = x.Key,
                });
            }
        }

        protected override void InitilizeRibbonGroup(IExportableGrid view = null)
        {
            base.InitilizeRibbonGroup(view);
            SetGapTraitementGroup();
            SetInventToolGroup();
            SetViewGroup();
        }

        private void SetGapTraitementGroup()
        {
            Group gapTraitement = new Group()
            {
                Caption = "Adjustement de l'inventaire"
            };
            this.Groups.Add(gapTraitement);
            gapTraitement.AddCommand("Théorique", IconProvider.Undo, this.AdjustToOldPosition);
            gapTraitement.AddCommand("Physique", IconProvider.Redo, this.AdjustToNewPosition);
        }

        private void AdjustToNewPosition()
        {
            List<InventoryData> rowToUpdate = this.Selection.Where(x => x.InventoryRow != null).ToList();
            foreach (InventoryData item in rowToUpdate)
            {
                AllOffices.TryGetValue(item.Localization, out Office office);
                if (office != null)
                {
                    item.Item.Office = office;
                    item.GapType = ComputeGap(item);
                    this.RefreshView(item);
                    itemService.Update(item.Item);
                }
            }
        }

        private void AdjustToOldPosition()
        {
            List<InventoryData> rowToUpdate = this.Selection.Where(x => x.InventoryRow != null).ToList();
            foreach (InventoryData row in rowToUpdate)
            {
                row.InventoryRow.Localization = row.Item.Office.Key;
                row.Office = row.Item.Office;
                row.GapType = ComputeGap(row);
                this.inventoryService.Update(row.InventoryRow);
                this.RefreshView(row);
            }
        }

        private void SetViewGroup()
        {
            Group viewGroup = this.AddNewGroup("Vues");
            viewGroup.AddCommand("Scannés", IconProvider.Phone, this.DisplayScannedItemOnly);
            viewGroup.Commands.Add(new RibbonButton() { IsSmall = true, Caption = "Non scannés", Action = () => { Filter = "GapType =  'Item non scanné'"; } });
            viewGroup.Commands.Add(new RibbonButton() { IsSmall = true, Caption = "Non identifiés", Action = () => { Filter = "GapType = 'Item non identifié'"; } });
            viewGroup.Commands.Add(new RibbonButton() { IsSmall = true, Caption = "Non Deplacés", Action = () => { Filter = "GapType ='Aucun' "; } });
            viewGroup.Commands.Add(new RibbonButton() { IsSmall = true, Caption = "Deplacés", Action = () => { Filter = "GapType = 'Déplacement'"; } });
            viewGroup.Commands.Add(new RibbonButton() { IsSmall = true, Caption = "Tous", Action = () => { Filter = string.Empty; } });
            viewGroup.Commands.Add(new RibbonButton() { IsSmall = true, Caption = "Manquants par local", Action = () => { Filter = "[gaptype] = 'Déplacement' Or [gaptype] = 'Item non scanné'"; } });
        }

        private void DisplayScannedItemOnly()
        {
            Filter = "[gaptype] = 'Déplacement' Or [gaptype] = 'Aucun' Or [gaptype] = 'Positif' Or [gaptype] = 'Inventorié par defaut'";
        }

        private void SetInventToolGroup()
        {
            Group inventToolsGroup = this.AddNewGroup("Outils d'inventaire");
            if (this.parameterProvider.GetValue("AllowInventFileLoading", false))
            {
                inventToolsGroup.AddCommand("Charger fichier", IconProvider.DownloadSmall, this.LoadFileFromDisk, true);
            }

            inventToolsGroup.AddCommand("Archives inventaire", IconProvider.DownloadSmall, this.DisplayArchive, true);

            inventToolsGroup.AddCommand("Recolter", IconProvider.DownloadSmall, this.LoadFileFromPDA, true);

            inventToolsGroup.AddCommand("Supprimer", IconProvider.DeleteSmall, this.DeleteItem, true);

            inventToolsGroup.AddCommand("Archiver", IconProvider.DeleteSheet, this.ClearInvent);
        }

        private void DisplayArchive()
        {
            this.UIMessage.TryDoAction(this.Logger, () => ExternalProcess.StartProcess("ArchiveINV.exe"));
        }

        private void LoadFileFromPDA()
        {
            StartBackGroundAction(() =>
            {
                if (this.DeviceFileManager.UpdateInventFile())
                {
                    DataImporter.LoadFile(this.DeviceFileManager.TargetPath);
                    this.InitData();
                }
            });
        }

        private void LoadFileFromDisk()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                StartBackGroundAction(() =>
                {
                    DataImporter.LoadFile(openFileDialog.FileName);
                    this.InitData();
                });
            }
        }

        private void ClearInvent()
        {
            this.UIMessage.TryDoUIActionAsync(this.Logger, () =>
            {
                var vm = new Wind1VM();
                var v = new Window1 { DataContext = vm };
                if (v.ShowDialog() == true)
                {
                    this.SaveAll(vm.Comment ?? $"Inventaire du : {DateTime.Today.ToShortDateString()}");
                    this.inventoryService.DeleteAll();
                    this.InitData();
                }
            });

        }

        private void SaveAll(string comment)
        {
            DateTime archiveDate = DateTime.Now;
            List<InventoryArchiveRow> itemToSave = this.ListOfRows.Select(x => new InventoryArchiveRow
            {
                Code = x.Key,
                Ancien_Code = x.Item?.OldCode,
                Type_ecart = x.GapType,

                Nom_inventaire = comment,
                Date_archive = archiveDate,
                Compte_analytique = x.Item?.AnalyticalAccount?.Key,
                Compte_investissement = x.Item?.GeneralAccount?.Key,
                Date_aquisition = x?.Item?.AquisitionDate ?? DateTime.MinValue,
                date_import = x?.InventoryRow?.OpertationDate ?? DateTime.MinValue,
                Designation_article = x?.Item?.Description,
                Etat_theorique = x?.Item?.ItemState?.Key,
                Libelle_compte_analytique = x?.Item?.AnalyticalAccount?.Caption,
                Libelle_compte_investissement = x?.Item?.GeneralAccount?.Caption,
                Montant = x?.Item?.Amount ?? 0,
                Observation = x?.Item?.Comment,
                Libelle_Local_physique = x?.Office?.Caption,
                Libelle_Local_theorique = x?.Item?.Office?.Caption,
                Localisation_physique = x?.Office?.Key,
                Localisation_theorique = x.Item?.Office?.Key,
                Reference = x.Item?.Reference?.Key
            }).ToList();
            SqlHelper.insertRawRows(itemToSave, "InventoryArchive");
            //throw new InvalidOperationException($"Access denied for user '{Environment.UserName}'");
        }

        public override void DeleteItem()
        {
            ConfirmeAndStartBackGroundAction("Etes vous sûr de vouloir supprimer cette ligne d'inventaire ? ",
            () =>
            {
                this.ShowLoadingPanel = true;
                ConcernedRow = SelectedRow;
                if (SelectedRow.Id != 0)
                {
                    this.inventoryService.Delete(new InventoryRow() { Id = SelectedRow.Id });
                }
                this.ListOfRows.Remove(SelectedRow);
            });
        }
    }
}