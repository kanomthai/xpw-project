using BookingApp;
using DevExpress.Export.Xl;
using DevExpress.LookAndFeel;
using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using InvoiceApp.Properties;
using OrderApp;
using ShortingApp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;
using IronXL;
using System.Linq;
using System.Diagnostics;

namespace InvoiceApp
{
    public partial class InvoiceMainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        bool stload = true;
        bool loadinv = false;
        string fileGridInvoiceName = $"{AppDomain.CurrentDomain.BaseDirectory}Templates\\invoice_controller.xml";
        string fileGridOnWeek = $"{AppDomain.CurrentDomain.BaseDirectory}Templates\\invoice_onweek_controller.xml";
        string fileGridForword = $"{AppDomain.CurrentDomain.BaseDirectory}Templates\\invoice_nextweek_controller.xml";
        int itick = 0;
        public InvoiceMainForm()
        {
            InitializeComponent();
            bbiEtd.EditValue = DateTime.Now;
            bbiFactory.EditValue = StaticFunctionData.Factory;
            bbiStVersion.Caption = StaticFunctionData.AppVersion;
            bbiDbName.Caption = StaticFunctionData.DBname;
            bbiOrderStatus.Caption = "";
            bbiRunningReload.Caption = $"";
            bbiSendGedi.Enabled = false;
            //ReloadData();
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        void AfterFormLoad()
        {
            if (loadinv)
            {
                try
                {
                    //Thread thr0 = new Thread(ReloadGridControl);
                    Thread thr1 = new Thread(GetToWeek);
                    Thread thr2 = new Thread(GetForwardWeek);
                    Thread thorder = new Thread(GetOrderNotCreateJobList);
                    Thread thcheck = new Thread(CheckVerSion);
                    thr1.Start();
                    thr2.Start();
                    thorder.Start();
                    //thr0.Start();
                    thcheck.Start();
                    //await new GreeterFunction().CheckGitHubVersionAsync();

                    //After running
                    thr1.Abort();
                    thr2.Abort();
                    thorder.Abort();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
            }
        }

        void CheckVerSion()
        {
            //this.Invoke(new MethodInvoker(delegate {
            //    bool x = new GreeterFunction().CheckVersionAsync();
            //    if (x)
            //    {
            //        this.notifyIcon1.ShowBalloonTip(100, "Notify Message", "อัพเดทโปรแกรมด้วย", ToolTipIcon.Info);
            //        new GreeterFunction().CheckGitHubVersionAsync();
            //    }
            //}));
        }

        void ReloadGridControl()
        {
            this.Invoke(new MethodInvoker(delegate {
                List<InvoiceData> obj = new InvoiceControllers().GetInvoiceData(DateTime.Parse(bbiEtd.EditValue.ToString()));
                gridControl.DataSource = obj;
                bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
            }));
        }

        void ReloadData()
        {
            SplashScreenManager.ShowDefaultWaitForm();
            if (stload)
            {
                List<InvoiceData> obj = new InvoiceControllers().GetInvoiceData(DateTime.Parse(bbiEtd.EditValue.ToString()));
                gridControl.DataSource = obj;
                bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
            }
            AfterFormLoad();
            //GetToWeek();
            //GetForwardWeek();
            stload = false;
            SplashScreenManager.CloseDefaultWaitForm();
        }

        private void GetOrderNotCreateJobList()
        {
            DateTime d = DateTime.Parse(bbiEtd.EditValue.ToString());
            this.Invoke(new MethodInvoker(delegate {
                int x = new OrderControllers().GetOrderNotCreateJobList(d);
                bbiOrderStatus.Caption = "              ";
                if (x > 0)
                {
                    bbiOrderStatus.Caption = $"{x} PO NOT CREATE.";
                } 
            } ));
        }

        private void GetForwardWeek()
        {
            DateTime d = DateTime.Parse(bbiEtd.EditValue.ToString());
            this.Invoke(new MethodInvoker(delegate { gridForwardControl.DataSource = GetMasterWeek(7, 14); }));
            
        }

        private BindingList<InvoiceMasterData> GetMasterWeek(int b, int en)
        {
            DateTime d = DateTime.Parse(bbiEtd.EditValue.ToString());
            BindingList<InvoiceMasterData> list = new InvoiceControllers().GetInvoiceWeek(d, b, en) as BindingList<InvoiceMasterData>;
            return list;
        }

        private void GetToWeek()
        {
            DateTime d = DateTime.Parse(bbiEtd.EditValue.ToString());
            //gridWeekControl.DataSource = new InvoiceControllers().GetInvoiceToWeek(d) as BindingList<InvoiceMasterData>;
            this.Invoke(new MethodInvoker(delegate { gridWeekControl.DataSource = new InvoiceControllers().GetInvoiceToWeek(d) as BindingList<InvoiceMasterData>; }));
        }

        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                //List<InvoiceData> obj = gridControl.DataSource as List<InvoiceData>;
                //if (obj.Count < 1)
                //{
                //    bbiSendGedi.Enabled = false;
                //    ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
                //}
                bbiSendGedi.Enabled = true;
                ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else {
                ppMenu.HidePopup();
            }
        }

        private void bbiFactory_EditValueChanged(object sender, EventArgs e)
        {
            StaticFunctionData.Factory = bbiFactory.EditValue.ToString();
            stload = true;
            ReloadData();
        }

        private void bbiEtd_EditValueChanged(object sender, EventArgs e)
        {
            stload = true;
            ReloadData();
        }

        private void bbiAllWeek_CheckedChanged(object sender, ItemClickEventArgs e)
        {
            StaticFunctionData.AllWeek = bool.Parse(bbiAllWeek.Checked.ToString());
            ReloadData();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            stload = true;
            ReloadData();
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName.ToString())
            {
                case "Itm":
                case "Ctn":
                case "Issue":
                case "RmCtn":
                case "Pl":
                case "Plno":
                case "RmCon":
                case "ShCtn":
                case "Conn":
                    try
                    {
                        if (e.Value.ToString() == "0")
                        {
                            e.DisplayText = "";
                        }
                        else
                        {
                            e.DisplayText = string.Format("{0:n0}", int.Parse(e.Value.ToString()));
                        }
                    }
                    catch (Exception)
                    {
                    }
                    break;
                case "Status":
                    //new GreeterFunction().GetStatus(1, int.Parse(e.Value.ToString()));
                    if (int.Parse(e.Value.ToString()) == StaticFunctionData.StatusSendGEDI)
                    {
                        e.DisplayText = "Send GEDI";
                    }
                    else
                    {
                        switch (e.Value.ToString())
                        {
                            case "0":
                                e.DisplayText = "JobList";
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
                                e.DisplayText = "Closed";
                                break;
                            default:
                                e.DisplayText = "";
                                break;
                        }
                    }
                    break;
                case "Invoice":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkRed;
                    break;
                default:
                    break;
            }
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //int i = int.Parse(gridView.GetFocusedRowCellValue("Id").ToString()) - 1;
                //List<InvoiceData> obj = gridControl.DataSource as List<InvoiceData>;
                InvoiceData obj = gridView.GetFocusedRow() as InvoiceData;
                InvoiceDetailForm frm = new InvoiceDetailForm(obj);
                frm.ShowDialog();
                //bbiEtd.EditValue = obj.Etddte;
                gridView.UpdateCurrentRow();
                if (bbiAllWeek.Checked != true)
                {
                    Thread th = new Thread(ReloadGridControl);
                    th.Start();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "ข้อความแจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbiShowDetail_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                int i = int.Parse(gridView.GetFocusedRowCellValue("Id").ToString()) - 1;
                List<InvoiceData> obj = gridControl.DataSource as List<InvoiceData>;
                InvoiceDetailForm frm = new InvoiceDetailForm(obj[i]);
                frm.ShowDialog();
            }
            catch (Exception)
            {
            }
        }

