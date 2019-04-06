using CORESI.Tools;
using EXGEPA.Model;

namespace EXGEPA.Items.Core
{
    public static class ItemExtensions
    {
        public static void SetExtendedProperties(this Item item)
        {
            if (!item.Json.TryDeserialize(out ItemExtendedProperties deserializedObject))
            {
                deserializedObject = new ItemExtendedProperties
                {
                    PreviouseDepreciationDate = item.AquisitionDate
                };
            }

            item.ExtendedProperties = deserializedObject;
        }

        public static void SerializeExtendedProperties(this Item item)
        {
            item.Json = JsonHelper.Serialize(item.ExtendedProperties);
        }
    }
}
