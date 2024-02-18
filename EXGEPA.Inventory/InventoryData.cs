using CORESI.Data;
using EXGEPA.Model;
using System;

namespace EXGEPA.Inventory
{
    public class InventoryData : KeyRow
    {
        public InventoryRow InventoryRow { get; set; }
        public string Localization { get; set; }
        public ItemState ItemState { get; set; }
        public Item Item { get; set; }
        public Office Office { get; set; }
        public string GapType { get; set; }
        public DateTime? ImportDate { get; set; }
    }
}