        private void bbiPrintJobList_ItemClick(object sender, ItemClickEventArgs e)
        {
            //string refinv = gridView.GetFocusedRowCellValue("RefInv").ToString();
            //OrderJobListPreviewForm frm = new OrderJobListPreviewForm(refinv);
            //frm.ShowDialog();
        }

        private void bbiPartShorting_ItemClick(object sender, ItemClickEventArgs e)
        {
            //var id = gridView.GetFocusedRowCellValue("Id").ToString();
            //int i = (int.Parse(id) - 1);
            //Console.WriteLine(id);
            //List<InvoiceData> ob = gridControl.DataSource as List<InvoiceData>;
            InvoiceData ob = gridView.GetFocusedRow() as InvoiceData;
            OrderData obj = new OrderData();
            obj.Factory = ob.Factory;
            obj.Zone = ob.Zname;
            obj.Etd = ob.Etddte;
            obj.Affcode = ob.Affcode;
            obj.Custcode = ob.Bishpc;
            obj.Custname = ob.Custname;
            obj.Ship = ob.Ship;
            obj.PoType = ob.Potype;
            obj.CustPoType = ob.Ord;
            obj.RefInv = ob.Invoice;
            obj.RefNo = ob.RefInv;

            //OrderPartShortingForm frm = new OrderPartShortingForm(obj);
            ShortingForm frm = new ShortingForm(obj);
            frm.ShowDialog();
        }

