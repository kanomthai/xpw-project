using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Data;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;
using DevExpress.XtraSplashScreen;
using System.Collections.Generic;
using XPWLibrary.Controllers;

namespace OrderApp.Reportings
{
    public partial class XtraJobListReport : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraJobListReport()
        {
            InitializeComponent();
        }

        internal void InitData(OrderData obj)
        {
            List<OrderBody> ob = new OrderControllers().GetReportJobList(obj);
            int ctn = 0;
            int xqty = 0;
            ob.ForEach(i => {
                ctn += i.Ctn;
                xqty += i.BalQty;
            });
            if (ob.Count > 0)
            {
                var r = ob[0];
                prQrCode.Value = r.RefNo;
                prRefNo.Value = r.RefNo;
                prInvoice.Value = r.RefInv;
                prZone.Value = r.Zone;
                prCountry.Value = r.Custname;
                prEtd.Value = r.Etd;
                prShipType.Value = r.Ship;
                prFactory.Value = r.Factory;
                prGroupOrder.Value = r.PoType;
                prPrintDate.Value = DateTime.Now;
                prCustCode.Value = r.Custcode;
                prBalQty.Value = xqty;
                prTotal.Value = ctn;
            }

            objectDataSource1.DataSource = ob;
            SplashScreenManager.CloseDefaultWaitForm();
        }
    }
}
