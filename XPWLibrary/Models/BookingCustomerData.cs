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

    public class Bookings
    {
        public int Id { get; set; }
        public DateTime Etd { get; set; }
        public string Invoice { get; set; }
        public string Custname { get; set; }
        public string ContainerSize { get; set; }
        public string ContainerNo { get; set; }
        public string SealNo { get; set; }
        public int LoadStatus { get; set; }
        public int CloseStatus { get; set; }
        public int GrossWeight { get; set; }
        public int NetWeight { get; set; }
        public string PlNo { get; set; }
        public string PlOutNo { get; set; }
        public string OrderNo { get; set; }
        public int Pallet { get; internal set; }
        public DateTime ReleaseDate { get;  set; }
    }
}
