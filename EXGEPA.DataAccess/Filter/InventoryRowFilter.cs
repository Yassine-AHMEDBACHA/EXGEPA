using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace EXGEPA.DataAccess.Items
{
    [Export(typeof(IDataFilter<InventoryRow>))]
    public class InventoryRowFilter : IDataFilter<InventoryRow>, IPriority
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }

        public IList<InventoryRow> Filter(IList<InventoryRow> ItemsTofilter)
        {
            return ItemsTofilter;
        }
    }
}
