using System;
using System.Collections.Generic;
using System.Linq;
using CORESI.IoC;
using CORESI.Tools.Collections;
using CORESI.WPF.Controls;
using CORESI.WPF.Core.Interfaces;
using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Items
{
    internal class ItemGridBaseViewModel : GenericEditableViewModel<Item>
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
            repositoryDataProvider.BindProperties(items);
            this.ListOfRows = items.ToObservable();
            this.Selection = this.ListOfRows.Take(1).ToObservable();
        }

       
    }
}