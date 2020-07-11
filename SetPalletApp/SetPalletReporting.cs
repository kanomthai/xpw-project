using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using DevExpress.XtraReports.UI;
using XPWLibrary.Controllers;
using XPWLibrary.Models;
using System.Collections.Generic;

namespace SetPalletApp
{
    public partial class SetPalletReporting : DevExpress.XtraReports.UI.XtraReport
    {
        public SetPalletReporting()
        {
            InitializeComponent();
        }

        internal void InitData(string refinv)
        {
            List<SetPalletListData>  obj =new SetPalletControllers().GetJobListPallet(refinv);
            if (obj.Count > 0)
            {
                int x = 0;
                obj.ForEach(i => {
                    x += i.ITem;
                });
                prInvoiceNo.Value = obj[0].RefInv;
                prRefNo.Value = obj[0].RefNo;
                prQrCode.Value = obj[0].RefNo;
                prCustCode.Value = obj[0].CustCode;
                prCountry.Value = obj[0].CustName;
                prShipType.Value = obj[0].ShipType;
                prFactory.Value = obj[0].Factory;
                prGroupOrder.Value = obj[0].CombInv;
                pPartName.Value = "PARTNO";
                if (obj[0].Factory != "INJ")
                {
                    pPartName.Value = "PARTNAME";
                }

                prContainerType.Value = obj[0].ContainerType;
                prDesinations.Value = "";
                prEtdDate.Value = obj[0].EtdDte;
                prQty.Value = x;
                prPrintDate.Value = DateTime.Now;
            }
            objectDataSource1.DataSource = obj;
        }
    }
}
