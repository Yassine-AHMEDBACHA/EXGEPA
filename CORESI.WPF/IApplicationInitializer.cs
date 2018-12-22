using System.ComponentModel.Composition;

namespace CORESI.WPF
{
    [InheritedExport]
    public interface IApplicationInitializer
    {
        string ApplicationTheme { get; set; }

        void SetTheme(string themeName);


    }
}
