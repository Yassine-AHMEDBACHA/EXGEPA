using System;
using System.Collections;

namespace CORESI.Data
{
    public interface IParameterProvider
    {
        T GetValue<T>(string key, T defaultValue);

        T GetValue<T>(string key);

        [Obsolete]
        T GetAndSetIfMissing<T>(string key, T value);

        T TryGet<T>(string key, T defaultValue);

        string GetStringValue(string key);

        bool TrySetOrAdd(string key, object value);

        IDictionary GetAllParameters();
    }
}
