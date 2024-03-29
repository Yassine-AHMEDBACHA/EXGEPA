﻿using CORESI.WPF.Core.Interfaces;
using System.Windows;
using System.Windows.Controls;

namespace EXGEPA.Invoice.Controls
{
    /// <summary>
    /// Interaction logic for InvoiceView.xaml
    /// </summary>
    public partial class InvoiceView : UserControl, IExportableGrid
    {
        public string DisplayedFilter => this.mainTableView.FilterPanelText;


        public void Print(string documentName)
        {
            this.mainTableView.ShowPrintPreviewDialog(Application.Current.MainWindow, documentName);
        }

        public void ExportExcel(string documentName)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".Xlsx",
                Filter = "(.Xlsx)|*.Xlsx",
                FileName = documentName
            };
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                this.mainTableView.ExportToXlsx(filename);
            }
        }

        public void ExportPDF(string documentName)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                DefaultExt = ".pdf",
                Filter = " (.pdf)|*.pdf",
                FileName = documentName
            };
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                this.mainTableView.ExportToPdf(filename);
            }
        }
        public InvoiceView()
        {
            InitializeComponent();
        }
    }
}
