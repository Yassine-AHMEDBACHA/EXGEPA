using CORESI.Data;

namespace EXGEPA.Model
{
    public class AnalyticalAccount : NamedKeyRow
    {
        public string ThirdPartyAccount { get; set; }

        [DataAttribute(IsNullable = false)]
        public AnalyticalAccountType AnalyticalAccountType { get; set; }

    }
}
