using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

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
