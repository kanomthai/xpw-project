﻿using System;
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
using BookingApp;
using ShortingApp;

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
                ppMenu.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
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
            var id = gridView.GetFocusedRowCellValue("Id").ToString();
            int i = (int.Parse(id) - 1);
            Console.WriteLine(id);
            List<InvoiceData> ob = gridControl.DataSource as List<InvoiceData>;
            OrderData obj = new OrderData();
            obj.Factory = ob[i].Factory;
            obj.Zone = ob[i].Zname;
            obj.Etd = ob[i].Etddte;
            obj.Affcode = ob[i].Affcode;
            obj.Custcode = ob[i].Bishpc;
            obj.Custname = ob[i].Custname;
            obj.Ship = ob[i].Ship;
            obj.PoType = ob[i].Potype;
            obj.CustPoType = ob[i].Ord;

            //OrderPartShortingForm frm = new OrderPartShortingForm(obj);
            ShortingForm frm = new ShortingForm(obj);
            frm.ShowDialog();
        }

        private void bbiOrderControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            OrderMainForm frm = new OrderMainForm();
            frm.ShowDialog();
            bbiFactory.EditValue = StaticFunctionData.Factory;
        }

        private void bbiBookingControl_ItemClick(object sender, ItemClickEventArgs e)
        {
            BookingForm frm = new BookingForm();
            frm.ShowDialog();
        }
    }
}