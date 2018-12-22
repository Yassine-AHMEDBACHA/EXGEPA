using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Data;

namespace EXGEPA.Model
{
    public interface INamedKeyRepository : INamedKey
    {
        List<Item> Items { get; }
    }
}
