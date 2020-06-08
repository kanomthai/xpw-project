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
        List<InvoiceBodyData> spobj = new List<InvoiceBodyData>();
        public InvoiceDetailForm(InvoiceData ord)
        {
            InitializeComponent();
            ob = ord;
            ReloadData();
        }

        void ReloadData()
        {
            SplashScreenManager.ShowDefaultWaitForm();
            this.Text = StaticFunctionData.JobListTilte;
            groupControl2.Text = StaticFunctionData.JobListInformation;


            bbiPlConfirm.Caption = StaticFunctionData.JoblistConfirmInv;
            bbiPrintJobList.Caption = StaticFunctionData.JoblistPrintJobList;
            bbiShippingList.Caption = StaticFunctionData.JoblistShippingList;
            bbiShipingPart.Caption = StaticFunctionData.JoblistShippingListByPart;
            bbiShippingAll.Caption = StaticFunctionData.JoblistShippingListByAll;
            bbiPalletList.Caption = StaticFunctionData.JoblistPalletList;
            bbiSplitPart.Caption = StaticFunctionData.JoblistSplitPart;
            bbiSetMultiLot.Caption = StaticFunctionData.JoblistSetMultiLot;
            bbiPartShort.Caption = StaticFunctionData.JoblistPartShort;
            bbiEditOrder.Caption = StaticFunctionData.JoblistEditOrder;
            bbiReviseOrder.Caption = StaticFunctionData.JoblistOrderHold;
            bbiCancelOrder.Caption = StaticFunctionData.JoblistOrderCancel;


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
            bbiNewOrder.Enabled = false;
            bbiSplitInvoice.Caption = "";
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
                bbiSetMultiLot.Enabled = true;
                if (StaticFunctionData.Factory == "INJ")
                {
                    bbiSetMultiLot.Enabled = false;
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
            //InvoiceConfirmInvForm frm = new InvoiceConfirmInvForm(ob.RefInv);
            //frm.ShowDialog();
            string invno = bbiRefInv.EditValue.ToString();
            string invoiceno = new GreeterFunction().GetLastInvoice(invno);
            var result = XtraInputBox.Show("ระบุเลขที่เอกสาร", "ยืนยันการสร้าง Invoice", invoiceno);
            if (result.Length > 0)
            {
                string sql = $"UPDATE TXP_ISSTRANSENT e SET e.REFINVOICE = '{result}' WHERE e.ISSUINGKEY = '{invno}'";
                if (new ConnDB().ExcuteSQL(sql))
                {
                    XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
                    bbiInv.EditValue = invoiceno;
                }
            }
        }

        private void bbiPrintJobList_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderData obj = new OrderData();
            obj.Factory = ob.Factory;
            obj.Etd = ob.Etddte;
            obj.Ship = ob.Ship;
            obj.Affcode = ob.Affcode;
            obj.Custcode = ob.Bishpc;
            obj.Custname = ob.Custname;
            obj.CustPoType = ob.Ord;
            obj.PoType = ob.Ord;
            obj.Zone = ob.Zname;
            obj.Combinv = ob.Combinv;
            obj.RefNo = ob.RefInv;
            obj.RefInv = ob.Invoice;
            obj.Commercial = ob.Commercial;
            obj.BioABT = ob.BioABT;
            obj.Pc = ob.Pc;
            OrderJobListPreviewForm frm = new OrderJobListPreviewForm(obj);
            frm.ShowDialog();
        }

        private void bbiPrintCardBoard_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                InvoiceBodyData obj = gridView.GetFocusedRow() as InvoiceBodyData;
                InvoiceJobCardForm frm = new InvoiceJobCardForm(obj, false);
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
                case "RemCtn":
                case "LotSeq":
                case "PartRmCtn":
                    if (e.Value.ToString() == "0")
                    {
                        e.DisplayText = "";
                    }
                    else if (int.Parse(e.Value.ToString()) < 1)
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
                            e.DisplayText = "None";
                            break;
                        case "1":
                            e.DisplayText = "Remain";
                            break;
                        case "2":
                            e.DisplayText = "Prepare";
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
            DialogResult r = XtraMessageBox.Show("คุณต้องการที่จะปริ้น Shipping Label ใช่หรือไม่", "ยืนยันคำสั่ง", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
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

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                InvoiceBodyData obj = gridView.GetFocusedRow() as InvoiceBodyData;
                InvoiceJobCardForm frm = new InvoiceJobCardForm(obj, true);
                frm.ShowDialog();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private void bbiSplitPart_ItemClick(object sender, ItemClickEventArgs e)
        {
            InvoiceBodyData obj = gridView.GetFocusedRow() as InvoiceBodyData;
            DialogResult r;
            if (obj.PartRmCtn < 1)
            {
                r = XtraMessageBox.Show($"ยืนยันคำสั่ง Split {obj.PartNo}", "XPW Alert!", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                {
                    spobj.Add(obj);
                    gridView.DeleteSelectedRows();
                }
            }
            else
            {
                XtraMessageBox.Show($"ไม่สามารถ Split {obj.PartNo} ได้\nเนื่องจากทำการจัดเตรียมแล้ว", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (spobj.Count > 0)
            {
                bbiNewOrder.Enabled = true;
                bbiSplitInvoice.Caption = $"Split {spobj.Count}";
            }
            else
            {
                bbiNewOrder.Enabled = false;
                bbiSplitInvoice.Caption = $"";
            }
        }

        private void bbiNewOrder_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bbiPalletList_ItemClick(object sender, ItemClickEventArgs e)
        {
            InvoiceConfirmInvForm frm = new InvoiceConfirmInvForm(ob.RefInv);
            frm.ShowDialog();
        }

        private void bbiShipingPart_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                InvoiceBodyData obj = gridView.GetFocusedRow() as InvoiceBodyData;
                InvoiceJobCardForm frm = new InvoiceJobCardForm(obj, true);
                frm.ShowDialog();
                ReloadData();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}