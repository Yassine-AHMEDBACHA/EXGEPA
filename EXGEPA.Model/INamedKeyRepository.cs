using System.Collections.Generic;
using CORESI.Data;

namespace EXGEPA.Model
{
    public interface INamedKeyRepository : INamedKey
    {
        List<Item> Items { get; }
    }
}
