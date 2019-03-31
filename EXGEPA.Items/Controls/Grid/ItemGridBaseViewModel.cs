using System;
using System.Collections.Generic;
using System.Linq;
using CORESI.IoC;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Items
{
    class ItemGridBaseViewModel : GenericEditableViewModel<Item>
    {
        Predicate<Item> displayFilter;

        IRepositoryDataProvider repositoryDataProvider;

        public ItemGridBaseViewModel(Predicate<Item> filter, IExportableGrid view, Action<IEnumerable<Item>> report) : base()
        {
            this.displayFilter = filter;
            ServiceLocator.Resolve(out this.repositoryDataProvider);
            this.repositoryDataProvider.Refresh();
            this.AutoWidth = false;
            SetExportGroup(view);
            SetToolGroup();
            this.AddNewGroup().AddCommand("Fiche", () => report(this.Selection));
        }

        public override void InitData()
        {
            var items = this.DBservice.SelectAll().Where(x => displayFilter(x)).ToList();
            repositoryDataProvider.BindItemFields(items);
            this.ListOfRows = new System.Collections.ObjectModel.ObservableCollection<Item>(items);
            this.Selection = new System.Collections.ObjectModel.ObservableCollection<Item>(ListOfRows.Take(1));
        }
    }
}