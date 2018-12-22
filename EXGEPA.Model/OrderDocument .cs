using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Data;

namespace EXGEPA.Model
{
    public class OrderDocument : Certificate
    {
        public OrderDocumentType OrderDocumentType { get; set; }
    }
}
