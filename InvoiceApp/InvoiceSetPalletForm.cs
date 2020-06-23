using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace InvoiceApp
{
    public partial class InvoiceSetPalletForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string refinv;
        public InvoiceSetPalletForm(string refinvoice)
        {
            InitializeComponent();
            refinv = refinvoice;
            ReloadData();
        }

        void ReloadData()
        {
            this.Text = $"SET PALLET {refinv}";
            List<SetPlData> dataSource = GetDataSource(refinv);
            gridControl.DataSource = dataSource;
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }

        private List<SetPlData> GetDataSource(string refinv)
        {
            gridView.OptionsBehavior.Editable = false;
            return new SelPlControllers().GetPlData(refinv);
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        bool UpdatePlInvoice(string refno, string plnum, string pltype, string ctn)
        {
            bool checkpl = new GreeterFunction().CheckPlDuplicate(refno, plnum);
            //insert
            string ins = $"INSERT INTO TXP_ISSPALLET(FACTORY, ISSUINGKEY, PALLETNO, PLTYPE, PLOUTSTS, UPDDTE, SYSDTE, PLTOTAL, BOOKED)\n" +
                        $"VALUES('{StaticFunctionData.Factory}', '{refno}', '{plnum}', '{pltype}', 0, current_timestamp, current_timestamp, {ctn}, 0)";
            if (checkpl)
            {
                //update
                ins = $"UPDATE TXP_ISSPALLET SET PLTYPE='{pltype}', UPDDTE=current_timestamp, PLTOTAL={ctn}\n" +
                        $"WHERE ISSUINGKEY = '{refno}' AND PALLETNO = '{plnum}'";
            }
            return new ConnDB().ExcuteSQL(ins);
        }

        private void gridView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                string refno = gridView.GetFocusedRowCellValue("RefNo").ToString();
                string pldim = gridView.GetFocusedRowCellValue("pldim").ToString();
                string pltype = gridView.GetFocusedRowCellValue("pltype").ToString();
                string plno = gridView.GetFocusedRowCellValue("plno").ToString();
                string ctn = gridView.GetFocusedRowCellValue("ctn").ToString();
                string qty = gridView.GetFocusedRowCellValue("qty").ToString();
                if (int.Parse(qty) <= 0)
                {
                    XtraMessageBox.Show("กรุณาระบุจำนวนมากว่า 0 ด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string pl = "1P";
                if (pltype == "BOX")
                {
                    pl = "1C";
                }
                var result = XtraInputBox.Show("ระบุเลขที่ PL", "Create PlNO.", "");
                Console.WriteLine(result);
                try
                {
                    if (result.IndexOf("-") < 0)
                    {
                        string plnum = $"{pl}{int.Parse(result).ToString("D3")}";
                        if (UpdatePlInvoice(refno, plnum, pltype, qty))
                        {
                            gridView.SetFocusedRowCellValue("plno", plnum);
                        }
                    }
                    else
                    {
                        int drg = result.IndexOf("-");
                        string a = result.Substring(0, drg);
                        string b = result.Substring(drg).Replace("-", "").Trim();
                        int i = 0;
                        while (i <= (int.Parse(b) - int.Parse(a)))
                        {
                            string plnum = $"{pl}{(i + 1).ToString("D3")}";
                            if (UpdatePlInvoice(refno, plnum, pltype, qty))
                            {
                                i++;
                            }
                        }
                        gridView.SetFocusedRowCellValue("plno", $"{pl}{int.Parse(a).ToString("D3")}-{pl}{int.Parse(b).ToString("D3")}");
                    }
                    //if (pl == "1C")
                    //{
                    //    int i = 0;
                    //    while (i < int.Parse(qty))
                    //    {
                    //        string plnum = $"{pl}{(i + 1).ToString("D3")}";
                    //        if (UpdatePlInvoice(refno, plnum, pltype, ctn))
                    //        {
                    //            i++;
                    //        }
                    //    }
                    //    gridView.SetFocusedRowCellValue("plno", $"{pl}{(1).ToString("D3")}-{pl}{int.Parse(ctn).ToString("D3")}");
                    //}
                    //else
                    //{
                    //    if (result.IndexOf("-") < 0)
                    //    {
                    //        string plnum = $"{pl}{int.Parse(result).ToString("D3")}";
                    //        if (UpdatePlInvoice(refno, plnum, pltype, ctn))
                    //        {
                    //            gridView.SetFocusedRowCellValue("plno", plnum);
                    //        }
                    //    }
                    //    else
                    //    {
                    //        int drg = result.IndexOf("-");
                    //        string a = result.Substring(0, drg);
                    //        string b = result.Substring(drg).Replace("-", "").Trim();
                    //        int i = 0;
                    //        while (i <= (int.Parse(b) - int.Parse(a)))
                    //        {
                    //            string plnum = $"{pl}{(i + 1).ToString("D3")}";
                    //            if (UpdatePlInvoice(refno, plnum, pltype, ctn))
                    //            {
                    //                i++;
                    //            }
                    //        }
                    //        gridView.SetFocusedRowCellValue("plno", $"{pl}{int.Parse(a).ToString("D3")}-{pl}{int.Parse(b).ToString("D3")}");
                    //    }
                    //}
                }
                catch (Exception)
                {
                }
            }
            catch (Exception)
            {
            }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<SetPlData> obj = gridControl.DataSource as List<SetPlData>;
            obj.Add(new SetPlData()
            {
                id = obj.Count + 1,
                RefInv = obj[0].RefInv,
                RefNo = obj[0].RefNo,
                plno = "",
                pldim = "ADD",
                pltype = "New Pl Type",
                ctn = 0,
                unit = obj[0].unit,
            });
            gridControl.BeginUpdate();
            gridControl.DataSource = obj;
            bsiRecordsCount.Caption = "RECORDS : " + obj.Count;
            gridControl.EndUpdate();
        }

        private void bbiEditPallet_ItemClick(object sender, ItemClickEventArgs e)
        {
            var result = XtraInputBox.Show("ระบุขนาด Pallet", "Create PlType.", "");
            gridView.SetFocusedRowCellValue("pltype", result);
        }

        private void bbiPalletDetail_ItemClick(object sender, ItemClickEventArgs e)
        {

        }

        private void gridView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                popupMenu1.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else
            {
                popupMenu1.HidePopup();
            }
        }

        private void bbiTruncatePl_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<SetPlData> obj = gridControl.DataSource as List<SetPlData>;
            string sql = $"DELETE FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY = '{obj[0].RefNo}'";
            new ConnDB().ExcuteSQL(sql);
            ReloadData();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            ReloadData();
        }

        private void bbiEditQty_ItemClick(object sender, ItemClickEventArgs e)
        {
            try
            {
                var result = XtraInputBox.Show("ระบุจำนวน QTY ให้ถูกต้องด้วย", "Create PlType.", "");
                if (result != "")
                {
                    gridView.SetFocusedRowCellValue("qty", int.Parse(result));
                }
            }
            catch (Exception)
            {
                XtraMessageBox.Show("กรุณาระบุข้อมูลให้ถูกต้องด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}