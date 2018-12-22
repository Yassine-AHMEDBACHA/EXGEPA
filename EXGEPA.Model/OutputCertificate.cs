
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Model
{
    public class OutputCertificate : Certificate
    {
        public OutputType OutputType { get; set; }
    }
    public enum OutputType
    {
        Cession = 1,
        Destruction,
        Disparition,
        Vente
    }
}
