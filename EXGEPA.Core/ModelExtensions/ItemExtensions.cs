// <copyright file="ItemExtensions.cs" company="PlaceholderCompany">
// Copyright (c) CORESI. All rights reserved.
// </copyright>

namespace EXGEPA.Items.Core
{
    using CORESI.Tools;
    using EXGEPA.Model;

    public static class ItemExtensions
    {
        public static void SetExtendedProperties(this Item item)
        {
            if (!JsonHelper.TryDeserialize(item.Json, out ItemExtendedProperties itemExtendedProperties))
            {
                itemExtendedProperties = new ItemExtendedProperties
                {
                    PreviouseDepreciationDate = item.AquisitionDate
                };
            }

            item.ExtendedProperties = itemExtendedProperties;
        }

        public static void SerializeExtendedProperties(this Item item)
        {
            var json = JsonHelper.Serialize(item.ExtendedProperties);
            item.Json = json;
        }
    }
}