using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using EXGEPA.Core.Tools;
using EXGEPA.Model;

namespace EXGEPA.Saidal.Core
{
    public class SaidalCodeGenrator : IKeyGenerator<Item>
    {
        public int Priority => 101;
        public Region Region { get; set; }
        public SaidalCodeGenrator()
        {
            this.Region = ServiceLocator.Resolve<IDataProvider<Region>>().SelectAll().First();
        }

        public bool CheckKey(string key)
        {
            return true;
        }

        public string GenerateKey(params object[] parameters)
        {
            var reference = parameters[0] as Reference;
            var dbFacade = ServiceLocator.Resolve<IDbFacade>();
            var Key = KeyLengthNormalizer.Normalize(reference.Key, 6);
            string query = $"select isnull(max(CONVERT(int, SUBSTRING([Key],10,6))),0)  from Items where Reference_Id = {reference.Id}";
            var result = dbFacade.ExecuteScalaire<int>(query) + 1;
            Key = $"{Region.Key}{Key}{ KeyLengthNormalizer.Normalize(result.ToString(), 6)}";
            return Key;
        }
    }
}
