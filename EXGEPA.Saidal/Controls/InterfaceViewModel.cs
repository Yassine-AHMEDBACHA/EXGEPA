// <copyright file="InterfaceViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Saidal.Controls
{
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;
    using CORESI.IoC;
    using CORESI.Tools;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Core.Interfaces;
    using EXGEPA.Model;
    using EXGEPA.Saidal.Core;

    public class InterfaceViewModel : GenericEditableViewModel<Item>
    {
        private readonly IRepositoryDataProvider repositoryDataProvider;

        public InterfaceViewModel(IExportableGrid exportableView)
            : base(exportableView, false)
        {
            this.AutoWidth = false;
            this.Caption = "Module d'interface";
            this.LoadButtons();
            ServiceLocator.Resolve(out this.repositoryDataProvider);
            this.InitData();
        }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                using (var scoopLogger = new ScoopLogger("Loading Data", this.Logger, true))
                {
                    var list = this.DBservice.SelectAll();
                    scoopLogger.Snap("Loading Data ");
                    Parallel.ForEach(list, this.repositoryDataProvider.BindItemFields);
                    this.ListOfRows = new ObservableCollection<Item>(list);
                }
            });
        }

        private void LoadButtons()
        {
            var aquisitionSeriliazer = new AquisitionSerializer();
            this.AddNewGroup().AddCommand("Aquisitions", () => aquisitionSeriliazer.Serialize(this.Selection));
        }
    }
}