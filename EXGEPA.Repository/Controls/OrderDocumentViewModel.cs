// <copyright file="OrderDocumentViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Repository.Controls
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class OrderDocumentViewModel : GenericEditableViewModel<OrderDocument>
    {
        private readonly IDataProvider<OrderDocumentType> orderDocumentTypeTypeService;
        private ObservableCollection<OrderDocumentType> listOfOrderDocumentType;

        public OrderDocumentViewModel(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.Caption = "Liste de documents de base";
            ServiceLocator.Resolve(out this.orderDocumentTypeTypeService);
        }

        public ObservableCollection<OrderDocumentType> ListOfOrderDocumentType
        {
            get => this.listOfOrderDocumentType;
            set
            {
                this.listOfOrderDocumentType = value;
                this.RaisePropertyChanged("ListOfOrderDocumentType");
            }
        }

        public override void InitData()
        {
            this.StartBackGroundAction(() =>
            {
                this.ListOfRows = new ObservableCollection<OrderDocument>(this.DBservice.SelectAll());
                this.ListOfOrderDocumentType = new ObservableCollection<OrderDocumentType>(this.orderDocumentTypeTypeService.SelectAll());
                foreach (OrderDocument item in this.ListOfRows)
                {
                    item.OrderDocumentType = this.ListOfOrderDocumentType.Single(x => x.Id == item.OrderDocumentType.Id);
                }
            });
        }

        public override void AddItem()
        {
            base.AddItem();
            this.ConcernedRow.OrderDocumentType = this.ListOfOrderDocumentType.FirstOrDefault();
            this.RaisePropertyChanged();
        }
    }
}
