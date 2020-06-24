using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using XPWLibrary.Controllers;
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
    }
}