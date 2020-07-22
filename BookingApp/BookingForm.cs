using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace BookingApp
{
    public partial class BookingForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public BookingForm()
        {
            InitializeComponent();
            bbiEtdDate.EditValue = DateTime.Now;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            BookingAddForm frm = new BookingAddForm(null);
            frm.ShowDialog();
        }

        void Reload()
        {
            DateTime d = DateTime.Parse(bbiEtdDate.EditValue.ToString());
            List<Bookings> obj = new BookingControllers().GetContainerList(d);
            gridControl.DataSource = obj;
            bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
        }

        private void bbiEtdDate_EditValueChanged(object sender, EventArgs e)
        {
            Reload();
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            Bookings obj = gridView.GetFocusedRow() as Bookings;
            BookingAddForm frm = new BookingAddForm(obj);
            frm.ShowDialog();
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                switch (e.Column.FieldName.ToString())
                {
                    case "LoadStatus":
                    case "CloseStatus":
                    case "GrossWeight":
                    case "NetWeight":
                    case "Pallet":
                        if (e.Value.ToString() == "0")
                        {
                            e.DisplayText = "";
                        }
                        break;
                    default:
                        break;
                }
            }
            catch (Exception)
            {
            }
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            Bookings obj = gridView.GetFocusedRow() as Bookings;
            DialogResult r = XtraMessageBox.Show($"ยืนยันคำสั่งลบข้อมูล {obj.ContainerNo}", "ข้อความแจ้งเตือน", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
            if (r == DialogResult.OK)
            {
                string sql = $"DELETE TXP_LOADCONTAINER  WHERE CONTAINERNO  = '{obj.ContainerNo}'";
                string sqlload = $"DELETE FROM TXP_LOADINVOICE WHERE CONTAINERNO = '{obj.ContainerNo}'";
                string sqllet = $"UPDATE TXP_ISSPALLET SET CONTAINERNO = '',PLOUTSTS=0 WHERE CONTAINERNO = '{obj.ContainerNo}'";
                new ConnDB().ExcuteSQL(sql);
                new ConnDB().ExcuteSQL(sqlload);
                new ConnDB().ExcuteSQL(sqllet);
                XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว", "ข้อความแจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Reload();
            }
        }

        private void gridView_MouseUp(object sender, MouseEventArgs e)
        {
            List<Bookings> list = gridControl.DataSource as List<Bookings>;
            if (list.Count > 0)
            {
                if (e.Button.ToString() == "Right")
                {
                    Bookings obj = gridView.GetFocusedRow() as Bookings;
                    bbiDelete.Caption = $"Delete {obj.ContainerNo}";
                    popupMenu1.ShowPopup(new System.Drawing.Point(MousePosition.X, MousePosition.Y));
                }
                else
                {
                    popupMenu1.HidePopup();
                }
            }
        }
    }
}