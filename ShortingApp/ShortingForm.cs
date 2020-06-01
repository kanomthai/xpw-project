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
using System.IO;

namespace ShortingApp
{
    public partial class ShortingForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        OrderData obj = new OrderData();
        BindingList<ShortData> shlist = new BindingList<ShortData>();
        public ShortingForm(OrderData ob)
        {
            InitializeComponent();
            bbiConfirmShorting.Enabled = true;
            barStaticItem1.Caption = "";
            obj = ob;
            if (obj.Custname!= null)
            {
                BindingList<ShortData> dataSource = GetDataSource(obj.RefNo);
                gridControl.DataSource = dataSource;
                bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
            }
        }

        private BindingList<ShortData> GetDataSource(string refNo)
        {
            BindingList<ShortData> list = new BindingList<ShortData>();
            string sql = $"SELECT * FROM TBT_SHORTING t WHERE t.ISSUINGKEY = '{refNo}' ORDER BY t.PONO,t.PARTNO";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new ShortData()
                { 
                    Id = list.Count,
                    Seq = list.Count + 1,
                    Issuekey = r["issuingkey"].ToString(),///ISSUINGKEY
                    Pono=r["pono"].ToString(),//PONO
                    PartNo = r["partno"].ToString(),//PARTNO
                    FticketNo = r["fticketno"].ToString(),//FTICKETNO
                    LotNo = r["lotno"].ToString(),//LOTNO
                    Sn = r["sn"].ToString(),//Sn//SN
                    OrderQty = int.Parse(r["orderqty"].ToString()),//OrderQty//ORDERQTY
                    CurStk = int.Parse(r["curstk"].ToString()),//CurStk//CURSTK
                    WaitCtn = int.Parse(r["waitrec"].ToString()),//WaitCtn//WAITREC
                    StkNoSame = int.Parse(r["stknosame"].ToString()),//StkNoSame//STKNOSAME
                    PreCtn = int.Parse(r["prectn"].ToString()),//PreCtn//PRECTN
                    ShCtn = int.Parse(r["shorderqty"].ToString()),//ShCtn//SHORDERQTY
                    CmSh =false//CMSH
                });
            }
            return list;
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            //gridControl.ShowRibbonPrintPreview();
        }

        private void bandedGridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName.ToString())
            {
                case "OrderQty":
                case "CurStk":
                case "WaitCtn":
                case "StkNoSame":
                case "PreCtn":
                case "ShCtn":
                    e.DisplayText = "";
                    if (int.Parse(e.Value.ToString()) > 0)
                    {
                        e.DisplayText = string.Format("{0:n0}", int.Parse(e.Value.ToString()));
                    }
                    break;
                default:
                    break;
            }
        }

        private void bbiConfirm_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("ยืนยันคำสั่งตัด Short", "XPW Alert!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {
                BindingList<ShortData> ob = gridControl.DataSource as BindingList<ShortData>;
                ShortData a = bandedGridView.GetFocusedRow() as ShortData;
                shlist.Add(a);
                bandedGridView.DeleteSelectedRows();
                //ob.RemoveAt(a.Id);
                //gridControl.DataSource = ob;
            }
            if (shlist.Count > 0)
            {
                bbiConfirmShorting.Enabled = true;
                barStaticItem1.Caption = $"SHORTING NOT COMFIRM: {shlist.Count}";
            }
            else
            {
                bbiConfirmShorting.Enabled = false;
                barStaticItem1.Caption = $"";
            }
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            BindingList<ShortData> dataSource = GetDataSource(obj.RefNo);
            gridControl.DataSource = dataSource;
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
            shlist.Clear();
            barStaticItem1.Caption = $"";
            bbiConfirmShorting.Enabled = false;
        }

        void CreateOrderShorting()
        {
            int i = 0;
            while (i < shlist.Count)
            {
                Console.WriteLine($"edit {shlist[i].Issuekey} {shlist[i].PartNo} {shlist[i].FticketNo} ");
                i++;
            }
        }

        private void ShortingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult r;
            if (shlist.Count > 0)
            {
                r = XtraMessageBox.Show("คุณต้องการที่จะบันทึกข้อมูลตัด Short ก่อนหรือไม่?", "XPW Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (r == DialogResult.Yes)
                {
                    CreateOrderShorting();
                }
            }
        }

        private void bbiConfirmShorting_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult r;
            if (shlist.Count > 0)
            {
                r = XtraMessageBox.Show("ยืนยันการบันทึกข้อมูลตัด Short", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (r == DialogResult.OK)
                {
                    CreateOrderShorting();
                    shlist.Clear();
                    barStaticItem1.Caption = $"";
                    bbiConfirmShorting.Enabled = false;
                }
            }
        }

        private void ShortingForm_Load(object sender, EventArgs e)
        {
            string shtemp = $"{AppDomain.CurrentDomain.BaseDirectory}Templates\\new_current_shorting.xml";
            if (File.Exists(shtemp) == false)
            {
                shtemp = $"{AppDomain.CurrentDomain.BaseDirectory}Templates\\current_shorting.xml";
            }
            bandedGridView.RestoreLayoutFromXml(shtemp);
        }

        private void bandedGridView_Layout(object sender, EventArgs e)
        {
            string shtemp = $"{AppDomain.CurrentDomain.BaseDirectory}Templates\\new_current_shorting.xml";
            bandedGridView.SaveLayoutToXml(shtemp);
        }
    }
}