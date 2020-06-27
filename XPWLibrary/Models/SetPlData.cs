using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPWLibrary.Models
{
    public class SetPlData : InvoicePartDetailData
    {
        public int id { get; set; }
        public string plno { get; set; }
        public string pldim { get; set; }
        public string pltype { get; set; }
        public int qty { get; set; }
        public int ctn { get; set; }
        public string unit { get; set; }
    }

    public class SetPallatData
    {
        public int Id { get; set; }
        public string Factory { get; set; }//FACTORY
        public string ShipType { get; set; }//SHIPTYPE
        public string ZName { get; set; }//ZNAME
        public DateTime EtdDte { get; set; }//ETDDTE
        public string AffCode { get; set; }//AFFCODE
        public string CustCode { get; set; }//BISHPC
        public string CustName { get; set; }//CUSTNAME
        public string CombInv { get; set; }//COMBINV
        public string RefInv { get; set; }//REFINVOICE
        public string RefNo { get; set; }//ISSUINGKEY
        public string  OrderNo {get;set;}//ORDERNO
        public string PName { get; set; }//PNAME
        public string PartNo { get; set; }//PARTNO
        public string PartName { get; set; }//PARTNAME
        public string PlSize { get; set; }//PLSIZE
        public int CtnQty { get; set; }//CTN
        public int Ctn { get; set; }//CTN
        public int Qty { get; set; }//CTN
        public int StdPack { get; set; }//CTN
        public string ShipPlNo { get; set; }//SHIPPLNO
        public bool MgrPl { get; set; }
    }

    public class SetPalletListData: SetPallatData
    { 
        public string PlOutNo { get; set; }
        public string FTicket { get; set; }
        public string SerialNo { get; set; }
        public string LotNo { get; set; }
        public string ContainerType { get; set; }
        public int ITem { get; set; }
        public int PrePareCtn { get; set; }
        public int ShCtn { get; set; }
        public int WaitCtn { get; set; }
        public int Status { get; set; }
    }
}
