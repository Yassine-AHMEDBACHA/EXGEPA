using System;
using System.Collections.Generic;
using CORESI.Data;

namespace EXGEPA.Model
{
    public class Item : KeyRow
    {
        public Item()
        {
            this.ElementCount = 1;
            this.StartDepreciationDate = StartDepreciationDate.AqusitionDate;
        }

        public string Description { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string SerialNumber { get; set; }
        public Reference Reference { get; set; }

        [DataAttribute(IsNullable = false)]
        public Office Office { get; set; }
        public DateTime OfficeAssignmentStartDate { get; set; }
        public DateTime UserAssignmentStartDate { get; set; }
        public AnalyticalAccount AnalyticalAccount { get; set; }
        public Person Person { get; set; }
        public decimal Amount { get; set; }
        public decimal PreviousDepreciation { get; set; }
        public DateTime AquisitionDate { get; set; }
        public DateTime StartServiceDate { get; set; }
        public Invoice Invoice { get; set; }
        public TransferOrder TransferOrder { get; set; }
        public InputSheet InputSheet { get; set; }
        public Provider Provider { get; set; }
        public Tva Tva { get; set; }
        public decimal TvaAmount { get; set; }
        public GeneralAccount GeneralAccount { get; set; }
        public string Comment { get; set; }
        public bool IsLocked { get; set; }
        public string OldCode { get; set; }
        public bool ExcludedFromInventory { get; set; }
        public decimal FiscalRate { get; set; }
        public decimal AccountingRate { get; set; }
        public decimal DepreciationBase { get; set; }
        public bool IsTvaDepreciatible { get; set; }
        public decimal AmountHT { get; set; }
        public DateTime LimiteDate { get; set; }
        public int ElementCount { get; set; }
        public StartDepreciationDate StartDepreciationDate { get; set; }
        public ReceiveOrder ReceiveOrder { get; set; }
        public string SmallDescription { get; set; }
        public AccountingPeriod AccountingPeriod { get; set; }
        public OutputCertificate OutputCertificate { get; set; }
        public ItemState ItemState { get; set; }
        public ProposeToReformCertificate ProposeToReformCertificate { get; set; }
        public ReformeCertificate ReformeCertificate { get; set; }
        public bool PrintLabel { get; set; }
        public string ImagePath { get; set; }
        public List<Depreciation> Depreciations { get; set; }
        public Item Owner { get; set; }
        
        public string Relation { get; set; }

        public List<Repair> Repairs { get; set; }

        [DataAttribute(Ignore =true)]
        public ItemExtendedProperties ItemExtendedProperties { get; set; }
    }

    public enum StartDepreciationDate
    {
        AqusitionDate = 1,
        ServiceDate
    }



}
