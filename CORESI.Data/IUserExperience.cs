using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CORESI.DataAccess
{
    public interface IUserExperience
    {
        T GetValue<T>(int id,string parameterName, T defaultValue = default(T));
        bool TrySetOrAdd(int id, string key, object value);
    }
}
