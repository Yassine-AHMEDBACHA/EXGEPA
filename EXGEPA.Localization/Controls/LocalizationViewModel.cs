using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.DataAccess;
using EXGEPA.DataAccess.Service;
using EXGEPA.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Composition;
using System.Linq;
using System.Windows.Input;
using EXGEPA.Core;
using CORESI.Tools.StringTools;
using CORESI.Tools.Collections;

namespace EXGEPA.Localization.Controls
{
    public class LocalizationViewModel : BasicGridViewModel<Site>
    {
        #region Properties

        IDataProvider<AnalyticalAccount> AnalyticalAccountService { get; set; }

        public ObservableCollection<Office> SelectedOffices { get; set; }

        [Import(typeof(IDataProvider<Region>))]
        RegionService RegionalService { get; set; }

        [Import(typeof(IDataProvider<Site>))]
        SiteService SiteService { get; set; }

        [Import(typeof(IDataProvider<Building>))]
        BuildingService BuildingService { get; set; }

        [Import(typeof(IDataProvider<Level>))]
        LevelService LevelService { get; set; }

        [Import(typeof(IDataProvider<Office>))]
        OfficeService OfficeService { get; set; }

        IInventorySheetProvider OfficeInventoryReportProvider { get; set; }
        IRepositoryDataProvider RepositoryDataProvider { get; set; }

        IDataProvider<InventoryRow> InventoryRowService { get; set; }
        IDataProvider<Item> ItemService { get; set; }
        public LocalizationViewModel()
        {
            SelectedOffices = new ObservableCollection<Office>();
            ServiceLocator.ComposeParts(this);
            CancelSiteCommand = new Command(() => { this.DisplaySiteDetail = false; });
            CancelBuildingCommand = new Command(() => { this.DisplayBuildingDetail = false; });
            CancelLevelCommand = new Command(() => { this.DisplayLevelDetail = false; });
            CancelOfficeCommand = new Command(() => { this.DisplayOfficeDetail = false; });
            OfficeInventoryReportProvider = ServiceLocator.Resolve<IInventorySheetProvider>();
            if (OfficeInventoryReportProvider != null)
            {
                RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
                ItemService = ServiceLocator.Resolve<IDataProvider<Item>>();
                InventoryRowService = ServiceLocator.Resolve<IDataProvider<InventoryRow>>();
                this.ContextMenuItems = new ObservableCollection<SimpleButton>();
                ContextMenuItems.Add(new SimpleButton() { Caption = "Fiche d'inventaire theorique ", Action = PrintTheoricalInventory });
                ContextMenuItems.Add(new SimpleButton() { Caption = "Fiche d'inventaire physique ", Action = PrintPhysicalInventory });
            }

            this.AddNewGroup().AddCommand("Refresh", IconProvider.Refresh, this.InitData);

            this.InitData();
        }

        private void InitData()
        {
            this.UIMessage.TryDoActionAsync(Logger, () =>
            {
                this.ShowLoadingPanel = true;
                this.AnalyticalAccountService = ServiceLocator.Resolve<IDataProvider<AnalyticalAccount>>();
                InitRibbonCommand();
                IList<Region> allRegion = RegionalService.SelectAll();
                IList<Site> sites = this.SiteService.SelectAll();
                IList<Office> allOffices = this.OfficeService.SelectAll();
                IList<Level> allLevels = this.LevelService.SelectAll();
                IList<Building> allBuildings = this.BuildingService.SelectAll();
                Core.LoacalizationTools.BindLocalization(allOffices, allLevels, sites, allBuildings, allRegion);
                CurrentRegion = sites.FirstOrDefault().Region;
                ListOfAnalyticalAccount = new ObservableCollection<AnalyticalAccount>(AnalyticalAccountService.SelectAll());
                foreach (Office office in allOffices)
                {
                    if (office.AnalyticalAccount != null)
                        office.AnalyticalAccount = ListOfAnalyticalAccount.FirstOrDefault(aAccout => aAccout.Id == (office.AnalyticalAccount.Id));
                }

                UpdateButtonVisibilities();
                this.ListOfRows = new ObservableCollection<Site>(sites);
            }, () => this.ShowLoadingPanel = false);
        }

