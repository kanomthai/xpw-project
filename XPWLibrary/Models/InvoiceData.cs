using System;

namespace XPWLibrary.Models
{
    public class InvoiceData: OrderData
    {
        public string Zname { get; set; }
        public DateTime Etddte { get; set; }
        public string Bishpc { get; set; }
        public string Invoice { get; set; }
        public int Zoneid { get; set; }
        public string Ord { get; set; }
        public string Potype { get; set; }
        public int Itm { get; set; }
        public int Ctn { get; set; }
        public int Issue { get; set; }
        public int RmCtn { get; set; }
        public int ShCtn { get; set; }
        public int Pl { get; set; }
        public int Plno { get; set; }
        public int RmCon { get; internal set; }
        public int Conn { get; set; }
        public string Note1 { get; set; }
        public string Note2 { get; set; }
        public string Note3 { get; set; }
        public string ZCode { get; set; }
        public DateTime Upddte { get; set; }
    }

    public class InvoiceBodyData : InvoiceData
    {
        public string OrderNo { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string LotNo { get; set; }
        public int LotSeq { get; set; }
        public int BalQty { get; set; }
        public int BalCtn { get; set; }
        public int CurCtn { get; set; }
        public int WaitCtn { get; set; }
        public int PartRmCtn { get; set; }
        public int RemCtn { get; set; }
        public int StartFticket { get; set; }
    }

    public class InvoicePartDetailData : InvoiceBodyData
    {
        public string FticketNo { get; set; }
        public bool PrintTicket { get; set; }
    }
}
