using DevExpress.Data.Mask;
using System;

namespace XPWLibrary.Models
{
    public class BookingCustomerData
    {
        public int id { get; set; }
        public string affcode { get; set; }
        public string custcode { get; set; }
        public string custname { get; set; }
    }

    public class BookingPlData : BookingCustomerData
    { 
        public string issuekey { get; set; }
        public string refinv { get; set; }
        public int poctn { get; set; }
        public int plctn { get; set; }
        public int booked { get; set; }
        public int ubook { get; set; }
    }

    public class BookingInvoicePallet: BookingPlData
    {
        public string pltype { get; set; }
        public string plno { get; set; }
        public int pltotal { get; set; }
        public string ploutno { get; set; }
        public int plstatus { get; set; }
        public bool slpl { get; set; }
        public string conno { get; set; }
        public string sealno { get; set; }
        public string consize { get; set; }
        public DateTime condate { get; set; }
    }
}
