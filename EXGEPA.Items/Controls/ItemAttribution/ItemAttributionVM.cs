using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using CORESI.IoC;
using CORESI.Tools;
using CORESI.WPF.Controls;
using CORESI.WPF.Model;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Items.Controls
{
    public class ItemAttributionVM : GenericEditableViewModel<Item>
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IRepositoryDataProvider RepositoryDataProvider { get; set; }
        public IUIItemService UIItemService { get; set; }
        public ICommand SetCommand { get; set; }
        public ICommand SetAllCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand ResetAllCommand { get; set; }



        public ICommand AffectedRowDoubleClick { get; set; }

        public ItemAttributionOptions Options { get; private set; }

        private ItemAttributionVM() : base()
        {
            this.UIItemService = ServiceLocator.Resolve<IUIItemService>();
            this.RepositoryDataProvider = ServiceLocator.Resolve<IRepositoryDataProvider>();
            this.EnableTotalSumary = true;
            this.AutoWidth = false;
            this.AffectedRows = new ObservableCollection<Item>();
            this.AffectedRowsSelection = new ObservableCollection<Item>();
            this.SetCommand = new Command(this.MoveSelectionToRight);
            this.ResetCommand = new Command(this.MoveSelectionToLeft);
            this.ResetAllCommand = new Command(this.MoveAllToLeft);
            SetToolGroup();

            var command = new Command(() =>
            {
                this.UIItemService.EditItem(this.SelectedRow);
            });

            command.SetAbility<Item>("Modifier");
            RowDoubleClickCommand = command;

            var command2 = new Command(() =>
            {
                this.UIItemService.EditItem(this.AffectedRowsSelection?.FirstOrDefault());
            });

            command.SetAbility<Item>("Modifier");
            AffectedRowDoubleClick = command2;

            this.MoveAllToLeftButtonVisibility = this.ParameterProvider.TryGet(nameof(this.MoveAllToLeftButtonVisibility), false) ? Visibility.Visible : Visibility.Collapsed;
        }


        public ItemAttributionVM(ItemAttributionOptions options, ItemAttributionView view) : this()
        {
            this.Options = options;
            this.Caption = options.PageCaption;
            this.Categorie = options.Categorie;
            this.SetExportGroup(view);
            if (options.Groups != null)
            {
                options.Groups.ForEach(group => this.Groups.Add(group));
            }
        }

        private string _LeftPanelCaption;
        public string LeftPanelCaption
        {
            get => _LeftPanelCaption;
            set
            {
                _LeftPanelCaption = value;
                RaisePropertyChanged("LeftPanelCaption");
            }
        }

        private Visibility _MoveAllToleftButtonVisibility;

        public Visibility MoveAllToLeftButtonVisibility
        {
            get { return _MoveAllToleftButtonVisibility; }
            set
            {
                RaisePropertyChanged(nameof(this.MoveAllToLeftButtonVisibility));
                _MoveAllToleftButtonVisibility = value;
            }
        }


        private string _RightPanelCaption;
        public string RightPanelCaption
        {
            get => _RightPanelCaption;
            set
            {
                _RightPanelCaption = value;
                RaisePropertyChanged("RightPanelCaption");
            }
        }

        private ObservableCollection<Item> _AffectedRows;
        public ObservableCollection<Item> AffectedRows
        {
            get => _AffectedRows;
            set
            {
                _AffectedRows = value;
                RaisePropertyChanged("AffectedRows");
            }
        }

        private ObservableCollection<Item> _AffectedRowsSelection;
        public ObservableCollection<Item> AffectedRowsSelection
        {
            get => _AffectedRowsSelection;
            set
            {
                _AffectedRowsSelection = value;
                RaisePropertyChanged("AffectedRowsSelection");
            }
        }

        void MoveSelectionToRight()
        {
            this.MoveItemToRight(this.Selection.Take(1).ToList());
        }

        void MoveSelectionToLeft()
        {
            this.MoveItemToLeft(this.AffectedRowsSelection.Take(1).ToList());
        }

        void MoveAllToLeft()
        {
            this.MoveItemToLeft(this.AffectedRows.ToList());
        }

        void MoveItemToRight(List<Item> items)
        {
            if (items.Count > 0)
            {
                ConfirmeAndStartBackGroundAction(Options.SetConfirmationMessage, () => items.ForEach(item =>
                 {
                     this.Options.Setter(item);
                     this.DBservice.Update(item);
                     ListOfRows.Remove(item);
                     AffectedRows.Add(item);
                 }));
            }
            else
            {
                this.UIMessage.Warning("la selection est vide !");
            }
        }

        void MoveItemToLeft(List<Item> items)
        {
            if (items.Count > 0)
            {
                ConfirmeAndStartBackGroundAction(Options.ResetConfirmationMessage, () => items.ForEach(item =>
              {
                  this.Options.Resetter(item);
                  this.DBservice.Update(item);
                  AffectedRows.Remove(item);
                  ListOfRows.Add(item);
              }));
            }
            else
            {
                this.UIMessage.Warning("la selection est vide !");
            }
        }

        public override void InitData()
        {
            StartBackGroundAction(() =>
           {
               using (ScoopLogger scoopLooger = new ScoopLogger("Loading items", logger))
               {
                   var allItems = this.DBservice.All;

                   if (this.Options.RowFilter != null)
                   {
                       allItems = allItems.Where(item => this.Options.RowFilter(item)).ToList();
                   }

                   RepositoryDataProvider.Refresh();
                   scoopLooger.Snap("Loading raw data");
                   RepositoryDataProvider.BindPropertyAndSetExtended(allItems);
                   scoopLooger.Snap("Binding data");
                   List<Item> affectedRows = allItems.Where(item => this.Options.Tester(item)).ToList();
                   IEnumerable<Item> otherRows = allItems.Except(affectedRows).Where(x => x.OutputCertificate == null);
                   this.ListOfRows = new ObservableCollection<Item>(otherRows);
                   this.AffectedRows = new ObservableCollection<Item>(affectedRows);
               }
           });
        }
    }
}
