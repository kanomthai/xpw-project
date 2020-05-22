using DevExpress.XtraBars;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace OrderApp
{
    public partial class OrderPalletForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string plno = "";
        public OrderPalletForm(string pl)
        {
            InitializeComponent();
            plno = pl;
            this.Text = $"{plno} DETAIL";
            ReLoadPallet();
        }

        void ReLoadPallet()
        {
            BindingList<PlDetailData> dataSource = GetDataSource(plno);
            gridControl.DataSource = dataSource;
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }
        public BindingList<PlDetailData> GetDataSource(string plno)
        {
            string sql = $"SELECT c.PONO,c.PARTNO,p.PARTNAME,c.LOTNO,c.RUNNINGNO,c.RVMANAGINGNO,c.SHELVE,l.ISSUINGKEY FROM TXP_CARTONDETAILS c \n" +
                $"INNER JOIN TXP_PART p ON c.PARTNO = p.PARTNO\n" +
                $"LEFT JOIN TXP_ISSPALLET l ON c.PLOUTNO = l.PLOUTNO\n" +
                $"WHERE c.PLOUTNO = '{plno}' \n" +
                "GROUP BY c.PONO,c.PARTNO,p.PARTNAME,c.LOTNO,c.RUNNINGNO,c.RVMANAGINGNO,c.SHELVE,l.ISSUINGKEY\n" +
                $"ORDER BY c.PONO,p.PARTNAME,c.LOTNO,c.RUNNINGNO";
            System.Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            BindingList<PlDetailData> list = new BindingList<PlDetailData>();
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

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            ReLoadPallet();
        }
    }
}