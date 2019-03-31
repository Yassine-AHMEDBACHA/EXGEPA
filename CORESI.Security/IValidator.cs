using System.Collections.Generic;
using CORESI.Data;

namespace CORESI.Security
{
    public interface IValidator<T> where T : Row
    {
        List<int> ValidationLevels { get; }
    }
}
