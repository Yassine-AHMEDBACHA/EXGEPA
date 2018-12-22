using CORESI.WPF.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORESI.WPF.Core.Interfaces
{
    public interface IRibbonCommand
    {
        List<Page> GetPages();
        List<Categorie> GetCategories(); 
    }
}
