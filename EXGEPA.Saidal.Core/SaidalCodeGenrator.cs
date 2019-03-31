// <copyright file="SaidalCodeGenrator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace EXGEPA.Saidal.Core
{
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using CORESI.WPF.Controls;
    using EXGEPA.Core.Tools;
    using EXGEPA.Model;

    public class SaidalCodeGenrator : IKeyGenerator<Item>
    {
        public SaidalCodeGenrator() => this.Region = ServiceLocator.Resolve<IDataProvider<Region>>().SelectAll().First();

        public int Priority => 101;

        public Region Region { get; set; }

        public bool CheckKey(string key)
        {
            return true;
        }

        public string GenerateKey(params object[] parameters)
        {
            Reference reference = parameters[0] as Reference;
            IDbFacade dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string key = KeyLengthNormalizer.Normalize(reference.Key, 6);
            string query = $"select isnull(max(CONVERT(int, SUBSTRING([Key],10,6))),0)  from Items where Reference_Id = {reference.Id}";
            int result = dbFacade.ExecuteScalaire<int>(query) + 1;
            key = $"{this.Region.Key}{key}{KeyLengthNormalizer.Normalize(result.ToString(), 6)}";
            return key;
        }
    }
}
