// <copyright file="FieldVisibilityBase.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Output.Controls
{
    using System.Windows;
    using CORESI.WPF.Controls;
    using CORESI.WPF.Core.Interfaces;
    using EXGEPA.Model;

    public class FieldVisibilityBase<T> : GenericEditableViewModel<T>
        where T : Certificate
    {
        private Visibility takerVisibility;

        private Visibility takerOptionVisibilty;

        public FieldVisibilityBase(IExportableGrid exportableView)
            : base(exportableView)
        {
            this.TakerOptionVisibilty = this.TakerVisibility = Visibility.Collapsed;
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

        public Visibility TakerOptionVisibilty
        {
            get => this.takerOptionVisibilty;
            set
            {
                this.takerOptionVisibilty = value;
                this.RaisePropertyChanged("TakerOptionVisibilty");
            }
        }
    }
}
