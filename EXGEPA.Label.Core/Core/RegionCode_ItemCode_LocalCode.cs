// <copyright file="LabelGeneratorFactoy.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace EXGEPA.Core
{
    using System.Collections.Generic;
    using System.Linq;
    using CORESI.Data;
    using CORESI.IoC;
    using EXGEPA.Label.Core.Model;
    using EXGEPA.Model;

    public class RegionCode_ItemCode_LocalCode : ILabelItemGenerator
    {
        public List<ItemLabel> LoadLabels()
        {
            var regionService = ServiceLocator.Resolve<IDataProvider<Region>>();
            var region = regionService.SelectAll().FirstOrDefault();
            var dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string query = "SELECT Items.[key]  as [Key],Offices.[Code] as office ,isnull([SmallDescription],[Description]) as Description FROM Items INNER JOIN Offices on Items.Office_Id = Offices.Id WHERE (Items.PrintLabel = 1)";
            var result = dbFacade.ExecuteReader<ItemLabel>(query, (r) =>
            {
                var itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["Key"].ToString(),
                    Code = r["Key"].ToString(),
                    PrefixCode = region.Key,
                    SuffixCode = r["office"].ToString(),
                };
                return itemLabel;
            });
            return result;
        }
    }
}
