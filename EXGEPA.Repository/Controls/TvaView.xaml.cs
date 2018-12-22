using CORESI.WPF.Controls;
using System.Windows;

namespace EXGEPA.Repository.Controls
{
    /// <summary>
    /// Interaction logic for TvaView.xaml
    /// </summary>
    public partial class TvaView : ExportableView
    {


        public void Print(string documentName)
        {
            this.mainTableView.ShowPrintPreviewDialog(Application.Current.MainWindow, documentName);
        }

        public void ExportExcel(string documentName)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".Xlsx";
            dlg.Filter = "(.Xlsx)|*.Xlsx";
            dlg.FileName = documentName;
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                this.mainTableView.ExportToXlsx(filename);
            }
        }

        public void ExportPDF(string documentName)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.DefaultExt = ".pdf";
            dlg.Filter = " (.pdf)|*.pdf";
            dlg.FileName = documentName;
            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                this.mainTableView.ExportToPdf(filename);
            }
        }
        public TvaView()
        {
            InitializeComponent();
        }
    }
}
