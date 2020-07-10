using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace BookingApp
{
    public partial class BookingAddForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string containerno;
        List<BookingInvoicePallet> cpl = new List<BookingInvoicePallet>();
        public BookingAddForm(Bookings b)
        {
            InitializeComponent();
            if (b is null)
            {
                this.Text = "ADD NEW BOOKING";
                bbiEtd.EditValue = DateTime.Now;
                Reload();
            }
            else
            {
                containerno = b.ContainerNo;
                this.Text = $"{containerno} DETAIL";
                ReloadContainerDetail(b);
            }
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            //gridControl.ShowRibbonPrintPreview();
            try
            {
                if (bbiContainer.EditValue.ToString() != "")
                {
                    BookingPreviewForm frm = new BookingPreviewForm(bbiContainer.EditValue.ToString());
                    frm.ShowDialog();
                }
            }
            catch (Exception)
            {
            }
        }

        void ReloadContainerDetail(Bookings b)
        {
            bbiContainer.EditValue = b.ContainerNo;
            bbiSealNo.EditValue = b.SealNo;
            bbiCustomer.EditValue = b.Custname;
            bbiEtd.EditValue = b.Etd;
            bbiRelDate.EditValue = b.ReleaseDate;
            bbiRelTimer.EditValue = b.ReleaseDate;
            if (b.ContainerSize == "20F")
            {
                bbi4oFt.Checked = false;
                bbi20Ft.Checked = true;
            }
            else
            {
                bbi4oFt.Checked = true;
                bbi20Ft.Checked = false;
            }
            try
            {
                ReloadContainer();
            }
            catch (Exception)
            {
            }
        }

        void Reload()
        {
            bbiRelDate.EditValue = DateTime.Now;
            bbiRelTimer.EditValue = DateTime.Now;
            bbiContainer.EditValue = "";
            bbiSealNo.EditValue = "";
            bbi20Ft.Checked = true;
        }

        void GetCustomer()
        {
            gridInvControl.DataSource = null;
            bbiCustomer.Properties.Items.Clear();
            DateTime d = DateTime.Parse(bbiEtd.EditValue.ToString());
            List<BookingCustomerData> obj = new BookingControllers().GetCustomer(d);
            obj.ForEach(i => {
                bbiCustomer.Properties.Items.Add(i.custname);
            });
        }

        private void bbiEtd_EditValueChanged(object sender, EventArgs e)
        {
            //get customer
            gridInvControl.DataSource = null;
            bbiCustomer.EditValue = "";
            GetCustomer();
        }

        void GetInvoiceList()
        {
            if (bbiCustomer.EditValue.ToString() != "")
            {
                DateTime etd = DateTime.Parse(bbiEtd.EditValue.ToString());
                List<BookingPlData> obj = new BookingControllers().GetPalletInvoice(etd, bbiCustomer.EditValue.ToString());
                gridInvControl.DataSource = obj;
            }
        }

        private void bbiCustomer_SelectedValueChanged(object sender, EventArgs e)
        {
            GetInvoiceList();
        }

        private void btnCheckInvoice_Click(object sender, EventArgs e)
        {
            GetInvoiceList();
        }

        private void gridInvView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            try
            {
                switch (e.Column.FieldName.ToString())
                {
                    case "poctn":
                    case "ubook":
                    case "booked":
                        e.DisplayText = "";
                        if (e.Value.ToString() != "0")
                        {
                            e.DisplayText = string.Format("{0:n0}", int.Parse(e.Value.ToString()));
                        }
                        break;
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
                            //case "5":
                            //    e.DisplayText = "Closed";
                            //    break;
                            //case "6":
                            //    e.DisplayText = "Completed";
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

        void GetPalletList(string issuekey)
        {
            gridPalletControl.DataSource = null;
            gridSlPlControl.DataSource = null;
            gridPalletControl.DataSource = new BookingControllers().GetInvoicePl(issuekey);
            gridSlPlControl.DataSource= new BookingControllers().GetInvoicePlBooked(issuekey);
        }

        private void gridInvView_Click(object sender, EventArgs e)
        {
            try
            {
                var issuekey = gridInvView.GetFocusedRowCellValue("issuekey").ToString();
                GetPalletList(issuekey);
            }
            catch (Exception)
            {
            }
        }

        void UpdateContainer(BookingInvoicePallet pl)
        {
            try
            {
                string sql = $"select * from txp_loadcontainer where containerno = '{bbiContainer.EditValue.ToString().ToUpper()}'";
                DateTime d = DateTime.Parse(bbiRelDate.EditValue.ToString());
                string txttime = $"{d.ToString("dd/MM/yyyy")} {bbiRelTimer.EditValue.ToString().Substring(10, 9).Trim()}";
                DateTime dx = DateTime.Parse(bbiEtd.EditValue.ToString());
                string containersize = "20F";
                if (bbi4oFt.Checked)
                {
                    containersize = "40F";
                }
                DataSet dr = new ConnDB().GetFill(sql);
                string sqlupdate = $"update txp_loadcontainer set etddte=to_date('{dx.ToString("dd/MM/yyyy")}', 'DD/MM/YYYY')," +
                                    $"sealno='{bbiSealNo.EditValue.ToString().ToUpper()}',containersize='{containersize}'," +
                                    $"receivedte=to_date('{d.ToString("dd/MM/yyyy")}', 'DD/MM/YYYY')," +
                                    $"releasedte=to_date('{txttime}', 'DD/MM/YYYY HH24:MI:SS'),upddte=sysdate\n" +
                                    $"where containerno='{bbiContainer.EditValue.ToString().ToUpper()}'";
                if (dr.Tables[0].Rows.Count <= 0)
                {
                    sqlupdate = "insert into txp_loadcontainer(containerno,custname,etddte,sealno,containersize,receivedte,releasedte,sysdte,upddte) \n" +
                                   "values \n" +
                                   $"('{bbiContainer.EditValue.ToString().ToUpper()}', " +
                                   $"'{pl.custname.Trim().ToUpper()}', " +
                                   $"to_date('{dx.ToString("dd/MM/yyyy")}', 'DD/MM/YYYY'), '{bbiSealNo.EditValue.ToString().ToUpper()}', " +
                                   $"'{containersize}', to_date('{d.ToString("dd/MM/yyyy")}', 'DD/MM/YYYY'), " +
                                   $"to_date('{txttime}', 'DD/MM/YYYY HH24:MI:SS'), sysdate, sysdate)";

                }
                new ConnDB().ExcuteSQL(sqlupdate);
                new ConnDB().ExcuteSQL($"UPDATE TXP_LOADPALLET SET CONTAINERNO = '{bbiContainer.EditValue.ToString().ToUpper()}' WHERE PLOUTNO = '{pl.ploutno}'");
            }
            catch (Exception ex)
            {
                GreeterFunction.Logs(ex.Message);
            }
        }

        bool UpdatePallet(BookingInvoicePallet pl, int sl)
        {
            string sql = $"SELECT * FROM TXP_LOADINVOICE i WHERE ISSUINGKEY = '{pl.issuekey}' AND CONTAINERNO = '{bbiContainer.EditValue.ToString().ToUpper()}'";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            string sql_isspl = $"UPDATE txp_isspallet l SET l.booked = {sl},l.CONTAINERNO = '{bbiContainer.EditValue.ToString().ToUpper()}',l.custname = '{pl.custname.Trim().ToUpper()}' " +
                               $"WHERE l.issuingkey = '{pl.issuekey}' AND l.palletno = '{pl.plno}'";
            if (sl == 6)
            {
                if (dr.Tables[0].Rows.Count <= 0)
                {
                    DateTime dx = DateTime.Parse(bbiEtd.EditValue.ToString());

                    string fac = "AW";
                    if (pl.issuekey.Substring(0, 1) == "I")
                    {
                        fac = "INJ";
                    }
                    string sql_loadinvoice = "insert into txp_loadinvoice(issuingkey,custname,etddte,factory,containerno,sysdte,upddte) \n" +
                             "values \n" +
                             $"('{pl.issuekey}', '{bbiCustomer.EditValue.ToString().Trim().ToUpper()}', " +
                             $"to_date('{dx.ToString("dd/MM/yyyy")}', 'DD/MM/YYYY'), " +
                             $"'{fac}', '{bbiContainer.EditValue.ToString().ToUpper()}', sysdate,sysdate)";
                    new ConnDB().ExcuteSQL(sql_loadinvoice);
                }
            }
            else
            {
                sql_isspl = $"UPDATE txp_isspallet l SET l.booked = {sl},l.CONTAINERNO = '',l.custname = '' " +
                            $"WHERE l.issuingkey = '{pl.issuekey}' AND l.palletno = '{pl.plno}'";
                new ConnDB().ExcuteSQL($"UPDATE TXP_LOADPALLET SET CONTAINERNO = '' WHERE PLOUTNO = '{pl.ploutno}'");
            }
            new ConnDB().ExcuteSQL(sql_isspl);
            return true;
        }

        private void gridPalletView_Click(object sender, EventArgs e)
        {
            
        }

        private void gridSlPlView_Click(object sender, EventArgs e)
        {
            
        }

        void ReloadContainer()
        {
            gridContainerControl.DataSource = new BookingControllers().GetContainerDeaitl(bbiContainer.EditValue.ToString().ToUpper());
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            UpdateContainer(null);
            XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            try
            {
                if (bbiContainer.EditValue.ToString() != "")
                {
                    BookingPreviewForm frm = new BookingPreviewForm(bbiContainer.EditValue.ToString());
                    frm.ShowDialog();
                }
            }
            catch (Exception)
            {
            }
        }

        private void gridContainerView_Click(object sender, EventArgs e)
        {
            try
            {
                var issuekey = gridContainerView.GetFocusedRowCellValue("issuekey").ToString();
                GetPalletList(issuekey);
                GetInvoiceList();
            }
            catch (Exception)
            {
            }
        }

        private void bbiSealNo_Enter(object sender, EventArgs e)
        {
            try
            {
                string txt = bbiContainer.EditValue.ToString();
                bbiContainer.EditValue = txt.ToUpper();
                ReloadContainer();
            }
            catch (Exception)
            {
            }
        }

        private void gridInvView_DoubleClick(object sender, EventArgs e)
        {

        }

        private void gridPalletView_DoubleClick(object sender, EventArgs e)
        {
            if (bbiContainer.EditValue.ToString() == "")
            {
                XtraMessageBox.Show("กรุณาระบุเลขที่ CONTAINER ด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bbiContainer.Focus();
                return;
            }
            else if (bbiSealNo.EditValue.ToString() == "")
            {
                XtraMessageBox.Show("กรุณาระบุเลขที่ SEALNO ด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bbiSealNo.Focus();
                return;
            }
            else
            {
                string txt = bbiSealNo.EditValue.ToString().ToUpper();
                bbiSealNo.EditValue = txt;
                BookingInvoicePallet pl = gridPalletView.GetFocusedRow() as BookingInvoicePallet;
                pl.conno = bbiContainer.EditValue.ToString().ToUpper();
                pl.sealno = txt;
                cpl.Add(pl);
                int x = 1;
                cpl.ForEach(i => {
                    i.id = x;
                    x++;
                });
                if (UpdatePallet(pl, 6))
                {
                    UpdateContainer(pl);
                }


                gridSlPlControl.BeginUpdate();
                gridSlPlControl.DataSource = cpl;
                gridSlPlControl.EndUpdate();

                List<BookingInvoicePallet> obj = gridPalletControl.DataSource as List<BookingInvoicePallet>;
                obj.Remove(pl);
                x = 1;
                obj.ForEach(i => {
                    i.id = x;
                    x++;
                });
                gridPalletControl.BeginUpdate();
                gridPalletControl.DataSource = obj;
                gridPalletControl.EndUpdate();

                ReloadContainer();
            }
        }

        private void gridSlPlView_DoubleClick(object sender, EventArgs e)
        {
            BookingInvoicePallet p = gridSlPlView.GetFocusedRow() as BookingInvoicePallet;
            if (p.plstatus > 3)
            {
                XtraMessageBox.Show($"ไม่สามารถลบข้อมูลนี้ได้\nเนื่องจากถูก LOAD เรียบร้อยแล้ว", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else
            {
                DialogResult r = XtraMessageBox.Show($"คุณต้องการที่จะนำ {p.plno} ออกใช่หรือไม่?", "XPW Alert!", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    List<BookingInvoicePallet> pl = gridPalletControl.DataSource as List<BookingInvoicePallet>;
                    pl.Add(p);
                    int x = 1;
                    pl.ForEach(i => {
                        i.id = x;
                        x++;
                    });
                    gridPalletControl.BeginUpdate();
                    gridPalletControl.DataSource = pl;
                    gridPalletControl.EndUpdate();

                    List<BookingInvoicePallet> px = gridSlPlControl.DataSource as List<BookingInvoicePallet>;
                    if (UpdatePallet(p, 0))
                    {
                        px.Remove(p);
                        x = 1;
                        px.ForEach(i => {
                            i.id = x;
                            x++;
                        });
                        gridSlPlControl.BeginUpdate();
                        gridSlPlControl.DataSource = px;
                        gridSlPlControl.EndUpdate();
                    }
                }
            }
            ReloadContainer();
        }
    }
}