using System;
using System.Linq;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using EXGEPA.Core.Tools;
using EXGEPA.Model;

namespace Hyproc.Core
{
    public class ItemKeyGenrator : IKeyGenerator<Item>
    {
        private readonly IParameterProvider parameterProvider;
        private readonly IDbFacade dbFacade;
        private readonly string prefix;
        private readonly int minGeneratedItemKey;
        private readonly int maxGeneratedItemKey;
        private readonly int KeyLength;

        public int Priority { get { return 50; } }

        public ItemKeyGenrator()
        {
            ServiceLocator.Resolve(out this.parameterProvider);
            ServiceLocator.Resolve(out this.dbFacade);
            this.prefix = this.parameterProvider.GetAndSetIfMissing("Prefix", "ADM");
            this.minGeneratedItemKey = this.parameterProvider.GetAndSetIfMissing("MinGeneratedItemKey", 0);
            this.maxGeneratedItemKey = this.parameterProvider.GetAndSetIfMissing("MaxGeneratedItemKey", 500000);
            this.KeyLength = this.parameterProvider.GetValue<int>("ItemKeyLength");
        }
        
        public bool CheckKey(string key)
        {
            return key.Length == 6 + prefix.Length;
        }

        public string GenerateKey(params object[] parameters)
        {
            var maxId = this.GetMaxKeyInDb() + 1;
            string Key = KeyLengthNormalizer.Normalize(maxId.ToString(), this.KeyLength);
            return $"{prefix}{Key}";
        }

        private int GetMaxKeyInDb()
        {
            var query = "select [Key] from items";
            var avialableItemKeys = dbFacade.ExecuteReader(query, dr => int.Parse(dr["Key"].ToString().Replace(prefix, "")))
                .Where(x => x >= minGeneratedItemKey && x < maxGeneratedItemKey)
                .ToList();
            if (avialableItemKeys.Count > 0)
                return avialableItemKeys.Max();
            return minGeneratedItemKey;
        }
    }
}
