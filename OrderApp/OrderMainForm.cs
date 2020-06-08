using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace OrderApp
{
    public partial class OrderMainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        bool sload = false;
        int irunning = 0;
        public OrderMainForm()
        {
            InitializeComponent();
            bbiFooterRunning.Caption = "";
            this.Text = "ORDER CONTROL";
            bbiOrderId.EditValue = "";
            bbiEtd.EditValue = DateTime.Now;
            bbiFactory.EditValue = StaticFunctionData.Factory;
            string fileGridInvoiceName = $"{AppDomain.CurrentDomain.BaseDirectory}Templates\\CurrentOrderEnt.xml";
            gridView.RestoreLayoutFromXml(fileGridInvoiceName);
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
            sload = true;
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
            //gridView.BeginUpdate();
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
                        case "10":
                            e.DisplayText = "Cancel";
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
            //gridView.EndUpdate();
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            //var i = gridView.GetFocusedDataRow();
            //var obj = gridView.GetDataRow(0);
            List<OrderData> obj = gridControl.DataSource as List<OrderData>;
            int i = int.Parse(gridView.GetFocusedRowCellValue("Id").ToString()) - 1;
            OrderDetailForm frm = new OrderDetailForm(obj[i]);
            frm.ShowDialog();
            gridView.BeginUpdate();
            int st = new GreeterFunction().GetInvoiceStatus(obj[i].RefNo);
            gridView.SetRowCellValue(i, "Status", st);
            gridView.EndUpdate();
        }

        private void bbiEtd_EditValueChanged(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void bbiOnDay_EditValueChanged(object sender, EventArgs e)
        {
            ReloadData();
        }

        private void gridView_Layout(object sender, EventArgs e)
        {
            string fileGridInvoiceName = $"{AppDomain.CurrentDomain.BaseDirectory}Templates\\CurrentOrderEnt.xml";
            gridView.SaveLayoutToXml(fileGridInvoiceName);
        }

        void ReloadOrder()
        {
            this.Invoke(new MethodInvoker(delegate {
                List<OrderData> obj = new OrderControllers().GetOrderData(bbiFactory.EditValue.ToString(), DateTime.Parse(bbiEtd.EditValue.ToString()), bool.Parse(bbiOnDay.EditValue.ToString()));
                gridControl.DataSource = obj;
                bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
            }));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            irunning++;
            this.Invoke(new MethodInvoker(delegate { bbiFooterRunning.Caption = $"RELOAD AFTER: {irunning}"; }));
            if (irunning > StaticFunctionData.ReloadGrid)
            {
                //Thread th0 = new Thread(ReloadOrder);
                //th0.Start();
                irunning = 0;
            }
        }

        private void OrderMainForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void OrderMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = XtraInputBox.Show("ระบุเลขที่ออร์เดอร์ที่ต้องการค้นหา", "ค้นหาข้อมูล", null);
            Console.WriteLine(result);
            try
            {
                SplashScreenManager.ShowDefaultWaitForm();
                if (result != null)
                {
                    if (result.ToString() != "")
                    {
                        List<OrderData> obj = new OrderControllers().GetOrderData(result.ToString());
                        if (obj.Count > 0)
                        {
                            gridControl.DataSource = obj;
                            bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
                        }
                        else
                        {
                            new GreeterFunction().CreateLogSearch(result.ToString());
                            XtraMessageBox.Show("ไม่พบข้อมูลที่ต้องการค้นหา");
                        }
                    }
                }
                SplashScreenManager.CloseDefaultWaitForm();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = XtraInputBox.Show("ระบุเลขที่ LOTNO ที่ต้องการค้นหา", "ค้นหาข้อมูล", null);
            Console.WriteLine(result);
            try
            {
                SplashScreenManager.ShowDefaultWaitForm();
                if (result != null)
                {
                    if (result.ToString() != "")
                    {
                        List<OrderData> obj = new OrderControllers().GetLotNoData(result.ToString());
                        if (obj.Count > 0)
                        {
                            gridControl.DataSource = obj;
                            bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
                        }
                        else
                        {
                            new GreeterFunction().CreateLogSearch(result.ToString());
                            XtraMessageBox.Show("ไม่พบข้อมูลที่ต้องการค้นหา");
                        }
                    }
                }
                SplashScreenManager.CloseDefaultWaitForm();
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void gridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else
            {
                ppMenu.HidePopup();
            }
        }

        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                List<OrderData> db = gridControl.DataSource as List<OrderData>;
                if (db.Count < 1)
                {
                    ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
                }
            }
        }
    }
}