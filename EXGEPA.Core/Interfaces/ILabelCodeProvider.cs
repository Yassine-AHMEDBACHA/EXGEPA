using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;

namespace EXGEPA.Core.Interfaces
{
    [InheritedExport]
    public interface ILabelCodeProvider
    {
        int Weight { get; }
        string GenerateCode(Reference reference);
    }
}
