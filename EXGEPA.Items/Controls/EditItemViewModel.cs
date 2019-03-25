﻿namespace EXGEPA.Items.Controls
{
    using System.Linq;
    using CORESI.IoC;
    using CORESI.Tools;
    using CORESI.WPF.Core;
    using CORESI.WPF.Model;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;
    using Newtonsoft.Json;

    public class EditItemViewModel : ItemViewModelBase
    {
        public static Categorie OldItemRibbonCategorie { get; private set; }

        public CheckedRibbonButton ProposeToReform { get; set; }

        static EditItemViewModel()
        {
            OldItemRibbonCategorie = new Categorie()
            {
                Caption = "Modification d'article",
                Color = System.Windows.Media.Color.FromRgb(80, 246, 88)
            };
        }

        public EditItemViewModel(Item item) : base()
        {
            this.IsKeyReadOnly = true;
            this.IsBaseDepreciationReadOnly = this.parameterProvider.GetAndSetIfMissing("IsBaseDepreciationReadOnlyInEdition", true);
            this.Categorie = OldItemRibbonCategorie;
            InitialItem = item;
            this.ConcernedItem = (Item)item.Clone();

            this.IsOldItem = true;
            this.Caption = Caption = "Article N°: " + item.Key;
            this.IsSelected = true;
            var group = this.AddNewGroup();
            if (!JsonHelper.TryDeserialize(this.ConcernedItem.Json, out this.itemExtendedProperties))
            {
                this.itemExtendedProperties = new ItemExtendedProperties();
            }

            group.AddCommand("Sauver & Fermer", IconProvider.SaveAndClose, this.UpdateItem);
            BindFields();
            var immobilisationSheetProvider = ServiceLocator.Resolve<IImmobilisationSheetProvider>();
            if (immobilisationSheetProvider != null)
            {
                var immobilisationSheetGroup = this.AddNewGroup("Fiche immobilisation");
                immobilisationSheetGroup.AddCommand("Mensuel", () => immobilisationSheetProvider.PrintImmobilisationSheet(this.ListOfMonthelyDepreciation.ToList(), "Mensuel"));
                immobilisationSheetGroup.AddCommand("Journalier", () => immobilisationSheetProvider.PrintImmobilisationSheet(this.ListOfDailyDepreciation.ToList(), "Journalier"));
                immobilisationSheetGroup.AddCommand("Fiche d'exploitation", () => immobilisationSheetProvider.PrintExploitationStartupSheet(new[] { item }));
            }
        }

        public void UpdateItem()
        {
            this.uIMessage.TryDoAction(logger, () =>
            {
                var result = Core.ItemValidator.CheckItem(this.ConcernedItem);
                if (result != null)
                {
                    this.uIMessage.Error(result);
                }
                else
                {
                    if (this._SavePicture != null)
                        this._SavePicture();
                    this.ConcernedItem.Json = JsonConvert.SerializeObject(this.itemExtendedProperties);
                    itemService.Update(this.ConcernedItem);
                    this.ClosePage();
                    this.Notify(this.ConcernedItem);
                }
            });
        }

        public override void UpdateFiscaleRate(GeneralAccount generalAccount)
        {
            this.FiscaleRate = this.FiscaleRate;
        }

        public override void UpdateGeneralAccount(Reference reference)
        {
            this.GeneralAccount = this.GeneralAccount;
        }

        public override void SetAccoutToDisplay()
        {
            base.SetAccoutToDisplay();
            if (!this.ListOfGeneralAccount.Contains(this.GeneralAccount))
            {
                this.ListOfGeneralAccount.Add(this.GeneralAccount);
            }
        }

        public override void UpdateDescription(Reference reference)
        {
            return;
        }
    }
}