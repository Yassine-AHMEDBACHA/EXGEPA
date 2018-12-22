using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.WPF.Core.Interfaces
{
    public interface IReportViewer
    {
        void Print();
        void QuickPrint();
        void ShowPageSetup();
        void ShowScalePageDialog();
        void ZoomIn();
        void ZoomOut();
        void ZoomToWholePage();
        void ShowWaterMarkDialog();
        void MoveToFirstPage();
        void MoveToPreviousPage();
        void MoveToNextPage();
        void MoveToLastPage();

    }
}
