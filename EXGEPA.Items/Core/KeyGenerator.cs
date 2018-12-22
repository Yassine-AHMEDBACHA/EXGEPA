using System;
using CORESI.Data;
using CORESI.IoC;
using CORESI.WPF.Controls;
using EXGEPA.Core.Tools;
using EXGEPA.Model;

namespace EXGEPA.Items.Core
{
    public class KeyGenerator : IKeyGenerator<Item>
    {
        private readonly IParameterProvider parameterProvider;
        private readonly IDbFacade dbFacade;
        private readonly string KeyStyle;
        private readonly int keyLength;

        public int Priority => 5;

        public KeyGenerator()
        {
            ServiceLocator.Resolve(out this.parameterProvider);
            ServiceLocator.Resolve(out this.dbFacade);
            this.KeyStyle = this.parameterProvider.GetValue("ItemKeyStyle", "6C").ToUpperInvariant();
            this.keyLength = this.parameterProvider.GetValue<int>("ItemKeyLength");
        }

        public string GetCode6()
        {
            var dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string query = "SELECT ISNULL(MAX(CONVERT(INTEGER,[Key])),0) FROM ITEMS";
            var result = dbFacade.ExecuteScalaire<int>(query) + 1;
            var key = KeyLengthNormalizer.Normalize(result.ToString(), 6);
            return key;
        }

        public string GetCode12(params object[] parameters)
        {
            var reference = parameters[0] as Reference;
            var key = KeyLengthNormalizer.Normalize(reference.Key, 4);
            string query = "select ISNULL(MAX(CONVERT(INTEGER,[Key])),0) from items where [Key] like '" + key + "%'";
            var result = this.dbFacade.ExecuteScalaire<int>(query) + 1;
            return KeyLengthNormalizer.Normalize(result.ToString(), 12);
        }

        public bool CheckKey(string key)
        {
            return key.Length == this.keyLength;
        }

        public string GenerateKey(params object[] parameters)
        {
            switch (this.KeyStyle)
            {
                case "6C":
                    return GetCode6();

                case "12C":
                    return GetCode12(parameters);
                default:
                    throw new ArgumentException("Key style not recognized");
            }
        }
    }
}
