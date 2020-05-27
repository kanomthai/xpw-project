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
using DevExpress.XtraSplashScreen;
using XPWLibrary.Interfaces;
using OrderApp;

namespace InvoiceApp
{
    public partial class InvoiceDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        InvoiceData ob;
        public InvoiceDetailForm(InvoiceData ord)
        {
            InitializeComponent();
            ob = ord;
            ReloadData();
        }

        void ReloadData()
        {
            SplashScreenManager.ShowDefaultWaitForm();
            this.Text = $"{ob.RefInv} DETAIL";
            bbiFactory.EditValue = ob.Factory;
            bbiShip.EditValue = ob.Ship;
            bbiZone.EditValue = ob.Zname;
            bbiOrderBy.EditValue = ob.Potype;
            bbiInv.EditValue = ob.Invoice;
            bbiRefInv.EditValue = ob.RefInv;
            bbiAff.EditValue = ob.Affcode;
            bbiCustCode.EditValue = ob.Bishpc;
            bbiCustName.EditValue = ob.Custname;
            bbiEtd.EditValue = ob.Etddte;
            bbiEtd.Enabled = false;
            bbiShip.Enabled = false;
            List<InvoiceBodyData> obj = new InvoiceControllers().GetInvoiceBody(ob);
            gridControl.DataSource = obj;
            bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
            SplashScreenManager.CloseDefaultWaitForm();
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            ReloadData();
        }

        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Location);
            if (e.Button.ToString() == "Right")
            {
                ppMenu.MenuCaption = gridView.GetFocusedRowCellValue("PartNo").ToString();
                bbiShowLotDetail.Enabled = false;
                if (StaticFunctionData.Factory == "INJ")
                {
                    bbiShowLotDetail.Caption = $"{gridView.GetFocusedRowCellValue("PartNo").ToString()} Detail";
                }
                else
                {
                    bbiShowLotDetail.Caption = $"{gridView.GetFocusedRowCellValue("LotNo").ToString()} Detail";
                }
                bbiEditOrder.Enabled = StaticFunctionData.EditOrder;
                ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else
            {
                ppMenu.HidePopup();
            }
        }

        private void bbiPlConfirm_ItemClick(object sender, ItemClickEventArgs e)
        {
            InvoiceConfirmInvForm frm = new InvoiceConfirmInvForm(ob.RefInv);
            frm.ShowDialog();
        }

        private void bbiPrintJobList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //OrderJobListPreviewForm frm = new OrderJobListPreviewForm(ob);
            //frm.ShowDialog();
        }

        private void bbiPrintCardBoard_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                InvoiceBodyData obj = gridView.GetFocusedRow() as InvoiceBodyData;
                InvoiceJobCardForm frm = new InvoiceJobCardForm(obj);
                frm.ShowDialog();
                ReloadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void bbiEditOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            bbiEtd.Enabled = true;
            bbiShip.Enabled = true;
        }

        private void bbiSetMultiLot_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult r;
            if (StaticFunctionData.confirmlotno)
            {
                r = XtraMessageBox.Show("ยืนยันการจ่ายด้วย LOTNO. อื่น", "ยืนยันคำสั่ง", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                {
                    Console.WriteLine($"UPDATE TXP_ISSTRANSBODY SET LOTSTKFLAG='Y' WHERE ISSUINGKEY = '{ob.RefInv}'");
                    bool x = new ConnDB().ExcuteSQL($"UPDATE TXP_ISSTRANSBODY SET LOTSTKFLAG='Y' WHERE ISSUINGKEY = '{ob.RefInv}'");
                    if (x)
                    {
                        XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
                    }
                }
            }
            else
            {
                r = XtraMessageBox.Show("ขอ อภัยคุณไม่มีสิทธิ์เข้าใช้งาน", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                {
                    return;
                }
            }
            return;
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            gridView.BeginUpdate();
            switch (e.Column.FieldName.ToString())
            {
                case "BalQty":
                case "BalCtn":
                case "ShCtn":
                case "PartRmCtn":
                    if (e.Value.ToString() == "0")
                    {
                        e.DisplayText = "";
                    }
                    else 
                    {
                        e.DisplayText = string.Format("{0:n0}", int.Parse(e.Value.ToString()));
                    }
                    break;
                case "Status":
                    e.DisplayText = "";
                    switch (e.Value.ToString())
                    {
                        case "0":
                            break;
                        case "1":
                            e.DisplayText = "JobList";
                            break;
                        case "2":
                            e.DisplayText = "Invoice";
                            break;
                        case "3":
                            e.DisplayText = "Shorted";
                            break;
                        case "4":
                            break;
                        case "5":
                            break;
                        case "6":
                            break;
                        case "7":
                            break;
                        case "8":
                            break;
                        case "9":
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            gridView.EndUpdate();
        }

        private void bbiShowLotDetail_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bbiPrintAllShipingLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool checkinv = new InvoiceControllers().CheckInvoiceStatus(ob.RefInv);
            if (checkinv)
            {
                List<InvoiceBodyData> obj = gridControl.DataSource as List<InvoiceBodyData>;
                SplashScreenManager.ShowDefaultWaitForm();
                int i = 0;
                while (i < obj.Count)
                {
                    InvoiceBodyData j = obj[i];
                    bool plabel = new InvoiceControllers().PrintFTicket(j.RefInv, j.PartNo, j.OrderNo, j.StartFticket, j.StartFticket.ToString());
                    if (plabel)
                    {
                        i++;
                    }
                }
                XtraMessageBox.Show("ปริ้นข้อมูลเสร็จแล้ว");
            }
            else
            {
                XtraMessageBox.Show("กรุณาทำการยืนยัน Invoice ก่อน", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}