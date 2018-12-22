using EXGEPA.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
