using EXGEPA.Model;
using System.Collections.Generic;

namespace EXGEPA.Core.Interfaces
{
    public interface IItemByCompteProvider
    {
        void PrintImmobilisationSheet(IEnumerable<Item> items, string title = null);

        void PrintImmobilisationByAccount(IEnumerable<Item> items, string title = null, string filtreCreteria = null);

        void PrintRecapByAccount(IEnumerable<Item> items, string title = null, string filtreCreteria = null);


    }
}
