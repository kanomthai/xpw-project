using System;

namespace XPWLibrary.Models
{
    public class SummaryReportData
    {
        public int Id { get; set; }
        public string Affcode { get; set; }
        public string Custname { get; set; }
        public string InvNo { get; set; }
        public DateTime Etd { get; set; }
        public string IssueNo { get;set; }
        public string ZName { get; set; }
    }

    public class SummaryReportDataDetail: SummaryReportData
    {
        public string PlNo { get; set; }
        public string PlSize { get; set; }
    }
}
