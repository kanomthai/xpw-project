using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace OrderApp
{
    public partial class OrderDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        OrderData ord;
        int ir = 0;
        public OrderDetailForm(OrderData obj)
        {
            InitializeComponent();
            this.Text = "ORDER DETAIL";
            ord = obj;
            bool x = new OrderControllers().GetOrderBodyRefinvoice(obj);
            if (x)
            {
                DialogResult r = XtraMessageBox.Show("พบรายการออร์เดอร์ที่ไม่ตรงกัน\nคุณต้องการที่จะสร้าง JobList ก่อนใหม่", "XPW Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (r == DialogResult.Yes)
                {
                    if (CreateJobList())
                    {
                        OrderJobListPreviewForm frm = new OrderJobListPreviewForm(ord);
                        frm.ShowDialog();
                    }
                }
            }
            ReloadData();
        }

        void ReloadData()
        {
            bbiEtd.EditValue = ord.Etd;
            bbiShip.EditValue = ord.Ship;
            bbiZone.EditValue = ord.Zone;
            bbiAffcode.EditValue = ord.Affcode;
            bbiCustCode.EditValue = ord.Custcode;
            bbiCustName.EditValue = ord.Custname;
            bbiOrderBy.EditValue = ord.PoType;
            bbiRefInv.EditValue = ord.RefNo;
            List<OrderBody> ob = new OrderControllers().GetOrderDetail(ord);
            bbiInvoice.EditValue = "";
            if (ob.Count > 0)
            {
                bbiInvoice.EditValue = ob[0].RefInv;
            }
            gridControl.DataSource = ob;
            bsiRecordsCount.Caption = "RECORDS : " + ob.Count;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            //gridView.BeginUpdate();
            try
            {
                switch (e.Column.FieldName.ToString())
                {
                    case "BalQty":
                        e.DisplayText = "";
                        if (int.Parse(e.Value.ToString()) > 0)
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
                    case "ReasonCD":
                        e.DisplayText = "";
                        switch (e.Value.ToString())
                        {
                            case "P":
                                //e.Column.AppearanceCell.ForeColor = Color.DarkOrange;
                                e.DisplayText = "ADD/REM.";
                                break;
                            case "M":
                                //e.Column.AppearanceCell.ForeColor = Color.DarkBlue;
                                e.DisplayText = "SHIP.";
                                break;
                            case "D":
                                //e.Column.AppearanceCell.ForeColor = Color.DarkCyan;
                                e.DisplayText = "ETD.";
                                break;
                            case "Q":
                                //e.Column.AppearanceCell.ForeColor = Color.DarkRed;
                                e.DisplayText = "QTY.";
                                break;
                            case "L":
                                //e.Column.AppearanceCell.ForeColor = Color.DarkKhaki;
                                e.DisplayText = "LOCAT.";
                                break;
                            //AW
                            case "0":
                                e.DisplayText = "UPDATE";
                                break;
                            case "1":
                                e.DisplayText = "CANCEL";
                                break;
                            case "2":
                                e.DisplayText = "CHANGE";
                                break;
                            case "3":
                                e.DisplayText = "REP.";
                                break;
                            default:
                                break;
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            //gridView.EndUpdate();
        }

        private void gridView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                List<OrderBody> ob = gridControl.DataSource as List<OrderBody>;
                if (ob.Count < 0)
                {
                    switch (e.Button.ToString())
                    {
                        case "Right":
                            bbiPrintJobList.Enabled = false;
                            bbiPartDetail.Enabled = false;
                            bbiConfirmInvoice.Enabled = false;
                            bbiSetMultiLot.Enabled = false;
                            //if (ob[0].Status > 0)
                            //{
                            //    bbiCreateJobList.Caption = "Re-Create JobList";
                            //    bbiPrintJobList.Enabled = true;
                            //    bbiConfirmInvoice.Enabled = true;
                            //    bbiSetMultiLot.Enabled = true;
                            //    if (ob[0].Factory == "INJ")
                            //    {
                            //        bbiSetMultiLot.Enabled = false;
                            //    }
                            //}
                            //bbiShipingLabel.Enabled = false;
                            //if (ob[0].Status > 1)
                            //{
                            //    bbiPartDetail.Enabled = true;
                            //    bbiShipingLabel.Enabled = true;
                            //}

                            popupMenu1.ShowPopup(new System.Drawing.Point(MousePosition.X, MousePosition.Y));
                            break;
                        default:
                            popupMenu1.HidePopup();
                            break;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        bool CreateJobList()
        {
            SplashScreenManager.ShowDefaultWaitForm();
            string refinv = new OrderControllers().CreatedJobList(ord);
            ord.RefNo = refinv;
            ord.RefInv = refinv;
            bbiRefInv.EditValue = refinv;
            bbiInvoice.EditValue = refinv;
            SplashScreenManager.CloseDefaultWaitForm();
            return true;
        }

        private void bbiCreateJobList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (CreateJobList())
            {
                OrderJobListPreviewForm frm = new OrderJobListPreviewForm(ord);
                frm.ShowDialog();
                ReloadData();
            }
        }

        private void bbiPrintJobList_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderJobListPreviewForm frm = new OrderJobListPreviewForm(ord);
            frm.ShowDialog();
            ReloadData();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            ReloadData();
        }

        private void bbiConfirmInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderPalletDetailForm frm = new OrderPalletDetailForm(ord);
            frm.ShowDialog();
            //after confirm order
            ReloadData();
        }

        private void bbiPartDetail_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bbiShipingLabel_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowDefaultWaitForm();
            List<OrderBody> ob = gridControl.DataSource as List<OrderBody>;
            string sql = $"SELECT * FROM TXP_ISSPACKDETAIL d WHERE d.ISSUINGKEY = '{ob[0].RefNo}' AND d.SHIPPLNO  IS NOT NULL ORDER BY d.SHIPPLNO,d.FTICKETNO";
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 0)
            {
                int stctn = 0;
                foreach (DataRow r in dr.Tables[0].Rows)
                {
                    stctn += 1;
                    new InvoiceControllers().PrintFTicket(ob[0].RefNo, r["fticketno"].ToString(), stctn.ToString());
                }
            }
            //int stctn = 0;
            //int i = 0;
            //while (i < ob.Count)
            //{
            //    OrderBody r = ob[i];
            //    if (r.Ctn > 0)
            //    {
            //        stctn += r.Ctn;
            //        int startlb = stctn - (r.Ctn - 1);
            //        Console.WriteLine($"{i}. {r.RefNo} PARTNO: {r.PartNo} CTN: {r.Ctn} SEQ START: {startlb} SEQ END: {stctn}");
            //        new InvoiceControllers().PrintFTicket(r.RefNo, r.PartNo, r.OrderNo, (startlb - 1), null);
            //    }
            //    i++;
            //}
            //Console.WriteLine($"TOTAL: {i}");
            SplashScreenManager.CloseDefaultWaitForm();
        }

        private void bbiSetMultiLot_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult r;
            if (StaticFunctionData.confirmlotno)
            {
                r = XtraMessageBox.Show("ยืนยันการจ่ายด้วย LOTNO. อื่น", "ยืนยันคำสั่ง", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (r == DialogResult.OK)
                {
                    Console.WriteLine($"UPDATE TXP_ISSTRANSBODY SET LOTSTKFLAG='Y',UPDDTE=sysdate WHERE ISSUINGKEY = '{ord.RefNo}'");
                    bool x = new ConnDB().ExcuteSQL($"UPDATE TXP_ISSTRANSBODY SET LOTSTKFLAG='Y',UPDDTE=sysdate WHERE ISSUINGKEY = '{ord.RefNo}'");
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

        private void gridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            switch (e.Button.ToString())
            {
                case "Right":
                    List<OrderBody> ob = gridControl.DataSource as List<OrderBody>;
                    bbiPrintJobList.Enabled = false;
                    bbiPartDetail.Enabled = false;
                    bbiConfirmInvoice.Enabled = false;
                    bbiSetMultiLot.Enabled = false;
                    bbiCreateJobList.Enabled = true;
                    if (ob[0].Status > 0)
                    {
                        //if (ob[0].Status >= 2)
                        //{
                        //    bbiCreateJobList.Enabled = false;
                        //}
                        bbiCreateJobList.Caption = "Re-Create JobList";
                        bbiPrintJobList.Enabled = true;
                        bbiConfirmInvoice.Enabled = true;
                        bbiSetMultiLot.Enabled = true;
                        if (ob[0].Factory == "INJ")
                        {
                            bbiSetMultiLot.Enabled = false;
                        }
                    }
                    bbiShipingLabel.Enabled = false;
                    if (ob[0].Status > 1)
                    {
                        bbiPartDetail.Enabled = true;
                        bbiShipingLabel.Enabled = true;
                    }

                    popupMenu1.ShowPopup(new System.Drawing.Point(MousePosition.X, MousePosition.Y));
                    break;
                default:
                    popupMenu1.HidePopup();
                    break;
            }
        }

        private void OrderDetailForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void OrderDetailForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        void ReloadForm()
        {
            this.Invoke(new MethodInvoker(delegate {
                bbiEtd.EditValue = ord.Etd;
                bbiShip.EditValue = ord.Ship;
                bbiZone.EditValue = ord.Zone;
                bbiAffcode.EditValue = ord.Affcode;
                bbiCustCode.EditValue = ord.Custcode;
                bbiCustName.EditValue = ord.Custname;
                bbiOrderBy.EditValue = ord.PoType;
                bbiRefInv.EditValue = ord.RefNo;
                List<OrderBody> ob = new OrderControllers().GetOrderDetail(ord);
                bbiInvoice.EditValue = ob[0].RefInv;
                gridControl.DataSource = ob;
                bsiRecordsCount.Caption = "RECORDS : " + ob.Count;
            }));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            ir++;
            if (ir > StaticFunctionData.ReloadGrid)
            {
                //Thread th = new Thread(ReloadForm);
                //th.Start();
                ir = 0;
            }
        }
    }
}