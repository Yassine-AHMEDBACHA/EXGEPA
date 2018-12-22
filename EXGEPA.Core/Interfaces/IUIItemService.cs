using System;
using System.Collections.Generic;
using EXGEPA.Model;

namespace EXGEPA.Core.Interfaces
{
    public interface IUIItemService
    {
        void AddNewItem();
        void DisplayAllItems();
        void EditItem(Item item);
        void ShowPrintLabelPanel();
        void ShowItemAttribution(ItemAttributionOptions itemAttributionOptions);

        void DisplayItems(Predicate<Item> filter, string pageCaption, Action<IEnumerable< Item>> report);
    }
}
