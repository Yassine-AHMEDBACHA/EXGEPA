﻿using CORESI.Data;
using System;

namespace EXGEPA.Model
{
    public class Repair : KeyRow
    {
        [DataAttribute(IsUnique = true)]
        public string Code { get; set; }
        public DateTime Date { get; set; }
        public Item Item { get; set; }
        public decimal Amount { get; set; }
        public decimal Forex { get; set; }
        public Currency Currency { get; set; }
        public Provider Provider { get; set; }
    }
}
