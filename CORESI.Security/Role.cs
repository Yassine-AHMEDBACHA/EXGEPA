using System.Collections.Generic;
using CORESI.Data;
using CORESI.Security;

namespace CORESI.Security
{
    public class Role : KeyRow
    {
        public List<Ability> Abilities { get; set; }
    }
}
