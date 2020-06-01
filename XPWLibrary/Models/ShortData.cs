namespace XPWLibrary.Models
{
    public class ShortData
    {
        public int Id { get; set; }
        public int Seq { get; set; }
        public string Issuekey { get; set; }//ISSUINGKEY
        public string Pono { get; set; }//PONO
        public string PartNo { get; set; }//PARTNO
        public string FticketNo { get; set; }//FTICKETNO
        public string LotNo { get; set; }//LOTNO
        public string Sn { get; set; }//SN
        public int OrderQty { get; set; }//ORDERQTY
        public int CurStk { get; set; }//CURSTK
        public int WaitCtn { get; set; }//WAITREC
        public int StkNoSame { get; set; }//STKNOSAME
        public int PreCtn { get; set; }//PRECTN
        public int ShCtn { get; set; }//SHORDERQTY
        public bool CmSh { get; set; }//CMSH
    }
}