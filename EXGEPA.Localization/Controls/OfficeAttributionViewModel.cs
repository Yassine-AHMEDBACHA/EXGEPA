using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Model;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Input;

namespace EXGEPA.Localization.Controls
{
    public class OfficeAttributionViewModel : GenericEditableViewModel<Office>
    {


        public ICommand SetCommand { get; set; }
        public ICommand SetAllCommand { get; set; }
        public ICommand ResetCommand { get; set; }
        public ICommand ResetAllCommand { get; set; }

        

        public Func<Office, bool> ItemTester { get; set; }


        private ObservableCollection<Office> _AffectedRows;
        public ObservableCollection<Office> AffectedRows
        {
            get { return _AffectedRows; }
            set
            {
                _AffectedRows = value;
                RaisePropertyChanged("AffectedRows");
            }
        }

        private ObservableCollection<Office> _AffectedRowsSelection;
        public ObservableCollection<Office> AffectedRowsSelection
        {
            get { return _AffectedRowsSelection; }
            set
            {
                _AffectedRowsSelection = value;
                RaisePropertyChanged("AffectedRowsSelection");
            }
        }


        public OfficeAttributionViewModel(PropertyInfo property, object value, object defaultValue)
        {
            
            this.DBservice = ServiceLocator.Resolve<IDataProvider<Office>>();
          
            this.InitilizeRibbonGroup();
            this.TryAddSummaryButton();
            object locker = new object();
            Func<Office, bool> itemTester;
            if (typeof(KeyRow).IsAssignableFrom(property.PropertyType))
                itemTester = (item) =>
                 {
                     var result = property.GetValue(item, null);
                     if ((value != null) && (result != null))
                     {
                         return ((KeyRow)value).Id == ((KeyRow)result).Id;
                     }
                     else
                         return false;
                 };
            else
                itemTester = (item) =>
                  {
                      var result = property.GetValue(item, null);
                      return value.Equals(result);
                  };
            Func<Office, bool> set = (Office) =>
            {
                property.SetValue(Office, value, null);
                return DBservice.Update(Office) > 0;
            };
            Func<Office, bool> reset = (Office) =>
            {

                property.SetValue(Office, defaultValue, null);
                return DBservice.Update(Office) > 0;

            };


            this.EnableTotalSumary = true;
            this.AutoWidth = false;
            this.AffectedRows = new ObservableCollection<Office>();
            this.AffectedRowsSelection = new ObservableCollection<Office>();
            SetCommand = new Command(() =>
                {
                    ConfirmeAndStartBackGroundAction("Etes vous sûr de vouloir deplacer ces items ?", () =>
                    {
                        var rows = Selection.ToList();
                        SetRows(locker, set, rows);
                    });
                });
            ResetAllCommand = new Command(() =>
                {
                    ConfirmeAndStartBackGroundAction("Etes vous sûr de vouloir deplacer ces items ?", () =>
                    {
                        var rows = AffectedRows.ToList();
                        ResetRows(locker, reset, rows);
                    });
                });

            ResetCommand = new Command(() =>
                {
                    ConfirmeAndStartBackGroundAction("Etes vous sûr de vouloir deplacer ces items ?", () =>
                        {
                            var rows = AffectedRowsSelection.ToList();
                            ResetRows(locker, reset, rows);
                        });
                });
            ItemTester = itemTester;
        }

        private void ResetRows(object locker, Func<Office, bool> reset, List<Office> rows)
        {
            rows.ForEach(office =>
            {
                if (reset(office))
                {
                    StartBackGroundAction(() =>
                    {
                        lock (locker)
                        {
                            this.AffectedRows.Remove(office);
                            this.ListOfRows.Add(office);
                            Thread.Sleep(2);


                        }
                    });
                }
            });
        }

        private void SetRows(object locker, Func<Office, bool> set, List<Office> rows)
        {
            rows.ForEach(office =>
            {
                if (set(office))
                {
                    StartBackGroundAction(() =>
                    {
                        lock (locker)
                        {
                            this.ListOfRows.Remove(office);
                            this.AffectedRows.Add(office);

                        }
                    });
                }
            });
        }




        public override void InitData()
        {
            StartBackGroundAction(() =>
            {
                AffectedRows.Clear();
                var list = DBservice.SelectAll();
                foreach (var office in list)
                {
                    if (ItemTester(office))
                    {
                        this.AffectedRows.Add(office);
                    }
                }
                AffectedRows.ToList().ForEach(item =>
                    {
                        list.Remove(item);
                    });
                ListOfRows = new ObservableCollection<Office>(list);
                RaisePropertyChanged("");
            });

        }
    }
}

