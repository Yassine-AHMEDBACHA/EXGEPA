using System;

namespace CORESI.Data
{
    public class Session : RowId
    {
        public string HostName { get; set; }
        public string UserName { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationLogin { get; set; }

        [DataAttribute(IsAuto = true, IsLong = true, SqldefaultColumValue = "GETDATE()")]
        public DateTime OpenDate { get; set; }

        [DataAttribute(IsAuto = true, IsLong = true)]
        public DateTime CloseDate { get; set; }
    }
}
