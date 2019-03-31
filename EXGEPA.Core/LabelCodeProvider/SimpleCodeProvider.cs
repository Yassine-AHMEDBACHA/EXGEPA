using EXGEPA.Core.Interfaces;
using EXGEPA.Model;

namespace EXGEPA.Core.LabelCodeProvider
{
    class SimpleCodeProvider : ILabelCodeProvider
    {
        public int Weight { get { return 0; } }
        public string GenerateCode(Reference reference)
        {
            return "";
        }
    }
}
