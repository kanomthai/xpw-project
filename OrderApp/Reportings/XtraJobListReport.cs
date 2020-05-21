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

namespace OrderApp.Reportings
{
    public partial class XtraJobListReport : DevExpress.XtraReports.UI.XtraReport
    {
        public XtraJobListReport()
        {
            InitializeComponent();
        }

        internal void InitData(string invoice)
        {
            string orderfilter = $"ORDER BY b.ORDERID,p.KIDS,p.SIZES,b.LOTNO,ROUND(b.ORDERQTY / b.STDPACK),p.PARTNO";
            if (invoice.Substring(0, 1) == "I")
            {
                orderfilter = $"ORDER BY b.ORDERID,p.PARTNO,b.LOTNO,ROUND(b.ORDERQTY / b.STDPACK)";
            }
            string sql = "SELECT SUBSTR(b.PONO, LENGTH(b.PONO) -2, 3) checkord,e.ISSUINGKEY,e.REFINVOICE,e.FACTORY,e.ETDDTE,e.SHIPTYPE,e.BISHPC,e.CUSTNAME,m.POTYPE,b.PARTNO,b.PARTNAME,b.PONO,b.ORDERID,b.ORDERQTY,ROUND(b.ORDERQTY/b.STDPACK) ctn,b.LOTNO,'' shelve,'' plno, 0 itemctn,0 total,get_zone(e.FACTORY,e.ZONEID) zname FROM TXP_ISSTRANSENT e\n" +
                        $"INNER JOIN TXP_ISSTRANSBODY b ON e.ISSUINGKEY = b.ISSUINGKEY\n" +
                        $"INNER JOIN TXM_CUSTOMER m ON e.AFFCODE = m.AFFCODE AND e.BISHPC = m.BISHPC AND e.CUSTNAME = m.CUSTNM AND e.FACTORY = m.FACTORY \n" +
                        $"INNER JOIN TXP_PART p ON b.PARTNO = p.PARTNO AND e.FACTORY = p.VENDORCD  \n" +
                        $"WHERE b.ISSUINGKEY = '{invoice}' AND b.ORDERQTY > 0\n" +
                        $"{orderfilter}";

            DataSet dr = new ConnDB().GetFill(sql);
            List<JobListData> list = new List<JobListData>();
            int xqty = 0;
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                string partno = r["partname"].ToString();
                if (invoice.Substring(0, 1) == "I")
                {
                    partno = r["partno"].ToString();
                }
                xqty += int.Parse(r["orderqty"].ToString());
                list.Add(new JobListData()
                {
                    Id = list.Count + 1,
                    RefInv = r["refinvoice"].ToString(),
                    Zone = r["zname"].ToString(),
                    Custcode = r["bishpc"].ToString(),
                    Custname = r["custname"].ToString(),
                    Etd = DateTime.Parse(r["etddte"].ToString()),
                    Ship = r["shiptype"].ToString(),
                    Factory = r["factory"].ToString(),
                    CustPoType = r["checkord"].ToString(),
                    PoType = r["potype"].ToString(),
                    OrderNo = r["pono"].ToString(),
                    PartNo = r["partno"].ToString(),
                    PartName = partno,
                    BalQty = int.Parse(r["orderqty"].ToString()),
                    Ctn = int.Parse(r["ctn"].ToString()),
                    LotNo = r["lotno"].ToString(),
                });
            }
            int ctn = 0;
            list.ForEach(i => ctn += i.Ctn);
            if (list.Count > 0)
            {
                var r = list[0];
                prQrCode.Value = invoice;
                prRefNo.Value = invoice;
                prInvoice.Value = r.RefInv;
                prZone.Value = r.Zone;
                prCountry.Value = r.Custname;
                prEtd.Value = r.Etd;
                prShipType.Value = r.Ship;
                prFactory.Value = r.Factory;
                prGroupOrder.Value = new GreeterFunction().GetOrderGroupBy(r.PoType, r.CustPoType);
                prPrintDate.Value = DateTime.Now;
                prCustCode.Value = r.Custcode;
                prBalQty.Value = xqty;
                prTotal.Value = ctn;
            }

            objectDataSource1.DataSource = list;
            SplashScreenManager.CloseDefaultWaitForm();
        }
    }
}
