using CORESI.WPF.Core.Interfaces;
using System;
using System.Windows;
using System.Windows.Controls;

namespace CORESI.Report.Controls
{
    /// <summary>
    /// Interaction logic for ReportView.xaml
    /// </summary>
    public partial class ReportView : UserControl, IReportViewer
    {
        private ReportView()
        {
            InitializeComponent();

        }

        private void documentViewer_DocumentChanged(object sender, RoutedEventArgs e)
        {
            this.documentViewer.ZoomMode = DevExpress.Xpf.DocumentViewer.ZoomMode.PageLevel;
        }

        public void Print()
        {
            this.documentViewer.PrintCommand.Execute(null);
        }

        public void QuickPrint()
        {
            this.documentViewer.PrintDirectCommand.Execute(null);
        }

        public void ShowPageSetup()
        {
            this.documentViewer.PageSetupCommand.Execute(null);
        }

        public void ShowScalePageDialog()
        {
            this.documentViewer.ScaleCommand.Execute(null);
        }

        public void ZoomIn()
        {
            this.documentViewer.ZoomInCommand.Execute(null);
        }

        public void ZoomOut()
        {
            this.documentViewer.ZoomOutCommand.Execute(null);
        }

        public void ZoomToWholePage()
        {
            this.documentViewer.ZoomMode = DevExpress.Xpf.DocumentViewer.ZoomMode.PageLevel;
        }

        public void ShowWaterMarkDialog()
        {
            this.documentViewer.SetWatermarkCommand.Execute(null);
        }

        public void MoveToFirstPage()
        {
            this.documentViewer.FirstPageCommand.Execute(null);
        }

        public void MoveToPreviousPage()
        {
            this.documentViewer.PreviousPageCommand.Execute(null);
        }

        public void MoveToNextPage()
        {
            this.documentViewer.NextPageCommand.Execute(null);
        }

        public void MoveToLastPage()
        {
            this.documentViewer.LastPageCommand.Execute(null);
        }

        public void ExportToExcel()
        {
            this.documentViewer.ExportCommand.Execute("Pdf");
        }

        public static ReportView GetInstance(object dataContext = null)
        {
            ReportView result = Application.Current.Dispatcher.Invoke((Func<ReportView>)(() =>
            {
                ReportView reportView = new ReportView();
                if (dataContext != null)
                {
                    reportView.DataContext = dataContext;
                }
                return reportView;
            }));
            return (ReportView)result;
        }
    }
}
