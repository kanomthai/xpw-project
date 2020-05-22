using DevExpress.Accessibility;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace OrderApp
{
    public partial class OrderMainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        bool sload = false;
        public OrderMainForm()
        {
            InitializeComponent();
            this.Text = "ORDER CONTROL";
            bbiOrderId.EditValue = "";
            bbiEtd.EditValue = DateTime.Now;
            bbiFactory.EditValue = StaticFunctionData.Factory;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        void ReloadData()
        {
            SplashScreenManager.ShowDefaultWaitForm();
            if (sload)
            {
                List<OrderData> obj = new OrderControllers().GetOrderData(bbiFactory.EditValue.ToString(), DateTime.Parse(bbiEtd.EditValue.ToString()), bool.Parse(bbiOnDay.EditValue.ToString()));
                gridControl.DataSource = obj;
                bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
            }
            sload = false;
            SplashScreenManager.CloseDefaultWaitForm();
        }

        private void bbiSearch_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowDefaultWaitForm();
                if (bbiOrderId.EditValue != null)
                {
                    if (bbiOrderId.EditValue.ToString() != "")
                    {
                        List<OrderData> obj = new OrderControllers().GetOrderData(bbiOrderId.EditValue.ToString());
                        gridControl.DataSource = obj;
                        bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
                    }
                    else
                    {
                        XtraMessageBox.Show("กรุณาระบุเลขที่ออร์เดอร์ที่ต้องการค้นหาด้วย");
                    }
                }
                SplashScreenManager.CloseDefaultWaitForm();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void bbiFactory_EditValueChanged(object sender, EventArgs e)
        {
            StaticFunctionData.Factory = bbiFactory.EditValue.ToString();
            sload = true;
            ReloadData();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            sload = true;
            ReloadData();
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            gridView.BeginUpdate();
            switch (e.Column.FieldName.ToString())
            {
                case "OrderCtn":
                case "ItemCtn":
                    try
                    {
                        e.DisplayText = "";
                        if (int.Parse(e.Value.ToString()) > 0)
                        {
                            e.DisplayText = string.Format("{0:n0}", int.Parse(e.Value.ToString()));
                        }
                    }
                    catch (Exception)
                    {
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
                case "OrderRewrite":
                    e.DisplayText = "";
                    try
                    {
                        if (int.Parse(e.Value.ToString()) > 0)
                        {
                            e.DisplayText = "REWRITE";
                        }
                    }
                    catch (Exception)
                    {
                    }
                    break;
                default:
                    break;
            }
            gridView.EndUpdate();
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            //var i = gridView.GetFocusedDataRow();
            //var obj = gridView.GetDataRow(0);
            List<OrderData> obj = gridControl.DataSource as List<OrderData>;
            int i = int.Parse(gridView.GetFocusedRowCellValue("Id").ToString()) - 1;
            OrderDetailForm frm = new OrderDetailForm(obj[i]);
            frm.ShowDialog();
            int st = new GreeterFunction().GetInvoiceStatus(obj[i].RefNo);
            gridView.SetRowCellValue(i, "Status", st);
        }

        private void bbiEtd_EditValueChanged(object sender, EventArgs e)
        {
            sload = true;
        }

        private void bbiOnDay_EditValueChanged(object sender, EventArgs e)
        {
            sload = true;
        }
    }
}