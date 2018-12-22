using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.Security
{
    public interface IOperator
    {
        string Name { get; set; }
        bool ExpiredPassword { get; set; }

        Role Role { get; set; }
    }
}
