using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPWLibrary.Models
{
    public class PartShortData
    {
        public int Id {get; set;}// = list.Count + 1,
        public DateTime Etd {get; set;} // = DateTime.Parse(r["etddte"].ToString()),
        public string Custname { get; set; } // = r["custname"].ToString(),//custname
        public string Invoice { get; set; } // = r["issuingkey"].ToString(),//issuingkey
        public string OrderNumber { get; set; } // = r["pono"].ToString(),//pono
        public string Lotno { get; set; } // = r["lotno"].ToString(),//lotno
        public string PartNo { get; set; } // = r["partno"].ToString(),//partname
        public string Partname { get; set; } // = r["partname"].ToString(),//partname
        public int Orderctn { get; set; } // = fn.GetTypeNumber(r["orderqty"].ToString()),//orderctn
        public int Prepctn { get; set; } // = fn.GetTypeNumber(r["prepqty"].ToString()),//prepctn
        public int Shqty { get; set; } // = fn.GetTypeNumber(r["shqty"].ToString()),//recctn
        public int Stdqty { get; set; } // = fn.GetTypeNumber(r["stdpack"].ToString()),//remctn
        public int Currentstk { get; set; } // = fn.GetTypeNumber(r["stklot"].ToString()),//lotstk
        public int Lotstk { get; set; } // = fn.GetTypeNumber(r["stklot"].ToString()),//lotstk
        public int Ormon { get; set; } // = fn.GetTypeNumber(r["ormon"].ToString()),//ormon
        public int Ortue { get; set; } // = fn.GetTypeNumber(r["ortue"].ToString()),//ortue
        public int Orwed { get; set; } // = fn.GetTypeNumber(r["orwed"].ToString()),//orwed
        public int Orthu { get; set; } // = fn.GetTypeNumber(r["orthu"].ToString()),//orthu
        public int Orfri { get; set; } // = fn.GetTypeNumber(r["orfri"].ToString()),//orfri
        public int Orsat { get; set; } // = fn.GetTypeNumber(r["orsat"].ToString()),//orsat
        public int Orsun { get; set; } // = fn.GetTypeNumber(r["orsun"].ToString()),//orsun
        public string Zone { get; set; } // = r["area"].ToString(),
        public string Shiptype { get; set; } // = r["shiptype"].ToString(),
    }

    public class SummaryData
    {
        public int Id { get; set; }
        public string Partno { get; set; }
        public string Partname { get; set; }
        public int StdPack { get; set; }
        public int CurCtn { get; set; }
        public int RecCtn { get; set; }
        public int OrderCtn { get; set; }
        public int PrepCtn { get; set; }
        public int ShortCtn { get; set; }
        public string AreaName { get; set; }
        public string ShipType { get; set; }
        public int Ormon { get; set; }
        public int Ortue { get; set; }
        public int Orwed { get; set; }
        public int Orthu { get; set; }
        public int Orfri { get; set; }
        public int Orsat { get; set; }
        public int Orsun { get; set; }
    }
}
