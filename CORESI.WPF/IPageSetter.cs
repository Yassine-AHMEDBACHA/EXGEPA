using CORESI.WPF.Model;
using System;
using System.Collections.ObjectModel;
namespace CORESI.WPF
{
    public interface IPageSetter
    {
        string Caption { get;}
        ObservableCollection<Group> Groups { get; set; }
        Categorie Categorie { get; set; }
        bool IsSelected { get; set; }
        Action ClosePage { set; }
    }
}
