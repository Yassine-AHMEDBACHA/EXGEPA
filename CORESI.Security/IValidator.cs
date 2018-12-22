using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CORESI.Data;

namespace CORESI.Security
{
    public interface IValidator<T> where T : Row
    {
        List<int> ValidationLevels { get; }
    }
}
