using System;
using System.Linq;
using CORESI.Tools.Collections;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Model;
using Newtonsoft.Json;

namespace EXGEPA.Items.Controls
{
    public class NewItemViewModel : ItemViewModelBase
    {
        public static Categorie NewItemRibbonCategorie { get; private set; }
        static NewItemViewModel()
        {
            NewItemRibbonCategorie = new Categorie()
            {
                Caption = "Ajouter un nouvel article",
                Color = System.Windows.Media.Color.FromRgb(246, 80, 88)
            };
        }

        public NewItemViewModel()
            : base()
        {
            this.Caption = "Nouvel Article";
            this.itemExtendedProperties = new ItemExtendedProperties();
            this.IsKeyReadOnly = this.parameterProvider.GetValue("IsItemKeyReadOnlyAtCreation", true);
            this.IsBaseDepreciationReadOnly = this.parameterProvider.GetAndSetIfMissing("IsBaseDepreciationReadOnlyAtCreation", false);
            this.Categorie = NewItemRibbonCategorie;
            this.IsSelected = true;
            Group group = this.AddNewGroup();
            group.AddCommand("Sauver & Fermer", IconProvider.SaveAndClose, this.AddNewItem);
            group = this.AddNewGroup();
            this.AccountingPeriods = group.AddCommand<ComboBoxRibbon<string>>("Exercice");
            this.Quantity = group.AddCommand<ComboBoxRibbon<int>>("Quantité");
            this.Quantity.Width = 80;
            int maxQuantityCanBeCreated = parameterProvider.GetAndSetIfMissing("MaxQuantityCanBeCreated", 100);
            Enumerable.Range(1, maxQuantityCanBeCreated).ForEach(x => this.Quantity.ItemsSource.Add(x));
            this.Quantity.EditValue = this.Quantity.ItemsSource.FirstOrDefault();
            this.UIMessage.TryDoActionAsync(Logger, this.InitData);
        }

        private void InitData()
        {
            InitialItem = new Item { PrintLabel = true };
            ConcernedItem = (Item)InitialItem.Clone();
            this.AquisitionDate = DateTime.Today;
            BindFields();
            this.AccountingPeriods.ItemsSource.Clear();
            foreach (string item in this.RepositoryDataProvider.ListOfAccountingPeriod.Where(x => !x.Approved).OrderBy(x => x.StartDate).Select(x => x.Key))
            {
                this.AccountingPeriods.ItemsSource.Add(item);
            }

            this.AccountingPeriods.EditValue = this.AccountingPeriods
                .ItemsSource
                .FirstOrDefault();
            this.IsTvaDepreciatible = true;
            this.RaisePropertyChanged();
        }

        private void AddNewItem()
        {
            string result = Core.ItemValidator.CheckItem(this.ConcernedItem);
            if (result != null)
            {
                this.UIMessage.Error(result);
                return;
            }

            this._SavePicture?.Invoke();

            this.ConcernedItem.Json = JsonConvert.SerializeObject(this.itemExtendedProperties);
            this.InsertItem(this.ConcernedItem);
            if (this.Quantity.EditValue > 1)
            {
                System.Collections.Generic.List<Item> ItemsToInsert = Enumerable.Range(0, this.Quantity.EditValue - 1).Select(x => (Item)ConcernedItem.Clone()).ToList();
                ItemsToInsert.ForEach(item => this.UIMessage.TryDoAction(this.Logger, () =>
                 {
                     item.Key = this.keyGenerator.GenerateKey(this.Reference, this.KeyLength);
                     this.InsertItem(item);
                 }));
            }
            this.ClosePage();
        }

        public void UpdateDescription()
        {
            if (Reference != null && !this.IsOldItem)
            {
                ManageChargeAccount?.Invoke();

                this.Description = this.Reference.Caption;
                this.SmallDescription = this.Description;
            }
        }

        private void InsertItem(Item item)
        {
            item.AccountingPeriod = this.RepositoryDataProvider
                  .ListOfAccountingPeriod
                  .FirstOrDefault(x => this.AccountingPeriods.EditValue == x.Key);

            itemService.Add(item);
            this.Notify(item);
        }

        private ComboBoxRibbon<string> _AccountingPeriods;

        public ComboBoxRibbon<string> AccountingPeriods
        {
            get => _AccountingPeriods;
            set
            {
                _AccountingPeriods = value;
                RaisePropertyChanged("AccountingPeriods");
            }

        }
    }
}