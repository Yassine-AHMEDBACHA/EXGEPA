namespace EXGEPA.Items
{
    using CORESI.IoC;
    using CORESI.Tools;
    using CORESI.WPF.Core;
    using CORESI.WPF.Model;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Items.Controls;

    public sealed class Module : AModule
    {
        public override int Priority
        {
            get
            {
                return 3;
            }
        }

        public override void AddGroups()
        {
            IUIItemService iUItemService = ServiceLocator.Resolve<IUIItemService>();
            Group immobilisationGroup = new Group("Immobilisations");
            immobilisationGroup.AddCommand("Consultation", IconProvider.Grid, iUItemService.DisplayAllItems);
            immobilisationGroup.AddCommand("Ajouter", IconProvider.AddItem, iUItemService.AddNewItem);
            this.UIService.AddGroupToHomePage(immobilisationGroup);
        }

        public override void InitializeModule()
        {
            using (ScoopLogger sccopLogger = new ScoopLogger("Initializing Item module", Logger))
            {
                DXControlInitializer.LoadComponent<ItemView>();
                DXControlInitializer.LoadComponent<ItemAttributionView>();
                DXControlInitializer.LoadComponent<ItemGridView>();
            }
        }
    }
}
