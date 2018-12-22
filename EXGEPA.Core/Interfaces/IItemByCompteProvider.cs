using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Core.Interfaces
{
    public interface IItemByCompteProvider
    {
        void PrintImmobilisationSheet(IEnumerable<Item> items, string title = null);

        void PrintImmobilisationByAccount(IEnumerable<Item> items, string title = null, string filtreCreteria = null);

        void PrintRecapByAccount(IEnumerable<Item> items, string title = null, string filtreCreteria = null);


    }
}
