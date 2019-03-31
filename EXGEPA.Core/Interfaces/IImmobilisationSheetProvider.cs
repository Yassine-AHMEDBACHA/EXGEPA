using EXGEPA.Model;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace EXGEPA.Core.Interfaces
{
    [InheritedExport]
    public interface IImmobilisationSheetProvider
    {
        void PrintImmobilisationSheet(IEnumerable<Depreciation> Depreciations, string title = "");

        void PrintExploitationStartupSheet(IEnumerable<Item> items, string title = "");

        void PrintOutputSheet(IEnumerable<Item> items,bool isCession , string title = null);
    }
}
