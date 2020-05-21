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
using XPWLibrary.Interfaces;
using XPWLibrary.Models;
using XPWLibrary.Controllers;
using DevExpress.XtraSplashScreen;
using OrderApp;

namespace InvoiceApp
{
    public partial class InvoiceMainForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        bool stload = true;
        public InvoiceMainForm()
        {
            InitializeComponent();
            bbiEtd.EditValue = DateTime.Now;
            bbiFactory.EditValue = StaticFunctionData.Factory;
            bbiStVersion.Caption = StaticFunctionData.AppVersion;
            bbiDbName.Caption = StaticFunctionData.DBname;
            //ReloadData();
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
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
            stload = false;
            SplashScreenManager.CloseDefaultWaitForm();
        }

        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                if (gridView.FocusedRowHandle >= 0)
                {
                    ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
                }
            }
            else
            {
                ppMenu.HidePopup();
            }
        }

        private void bbiFactory_EditValueChanged(object sender, EventArgs e)
        {
            StaticFunctionData.Factory = bbiFactory.EditValue.ToString();
            ReloadData();
        }

        private void bbiEtd_EditValueChanged(object sender, EventArgs e)
        {
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
            gridView.BeginUpdate();
            switch (e.Column.FieldName.ToString())
            {
                case "Itm":
                case "Ctn":
                case "Issue":
                case "RmCtn":
                case "Pl":
                case "Plno":
                case "RmCon":
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
                case "Invoice":
                    e.Column.AppearanceCell.ForeColor = Color.DarkRed;
                    break;
                default:
                    break;
            }
            gridView.EndUpdate();
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
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

        }

        private void bbiOrderControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderMainForm frm = new OrderMainForm();
            frm.ShowDialog();
            bbiFactory.EditValue = StaticFunctionData.Factory;
        }
    }
}