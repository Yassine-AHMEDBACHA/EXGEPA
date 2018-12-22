using CORESI.Data;
using CORESI.IoC;
using EXGEPA.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;

namespace EXGEPA.DataAccess.Items
{
    [Export(typeof(IDataFilter<Item>))]
    public class ItemDataFilter : IDataFilter<Item>, IPriority
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }

        public IList<Item> Filter(IList<Item> ItemsTofilter)
        {
            return ItemsTofilter;
        }
    }
}
