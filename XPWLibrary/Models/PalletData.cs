namespace XPWLibrary.Models
{
    public class PalletData
    {
        public string PlNo { get; set; }
        public string PlOut { get; set; }
        public string PlType { get; set; }
        public string ContainerNo { get; set; }
        public int PlSize { get; set; }
        public int PlTotal { get; set; }
        public int PlStatus { get; set; }
    }

    public class PlDetailData
    { 
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public string PartNo { get; set; }
        public string PartName { get; set; }
        public string LotNo { get; set; }
        public string SerialNo { get; set; }
        public string Shelve { get; set; }
        public string RefInv { get; set; }
    }
}