        private void bbiOrderControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderMainForm frm = new OrderMainForm();
            frm.Show();
            bbiFactory.EditValue = StaticFunctionData.Factory;
            stload = true;
            ReloadData();
        }

        private void bbiBookingControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            BookingForm frm = new BookingForm();
            frm.ShowDialog();
        }

        private void gridWeekView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName.ToString())
            {
                case "Status":
                    switch (e.Value.ToString())
                    {
                        case "0":
                            e.DisplayText = "";
                            break;
                        case "1":
                            e.DisplayText = "JobList";
                            break;
                        case "2":
                            e.DisplayText = "Invoice";
                            break;
                        case "3":
                            e.DisplayText = "Prepare";
                            break;
                        case "4":
                            e.DisplayText = "Booking";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    if (e.Value.ToString() == "0")
                    {
                        e.DisplayText = "";
                    }
                    break;
            }
        }

        private void gridWeekView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowDefaultWaitForm();
                var etd = gridWeekView.GetFocusedRowCellValue("Etd").ToString();
                //bbiEtd.EditValue = DateTime.Parse(etd);
                List<InvoiceData> obj = new InvoiceControllers().GetInvoiceData(DateTime.Parse(etd), e.Column.FieldName.ToString().ToUpper());
                gridControl.DataSource = obj;
                bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
                try
                {
                    SplashScreenManager.CloseDefaultWaitForm();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void gridForwardView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            try
            {
                SplashScreenManager.ShowDefaultWaitForm();
                var etd = gridForwardView.GetFocusedRowCellValue("Etd").ToString();
                //bbiEtd.EditValue = DateTime.Parse(etd);
                List<InvoiceData> obj = new InvoiceControllers().GetInvoiceData(DateTime.Parse(etd), e.Column.FieldName.ToString().ToUpper());
                gridControl.DataSource = obj;
                bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
                try
                {
                    SplashScreenManager.CloseDefaultWaitForm();
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void gridView_Layout(object sender, EventArgs e)
        {
            gridView.SaveLayoutToXml(fileGridInvoiceName);
            //gridWeekView.SaveLayoutToXml(fileGridOnWeek);
            //gridForwardView.SaveLayoutToXml(fileGridForword);
        }

        private void InvoiceMainForm_Load(object sender, EventArgs e)
        {
            if (Settings.Default.ApplicationSkinName.ToString().Length > 0)
            {
                UserLookAndFeel.Default.SetSkinStyle(Settings.Default.ApplicationSkinName.ToString());
            }
            try
            {
                gridView.RestoreLayoutFromXml(fileGridInvoiceName);
                gridWeekView.RestoreLayoutFromXml(fileGridOnWeek);
                gridForwardView.RestoreLayoutFromXml(fileGridForword);
            }
            catch (Exception)
            {
                try
                {
                    gridView.RestoreLayoutFromXml($"{AppDomain.CurrentDomain.BaseDirectory}Configures\\xpw-templates\\invoice_controller.xml");
                    gridWeekView.RestoreLayoutFromXml($"{AppDomain.CurrentDomain.BaseDirectory}Configures\\xpw-templates\\invoice_onweek_controller.xml");
                    gridForwardView.RestoreLayoutFromXml($"{AppDomain.CurrentDomain.BaseDirectory}Configures\\xpw-templates\\invoice_nextweek_controller.xml");
                }
                catch (Exception ex)
                {
                    XtraMessageBox.Show(ex.Message, "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            loadinv = true;
            AfterFormLoad();
            timer1.Start();
        }

        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            
        }

        private void gridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            
            if (e.Button.ToString() == "Right")
            {
                bbiSendGedi.Enabled = false;
                var custname = gridView.GetFocusedRowCellValue("Custname").ToString();
                var rmctn = gridView.GetFocusedRowCellValue("RmCtn").ToString();
                var rmcon = gridView.GetFocusedRowCellValue("RmCon").ToString();
                var rmpl = gridView.GetFocusedRowCellValue("Plno").ToString();
                var plcount = gridView.GetFocusedRowCellValue("Pl").ToString();
                var plstatus = gridView.GetFocusedRowCellValue("Status").ToString();
                bbiSendGedi.Caption = $"Send To GEDI";
                bbiPrintShippingMark.Enabled = true;
                var o = StaticFunctionData.shiping_label.FindAll(k => k.Contains(custname));
                if (o.Count <= 0)
                {
                    bbiPrintShippingMark.Enabled = false;
                }
                else
                {
                    if (int.Parse(plcount) <= 0)
                    {
                        bbiPrintShippingMark.Enabled = false;
                    }
                    if (int.Parse(plstatus) <= 0)
                    {
                        bbiPrintShippingMark.Enabled = false;
                    }
                }
                if (int.Parse(rmctn) <= 0)
                {
                    string ikey = gridView.GetFocusedRowCellValue("Invoice").ToString();
                    bbiSendGedi.Caption = $"Send {ikey} To GEDI";
                    switch (plstatus)
                    {
                        case "4":
                        case "5":
                        case "6":
                            break;
                        default:
                            if (int.Parse(rmcon) > 0)
                            {
                                bbiSendGedi.Enabled = false;
                            }
                            else if (int.Parse(rmpl) > 0)
                            {
                                bbiSendGedi.Enabled = false;
                            }
                            else {
                                bbiSendGedi.Enabled = true;
                            }
                            break;
                    }
                }
                ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else
            {
                ppMenu.HidePopup();
            }
        }

        private void InvoiceMainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Default.ApplicationSkinName = UserLookAndFeel.Default.SkinName;
            Settings.Default.Save();
            timer1.Stop();
        }

        private void gridWeekView_Layout(object sender, EventArgs e)
        {
            gridWeekView.SaveLayoutToXml(fileGridOnWeek);
        }

        private void gridForwardView_Layout(object sender, EventArgs e)
        {
            gridForwardView.SaveLayoutToXml(fileGridForword);
        }

        private void bbiRestoreLayOut_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                new GreeterFunction().RestoreTemplate();
                gridView.RestoreLayoutFromXml($"{AppDomain.CurrentDomain.BaseDirectory}Configures\\xpw-templates\\invoice_controller.xml");
                gridWeekView.RestoreLayoutFromXml($"{AppDomain.CurrentDomain.BaseDirectory}Configures\\xpw-templates\\invoice_onweek_controller.xml");
                gridForwardView.RestoreLayoutFromXml($"{AppDomain.CurrentDomain.BaseDirectory}Configures\\xpw-templates\\invoice_nextweek_controller.xml");
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            itick++;
            this.Invoke(new MethodInvoker(delegate { bbiRunningReload.Caption = $"RUNNING AT: {itick}"; }));
            if (itick > StaticFunctionData.ReloadGrid)
            {
                AfterFormLoad();
                itick = 0;
            }
        }

        private void bbiSendGedi_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("ยืนยันคำสั่งส่งข้อมูล GEDI", "XPW Confirm!", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
            if (r == DialogResult.OK)
            {
                //update status iss
                string ikey = gridView.GetFocusedRowCellValue("RefInv").ToString();
                string sql = $"UPDATE TXP_ISSTRANSENT e SET e.ISSUINGSTATUS = {StaticFunctionData.StatusSendGEDI},e.UPDDTE =sysdate WHERE e.ISSUINGKEY = '{ikey}'";
                if (new ConnDB().ExcuteSQL(sql))
                {
                    gridView.SetFocusedRowCellValue("Status", StaticFunctionData.StatusSendGEDI);
                    XtraMessageBox.Show("บันทึกข้อมูล GEDI เสร็จแล้ว");
                }
                else
                {
                    XtraMessageBox.Show("ไม่สามารถอัพโหลดข้อมูลได้", "Error Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void bbiUpdateApp_ItemClick(object sender, ItemClickEventArgs e)
        {
            DialogResult r = XtraMessageBox.Show("ยืนยันคำสั่งอัพเดทโปรแกรม", "XPW Alert!", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            if (r == DialogResult.OK)
            {
                SplashScreenManager.ShowDefaultWaitForm();
                new GreeterFunction().CheckGitHubVersionAsync();
                SplashScreenManager.CloseDefaultWaitForm();
                XtraMessageBox.Show("อัพเดทโปรแกรมเสร็จแล้ว", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Show();
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void bbiCheckOrder_ItemClick(object sender, ItemClickEventArgs e)
        {
            //CheckOrderForm frm = new CheckOrderForm();
            //frm.Show();
        }

        private void bbiSendToDraft_ItemClick(object sender, ItemClickEventArgs e)
        {
            new GreeterFunction().ErrorHadler("ขอ อภัย!\nขณะนี้กำลังพัฒนาระบบ");
        }

        private void bbiPrintShippingMark_ItemClick(object sender, ItemClickEventArgs e)
        {
            string ikey = gridView.GetFocusedRowCellValue("RefInv").ToString();
            //InvoiceAddShipMarkForm frm = new InvoiceAddShipMarkForm(ikey);
            //frm.ShowDialog();

            InvoiceShippingMarkPreviewForm frm = new InvoiceShippingMarkPreviewForm(ikey, null);
            frm.ShowDialog();
        }

        private List<string> GetPalletList(string invno)
        {
            string sql = $"SELECT * FROM TBT_SUMMARYPALLETLIST WHERE ISSUINGKEY ='{invno}'";
            List<string> obj = new List<string>();
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(r["plsize"].ToString());
            }
            return obj;
        }

        private void bbiExportSummaryPallet_ItemClick(object sender, ItemClickEventArgs e)
        {
            DateTime d = DateTime.Parse(bbiEtd.EditValue.ToString());
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "Excel|*.xlsx";
            saveFileDialog1.Title = "Save an Summary Pallet File";
            saveFileDialog1.ShowDialog();
            //saveFileDialog1.FileName = $"{StaticFunctionData.Factory}-{d.ToString("ddMMyyyy")}";
            if (saveFileDialog1.FileName != "")
            {
                SplashScreenManager.ShowDefaultWaitForm();
                string sql = $"SELECT * FROM TBT_GETSUMMARYPALLET t WHERE t.ETDDTE = TO_DATE('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') ";
                List<SummaryReportDataDetail> list = new List<SummaryReportDataDetail>();
                DataSet dr = new ConnDB().GetFill(sql);
                string txtpath = $"{AppDomain.CurrentDomain.BaseDirectory}Documents\\summary_pallet_delivery.xlsx";
                Microsoft.Office.Interop.Excel.Application oXL = null;
                Microsoft.Office.Interop.Excel._Workbook oWB = null;
                Microsoft.Office.Interop.Excel._Worksheet oSheet = null;

                try
                {
                    oXL = new Microsoft.Office.Interop.Excel.Application();
                    oWB = oXL.Workbooks.Open(txtpath);
                    oSheet = oWB.ActiveSheet;
                    int i = 6;
                    string refinv = null;
                    string custn = null;
                    string inv = null;
                    string dte = null;

                    foreach (DataRow r in dr.Tables[0].Rows)
                    {
                        SplashScreenManager.Default.SetWaitFormCaption("APPEND DATA");
                        SplashScreenManager.Default.SetWaitFormDescription($"{r["custname"].ToString()}");
                        d = DateTime.Parse(r["etddte"].ToString());
                        if (refinv is null)
                        {
                            custn = r["custname"].ToString();
                            refinv = r["invoice"].ToString();
                            inv = r["invoice"].ToString();
                            dte = d.ToString("dd/MM/yyyy");
                        }
                        else
                        {
                            if (refinv != r["invoice"].ToString())
                            {
                                custn = r["custname"].ToString();
                                refinv = r["invoice"].ToString();
                                inv = r["invoice"].ToString();
                                dte = d.ToString("dd/MM/yyyy");
                            }
                            else
                            {
                                custn = "";
                                inv = "";
                                dte = "";
                            }
                        }
                        oSheet.Cells[i, 1] = custn;
                        oSheet.Cells[i, 2] = inv;
                        oSheet.Cells[i, 3] = dte;
                        oSheet.Cells[i, 4] = r["plsize"].ToString();
                        oSheet.Cells[i, 9] = r["zname"].ToString();

                        string txt = $"{int.Parse(r["plmin"].ToString())}";
                        if (int.Parse(r["plmin"].ToString()) == int.Parse(r["plmax"].ToString()))
                        {
                            txt = $"{int.Parse(r["plmin"].ToString())}";
                        }
                        else
                        {
                            txt = $"{int.Parse(r["plmin"].ToString())}-{int.Parse(r["plmax"].ToString())}";
                        };
                        oSheet.Cells[i, 5] = txt;
                        i++;
                    }
                    oWB.SaveAs(saveFileDialog1.FileName.ToString());
                }
                catch (Exception ex)
                {
                    SplashScreenManager.CloseDefaultWaitForm();
                    MessageBox.Show(ex.ToString());
                    oWB.Close();
                }
                finally
                {
                    SplashScreenManager.CloseDefaultWaitForm();
                    if (oWB != null)
                        oWB.Close();
                }
                Process.Start(saveFileDialog1.FileName.ToString());
            }
        }

        void WritingShort(int type)
        {
            List<PartShortData> list = new InvoiceControllers().GetShortData(7, type);
            //List<SummaryData> obj = new InvoiceControllers().GetDataSource();
            if (list.Count <= 0)
            {
                XtraMessageBox.Show("ไม่พบข้อมูล", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbiShortingByPart_ItemClick(object sender, ItemClickEventArgs e)
        {
            WritingShort(1);
        }

        private void bbiShortingByCustomer_ItemClick(object sender, ItemClickEventArgs e)
        {
            WritingShort(2);
        }
    }
}