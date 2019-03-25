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

    public class SimpleWithRegionGenerator : ILabelItemGenerator
    {
        public List<ItemLabel> LoadLabels()
        {
            var dbFacade = ServiceLocator.Resolve<IDbFacade>();
            var regionService = ServiceLocator.Resolve<IDataProvider<Region>>();
            var region = regionService.SelectAll().FirstOrDefault();
            string query = "SELECT items.[Key],isnull([SmallDescription],[Description]) as Description, Caption FROM Items INNER JOIN [References] ON Reference_Id = [References].Id WHERE (PrintLabel = 1)";
            var result = dbFacade.ExecuteReader<ItemLabel>(query, (r) =>
            {
                var itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["Key"].ToString(),
                    Code = r["Key"].ToString()
                };

                if (string.IsNullOrEmpty(itemLabel.Caption) || string.IsNullOrWhiteSpace(itemLabel.Caption))
                {
                    itemLabel.Caption = r["Caption"].ToString();
                }

                itemLabel.CodeBare = region.Key + itemLabel.CodeBare;
                return itemLabel;
            });
            return result;
        }
    }
}
