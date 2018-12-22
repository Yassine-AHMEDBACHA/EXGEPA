using System;
using System.Collections.Generic;
using CORESI.Data;

namespace EXGEPA.Model
{
    public abstract class Certificate : NamedKeyRow, IDatable, INamedKeyRepository
    {
        public DateTime Date { get; set; }
        public List<Item> Items { get; set; }
    }
}
