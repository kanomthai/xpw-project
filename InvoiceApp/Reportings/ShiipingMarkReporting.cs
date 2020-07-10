using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using System.Collections.Generic;
using XPWLibrary.Models;
using System.Data;
using XPWLibrary.Interfaces;

namespace InvoiceApp.Reportings
{
    public partial class ShiipingMarkReporting : DevExpress.XtraReports.UI.XtraReport
    {
        public ShiipingMarkReporting()
        {
            InitializeComponent();
        }

        internal void initData(string invoice, string plout)
        {
            List<ShipingMarkData> list = new List<ShipingMarkData>();
            string sql = $"SELECT * FROM TBT_SHIPPINGREPORT WHERE ISSUINGKEY = '{invoice}'";
            if (plout != null)
            {
                sql = $"SELECT * FROM TBT_SHIPPINGREPORT WHERE ISSUINGKEY = '{invoice}' AND PLOUT='{plout}'";
            }
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                DateTime d = DateTime.Parse(r["etd"].ToString());
                string qrcode = $"S|{r["custcode"].ToString()}|{d.ToString("yyyyMMdd")}|{r["invoiceno"].ToString()}|{int.Parse(r["plno"].ToString().Substring(2)).ToString()}|{r["plout"].ToString()}";
                List<string> orderno = GetPono(r["plout"].ToString());
                list.Add(new ShipingMarkData()
                {
                    Pname = "PNA",//r[""].ToString(),
                    PTech = "TECHNOLOGY",
                    PMadeIn = "MADE IN THAILAND",
                    PCount = "MALAYSIA",
                    PPallet = int.Parse(r["plno"].ToString().Substring(2)).ToString(),
                    POrder = string.Join(",", orderno),
                    PInvoice = r["invoiceno"].ToString(),
                    PQrCode = qrcode,
                });
            }
            objectDataSource1.DataSource = list;
        }

        private List<string> GetPono(string ploutno)
        {
            List<string> x = new List<string>();
            string sql = $"SELECT d.PONO FROM TXP_ISSPACKDETAIL d WHERE d.PLOUTNO ='{ploutno}' GROUP BY d.PONO";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                x.Add(r["pono"].ToString());
            }
            return x;
        }

    }
}
