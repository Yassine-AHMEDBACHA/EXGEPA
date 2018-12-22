namespace CORESI.WPF.Core.Interfaces
{
    public interface IExportable
    {
        string DisplayedFilter { get; }
        void Print(string documentName);
        void ExportExcel(string documentName);
        void ExportPDF(string documentName);
    }
}
