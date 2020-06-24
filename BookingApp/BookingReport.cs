using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using XPWLibrary.Controllers;
using XPWLibrary.Models;
using System.Collections.Generic;

namespace BookingApp
{
    public partial class BookingReport : DevExpress.XtraReports.UI.XtraReport
    {
        public BookingReport()
        {
            InitializeComponent();
        }

        internal void initData(string containerno)
        {
            List<Bookings> list = new BookingControllers().GetContainerListDetail(containerno) as List<Bookings>;
            objectDataSource1.DataSource = list;
            if (list.Count > 0)
            {
                DateTime d = DateTime.Parse(list[0].Etd.ToString());
                DateTime prDate = DateTime.Now;
                pPrintDate.Value = prDate.ToString("dd/MM/yyyy HH:mm:ss");
                pQrCode.Value = $"|{containerno}|{d.ToString("ddMMyyyy")}";
                pCustname.Value = list[0].Custname;
                pSize.Value = list[0].ContainerSize;
                pContainerNumber.Value = containerno;
                pEtdDate.Value = d;
            }
        }
    }
}
