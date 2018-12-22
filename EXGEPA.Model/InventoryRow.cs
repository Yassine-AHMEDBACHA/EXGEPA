using CORESI.Data;
using System;

namespace EXGEPA.Model
{
    public class InventoryRow : KeyRow
    {
        public string Localization { get; set; }
        public string Data { get; set; }
        
        public ItemState ItemState { get; set; }
        public DateTime OpertationDate { get; set; }
    }
}
