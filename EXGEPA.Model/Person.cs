using CORESI.Data;
using System;

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
