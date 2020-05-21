using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPWLibrary.Models
{
    public class JobListData : OrderBody
    {
        public string Shelve { get; set; }
        public string PlNo { get; set; }
        public int Total { get; set; }
    }

    public class PlReportingData : JobListData
    {
        public string SerialNo { get; set; }
        public string PalletOut { get; set; }
    }
}
