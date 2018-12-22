using CORESI.Data;
using CORESI.DataAccess.Core;
using EXGEPA.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public DateTime ImportDate { get; set; }
    }
}