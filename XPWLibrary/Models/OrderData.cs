using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPWLibrary.Models
{
    public class OrderData
    {
        public int Id { get; set; }
        public string Factory { get; set; }
        public DateTime Etd { get; set; }
        public string Ship { get; set; }
        public string Zone { get; set; }
        public string Affcode { get; set; }
        public string Custcode { get; set; }
        public string Custname { get; set; }
        public string CustPoType { get; set; }
        public string Prefix { get; set; }
        public string PoType { get; set; }
        public string Commercial { get; set; }
        public string Pc { get; set; }
        public string BioABT { get; set; }
        public string BiComb { get; set; }
        public int OrderCtn { get; set; }
        public int ItemCtn { get; set; }
        public string RefNo { get; set; }
        public string RefInv { get; set; }
        public int Status { get; set; }
        public string Combinv { get; set; }
        public string OrderRewrite { get; set; }
        public DateTime LastUpdate { get; set; }
    }

    public class OrderBody : OrderData
    {
        public string OrderNo { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string LotNo { get; set; }
        public int LotSeq { get; set; }
        public int BalQty { get; set; }
        public int Ctn { get; set; }
        public string ReasonCD { get; set; }
        public string Uuid { get; set; }//UUID    UUID	15	VARCHAR2				36	36	0	VARCHAR
        public int BiSTDP { get; set; }//BISTDP  BISTDP	16	NUMBER				10	9	0	NUMERIC
        public int BiWidt { get; set; }//BIWIDT  BIWIDT	17	NUMBER				5	4	0	NUMERIC
        public int BiLeng { get; set; }//BILENG  BILENG	18	NUMBER				5	4	0	NUMERIC
        public int BiNetW { get; set; }//BINEWT  BINEWT	19	NUMBER				11	9	3	NUMERIC
        public int BiHigh { get; set; }//BIHIGH  BIHIGH	20	NUMBER				5	4	0	NUMERIC
        public int BiGrwt { get; set; }//BIGRWT  BIGRWT	21	NUMBER				11	9	3	NUMERIC
        public string PartType { get; set; }
        public string OrderType { get; set; }//ORDERTYPE   ORDERTYPE	22	VARCHAR2				1	1	0	VARCHAR
        public string BiComd { get; set; }//BICOMD  BICOMD	24	VARCHAR2				5	5	0	VARCHAR
    }
}
