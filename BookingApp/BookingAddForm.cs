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
using XPWLibrary.Controllers;
using XPWLibrary.Models;

namespace BookingApp
{
    public partial class BookingAddForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        List<BookingInvoicePallet> n = new List<BookingInvoicePallet>();
        public BookingAddForm(DateTime etd, string conno)
        {
            InitializeComponent();
            bbiReleaseDate.EditValue = DateTime.Now;
            bbiTimer.EditValue = DateTime.Now;
            bbiEtd.EditValue = etd;
            bbi20Ft.Checked = true;
            this.Text = $"{conno} DETAIL";
            if (conno is null)
            {
                this.Text = $"ADD NEW BOOKING";
                bbiContainer.Focus();
                gridContainerControl.DataSource = null;
            }
            //gridControl.DataSource = GetDataSource();
            //BindingList<Customer> dataSource = GetDataSource();
            //gridControl.DataSource = dataSource;
            //bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            //gridControl.ShowRibbonPrintPreview();
        }

        private void bbiEtd_EditValueChanged(object sender, EventArgs e)
        {
            bbiCustomer.Properties.Items.Clear();
            bbiCustomer.EditValue = "";
            DateTime d = DateTime.Parse(bbiEtd.EditValue.ToString());
            List <BookingCustomerData>  obj = new BookingControllers().GetCustomer(d);
            obj.ForEach(i => {
                bbiCustomer.Properties.Items.Add(i.custname);
            });
        }

        void CheckCustomerInvoice()
        {
            try
            {
                gridControlPallet.DataSource = null;
                var custname = bbiCustomer.EditValue.ToString();
                DateTime d = DateTime.Parse(bbiEtd.EditValue.ToString());
                List<BookingPlData> obj = new BookingControllers().GetPalletInvoice(d, custname);
                gridControlInvoice.DataSource = obj;
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message, "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void bbiCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckCustomerInvoice();
        }

        private void btnCheckInvoice_Click(object sender, EventArgs e)
        {
            CheckCustomerInvoice();
        }

        private void gridViewInvoice_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "poctn":
                case "plctn":
                case "booked":
                case "ubook":
                    e.DisplayText = "";
                    if (int.Parse(e.Value.ToString()) > 0)
                    {
                        e.DisplayText = string.Format("{0:n0}", int.Parse(e.Value.ToString()));
                    }
                    break;
                default:
                    break;
            }
        }

        private void gridViewInvoice_Click(object sender, EventArgs e)
        {
            var issuekey = gridViewInvoice.GetFocusedRowCellValue("issuekey").ToString();
            List<BookingInvoicePallet> obj = new BookingControllers().GetInvoicePl(issuekey);
            gridControlPallet.DataSource = obj;
        }

        private void gridViewPallet_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                switch (e.Column.FieldName)
                {
                    case "plstatus":
                        switch (e.Value.ToString())
                        {
                            case "0":
                                e.DisplayText = "NONE";
                                break;
                            case "1":
                                e.DisplayText = "Wait Print";
                                break;
                            case "2":
                                e.DisplayText = "Wait Loading";
                                break;
                            case "3":
                                e.DisplayText = "Cancel";
                                break;
                            case "4":
                                e.DisplayText = "Loaded";
                                break;
                            //    break;
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
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var conno = bbiContainer.EditValue.ToString().ToUpper();
                var sealno = bbiSealNo.EditValue.ToString().ToUpper();
                var conrelease = DateTime.Parse(bbiReleaseDate.EditValue.ToString()).ToString("dd/MM/yyyy") + " " + DateTime.Parse(bbiTimer.EditValue.ToString()).ToString("HH:mm:ss");
                var consize = "40F";
                if (bbi20Ft.Checked)
                {
                    consize = "20F";
                }

                List<BookingInvoicePallet> obj = gridControlPallet.DataSource as List<BookingInvoicePallet>;
                List<BookingInvoicePallet> ob = new List<BookingInvoicePallet>();
                foreach (BookingInvoicePallet r in obj)
                {
                    if (r.slpl)
                    {
                        BookingInvoicePallet b = new BookingInvoicePallet();
                        b.id = n.Count;
                        b.refinv = r.refinv;
                        b.issuekey = r.issuekey;
                        b.plno = r.plno;
                        b.pltype = r.pltype;
                        b.pltotal = r.pltotal;
                        b.ploutno = r.ploutno;
                        b.plstatus = r.plstatus;
                        b.conno = conno;
                        b.sealno = sealno;
                        b.consize = consize;
                        b.condate = DateTime.Parse(conrelease);
                        b.slpl = false;
                        var x = n.FindAll(j => j.ploutno == r.ploutno);
                        if (x.Count <= 0)
                        {
                            n.Add(b);
                        }
                    }
                    else
                    {
                        ob.Add(r);
                    }
                }
                gridContainerControl.BeginUpdate();
                gridContainerControl.DataSource = n;
                gridContainerControl.EndUpdate();

                gridControlPallet.BeginUpdate();
                gridControlPallet.DataSource = ob;
                gridControlPallet.EndUpdate();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                XtraMessageBox.Show("กรุณาตรวจสอบความถูกต้องของข้อมูลด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bbiContainer.Focus();
            }
        }

        private void btnRemove_Click(object sender, EventArgs e)
        {

        }
    }
}