using System;
using EXGEPA.Model;

namespace EXGEPA.Depreciations.Core
{
    public class Tools
    {
        public static DateTime GetStartComputationDate(Item item, DateTime startDate)
        {
            DateTime itemStartDate = item.TransferOrder?.Date ?? GetDefaultStartDate(item);
            return itemStartDate > startDate ? itemStartDate : startDate;
        }

        public static DateTime GetDefaultStartDate(Item item)
        {
            return item.StartDepreciationDate == StartDepreciationDate.AqusitionDate ? item.AquisitionDate : item.StartServiceDate;
        }

        public static DateTime GetEndComputationDate(Item item, DateTime endDate)
        {
            DateTime itemEndLife = GetEndComputationDate(item);
            return itemEndLife > endDate ? endDate : itemEndLife;
        }

        public static DateTime GetEndComputationDate(Item item)
        {
            DateTime? date = (item?.ReformeCertificate?.Date ?? item?.OutputCertificate?.Date);
            if (date.HasValue && date < item.LimiteDate)
            {
                return date.Value;
            }

            return item.LimiteDate;
        }

        public static DateTime GetStartComputationDate(Item item)
        {
            return item.TransferOrder?.Date ?? GetDefaultStartDate(item);

        }
    }
}
