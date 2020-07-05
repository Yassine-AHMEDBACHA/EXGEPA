namespace EXGEPA.Items.Controls
{
    using System.Linq;
    using CORESI.IoC;
    using CORESI.WPF.Core;
    using CORESI.WPF.Model;
    using EXGEPA.Core;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;

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
            this.IsBaseDepreciationReadOnly = this.ParameterProvider.TryGet("IsBaseDepreciationReadOnlyInEdition", true);
            this.Categorie = OldItemRibbonCategorie;
            InitialItem = item;
            this.ConcernedItem = (Item)item.Clone();

            this.IsOldItem = true;
            this.Caption = Caption = "Article N°: " + item.Key;
            this.IsSelected = true;
            base.ConcernedItem.SetExtendedProperties();
            var group = this.AddNewGroup();

            group.AddCommand("Sauver & Fermer", IconProvider.SaveAndClose, this.UpdateItem);
            var immoShtPdr = ServiceLocator.Resolve<IImmobilisationSheetProvider>();
            if (immoShtPdr != null)
            {
                Group immoShtGrp = this.AddNewGroup("Fiche immobilisation");
                immoShtGrp.AddCommand("Mensuel", () => immoShtPdr.PrintImmobilisationSheet(this.ListOfMonthelyDepreciation.ToList(), "Mensuel"));
                immoShtGrp.AddCommand("Journalier", () => immoShtPdr.PrintImmobilisationSheet(this.ListOfDailyDepreciation.ToList(), "Journalier"));
                immoShtGrp.AddCommand("Fiche d'exploitation", () => immoShtPdr.PrintExploitationStartupSheet(new[] { item }));
            }

            BindFields();
            this.UpdateDepreciations();
        }

        public void UpdateItem()
        {
            this.UIMessage.TryDoAction(Logger, () =>
            {
                string result = Core.ItemValidator.CheckItem(this.ConcernedItem);
                if (result != null)
                {
                    this.UIMessage.Error(result);
                }
                else
                {
                    this._SavePicture?.Invoke();
                    base.ConcernedItem.SerializeExtendedProperties();
                    this.ItemService.Update(this.ConcernedItem);
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
