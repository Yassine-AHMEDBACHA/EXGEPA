using CORESI.IoC;
using System.ComponentModel.Composition;

namespace CORESI.WPF
{
    [InheritedExport]
    public interface IModule : IPriority
    {
        void LoadModule();
    }
}
