using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EXGEPA.Model
{
    public class AccountingReference
    {
        public int Id { get; set; }
        public virtual Provider Provider { get; set; }
        
    }
}
