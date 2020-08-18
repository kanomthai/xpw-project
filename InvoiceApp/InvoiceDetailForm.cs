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
using SetPalletApp;

namespace InvoiceApp
{
    public partial class InvoiceDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        InvoiceData ob;
        List<InvoiceBodyData> spobj = new List<InvoiceBodyData>();
        List<InvoiceBodyData> shlist = new List<InvoiceBodyData>();
        bool chorderdate = false;
        public InvoiceDetailForm(InvoiceData ord)
        {
            InitializeComponent();
            ob = ord;
            ReloadData();
        }

        void ReloadData()
        {
            SplashScreenManager.ShowDefaultWaitForm();
            chorderdate = false;
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
            bbiAddShorting.Caption = StaticFunctionData.JoblistPartShort;
            bbiConfirmShort.Caption = StaticFunctionData.JoblistOrderShorting;

            bbiFactory.EditValue = ob.Factory;
            bbiShip.EditValue = ob.Ship;
            bbiZone.EditValue = ob.Zname;
            bbiOrderBy.EditValue = ob.Ord;
            bbiInv.EditValue = ob.Invoice;
            bbiRefInv.EditValue = ob.RefInv;
            bbiAff.EditValue = ob.Affcode;
            bbiCustCode.EditValue = ob.Bishpc;
            bbiCustName.EditValue = ob.Custname;
            bbiEtd.EditValue = ob.Etddte;
            txtNote1.EditValue = ob.Note1;
            txtNote2.EditValue = ob.Note2;
            txtZoneCode.EditValue = ob.ZCode;
            bbiConTypeCaption.EditValue = ob.ContainerType;
            bbiEtd.Enabled = false;
            bbiShip.Enabled = false;
            bbiNewOrder.Enabled = false;
            bbiConfirmShort.Enabled = false;
            bbiSplitInvoice.Caption = "";
            List<InvoiceBodyData> obj = new InvoiceControllers().GetInvoiceBody(ob);
            gridControl.DataSource = obj;
            bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
            SplashScreenManager.CloseDefaultWaitForm();
            if (shlist.Count > 0)
            {
                bbiConfirmShort.Enabled = true;
            }
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            shlist.Clear();
            spobj.Clear();
            ReloadData();
        }

        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            Console.WriteLine(e.Location);
            if (e.Button.ToString() == "Right")
            {
                ppMenu.MenuCaption = gridView.GetFocusedRowCellValue("PartNo").ToString();
                bbiShowLotDetail.Enabled = false;
                bbiPlConfirm.Enabled = false;
                bbiSetMultiLot.Enabled = true;
                bbiAddShorting.Enabled = true;
                //check print shipping
                bbiShipingPart.Enabled = new SetPalletControllers().CheckShippingSeq(bbiRefInv.EditValue.ToString());
                if (StaticFunctionData.Factory == "INJ")
                {
                    bbiSetMultiLot.Enabled = false;
                    bbiShowLotDetail.Caption = $"{gridView.GetFocusedRowCellValue("PartNo").ToString()} Detail";
                }
                else
                {
                    bbiShowLotDetail.Caption = $"{gridView.GetFocusedRowCellValue("LotNo").ToString()} Detail";
                }
                if (gridView.GetFocusedRowCellValue("RemCtn").ToString() == "0")
                {
                    bbiAddShorting.Enabled = false;
                }
                bbiEditOrder.Enabled = StaticFunctionData.EditOrder;
                if (bbiInv.EditValue.ToString().Length > 10)
                {
                    bbiPlConfirm.Enabled = true;
                }
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
            //bool chinvconfirm = new GreeterFunction().CheckUpdateInvoice(DateTime.Parse(bbiEtd.EditValue.ToString()));
            //if (chinvconfirm)
            //{
            //    string invno = bbiRefInv.EditValue.ToString();
            //    string invoiceno = new GreeterFunction().GetLastInvoice(invno);
            //    var result = XtraInputBox.Show("ระบุเลขที่เอกสาร", "ยืนยันการสร้าง Invoice", invoiceno);
            //    if (result.Length > 0)
            //    {
            //        string sql = $"UPDATE TXP_ISSTRANSENT e SET e.REFINVOICE = '{result}' WHERE e.ISSUINGKEY = '{invno}'";
            //        if (new ConnDB().ExcuteSQL(sql))
            //        {
            //            XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
            //            bbiInv.EditValue = invoiceno;
            //        }
            //    }
            //}
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
            //gridView.BeginUpdate();
            switch (e.Column.FieldName.ToString())
            {
                case "BalQty":
                case "BalCtn":
                case "ShCtn":
                case "RemCtn":
                case "LotSeq":
                case "PartRmCtn":
                case "CurCtn":
                case "WaitCtn":
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
                            e.DisplayText = "Wait";
                            break;
                        //case "1":
                        //    e.DisplayText = "1";
                        //    break;
                        case "2":
                            e.DisplayText = "On Procress";
                            break;
                        //case "3":
                        //    e.DisplayText = "3";
                        //    break;
                        case "4":
                            e.DisplayText = "Completed";
                            break;
                        //case "5":
                        //    break;
                        //case "6":
                        //    break;
                        //case "7":
                        //    break;
                        //case "8":
                        //    break;
                        //case "9":
                        //    break;
                        default:
                            e.DisplayText = e.Value.ToString();
                            break;
                    }
                    break;
                default:
                    break;
            }
            //gridView.EndUpdate();
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
            if (chorderdate)
            {
                DialogResult r = XtraMessageBox.Show("คุณต้องการเปลี่ยน ETD  นี้ใช่หรือไม่?", "ข้อความแจ้งเตือน", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    string sql = $"UPDATE TXP_ISSTRANSENT " +
                        $"SET ETDDTE = TO_DATE('{DateTime.Parse(bbiEtd.EditValue.ToString()).ToString("dd/MM/yyyy")}', 'dd/MM/yyyy'),SHIPTYPE='{bbiShip.EditValue.ToString().ToUpper()}'" +
                        $"WHERE ISSUINGKEY = '{ob.RefInv}'";
                    if (new ConnDB().ExcuteSQL(sql))
                    {
                        XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว", "ข้อความแจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        bbiNewOrder.Caption = $"Save";
                        bbiNewOrder.Enabled = false;
                        chorderdate = false;
                    }
                }
            }
        }

        private void bbiPalletList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //if (ob.RefInv.Substring(0, 1) == "A")
            //{
            //    InvoiceConfirmInvForm frm = new InvoiceConfirmInvForm(ob.RefInv);
            //    frm.ShowDialog();
            //}
            //else
            //{
            //    SetPalletForm frm = new SetPalletForm(ob.RefInv);
            //    frm.ShowDialog();
            //}
            SetPalletForm frm = new SetPalletForm(ob.RefInv);
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

        private void gridView_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (shlist.Count > 0)
            {
                bbiConfirmShort.Enabled = true;
            }
        }

        private void gridView_CellValueChanging(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            switch (e.Column.FieldName.ToString())
            {
                case "ShCtn":
                    var remctn = gridView.GetFocusedRowCellValue("RemCtn").ToString();
                    Console.WriteLine(e.Value.ToString());
                    if (int.Parse(e.Value.ToString()) > int.Parse(remctn))
                    {
                        XtraMessageBox.Show("กรุณาระบุจำนวนให้น้อยกว่าหรือเท่ากับ REMAIN", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridView.SetRowCellValue(e.RowHandle, "ShCtn", 0);
                        return;
                    }
                    else if (int.Parse(e.Value.ToString()) <= 0)
                    {
                        XtraMessageBox.Show("ระบุจำนวนให้ถูกต้องด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        gridView.SetRowCellValue(e.RowHandle, "ShCtn", 0);
                        return;
                    }
                    else
                    {
                        //add to list
                        DialogResult r = XtraMessageBox.Show("ยืนยันคำสั่งตัด Short", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        if (r == DialogResult.OK)
                        {
                            List<InvoiceBodyData> o = gridControl.DataSource as List<InvoiceBodyData>;
                            o[e.RowHandle].ShCtn = int.Parse(e.Value.ToString());
                            shlist.Add(o[e.RowHandle]);
                        }
                    }
                    break;
                default:
                    break;
            }
            if (shlist.Count > 0)
            {
                bbiConfirmShort.Enabled = true;
            }
        }

        bool SaveShorting()
        {
            bool x = false;
            //bool c = false;
            //DialogResult r = XtraMessageBox.Show("คุณต้องการที่จะสร้าง Invoice ใหม่เลยหรืไม่?", "ข้อความแจ้งเตือน", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            //if (r == DialogResult.Yes)
            //{
            //    c = true;
            //}
            //else
            //{
            //    c = false;
            //}
            SplashScreenManager.ShowDefaultWaitForm();
            try
            {
                int i = 0;
                List<string> ord = new List<string>();
                while (i < shlist.Count)
                {
                    var ob = shlist[i];
                    //update issbody
                    string refinv = ob.RefInv;
                    string orderno = ob.OrderNo;
                    string partno = ob.PartNo;
                    int shctn = ob.ShCtn;
                    ord.Add(orderno);
                    //update body 
                    int tctn = ob.BalCtn - ob.ShCtn;
                    while (tctn < ob.BalCtn)
                    {
                        tctn++;
                        string updetail = $"UPDATE TXP_ISSPACKDETAIL d SET d.ISSUINGSTATUS=1,d.UPDDTE=SYSDATE WHERE d.ISSUINGKEY = '{refinv}' AND d.PONO = '{orderno}' AND d.PARTNO = '{partno}' AND d.ITEM = '{tctn}'";
                        new ConnDB().ExcuteSQL(updetail);
                    }
                    //string upbody = $"UPDATE TXP_ISSTRANSBODY b SET b.ORDERQTY={(ob.BalCtn - ob.ShCtn)}*b.STDPACK,b.SHORDERQTY={shctn}*b.STDPACK,b.UPDDTE = sysdate WHERE b.ISSUINGKEY = '{refinv}' AND b.PONO = '{orderno}' AND b.PARTNO = '{partno}'";
                    string upbody = $"UPDATE TXP_ISSTRANSBODY b SET b.SHORDERQTY={shctn}*b.STDPACK,b.UPDDTE = sysdate WHERE b.ISSUINGKEY = '{refinv}' AND b.PONO = '{orderno}' AND b.PARTNO = '{partno}'";
                    string uporder = $"UPDATE TXP_ORDERPLAN p SET p.CURINV='',p.BALQTY={shctn}*p.BISTDP,p.ORDERSTATUS=3,p.UPDDTE=SYSDATE WHERE p.CURINV = '{refinv}' AND p.ORDERID = '{orderno}' AND p.PARTNO = '{partno}'";
                    new ConnDB().ExcuteSQL(upbody);
                    new ConnDB().ExcuteSQL(uporder);
                    i++;
                }
                //if (c)
                //{
                    
                //}
                shlist.Clear();
                x = true;
            }
            catch (Exception)
            {
                x = false;
            }
            SplashScreenManager.CloseDefaultWaitForm();
            return x;
        }

        private void bbiConfirmShort_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("ยืนยันคำสั่งตัด Short", "XPW Alert!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (r == DialogResult.OK)
            {
                if (SaveShorting())
                {
                    r = XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    if (r == DialogResult.OK)
                    {
                        //save split order
                        ReloadData();
                    }
                }
            }
        }

        private void InvoiceDetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (shlist.Count > 0)
            {
                DialogResult r = XtraMessageBox.Show("คุณยังไม่ได้บันทึกข้อมูลตัด Short\nคุณต้องการที่จะบันทึกข้อมูลนี้ไหม", "XPW Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    //save short
                    SaveShorting();
                }
            }
            else if (spobj.Count > 0)
            {
                DialogResult r = XtraMessageBox.Show("คุณยังไม่ได้บันทึกข้อมูล Split Order\nคุณต้องการที่จะบันทึกข้อมูลนี้ก่อนไหม", "XPW Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    //save split order
                }
            }
        }

        private void bbiAddShorting_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = XtraInputBox.Show("ระบุจำนวนที่ต้องการตัด Short", "Comfirm Shorting", "0");
            try
            {
                var remctn = gridView.GetFocusedRowCellValue("RemCtn").ToString();
                var oldctn = gridView.GetFocusedRowCellValue("ShCtn").ToString();
                int shctn = int.Parse(result) + int.Parse(oldctn);
                if (int.Parse(result) > int.Parse(remctn))
                {
                    XtraMessageBox.Show("กรุณาระบุจำนวนให้น้อยกว่าหรือเท่ากับ REMAIN", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else if (int.Parse(result) <= 0)
                {
                    XtraMessageBox.Show("ระบุจำนวนให้ถูกต้องด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    InvoiceBodyData o = gridView.GetFocusedRow() as InvoiceBodyData;
                    o.ShCtn = shctn;
                    string upbody = $"UPDATE TXP_ISSTRANSBODY b SET b.SHORDERQTY={shctn}*b.STDPACK,b.UPDDTE = sysdate " +
                        $"WHERE b.ISSUINGKEY = '{o.RefNo}' AND b.PONO = '{o.OrderNo}' AND b.PARTNO = '{o.PartNo}'";
                    shlist.Add(o);
                    gridView.SetFocusedRowCellValue("RemCtn", (int.Parse(remctn) - int.Parse(result)));
                    gridView.RefreshData();
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("ระบุจำนวนให้ถูกต้องด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (shlist.Count > 0)
            {
                bbiConfirmShort.Enabled = true;
            }
        }

        private void bbiEditCustomer_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bbiEtd_EditValueChanged(object sender, EventArgs e)
        {
            bbiNewOrder.Caption = $"*Save";
            if (DateTime.Parse(bbiEtd.EditValue.ToString()) != ob.Etddte)
            {
                bbiNewOrder.Caption = $"*Save";
                bbiNewOrder.Enabled = true;
                chorderdate = true;
            }
            else
            {
                bbiNewOrder.Enabled = false;
                chorderdate = false;
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<InvoiceBodyData> obj =  gridControl.DataSource as List<InvoiceBodyData>;
            if (obj.Count > 0)
            {
                OrderEditCustomerForm frm = new OrderEditCustomerForm(obj);
                frm.ShowDialog();
            }
        }

        private void bbiShip_SelectedValueChanged(object sender, EventArgs e)
        {
            bbiNewOrder.Caption = $"*Save";
            if (bbiShip.EditValue.ToString() != ob.Ship)
            {
                bbiNewOrder.Caption = $"*Save";
                bbiNewOrder.Enabled = true;
                chorderdate = true;
            }
            else
            {
                bbiNewOrder.Enabled = false;
                chorderdate = false;
            }
        }
    }
}