using System.ComponentModel.Composition;
using CORESI.Data;

namespace CORESI.WPF.Controls
{
    [InheritedExport]
    public interface IKeyGenerator<T> : IoC.IPriority where T : IKey
    {
        bool CheckKey(string key);

        string GenerateKey(params object[] parameters);
    }
}
