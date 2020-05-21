using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace OrderApp
{
    public partial class OrderDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        OrderData ord;
        string refinv;
        public OrderDetailForm(OrderData obj)
        {
            InitializeComponent();
            this.Text = "ORDER DETAIL";
            ord = obj;
            refinv = ord.RefNo;
            bool x = new OrderControllers().GetOrderBodyRefinvoice(obj);
            if (x)
            {
                DialogResult r = XtraMessageBox.Show("พบรายการออร์เดอร์ที่ไม่ตรงกัน\nคุณต้องการที่จะสร้าง JobList ก่อนใหม่", "XPW Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                if (r == DialogResult.Yes)
                {
                    if (CreateJobList())
                    {
                        OrderJobListPreviewForm frm = new OrderJobListPreviewForm(refinv);
                        frm.ShowDialog();
                    }
                }
            }
            List<OrderBody> ob = new OrderControllers().GetOrderDetail(obj);
            gridControl.DataSource = ob;
            bsiRecordsCount.Caption = "RECORDS : " + ob.Count;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            gridView.BeginUpdate();
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
                        if (StaticFunctionData.Factory == "INJ")
                        {
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
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            switch (e.Value.ToString())
                            {
                                case "0":
                                    //e.Column.AppearanceCell.ForeColor = Color.DarkOrange;
                                    e.DisplayText = "ADD/REM.";
                                    break;
                                case "1":
                                    //e.Column.AppearanceCell.ForeColor = Color.DarkGreen;
                                    e.DisplayText = "UPDATE";
                                    break;
                                case "2":
                                    //e.Column.AppearanceCell.ForeColor = Color.DarkBlue;
                                    e.DisplayText = "CHANGE";
                                    break;
                                case "3":
                                    //e.Column.AppearanceCell.ForeColor = Color.DarkOrange;
                                    e.DisplayText = "REPLACE";
                                    break;
                                case "4":
                                    //e.Column.AppearanceCell.ForeColor = Color.DarkRed;
                                    e.DisplayText = "DRAFF";
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
            gridView.EndUpdate();
        }

        private void gridView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            try
            {
                switch (e.Button.ToString())
                {
                    case "Right":
                        List<OrderBody> ob = gridControl.DataSource as List<OrderBody>;
                        bbiPrintJobList.Enabled = false;
                        if (ob[0].Status > 0)
                        {
                            bbiCreateJobList.Caption = "Re-Create JobList";
                            bbiPrintJobList.Enabled = true;
                        }
                        popupMenu1.ShowPopup(new System.Drawing.Point(MousePosition.X, MousePosition.Y));
                        break;
                    default:
                        popupMenu1.HidePopup();
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        bool CreateJobList()
        {
            SplashScreenManager.ShowDefaultWaitForm();
            refinv = new OrderControllers().CreatedJobList(ord);
            SplashScreenManager.CloseDefaultWaitForm();
            return true;
        }

        private void bbiCreateJobList_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (CreateJobList())
            {
                OrderJobListPreviewForm frm = new OrderJobListPreviewForm(refinv);
                frm.ShowDialog();
            }
        }

        private void bbiPrintJobList_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderJobListPreviewForm frm = new OrderJobListPreviewForm(refinv);
            frm.ShowDialog();
        }

    }
}