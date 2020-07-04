using CORESI.WPF.Core.Framework;
using CORESI.WPF.Model;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CORESI.WPF.Controls
{
    public class BasicGridViewModel<T> : PageViewModel
    {
        private ObservableCollection<SimpleButton> _ContextMenuItems;
        public ObservableCollection<SimpleButton> ContextMenuItems
        {
            get => _ContextMenuItems;
            set
            {
                _ContextMenuItems = value;
                RaisePropertyChanged("ContextMenuItems");
            }
        }
        public bool EnableTotalSumary { get; set; }
        public BasicGridViewModel() : base()
        {
            this.AutoWidth = true;
            RowDoubleClickCommand = new Command(() =>
            {
                DoubleClicAction?.Invoke(SelectedRow);
            });
        }
        #region Properties

        public string TypeName => typeof(T).Name;

        public T SelectedRow => Selection.FirstOrDefault();


        private ObservableCollection<T> _Selection = new ObservableCollection<T>();

        public ObservableCollection<T> Selection
        {
            get => _Selection;
            set
            {
                _Selection = value;
                RaisePropertyChanged("Selection");
                RaisePropertyChanged("SelectedRow");
            }
        }



        private bool _ShowLoadingPanel;

        public bool ShowLoadingPanel
        {
            get => _ShowLoadingPanel;
            set
            {
                _ShowLoadingPanel = value;
                RaisePropertyChanged("ShowLoadingPanel");
            }
        }

        protected ObservableCollection<T> _ListOfRows;

        public ObservableCollection<T> ListOfRows
        {
            get => _ListOfRows;
            set
            {
                _ListOfRows = value;
                RaisePropertyChanged("ListOfRows");
            }
        }

        #endregion
        #region Command

        public ICommand RowDoubleClickCommand { get; set; }

        #endregion
        #region Action
        public Action<T> DoubleClicAction { get; set; }

        #endregion

        private bool _ShowTotalSummary;

        public bool ShowTotalSummary
        {
            get => _ShowTotalSummary;
            set
            {
                _ShowTotalSummary = value;
                RaisePropertyChanged("ShowTotalSummary");
            }
        }

        private bool _ShowColumnFilter;

        public bool ShowColumnFilter
        {
            get => _ShowColumnFilter;
            set
            {
                _ShowColumnFilter = value;
                RaisePropertyChanged("ShowColumnFilter");
            }
        }

        private bool _ShowGroupPanel;

        public bool ShowGroupPanel
        {
            get => _ShowGroupPanel;
            set
            {
                _ShowGroupPanel = value;
                RaisePropertyChanged("ShowGroupPanel");
            }
        }

        private bool _ShowSysIdColumn;

        public bool ShowSysIdColumn
        {
            get => _ShowSysIdColumn;
            set
            {
                _ShowSysIdColumn = value;
                RaisePropertyChanged("ShowSysIdColumn");
            }
        }

        private bool _AutoWidth;

        public bool AutoWidth
        {
            get => _AutoWidth;
            set
            {
                _AutoWidth = value;
                RaisePropertyChanged("AutoWidth");
            }
        }
    }
}