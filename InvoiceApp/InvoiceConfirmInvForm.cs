using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using OrderApp;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace InvoiceApp
{
    public partial class InvoiceConfirmInvForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string invno;
        public InvoiceConfirmInvForm(string refinv)
        {
            InitializeComponent();
            invno = refinv;
            ReBuildingPallet();
        }

        void ReloadData()
        {
            List<PalletData> obj = new InvoiceControllers().GetPalletDetail(invno);
            gridControl.DataSource = obj;
            bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
        }


        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            ReloadData();
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderAddNewPalletForm frm = new OrderAddNewPalletForm(invno);
            frm.ShowDialog();
        }

        private void bbiEdit_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bbiRebuildPallet_ItemClick(object sender, ItemClickEventArgs e)
        {
            //rebuild pallete
            SplashScreenManager.ShowDefaultWaitForm();
            ReBuildingPallet();
            SplashScreenManager.CloseDefaultWaitForm();
        }

        void ReBuildingPallet()
        {
            if (invno.Substring(0, 1) == "I")
            {
                new GreeterFunction().SumPlInj(invno);
            }
            else
            {
                new GreeterFunction().SumPallet(invno);
            }
            ReloadData();
        }

        private void bbiConfirm_ItemClick(object sender, ItemClickEventArgs e)
        {
            string invoiceno = new GreeterFunction().GetLastInvoice(invno);
            var result = XtraInputBox.Show("ระบุเลขที่เอกสาร", "ยืนยันการสร้าง Invoice", invoiceno);
            if (result.Length > 0)
            {
                string sql = $"UPDATE TXP_ISSTRANSENT e SET e.REFINVOICE = '{result}' WHERE e.ISSUINGKEY = '{invno}'";
                if (new ConnDB().ExcuteSQL(sql))
                {
                    XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
                }
            }
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //gridView.BeginUpdate();
            switch (e.Column.FieldName.ToString())
            {
                case "PlSize":
                case "PlTotal":
                    if (e.Value.ToString() == "0")
                    {
                        e.DisplayText = "";
                    }
                    else 
                    {
                        e.DisplayText = string.Format("{0:n0}", int.Parse(e.Value.ToString()));
                    }
                    break;
                case "PlStatus":
                    switch (e.Value.ToString())
                    {
                        case "0":
                            e.DisplayText = "NONE";
                            break;
                        case "1":
                            e.DisplayText = "Invoice";
                            break;
                        case "2":
                            e.DisplayText = "Prepare";
                            break;
                        case "3":
                            e.DisplayText = "Booking";
                            break;
                        case "4":
                            e.DisplayText = "Send GEDI";
                            break;
                        case "5":
                            e.DisplayText = "Closed";
                            break;
                        case "6":
                            e.DisplayText = "Completed";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            //gridView.EndUpdate();
        }

        private void gridView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                bbiContainerDetail.Enabled = false;
                bbiPlDetail.Enabled = false;
                try
                {
                    var plout = gridView.GetFocusedRowCellValue("PlOut");
                    var conno = gridView.GetFocusedRowCellValue("ContainerNo");
                    if (plout != null)
                    {
                        bbiPlDetail.Enabled = true;
                    }
                    if (conno != null)
                    {
                        bbiContainerDetail.Enabled = true;
                    }
                }
                catch (System.Exception)
                {
                }
                ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else
            {
                ppMenu.HidePopup();
            }
        }

        private void bbiPlDetail_ItemClick(object sender, ItemClickEventArgs e)
        {
            var plout = gridView.GetFocusedRowCellValue("PlOut");
            InvoicePalletDetailForm frm = new InvoicePalletDetailForm(plout.ToString());
            frm.ShowDialog();
        }

        private void gridView_DoubleClick(object sender, System.EventArgs e)
        {
            try
            {
                var plout = gridView.GetFocusedRowCellValue("PlOut");
                InvoicePalletDetailForm frm = new InvoicePalletDetailForm(plout.ToString());
                frm.ShowDialog();
            }
            catch (System.Exception)
            {
            }
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var plout = gridView.GetFocusedRowCellValue("PlOut");
            if (plout != null)
            {
                DialogResult r = XtraMessageBox.Show("ยืนยันคำสั่ง Reprint Palet", "XPW Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                {
                    string sql = $"UPDATE TXP_LOADPALLET l SET l.PLOUTSTS = 0,l.PRINTDATE=sysdate,l.UPDDTE=sysdate WHERE l.PLOUTNO = '{plout}'";
                    if (new ConnDB().ExcuteSQL(sql))
                    {
                        gridView.SetFocusedRowCellValue("PlOut", 1);
                        XtraMessageBox.Show("อัพเดทข้อมูลเสร็จแล้ว");
                    }
                }
            }
        }
    }
}