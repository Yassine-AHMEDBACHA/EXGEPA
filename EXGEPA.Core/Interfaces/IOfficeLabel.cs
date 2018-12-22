using CORESI.WPF.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Reflection;
using System.Text;

namespace EXGEPA.Core.Interfaces
{
    [InheritedExport]
    public interface IOfficeLabel
    {
        void ShowOfficeAttribution(PropertyInfo property, object value, object defaultValue, Group group = null);
    }
}
