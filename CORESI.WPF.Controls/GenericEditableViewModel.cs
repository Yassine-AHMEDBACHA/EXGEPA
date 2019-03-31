using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Core;
using CORESI.WPF.Core.Interfaces;
using CORESI.WPF.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using EXGEPA.Core.Tools;

namespace CORESI.WPF.Controls
{
    public abstract class GenericEditableViewModel<T> : GenericExportableViewModel<T>
        where T : KeyRow, ICloneable
    {
        public GenericEditableViewModel(IExportableGrid exportableView, bool loadData = true)
            : this()
        {
            this.exportableView = exportableView;
            this.InitilizeRibbonGroup(exportableView);

            if (loadData)
            {
                this.InitData();
            }
        }

        public GenericEditableViewModel()
            : base()
        {
            this.ToolGroup = this.AddNewGroup("Outils");
            IParameterProvider parameterProvider = ServiceLocator.Resolve<IParameterProvider>();
            this.DBservice = ServiceLocator.Resolve<IDataProvider<T>>();
            this.KeyLength = parameterProvider.GetValue(typeof(T).Name + "KeyLength", 6);
            ConcernedRow = default;
            CancelCommand = new Command(() =>
            {
                this.DisplayDetail = false;
            });
            this.ListOfRows = new ObservableCollection<T>();
            DoubleClicAction = EditItem;
            Command command = new Command(() =>
            {
                if ((DoubleClicAction) != null)
                    DoubleClicAction();
            });

            command.SetAbility<T>("Modifier");
            RowDoubleClickCommand = command;
            NotifyUpdate += this.Refresh;
        }

        public static Action<object, T> NotifyUpdate { get; set; }

        public int KeyLength { get; set; }

        public IKeyGenerator<T> KeyGenerator { get; private set; }

        public IDataProvider<T> DBservice { get; set; }

        public ICommand CancelCommand { get; private set; }

        public ICommand ValidateCommand { get; set; }

        public new Action DoubleClicAction { get; set; }

        public bool HideEditButton { get; set; }

        public bool HideAddButton { get; set; }

        public bool HideDeleteButton { get; set; }

        public Group ToolGroup { get; set; }

        public virtual void InitData()
        {
            StartBackGroundAction(() =>
            {
                this.ListOfRows = new ObservableCollection<T>(this.DBservice.SelectAll());
            });
        }

        public virtual void Refresh(object sender, T value)
        {
            if (sender != this)
            {
                RefreshView(value);
            }
        }

        protected void RefreshView(T value)
        {
            T oldValue = ListOfRows.FirstOrDefault(x => x.Id == value.Id);
            int index = ListOfRows.IndexOf(oldValue);
            if (index < 0)
            {
                ListOfRows.Add(value);
            }
            else
            {
                ListOfRows[index] = value;
            }
        }

        private bool _DisplayDetail;

        public bool DisplayDetail
        {
            get => _DisplayDetail;
            set
            {
                _DisplayDetail = value;
                RaisePropertyChanged("DisplayDetail");
            }
        }

        private T _ConcernedRow;
        public T ConcernedRow
        {
            get => _ConcernedRow;
            set
            {
                _ConcernedRow = value;
                RaisePropertyChanged("ConcernedRow");
            }
        }

        protected T OldValues { get; set; }

        private string _Filter;

        public string Filter
        {
            get => _Filter;
            set
            {
                _Filter = value;
                RaisePropertyChanged("Filter");
            }
        }

        public virtual void RaiseDataChanged(T value)
        {
            NotifyUpdate?.Invoke(this, ConcernedRow);
        }

        public virtual void EditItem()
        {
            if (this.SelectedRow != null)
            {
                this.OldValues = this.SelectedRow;
                this.ConcernedRow = (T)SelectedRow.Clone();
                RaisePropertyChanged("");
                this.DisplayDetail = true;
                this.ValidateCommand = new Command(() =>
                {
                    this.StartBackGroundAction(() =>
                    {
                        UpdateItem();
                    }, null, true);
                });
                RaisePropertyChangedForEditionPanel();
            }
        }

        public virtual void UpdateItem()
        {
            DBservice.Update(ConcernedRow);
            this.DisplayDetail = false;
            RefreshView(ConcernedRow);
            RaiseDataChanged(ConcernedRow);
        }

        public void StartBackGroundAction(Action actionTodDo, Action FinalAction = null, bool showLoadingPanel = true)
        {
            this.ShowLoadingPanel = showLoadingPanel;
            this.UIMessage.TryDoActionAsync(
                this.Logger,
                actionTodDo,
                () =>
                {
                    this.ShowLoadingPanel = false;
                    FinalAction?.Invoke();
                });
        }

        public void StartUIBackGroundAction(Action actionTodDo, bool showLoadingPanel = true)
        {
            this.ShowLoadingPanel = showLoadingPanel;
            this.UIMessage.TryDoUIActionAsync(this.Logger, actionTodDo, () => this.ShowLoadingPanel = false);
        }

        public void ConfirmeAndStartBackGroundAction(string ConfirmationMessage, Action actionTodDo, bool showLoadingPanel = true)
        {
            this.ShowLoadingPanel = showLoadingPanel;
            this.UIMessage.ConfirmeAndTryDoAction(this.Logger, ConfirmationMessage, actionTodDo, true, () =>
            {
                this.ShowLoadingPanel = false;
            });
        }

        public virtual void AddItem()
        {
            ConcernedRow = GetNewInstance();
            this.ValidateCommand = new Command(() => AddItem(ConcernedRow));
            RaisePropertyChangedForEditionPanel();
            this.DisplayDetail = true;
        }

        private T GetNewInstance()
        {
            T newInstance = Activator.CreateInstance<T>();
            newInstance.Key = this.GetTemporaryKey();

            if (newInstance is IDatable databaleInstance)
            {
                databaleInstance.Date = DateTime.Today;
            }

            return newInstance;
        }

        public virtual void RaisePropertyChangedForEditionPanel()
        {
            RaisePropertyChanged("ValidateCommand");
        }

        public virtual void AddItem(T RowToInsert)
        {
            StartBackGroundAction(() =>
            {
                DBservice.Add(ConcernedRow);
                this.DisplayDetail = false;
                RefreshView(ConcernedRow);
                RaiseDataChanged(ConcernedRow);
            });
        }

        public virtual void DeleteItem()
        {
            ConfirmeAndStartBackGroundAction("Etes vous sûr de vouloir supprimer cette ligne ? ",
            () =>
            {
                this.ShowLoadingPanel = true;
                ConcernedRow = SelectedRow;
                DBservice.Delete(SelectedRow);
                this.ListOfRows.Remove(SelectedRow);
            });
        }

        protected virtual void InitilizeRibbonGroup(IExportableGrid view = null)
        {
            this.SetToolGroup();
            if (view != null)
            {
                this.SetExportGroup(view);
            }

            this.SetEditionGroup();
        }

        protected virtual void SetToolGroup()
        {
            this.ToolGroup.AddCommand("Refresh", IconProvider.Refresh, this.InitData).SetAbility<T>();
            this.ToolGroup.AddCommand("Filtre", IconProvider.Filtre, () => this.ShowColumnFilter = !ShowColumnFilter).SetAbility<T>();
            this.ToolGroup.AddCommand("Groupement", IconProvider.GroupRows, () => this.ShowGroupPanel = !ShowGroupPanel).SetAbility<T>();
            if (this.EnableTotalSumary)
            {
                this.TryAddSummaryButton();
            }

            ToolGroup.AddCommand(new CheckedRibbonButton()
            {
                IsChecked = this.AutoWidth,
                Caption = "Largeur automatique",
                LargeGlyph = IconProvider.FloatindWidth,
                Action = () => this.AutoWidth = !AutoWidth
            }).SetAbility<T>();

            ToolGroup.AddCommand<CheckedRibbonButton>("System ID", IconProvider.FindByID, () => this.ShowSysIdColumn = !ShowSysIdColumn).SetAbility<T>();
        }

        public void TryAddSummaryButton()
        {
            this.ToolGroup.AddCommand<CheckedRibbonButton>("Totaux", IconProvider.Summary, () => this.ShowTotalSummary = !ShowTotalSummary).IsChecked = ShowTotalSummary;
        }

        public virtual string GetTemporaryKey()
        {
            int lastId = 1;
            if (this.ListOfRows != null && this.ListOfRows.Count > 0)
            {
                lastId = this.ListOfRows.Max(x => x.Id);
                while (this.ListOfRows.Any(x => x.Id == lastId))
                {
                    lastId++;
                }
            }

            return KeyLengthNormalizer.Normalize(lastId.ToString(), this.KeyLength);
        }
        protected virtual void SetEditionGroup()
        {
            Group group = new Group("Edition");
            if (!HideEditButton)
            {
                group.AddCommand("Modifier", IconProvider.Edit, this.EditItem).SetAbility<T>();
            }

            if (!HideAddButton)
            {
                group.AddCommand("Ajouter", IconProvider.AddItem, this.AddItem).SetAbility<T>();
            }

            if (!HideDeleteButton)
            {
                group.AddCommand("Supprimer", IconProvider.Delete, this.DeleteItem).SetAbility<T>();
            }

            this.Groups.Insert(0, group);
        }
    }
}
