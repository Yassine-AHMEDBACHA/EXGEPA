// <copyright file="LabelGeneratorFactoy.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace EXGEPA.Core
{
    using System.Collections.Generic;
    using CORESI.Data;
    using CORESI.IoC;
    using EXGEPA.Label.Core.Model;

    public class ItemCode : ILabelItemGenerator
    {
        public List<ItemLabel> LoadLabels()
        {
            var dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string query = "SELECT Items.[key]  as [Key],isnull([SmallDescription],[Description]) as Description FROM Items WHERE (Items.PrintLabel = 1)";
            var result = dbFacade.ExecuteReader<ItemLabel>(query, (r) =>
            {
                var itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["Key"].ToString(),
                    Code = r["Key"].ToString(),
                };
                return itemLabel;
            });

            return result;
        }
    }
}
