using EXGEPA.Model;
using System.ComponentModel.Composition;

namespace EXGEPA.Core.Interfaces
{
    [InheritedExport]
    public interface ILabelCodeProvider
    {
        int Weight { get; }
        string GenerateCode(Reference reference);
    }
}
