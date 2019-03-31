using CORESI.WPF.Model;
using System.Collections.Generic;

namespace CORESI.WPF.Core.Interfaces
{
    public interface IRibbonCommand
    {
        List<Page> GetPages();
        List<Categorie> GetCategories();
    }
}
