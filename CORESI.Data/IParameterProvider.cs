using System.Collections;

namespace CORESI.Data
{
    public interface IParameterProvider
    {
        T GetValue<T>(string parameterName, T defaultValue);

        T GetValue<T>(string parameterName);

        T GetAndSetIfMissing<T>(string parameterName, T value);

        string GetStringValue(string parameterName);
        bool TrySetOrAdd(string key, object value);
        IDictionary GetAllParameters();
    }
}
