// <copyright file="LabelGeneratorFactoy.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace EXGEPA.Core
{
    using System.Collections.Generic;
    using CORESI.Data;
    using CORESI.IoC;
    using EXGEPA.Label.Core.Model;

    public class SimpleGenerator : ILabelItemGenerator
    {
        public List<ItemLabel> LoadLabels()
        {
            var dbFacade = ServiceLocator.Resolve<IDbFacade>();
            string query = "SELECT Items.[key] as [KEY],Offices.[key] as OfficeCode ,isnull([SmallDescription],[Description]) as Description,[References].Caption FROM Items INNER JOIN Offices on Items.Office_Id = Offices.Id INNER JOIN [References] ON Reference_Id = [References].Id WHERE (items.PrintLabel = 1)";
            var result = dbFacade.ExecuteReader<ItemLabel>(query, (r) =>
            {
                var itemLabel = new ItemLabel()
                {
                    Caption = r["Description"].ToString(),
                    CodeBare = r["KEY"].ToString(),
                    Code = r["KEY"].ToString(),
                    SuffixCode = r["OfficeCode"].ToString(),
                };

                if (string.IsNullOrEmpty(itemLabel.Caption) || string.IsNullOrWhiteSpace(itemLabel.Caption))
                {
                    itemLabel.Caption = r["Caption"].ToString();
                }

                return itemLabel;
            });
            return result;
        }
    }
}
