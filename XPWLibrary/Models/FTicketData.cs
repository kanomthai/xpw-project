namespace XPWLibrary.Models
{
    public class FTicketData
    {
        public int Id { get; set; }
        public int Seq { get; set; }
        public string OrderNo { get; set; }
        public string PartNo { get; set; }
        public string LotNo { get; set; }
        public string FTicketNo { get; set; }
        public string SerialNo { get; set; }
        public int OrderQty { get; set; }
        public string Unit { get; set; }
        public string PlOutNo { get; set; }
        public int Status { get; set; }
        public bool PrintFTicket { get; set; }
    }
}
