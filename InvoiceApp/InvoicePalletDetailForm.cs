using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraBars;
using System.ComponentModel.DataAnnotations;
using XPWLibrary.Models;
using XPWLibrary.Interfaces;

namespace InvoiceApp
{
    public partial class InvoicePalletDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public InvoicePalletDetailForm(string plno)
        {
            InitializeComponent();
            this.Text = $"{plno} DETAIL";
            BindingList<PlDetailData> dataSource = GetDataSource(plno);
            gridControl.DataSource = dataSource;
            gridView.ClearSelection();
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }
        public BindingList<PlDetailData> GetDataSource(string plno)
        {
            BindingList<PlDetailData> list = new BindingList<PlDetailData>();
            string sql = $"SELECT c.PONO,c.PARTNO,p.PARTNAME,c.LOTNO,c.RUNNINGNO,c.RVMANAGINGNO,c.SHELVE FROM TXP_CARTONDETAILS c \n" +
                $"INNER JOIN TXP_PART p ON c.PARTNO = p.PARTNO\n" +
                $" WHERE c.PLOUTNO = '{plno}' \n" +
                "GROUP BY c.PONO,c.PARTNO,p.PARTNAME,c.LOTNO,c.RUNNINGNO,c.RVMANAGINGNO,c.SHELVE\n" +
                $"ORDER BY c.PONO,p.PARTNAME,c.LOTNO,c.RUNNINGNO";
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
                });
            }
            return list;
        }
    }
}