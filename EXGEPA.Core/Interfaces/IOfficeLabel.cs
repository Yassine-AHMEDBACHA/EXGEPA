using CORESI.WPF.Model;
using System.ComponentModel.Composition;
using System.Reflection;

namespace EXGEPA.Core.Interfaces
{
    [InheritedExport]
    public interface IOfficeLabel
    {
        void ShowOfficeAttribution(PropertyInfo property, object value, object defaultValue, Group group = null);
    }
}
