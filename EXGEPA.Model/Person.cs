using CORESI.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EXGEPA.Model
{
    public class Person : KeyRow
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public DateTime BirthDate { get; set; }
        public string BirthPlace { get; set; }
        public string Function { get; set; }

       
    }
}
