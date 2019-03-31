using System.Collections.Generic;
using CORESI.Data;

namespace CORESI.Security
{
    public class Role : KeyRow
    {
        public List<Ability> Abilities { get; set; }
    }
}
