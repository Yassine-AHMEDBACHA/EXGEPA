using EXGEPA.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace EXGEPA.Core.Interfaces
{
    [InheritedExport]
    public interface IInventorySheetProvider
    {
        void PrintInventorySheet(IList<Item> items, bool isTheorical = true);
    }
}