        private void PrintPhysicalInventory()
        {
            this.ShowLoadingPanel = true;
            this.UIMessage.TryDoUIActionAsync(Logger, () =>
             {
                 IList<Item> items = null;
                 Dictionary<string, Office> offices = SelectedOffices.ToDictionary(x => x.Key);
                 List<InventoryRow> inventoryRows = InventoryRowService.SelectAll().Where(x => offices.ContainsKey(x.Localization)).ToList();
                 if (inventoryRows.Count > 0)
                 {
                     items = ItemService.SelectAll();
                     Dictionary<string, Item> allItems = items.Where(x => x.OutputCertificate == null).ToDictionary(x => x.Key, x => x);
                     items = inventoryRows.Select(x =>
                    {
                        if (allItems.TryGetValue(x.Key, out Item item))
                        {
                            offices.TryGetValue(x.Localization, out Office office);
                            item.Office = office;
                            item.ItemState = x.ItemState;
                        }
                        return item;
                    }).Where(x => x != null).ToList();
                     RepositoryDataProvider.BindItemFields(items);
                 }
                 OfficeInventoryReportProvider.PrintInventorySheet(items, false);
             }, () => this.ShowLoadingPanel = false);
        }



        private void PrintTheoricalInventory()
        {
            RepositoryDataProvider.Refresh();
            List<int> officeIds = SelectedOffices.Select(x => x.Id).ToList();
            IList<Item> items = ItemService.SelectAll();
            RepositoryDataProvider.BindItemFields(items);
            items = items.Where(x => x.OutputCertificate == null && officeIds.Contains(x.Office.Id)).ToList();
            OfficeInventoryReportProvider.PrintInventorySheet(items);
        }

        private Region _CurrentRegion;

        public Region CurrentRegion
        {
            get => _CurrentRegion;
            set
            {
                _CurrentRegion = value;
                RaisePropertyChanged("CurrentRegion");
            }
        }


        private ObservableCollection<AnalyticalAccount> _ListOfAnalyticalAccount;

        public ObservableCollection<AnalyticalAccount> ListOfAnalyticalAccount
        {
            get => _ListOfAnalyticalAccount;
            set
            {
                _ListOfAnalyticalAccount = value;
                RaisePropertyChanged("ListOfAnalyticalAccount");
            }
        }

        private ObservableCollection<Region> _ListOfRegion;

        public ObservableCollection<Region> ListOfRegion
        {
            get => _ListOfRegion;
            set
            {
                _ListOfRegion = value;
                RaisePropertyChanged("ListOfRegion");
            }
        }

        private bool _DisplaySiteDetail;
        public bool DisplaySiteDetail
        {
            get => _DisplaySiteDetail;
            set
            {
                _DisplaySiteDetail = value;
                RaisePropertyChanged("DisplaySiteDetail");
            }
        }

        private Site _ConecernedSite;
        public Site ConecernedSite
        {
            get => _ConecernedSite;
            set
            {
                _ConecernedSite = value;
                UpdateButtonVisibilities();
                RaisePropertyChanged("ConecernedSite");
            }
        }

        private bool _DisplayBuildingDetail;
        public bool DisplayBuildingDetail
        {
            get => _DisplayBuildingDetail;
            set
            {
                _DisplayBuildingDetail = value;
                RaisePropertyChanged("DisplayBuildingDetail");
            }
        }

        private Building _ConecernedBuilding;
        public Building ConecernedBuilding
        {
            get => _ConecernedBuilding;
            set
            {
                _ConecernedBuilding = value;
                UpdateButtonVisibilities();
                RaisePropertyChanged("ConecernedBuilding");
            }
        }


        private bool _DisplayLevelDetail;
        public bool DisplayLevelDetail
        {
            get => _DisplayLevelDetail;
            set
            {
                _DisplayLevelDetail = value;

                RaisePropertyChanged("DisplayLevelDetail");
            }
        }

        private Level _ConecernedLevel;
        public Level ConecernedLevel
        {
            get => _ConecernedLevel;
            set
            {
                _ConecernedLevel = value;
                UpdateButtonVisibilities();
                RaisePropertyChanged(nameof(this.ConecernedLevel));
                RaisePropertyChanged(nameof(this.Offices));
            }
        }

