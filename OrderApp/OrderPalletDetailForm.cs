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
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using DevExpress.XtraSplashScreen;

namespace OrderApp
{
    public partial class OrderPalletDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string refno = "";
        public OrderPalletDetailForm(OrderData obj)
        {
            InitializeComponent();
            List<string> refkey = new OrderControllers().GetOrderRefinvoice(obj);
            refno = "'"+string.Join("','", refkey)+"'";
            this.Text = $"{refno.Replace("'", "").Trim()} PALLET DETAIL";
            ReloadData();
        }

        void ReloadData()
        {
            SplashScreenManager.ShowDefaultWaitForm();
            //if (refno.Replace("'", "").Trim().StartsWith("I"))
            //{
            //    new GreeterFunction().SumPlInj(refno.Replace("'", "").Trim());
            //}
            //else
            //{
            //    new GreeterFunction().SumPallet(refno.Replace("'", "").Trim());
            //}
            BindingList<PalletData> dataSource = GetDataSource();
            gridControl.DataSource = dataSource;
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
            SplashScreenManager.CloseDefaultWaitForm();
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }
        public BindingList<PalletData> GetDataSource()
        {
            BindingList<PalletData> list = new BindingList<PalletData>();
            string sql = $"SELECT l.PALLETNO,l.PLOUTNO,l.PLTYPE,l.CONTAINERNO,l.PLTOTAL,case " +
                $"when l.PLOUTSTS is null then '0' else l.PLOUTSTS end PLOUTSTS FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY in ({refno}) " +
                $"ORDER BY SUBSTR(l.PALLETNO, 1, 2) DESC,l.PALLETNO";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new PalletData()
                { 
                    PlNo = r["palletno"].ToString(),
                    PlOut = r["ploutno"].ToString(),
                    PlType = r["pltype"].ToString(),
                    ContainerNo = r["containerno"].ToString(),
                    PlSize = int.Parse(r["pltotal"].ToString()),
                    PlStatus = int.Parse(r["ploutsts"].ToString()),
                });
            }
            return list;
        }

        private void gridControl_DoubleClick(object sender, EventArgs e)
        {
            string plno = gridView.GetFocusedRowCellValue("PlOut").ToString();
            if (plno != "")
            {
                //Console.WriteLine(refno);
                Console.WriteLine(plno);
                OrderPalletForm frm = new OrderPalletForm(plno);
                frm.ShowDialog();
            }
            else
            {
                XtraMessageBox.Show("ไม่พบข้อมูล", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbiConfirmInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            string invoiceno = new GreeterFunction().GetLastInvoice(refno.Replace("'", "").Trim());
            var result = XtraInputBox.Show("ระบุเลขที่เอกสาร", "ยืนยันการสร้าง Invoice", invoiceno);
            if (result.Length > 0)
            {
                string sql = $"UPDATE TXP_ISSTRANSENT e SET e.REFINVOICE = '{result}',e.ISSUINGSTATUS=2 WHERE e.ISSUINGKEY = '{refno.Replace("'", "").Trim()}'";
                string sqlord = $"UPDATE TXP_ORDERPLAN e SET e.ORDERSTATUS=2 WHERE e.CURINV = '{refno.Replace("'", "").Trim()}'";
                new ConnDB().ExcuteSQL(sqlord);
                if (new ConnDB().ExcuteSQL(sql))
                {
                    XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
                    ReloadData();
                }
            }
        }

        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            try
            {
                if (e.Button.ToString() == "Right")
                {
                    popupMenu1.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
                }
                else
                {
                    popupMenu1.HidePopup();
                }
            }
            catch (Exception)
            {
            }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderAddNewPalletForm frm = new OrderAddNewPalletForm(refno.Replace("'", "").Trim());
            frm.ShowDialog();
        }
    }
}