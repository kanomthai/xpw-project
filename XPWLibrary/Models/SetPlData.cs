using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPWLibrary.Models
{
    public class SetPlData: InvoicePartDetailData
    {
        public int id { get; set; }
        public string plno { get; set; }
        public string pldim { get; set; }
        public string pltype { get; set; }
        public int qty { get; set; }
        public int ctn { get; set; }
        public string unit { get; set; }
    }
}