        private bool _DisplayOfficeDetail;
        public bool DisplayOfficeDetail
        {
            get => _DisplayOfficeDetail;
            set
            {
                _DisplayOfficeDetail = value;
                RaisePropertyChanged("DisplayOfficeDetail");
                RaisePropertyChanged(nameof(OfficeCode));
            }
        }

        private Office _ConecernedOffice;
        public Office ConecernedOffice
        {
            get => _ConecernedOffice;
            set
            {
                _ConecernedOffice = value;
                UpdateButtonVisibilities();
                RaisePropertyChanged("ConecernedOffice");
            }
        }

        public string OfficeCode
        {
            get => this.ConecernedOffice?.Code;
            set
            {
                var code = value.Align(4, "0");
                this.ConecernedOffice.Code = code;
                var key = $"{this.ConecernedOffice.Level.Key}{code}";
                this.ConecernedOffice.Key = key;
                this.RaisePropertyChanged(nameof(this.OfficeCode));
            }
        }

        public ObservableCollection<Office> Offices
        {
            get => this.ConecernedLevel?.Offices.ToObservable();
        }

        #endregion

        #region Command



        private ICommand _CancelSiteCommand;

        public ICommand CancelSiteCommand
        {
            get => _CancelSiteCommand;
            set
            {
                _CancelSiteCommand = value;
                RaisePropertyChanged("CancelSiteCommand");
            }
        }



        private ICommand _CancelBuildingCommand;

        public ICommand CancelBuildingCommand
        {
            get => _CancelBuildingCommand;
            set { _CancelBuildingCommand = value; RaisePropertyChanged("CancelBuildingCommand"); }
        }


        private ICommand _CancelLevelCommand;

        public ICommand CancelLevelCommand
        {
            get => _CancelLevelCommand;
            set { _CancelLevelCommand = value; RaisePropertyChanged("CancelLevelCommand"); }
        }

        private ICommand _CancelOfficeCommand;

        public ICommand CancelOfficeCommand
        {
            get => _CancelOfficeCommand;
            set { _CancelOfficeCommand = value; RaisePropertyChanged("CancelOfficeCommand"); }
        }




        private ICommand _ValidateSiteCommand;
        public ICommand ValidateSiteCommand
        {
            get => _ValidateSiteCommand;
            set
            {
                _ValidateSiteCommand = value;
                RaisePropertyChanged("ValidateSiteCommand");
            }
        }

        private ICommand _ValidateBuildingCommand;
        public ICommand ValidateBuildingCommand
        {
            get => _ValidateBuildingCommand;
            set
            {
                _ValidateBuildingCommand = value;
                RaisePropertyChanged("ValidateBuildingCommand");
            }
        }

        private ICommand _ValidateLevelCommand;
        public ICommand ValidateLevelCommand
        {
            get => _ValidateLevelCommand;
            set
            {
                _ValidateLevelCommand = value;
                RaisePropertyChanged("ValidateLevelCommand");
            }
        }

        private ICommand _ValidateOfficeCommand;
        public ICommand ValidateOfficeCommand
        {
            get => _ValidateOfficeCommand;
            set
            {
                _ValidateOfficeCommand = value;
                RaisePropertyChanged("ValidateOfficeCommand");
            }
        }

        #endregion

        #region Methodes

        public void AddSite()
        {
            Site old = this.ConecernedSite;
            ValidateSiteCommand = new Command(() =>
            {
                this.UIMessage.TryDoAction(Logger, () =>
                    {
                        this.ConecernedSite.Code = LoacalizationTools.NormelizeCode(this.ConecernedSite.Code);
                        this.ConecernedSite.Key = this.CurrentRegion.Key + this.ConecernedSite.Code;
                        SiteService.Add(this.ConecernedSite);
                        this.ListOfRows.Add(this.ConecernedSite);
                        this.DisplaySiteDetail = false;
                        this.RaisePropertyChanged("");
                    }, this.UpdateButtonVisibilities);
                UpdateButtonVisibilities();
            });
            CancelSiteCommand = new Command(() =>
            {
                this.ConecernedSite = old;
                this.DisplaySiteDetail = false;
            });

            ConecernedSite = new Site()
            {
                Region = CurrentRegion
            };
            this.DisplaySiteDetail = true;

        }

