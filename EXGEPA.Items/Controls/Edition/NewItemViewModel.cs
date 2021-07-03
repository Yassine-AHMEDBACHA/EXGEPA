using System;
using System.Linq;
using CORESI.Tools.Collections;
using CORESI.WPF.Core;
using CORESI.WPF.Model;
using EXGEPA.Core;
using EXGEPA.Model;

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
            this.IsKeyReadOnly = this.ParameterProvider.GetValue("IsItemKeyReadOnlyAtCreation", true);
            this.IsBaseDepreciationReadOnly = this.ParameterProvider.TryGet("IsBaseDepreciationReadOnlyAtCreation", false);
            this.Categorie = NewItemRibbonCategorie;
            this.IsSelected = true;
            Group group = this.AddNewGroup();
            group.AddCommand("Sauver & Fermer", IconProvider.SaveAndClose, this.AddNewItem);
            group = this.AddNewGroup();
            this.AccountingPeriods = group.AddCommand<ComboBoxRibbon<string>>("Exercice");
            this.Quantity = group.AddCommand<ComboBoxRibbon<int>>("Quantité");
            this.Quantity.Width = 80;
            int maxQuantityCanBeCreated = ParameterProvider.TryGet("ItemMaxQuantityCanBeCreated", 100);
            Enumerable.Range(1, maxQuantityCanBeCreated).ForEach(x => this.Quantity.ItemsSource.Add(x));
            this.Quantity.EditValue = this.Quantity.ItemsSource.FirstOrDefault();
            this.UIMessage.TryDoActionAsync(Logger, this.InitData);
        }

        private void InitData()
        {
            AccountingPeriods.ItemsSource.Clear();
            InitialItem = new Item { PrintLabel = true };
            ConcernedItem = (Item)InitialItem.Clone();
            base.ConcernedItem.SetExtendedProperties();
            this.AquisitionDate = DateTime.Today;
            this.IsTvaDepreciatible = true;
            this.BindFields();
            var source = this.RepositoryDataProvider.ListOfAccountingPeriod
                .Where(x => !x.Approved)
                .OrderBy(x => x.StartDate)
                .Select(x => x.Key);
            AccountingPeriods.SetSource(source);
        }

        private void AddNewItem()
        {
            string result = Core.ItemValidator.CheckItem(this.ConcernedItem, true);
            if (result != null)
            {
                this.UIMessage.Error(result);
                return;
            }

            this._SavePicture?.Invoke();
            base.ConcernedItem.SerializeExtendedProperties();
            this.InsertItem(this.ConcernedItem);
            if (this.Quantity.EditValue > 1)
            {
                System.Collections.Generic.List<Item> ItemsToInsert = Enumerable.Range(0, this.Quantity.EditValue - 1).Select(x => (Item)ConcernedItem.Clone()).ToList();
                ItemsToInsert.ForEach(item => this.UIMessage.TryDoAction(this.Logger, () =>
                 {
                     item.Key = this.KeyGenerator.GenerateKey(this.Reference, this.KeyLength);
                     this.InsertItem(item);
                 }));
            }

            this.ClosePage();
        }

        public void UpdateDescription()
        {
            if (Reference != null && !this.IsOldItem)
            {
                manageChargeAccount?.Invoke();

                this.Description = this.Reference.Caption;
                this.SmallDescription = this.Description;
            }
        }

        private void InsertItem(Item item)
        {
            item.AccountingPeriod = this.RepositoryDataProvider
                  .ListOfAccountingPeriod
                  .FirstOrDefault(x => this.AccountingPeriods.EditValue == x.Key);

            var retry = true;
            while (retry)
            {
                try
                {
                    this.ItemService.Add(item);
                    retry = false;
                }
                catch (Exception ex)
                {
                    Logger.Error(ex);
                    if (ex.Message.Contains("UK_Items_Key"))
                    {
                        retry = true;
                       item.Key = this.KeyGenerator.GenerateKey(this.Reference, this.KeyLength);
                    }
                }
            }
            
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