// <copyright file="OutputViewModel.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows;
    using CORESI.Data;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class OutputViewModel : CommunOutputViewModel
    {
        private Visibility takerVisibility;

        private Visibility takerOptionVisibilty;

        private ObservableCollection<NamedKeyRow> listOfTakers;

        private string takerFieldName;

        public OutputViewModel(OutputType outputType, IExportableGrid exportableView)
            : base(outputType, exportableView)
        {
        }

        public ObservableCollection<NamedKeyRow> ListOfTakers
        {
            get => this.listOfTakers;
            set
            {
                this.listOfTakers = value;
                this.RaisePropertyChanged("ListOfTakers");
            }
        }

        public Visibility TakerVisibility
        {
            get => this.takerVisibility;
            set
            {
                this.takerVisibility = value;
                this.RaisePropertyChanged("TakerVisibility");
            }
        }

        public string TakerFieldName
        {
            get => this.takerFieldName;
            set
            {
                this.takerFieldName = value;
                this.RaisePropertyChanged("TakerFieldName");
            }
        }

        public Visibility TakerOptionVisibilty
        {
            get => this.takerOptionVisibilty;
            set
            {
                this.takerOptionVisibilty = value;
                this.RaisePropertyChanged("TakerOptionVisibilty");
            }
        }

        public NamedKeyRow Taker
        {
            get => this.GetTacker(this.ConcernedRow?.Tag.ToString());
            set
            {
                this.ConcernedRow.Tag = value.Key;
                this.RaisePropertyChanged("Taker");
            }
        }

        public decimal SaleAmount
        {
            get
            {
                decimal.TryParse(this.ConcernedRow?.Json, out decimal amount);
                return amount;
            }
            set => this.ConcernedRow.Json = value.ToString();
        }

        public virtual NamedKeyRow GetTacker(string key)
        {
            return this.ListOfTakers?.FirstOrDefault(x => x.Key == key);
        }
    }
}