        public void DeleteSite()
        {

            System.Windows.MessageBoxResult result = this.UIMessage.Warning("Etes vous sur de vouloir supprimer ce site ?");
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                this.UIMessage.TryDoAction(Logger, () =>
                {
                    SiteService.Delete(this.ConecernedSite);
                    this.ListOfRows.Remove(this.ConecernedSite);
                    this.ConecernedSite = ListOfRows.FirstOrDefault();
                });
                UpdateButtonVisibilities();
            }

        }

        public void EditSite()
        {
            if (ConecernedSite == null)
                return;
            this.DisplaySiteDetail = true;
            this.ValidateSiteCommand = new Command(() =>
                {
                    this.UIMessage.TryDoAction(Logger, () =>
                    {
                        SiteService.Update(this.ConecernedSite);
                        this.DisplaySiteDetail = false;
                    });
                });
            UpdateButtonVisibilities();
        }

        public void AddBuilding()
        {
            Building old = this.ConecernedBuilding;
            CancelBuildingCommand = new Command(() =>
                {
                    this.ConecernedBuilding = old;
                    this.DisplayBuildingDetail = false;
                });

            ConecernedBuilding = new Building()
            {
                Site = this.ConecernedSite
            };
            this.DisplayBuildingDetail = true;

            ValidateBuildingCommand = new Command(() =>
            {
                this.UIMessage.TryDoAction(Logger, () =>
                {
                    this.ConecernedBuilding.Code = LoacalizationTools.NormelizeCode(this.ConecernedBuilding.Code);
                    this.ConecernedBuilding.Key = this.ConecernedSite.Code + this.ConecernedBuilding.Code;
                    BuildingService.Add(this.ConecernedBuilding);
                    if (ConecernedSite.Buildings == null)
                    {
                        this.ConecernedSite.Buildings = new System.Collections.ObjectModel.ObservableCollection<Building>();
                    }
                    else
                    {
                        this.ConecernedSite.Buildings = new System.Collections.ObjectModel.ObservableCollection<Building>(ConecernedSite.Buildings);
                    }
                    this.ConecernedSite.Buildings.Add(this.ConecernedBuilding);
                    this.DisplayBuildingDetail = false;
                    this.RaisePropertyChanged("ConecernedSite");
                    this.RaisePropertyChanged("");
                }, this.UpdateButtonVisibilities);
            });
        }

