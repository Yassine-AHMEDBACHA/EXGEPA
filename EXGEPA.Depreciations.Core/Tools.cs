using System;
using EXGEPA.Model;

namespace EXGEPA.Depreciations.Core
{
    public class Tools
    {
        public static DateTime GetStartComputationDate(Item item, DateTime startDate)
        {
            DateTime dateTime = GetPreviouseDepreciationDate(item) ?? GetDefaultStartDate(item);
            if (!(dateTime > startDate))
            {
                return startDate;
            }
            return dateTime;
        }

        public static DateTime GetDefaultStartDate(Item item)
        {
            if (item.StartDepreciationDate != StartDepreciationDate.AqusitionDate)
            {
                return item.StartServiceDate;
            }
            return item.AquisitionDate;
        }

        public static DateTime GetEndComputationDate(Item item, DateTime endDate)
        {
            DateTime endComputationDate = GetEndComputationDate(item);
            if (!(endComputationDate > endDate))
            {
                return endComputationDate;
            }
            return endDate;
        }

        public static DateTime GetEndComputationDate(Item item)
        {
            DateTime? t = item?.ReformeCertificate?.Date ?? item?.OutputCertificate?.Date;
            if (t.HasValue && t < item.LimiteDate)
            {
                return t.Value;
            }
            return item.LimiteDate;
        }

        public static DateTime GetStartComputationDate(Item item)
        {
            return item.TransferOrder?.Date ?? GetDefaultStartDate(item);
        }

        private static DateTime? GetPreviouseDepreciationDate(Item item)
        {
            if (item.PreviousDepreciation > decimal.Zero && item.ExtendedProperties?.PreviouseDepreciationDate > GetDefaultStartDate(item))
            {
                return item.ExtendedProperties.PreviouseDepreciationDate;
            }
            return null;
        }
    }
}
