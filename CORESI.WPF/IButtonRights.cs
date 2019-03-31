using CORESI.IoC;
using System.ComponentModel.Composition;

namespace CORESI.WPF
{
    [InheritedExport]
    public interface IButtonRights : IPriority
    {
        bool CanDoAction(string actionName);
    }
}