        public void DeleteBuilding()
        {
            System.Windows.MessageBoxResult result = this.UIMessage.Warning("Etes vous sur de vouloir supprimer ce batiement ?");
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                this.UIMessage.TryDoAction(Logger, () =>
                {
                    BuildingService.Delete(this.ConecernedBuilding);
                    this.ConecernedSite.Buildings.Remove(this.ConecernedBuilding);
                });
                UpdateButtonVisibilities();
            }
        }

        public void EditBuilding()
        {
            if (ConecernedBuilding == null)
                return;
            this.DisplayBuildingDetail = true;
            this.ValidateBuildingCommand = new Command(() =>
            {
                this.UIMessage.TryDoAction(Logger, () =>
                {
                    BuildingService.Update(this.ConecernedBuilding);
                    this.DisplayBuildingDetail = false;
                }, this.UpdateButtonVisibilities);
            });
            UpdateButtonVisibilities();
        }

        public void AddLevel()
        {
            Level old = ConecernedLevel;
            CancelLevelCommand = new Command(() =>
                {
                    this.ConecernedLevel = old;
                    this.DisplayLevelDetail = false;
                });
            ConecernedLevel = new Level()
            {
                Building = this.ConecernedBuilding
            };
            this.DisplayLevelDetail = true;

            ValidateLevelCommand = new Command(() =>
            {
                this.UIMessage.TryDoAction(Logger, () =>
                    {
                        this.DisplayLevelDetail = false;
                        this.ConecernedLevel.Code = LoacalizationTools.NormelizeCode(this.ConecernedLevel.Code);
                        this.ConecernedLevel.Key = this.ConecernedBuilding.Code + this.ConecernedLevel.Code;
                        LevelService.Add(this.ConecernedLevel);
                        if (this.ConecernedBuilding.Levels == null)
                        {
                            this.ConecernedBuilding.Levels = new System.Collections.ObjectModel.ObservableCollection<Level>();
                        }
                        else
                        {
                            this.ConecernedBuilding.Levels = new System.Collections.ObjectModel.ObservableCollection<Level>(this.ConecernedBuilding.Levels);
                        }
                        this.ConecernedBuilding.Levels.Add(this.ConecernedLevel);
                        RaisePropertyChanged("ConecernedBuilding");
                    }, this.UpdateButtonVisibilities);
                UpdateButtonVisibilities();
            });
        }



        public void DeleteLevel()
        {
            System.Windows.MessageBoxResult result = this.UIMessage.Warning("Etes vous sur de vouloir supprimer cet etage ?");
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                this.UIMessage.TryDoAction(Logger, () =>
                {
                    LevelService.Delete(this.ConecernedLevel);
                    this.ConecernedBuilding.Levels.Remove(this.ConecernedLevel);
                });
                UpdateButtonVisibilities();
            }
        }

        public void EditLevel()
        {

            if (ConecernedLevel == null)
                return;
            this.DisplayLevelDetail = true;
            this.ValidateLevelCommand = new Command(() =>
            {
                this.UIMessage.TryDoAction(Logger, () =>
                {
                    LevelService.Update(this.ConecernedLevel);
                    this.DisplayLevelDetail = false;
                });
            });
            UpdateButtonVisibilities();
        }

        public void AddOffice()
        {
            if ((ListOfAnalyticalAccount == null) || (ListOfAnalyticalAccount.Count == 0))
            {
                this.UIMessage.Error("Veuiller creer un compte analytique avant de continuer SVP");
                return;
            }

            if (ConecernedLevel == null)
            {
                this.UIMessage.Error("Veuillez Selectionner un niveau SVP");
                return;
            }

            string result = string.Empty;
            if (ConecernedLevel.Offices == null)
            {
                result = "0001";
            }
            else
            {
                result = (ConecernedLevel.Offices.Count() + 1).ToString();
            }

            string code = LoacalizationTools.NormelizeOfficeCode(result);

            ConecernedOffice = new Office()
            {
                Code = code,
                Level = this.ConecernedLevel,
            };
            this.DisplayOfficeDetail = true;

            ValidateOfficeCommand = new Command(() =>
            {
                this.UIMessage.TryDoAction(Logger, () =>
                    {
                        if (ConecernedOffice.AnalyticalAccount == null)
                        {
                            this.UIMessage.Error("Veuillez selectionner un compte analytique SVP ");
                        }
                        else
                        {
                            this.ConecernedOffice.Code = LoacalizationTools.NormelizeOfficeCode(this.ConecernedOffice.Code);
                            this.ConecernedOffice.Key = $"{ConecernedLevel.Key}{this.ConecernedOffice.Code}";
                            OfficeService.Add(this.ConecernedOffice);
                            if (this.ConecernedLevel.Offices == null)
                            {
                                this.ConecernedLevel.Offices = new ObservableCollection<Office>();
                            }
                            else
                            {
                                this.ConecernedLevel.Offices = new ObservableCollection<Office>(this.ConecernedLevel.Offices);
                            }
                            this.ConecernedLevel.Offices.Add(this.ConecernedOffice);
                            this.DisplayOfficeDetail = false;
                            this.RaisePropertyChanged(nameof(this.Offices));
                            UpdateButtonVisibilities();
                        }
                    }, this.UpdateButtonVisibilities);

            });
        }

        public void DeleteOffice()
        {
            System.Windows.MessageBoxResult result = this.UIMessage.Warning("Etes vous sur de vouloir supprimer ce local ?");
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                this.UIMessage.TryDoAction(Logger, () =>
                {
                    OfficeService.Delete(this.ConecernedOffice);
                    this.ConecernedLevel.Offices.Remove(this.ConecernedOffice);
                    this.RaisePropertyChanged(nameof(this.Offices));
                });
                UpdateButtonVisibilities();
            }
        }

        public void EditOffice()
        {
            if (ConecernedOffice != null)
            {
                this.DisplayOfficeDetail = true;
                this.ValidateOfficeCommand = new Command(() =>
                {
                    this.UIMessage.TryDoAction(Logger, () =>
                    {
                        OfficeService.Update(this.ConecernedOffice);
                        this.RaisePropertyChanged(nameof(this.Offices));
                        this.DisplayOfficeDetail = false;
                    });
                });
            }
            UpdateButtonVisibilities();
        }

        public void UpdateButtonVisibilities()
        {

            SiteDeleteRibbonButton.IsEnabled = ConecernedSite != null && (ConecernedSite.Buildings == null || (ConecernedSite.Buildings != null) && (ConecernedSite.Buildings.Count == 0));
            SiteEditRibbonButton.IsEnabled = ConecernedSite != null;
            BuildingAddNewRibbonButton.IsEnabled = ConecernedSite != null;

            BuildingDeleteRibbonButton.IsEnabled = (ConecernedBuilding != null) && (ConecernedBuilding.Levels == null || (ConecernedBuilding.Levels != null && ConecernedBuilding.Levels.Count == 0));
            BuildingEditRibbonButton.IsEnabled = LevelAddNewRibbonButton.IsEnabled = this.ConecernedBuilding != null;

            LevelDeleteRibbonButton.IsEnabled = ConecernedLevel != null && (ConecernedLevel.Offices == null || (ConecernedLevel.Offices != null && ConecernedLevel.Offices.Count == 0));
            LevelEditRibbonButton.IsEnabled = OfficeAddNewRibbonButton.IsEnabled = this.ConecernedLevel != null;


            OfficeDeleteRibbonButton.IsEnabled = ConecernedOffice != null;
            OfficeEditRibbonButton.IsEnabled = ConecernedOffice != null;
        }

        public void InitRibbonCommand()
        {
            SiteAddNewRibbonButton = new RibbonButton() { Caption = "Ajouter", LargeGlyph = IconProvider.AddItem, Action = AddSite };
            SiteEditRibbonButton = new RibbonButton() { Caption = "Modifier", LargeGlyph = IconProvider.Edit, Action = EditSite };
            SiteDeleteRibbonButton = new RibbonButton() { Caption = "Supprimer", LargeGlyph = IconProvider.Delete, Action = DeleteSite };

            BuildingAddNewRibbonButton = new RibbonButton() { Caption = "Ajouter", LargeGlyph = IconProvider.AddItem, Action = AddBuilding };
            BuildingEditRibbonButton = new RibbonButton() { Caption = "Modifier", LargeGlyph = IconProvider.Edit, Action = EditBuilding };
            BuildingDeleteRibbonButton = new RibbonButton() { Caption = "Supprimer", LargeGlyph = IconProvider.Delete, Action = DeleteBuilding };

            LevelAddNewRibbonButton = new RibbonButton() { Caption = "Ajouter", LargeGlyph = IconProvider.AddItem, Action = AddLevel };
            LevelEditRibbonButton = new RibbonButton() { Caption = "Modifier", LargeGlyph = IconProvider.Edit, Action = EditLevel };
            LevelDeleteRibbonButton = new RibbonButton() { Caption = "Supprimer", LargeGlyph = IconProvider.Delete, Action = DeleteLevel };

            OfficeAddNewRibbonButton = new RibbonButton() { Caption = "Ajouter", LargeGlyph = IconProvider.AddItem, Action = AddOffice };
            OfficeEditRibbonButton = new RibbonButton() { Caption = "Modifier", LargeGlyph = IconProvider.Edit, Action = EditOffice };
            OfficeDeleteRibbonButton = new RibbonButton() { Caption = "Supprimer", LargeGlyph = IconProvider.Delete, Action = DeleteOffice };
        }

        #endregion

        #region RibbonCommand

        public RibbonButton SiteAddNewRibbonButton { get; set; }

        public RibbonButton SiteEditRibbonButton { get; set; }

        public RibbonButton SiteDeleteRibbonButton { get; set; }

        public RibbonButton BuildingAddNewRibbonButton { get; set; }

        public RibbonButton BuildingEditRibbonButton { get; set; }

        public RibbonButton BuildingDeleteRibbonButton { get; set; }

        public RibbonButton LevelAddNewRibbonButton { get; set; }

        public RibbonButton LevelEditRibbonButton { get; set; }

        public RibbonButton LevelDeleteRibbonButton { get; set; }

        public RibbonButton OfficeAddNewRibbonButton { get; set; }

        public RibbonButton OfficeEditRibbonButton { get; set; }

        public RibbonButton OfficeDeleteRibbonButton { get; set; }



        #endregion
    }
}