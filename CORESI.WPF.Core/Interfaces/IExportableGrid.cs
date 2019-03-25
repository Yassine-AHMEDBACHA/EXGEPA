// <copyright file="IExportableGrid.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace CORESI.WPF.Core.Interfaces
{
    public interface IExportableGrid
    {
        string DisplayedFilter { get; }

        void Print(string documentName);

        void ExportExcel(string documentName);

        void ExportPDF(string documentName);
    }
}
