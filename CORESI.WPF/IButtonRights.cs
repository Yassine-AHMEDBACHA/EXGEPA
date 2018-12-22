using CORESI.IoC;
using System.ComponentModel.Composition;
using CORESI.Security;

namespace CORESI.WPF
{
    [InheritedExport]
    public interface IButtonRights : IPriority
    {
        bool CanDoAction(string actionName);
    }
}
