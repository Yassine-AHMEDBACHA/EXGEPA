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
            this.uIMessage.TryDoActionAsync(logger, () =>
            {
                this.ShowLoadingPanel = true;
                this.AnalyticalAccountService = ServiceLocator.Resolve<IDataProvider<AnalyticalAccount>>();
                InitRibbonCommand();
                var allRegion = RegionalService.SelectAll();
                var sites = this.SiteService.SelectAll();
                var allOffices = this.OfficeService.SelectAll();
                var allLevels = this.LevelService.SelectAll();
                var allBuildings = this.BuildingService.SelectAll();
                Core.LoacalizationTools.BindLocalization(allOffices, allLevels, sites, allBuildings, allRegion);
                CurrentRegion = sites.FirstOrDefault().Region;
                ListOfAnalyticalAccount = new ObservableCollection<AnalyticalAccount>(AnalyticalAccountService.SelectAll());
                foreach (var office in allOffices)
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
            this.uIMessage.TryDoUIActionAsync(logger, () =>
             {
                 IList<Item> items = null;
                 var offices = SelectedOffices.ToDictionary(x => x.Key);
                 var inventoryRows = InventoryRowService.SelectAll().Where(x => offices.ContainsKey(x.Localization)).ToList();
                 if (inventoryRows.Count > 0)
                 {
                     items = ItemService.SelectAll();
                     var allItems = items.Where(x => x.OutputCertificate == null).ToDictionary(x => x.Key, x => x);
                     items = inventoryRows.Select(x =>
                    {
                        Item item;
                        if (allItems.TryGetValue(x.Key, out item))
                        {
                            Office office;
                            offices.TryGetValue(x.Localization, out office);
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
            var officeIds = SelectedOffices.Select(x => x.Id).ToList();
            var items = ItemService.SelectAll();
            RepositoryDataProvider.BindItemFields(items);
            items = items.Where(x => x.OutputCertificate == null && officeIds.Contains(x.Office.Id)).ToList();
            OfficeInventoryReportProvider.PrintInventorySheet(items);
        }

        private Region _CurrentRegion;

        public Region CurrentRegion
        {
            get { return _CurrentRegion; }
            set
            {
                _CurrentRegion = value;
                RaisePropertyChanged("CurrentRegion");
            }
        }


        private ObservableCollection<AnalyticalAccount> _ListOfAnalyticalAccount;

        public ObservableCollection<AnalyticalAccount> ListOfAnalyticalAccount
        {
            get { return _ListOfAnalyticalAccount; }
            set
            {
                _ListOfAnalyticalAccount = value;
                RaisePropertyChanged("ListOfAnalyticalAccount");
            }
        }

        private ObservableCollection<Region> _ListOfRegion;

        public ObservableCollection<Region> ListOfRegion
        {
            get { return _ListOfRegion; }
            set
            {
                _ListOfRegion = value;
                RaisePropertyChanged("ListOfRegion");
            }
        }

        private bool _DisplaySiteDetail;
        public bool DisplaySiteDetail
        {
            get { return _DisplaySiteDetail; }
            set
            {
                _DisplaySiteDetail = value;
                RaisePropertyChanged("DisplaySiteDetail");
            }
        }

        private Site _ConecernedSite;
        public Site ConecernedSite
        {
            get
            {
                return _ConecernedSite;
            }
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
            get { return _DisplayBuildingDetail; }
            set
            {
                _DisplayBuildingDetail = value;
                RaisePropertyChanged("DisplayBuildingDetail");
            }
        }

        private Building _ConecernedBuilding;
        public Building ConecernedBuilding
        {
            get { return _ConecernedBuilding; }
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
            get { return _DisplayLevelDetail; }
            set
            {
                _DisplayLevelDetail = value;

                RaisePropertyChanged("DisplayLevelDetail");
            }
        }

        private Level _ConecernedLevel;
        public Level ConecernedLevel
        {
            get { return _ConecernedLevel; }
            set
            {
                _ConecernedLevel = value;
                UpdateButtonVisibilities();
                RaisePropertyChanged("ConecernedLevel");
            }
        }

        private bool _DisplayOfficeDetail;
        public bool DisplayOfficeDetail
        {
            get { return _DisplayOfficeDetail; }
            set
            {
                _DisplayOfficeDetail = value;
                RaisePropertyChanged("DisplayOfficeDetail");
            }
        }

        private Office _ConecernedOffice;
        public Office ConecernedOffice
        {
            get { return _ConecernedOffice; }
            set
            {
                _ConecernedOffice = value;
                UpdateButtonVisibilities();
                RaisePropertyChanged("ConecernedOffice");
            }
        }

        #endregion

        #region Command



        private ICommand _CancelSiteCommand;

        public ICommand CancelSiteCommand
        {
            get { return _CancelSiteCommand; }
            set
            {
                _CancelSiteCommand = value;
                RaisePropertyChanged("CancelSiteCommand");
            }
        }



        private ICommand _CancelBuildingCommand;

        public ICommand CancelBuildingCommand
        {
            get { return _CancelBuildingCommand; }
            set { _CancelBuildingCommand = value; RaisePropertyChanged("CancelBuildingCommand"); }
        }


        private ICommand _CancelLevelCommand;

        public ICommand CancelLevelCommand
        {
            get { return _CancelLevelCommand; }
            set { _CancelLevelCommand = value; RaisePropertyChanged("CancelLevelCommand"); }
        }

        private ICommand _CancelOfficeCommand;

        public ICommand CancelOfficeCommand
        {
            get { return _CancelOfficeCommand; }
            set { _CancelOfficeCommand = value; RaisePropertyChanged("CancelOfficeCommand"); }
        }




        private ICommand _ValidateSiteCommand;
        public ICommand ValidateSiteCommand
        {
            get { return _ValidateSiteCommand; }
            set
            {
                _ValidateSiteCommand = value;
                RaisePropertyChanged("ValidateSiteCommand");
            }
        }

        private ICommand _ValidateBuildingCommand;
        public ICommand ValidateBuildingCommand
        {
            get { return _ValidateBuildingCommand; }
            set
            {
                _ValidateBuildingCommand = value;
                RaisePropertyChanged("ValidateBuildingCommand");
            }
        }

        private ICommand _ValidateLevelCommand;
        public ICommand ValidateLevelCommand
        {
            get { return _ValidateLevelCommand; }
            set
            {
                _ValidateLevelCommand = value;
                RaisePropertyChanged("ValidateLevelCommand");
            }
        }

        private ICommand _ValidateOfficeCommand;
        public ICommand ValidateOfficeCommand
        {
            get { return _ValidateOfficeCommand; }
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
            var old = this.ConecernedSite;
            ValidateSiteCommand = new Command(() =>
            {
                this.uIMessage.TryDoAction(logger, () =>
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

            var result = this.uIMessage.Warning("Etes vous sur de vouloir supprimer ce site ?");
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                this.uIMessage.TryDoAction(logger, () =>
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
                    this.uIMessage.TryDoAction(logger, () =>
                    {
                        SiteService.Update(this.ConecernedSite);
                        this.DisplaySiteDetail = false;
                    });
                });
            UpdateButtonVisibilities();
        }

        public void AddBuilding()
        {
            var old = this.ConecernedBuilding;
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
                this.uIMessage.TryDoAction(logger, () =>
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
            var result = this.uIMessage.Warning("Etes vous sur de vouloir supprimer ce batiement ?");
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                this.uIMessage.TryDoAction(logger, () =>
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
                this.uIMessage.TryDoAction(logger, () =>
                {
                    BuildingService.Update(this.ConecernedBuilding);
                    this.DisplayBuildingDetail = false;
                }, this.UpdateButtonVisibilities);
            });
            UpdateButtonVisibilities();
        }

        public void AddLevel()
        {
            var old = ConecernedLevel;
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
                this.uIMessage.TryDoAction(logger, () =>
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
            var result = this.uIMessage.Warning("Etes vous sur de vouloir supprimer cet etage ?");
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                this.uIMessage.TryDoAction(logger, () =>
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
                this.uIMessage.TryDoAction(logger, () =>
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
                this.uIMessage.Error("Veuiller creer un compte analytique avant de continuer SVP");
                return;
            }

            if (ConecernedLevel == null)
            {
                this.uIMessage.Error("Veuillez Selectionner un niveau SVP");
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

            var code = LoacalizationTools.NormelizeOfficeCode(result);

            ConecernedOffice = new Office()
            {
                Code = code,
                Level = this.ConecernedLevel,
            };
            this.DisplayOfficeDetail = true;

            ValidateOfficeCommand = new Command(() =>
            {
                this.uIMessage.TryDoAction(logger, () =>
                    {
                        if (ConecernedOffice.AnalyticalAccount == null)
                        {
                            this.uIMessage.Error("Veuillez selectionner un compte analytique SVP ");
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
                            UpdateButtonVisibilities();
                        }
                    }, this.UpdateButtonVisibilities);

            });
        }

        public void DeleteOffice()
        {
            var result = this.uIMessage.Warning("Etes vous sur de vouloir supprimer ce local ?");
            if (result == System.Windows.MessageBoxResult.Yes)
            {
                this.uIMessage.TryDoAction(logger, () =>
                {
                    OfficeService.Delete(this.ConecernedOffice);
                    this.ConecernedLevel.Offices.Remove(this.ConecernedOffice);
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
                    this.uIMessage.TryDoAction(logger, () =>
                    {
                        OfficeService.Update(this.ConecernedOffice);
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