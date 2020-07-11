using DevExpress.XtraBars;
using System;
using System.ComponentModel;
using System.Data;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace InvoiceApp
{
    public partial class InvoicePalletDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        PalletData plno = null;
        public InvoicePalletDetailForm(PalletData pl)
        {
            InitializeComponent();
            plno = pl;
            this.Text = $"{plno} DETAIL";
            ReBuildingPallet();
        }

        void ReloadData()
        {
            BindingList<PlDetailData> dataSource = GetDataSource();
            gridControl.DataSource = dataSource;
            gridView.ClearSelection();
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }
        public BindingList<PlDetailData> GetDataSource()
        {
            BindingList<PlDetailData> list = new BindingList<PlDetailData>();
            string sql = $"SELECT d.PONO,d.PARTNO,b.PARTNAME,b.LOTNO,d.CTNSN RUNNINGNO,'' RVMANAGINGNO,'' SHELVE,d.ISSUINGKEY,d.* FROM TXP_ISSPACKDETAIL d\n"+
                    $"INNER JOIN TXP_ISSTRANSBODY b ON d.ISSUINGKEY = b.ISSUINGKEY AND d.PONO = b.PONO AND d.PARTNO = b.PARTNO\n"+
                    $"INNER JOIN TXP_ISSPALLET l ON d.ISSUINGKEY = l.ISSUINGKEY AND d.SHIPPLNO = l.PALLETNO\n"+
                    $"WHERE l.PALLETNO = '{plno.PlNo}' AND l.ISSUINGKEY = '{plno.RefNo}'";
            if (plno.PlOut != "")
            {
                sql = $"SELECT c.PONO,c.PARTNO,p.PARTNAME,c.LOTNO,c.RUNNINGNO,c.RVMANAGINGNO,c.SHELVE,l.ISSUINGKEY FROM TXP_CARTONDETAILS c \n" +
                $"INNER JOIN TXP_PART p ON c.PARTNO = p.PARTNO\n" +
                "LEFT JOIN TXP_ISSPALLET l ON c.PLOUTNO = l.PLOUTNO\n" +
                $"WHERE c.PLOUTNO = '{plno.PlOut}' \n" +
                "GROUP BY c.PONO,c.PARTNO,p.PARTNAME,c.LOTNO,c.RUNNINGNO,c.RVMANAGINGNO,c.SHELVE,l.ISSUINGKEY\n" +
                $"ORDER BY c.PONO,p.PARTNAME,c.LOTNO,c.RUNNINGNO";
            }
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new PlDetailData()
                {
                    Id = list.Count + 1,
                    OrderNo = r["pono"].ToString(),
                    PartNo = r["partno"].ToString(),
                    PartName = r["partname"].ToString(),
                    LotNo = r["lotno"].ToString(),
                    SerialNo = r["runningno"].ToString(),
                    Shelve = r["shelve"].ToString(),
                    RefInv = r["issuingkey"].ToString(),
                });
            }
            return list;
        }

        void ReBuildingPallet()
        {
            ReloadData();
            //if (invno.Substring(0, 1) == "I")
            //{
            //    if (new GreeterFunction().SumPlInj(invno))
            //    {
            //        ReloadData();
            //    }
            //}
            //else
            //{
            //    if (new GreeterFunction().SumPallet(invno))
            //    {
            //        ReloadData();
            //    }
            //}
        }
    }
}