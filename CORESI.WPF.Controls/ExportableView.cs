using CORESI.WPF.Core.Interfaces;
using DevExpress.Xpf.Grid;
using System.Windows;
using System.Windows.Controls;

namespace CORESI.WPF.Controls
{
    public abstract class ExportableView : UserControl, IExportableGrid
    {
        public TableView TableView => this.FindName("mainTableView") as TableView;

        public string DisplayedFilter => this.TableView.FilterPanelText;

        public virtual void Print(string documentName)
        {
            this.TableView.ShowPrintPreviewDialog(Application.Current.MainWindow, documentName);
        }

        public virtual void ExportExcel(string documentName)
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
                this.TableView.ExportToXlsx(filename);
            }
        }

        public virtual void ExportPDF(string documentName)
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
                this.TableView.ExportToPdf(filename);
            }
        }
    }
}
