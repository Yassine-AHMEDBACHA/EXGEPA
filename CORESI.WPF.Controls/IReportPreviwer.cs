using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace CORESI.WPF.Controls
{
    [InheritedExport]
    public interface IReportPreviwer<T> 
    {
        void SetPreviewReportGroup(IPageSetter pageSetter);
    }
}
