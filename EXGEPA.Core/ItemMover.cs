using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Model;

namespace EXGEPA.Core
{
    public class ItemMover
    {
        protected readonly IDataProvider<Item> itemService;

        protected readonly IDataProvider<Office> officeService;

        public ItemMover()
        {
            ServiceLocator.Resolve(out this.itemService);
        }

        public bool MoveItem(Item item, Office office)
        {
            return true;
        }
    }
}
