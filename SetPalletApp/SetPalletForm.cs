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
using XPWLibrary.Controllers;
using XPWLibrary.Models;
using XPWLibrary.Interfaces;

namespace SetPalletApp
{
    public partial class SetPalletForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string inv;
        List<SetPallatData> npl = new List<SetPallatData>();
        public SetPalletForm(string issuekey)
        {
            InitializeComponent();
            this.Text = $"{issuekey} DETAIL";
            inv = issuekey;
            Reload();
        }

        void Reload()
        {
            gridPartControl.DataSource = null;
            gridPalletControl.DataSource = null;
            gridPalleteDetailControl.DataSource = null;
            List<SetPallatData> list = new SetPalletControllers().GetPartListDetail(inv);
            gridPartControl.DataSource = list;
            if (list.Count > 0)
            {
                var r = list[0];
                bbiFactory.EditValue = r.Factory;
                bbiShip.EditValue = r.ShipType;
                bbiZone.EditValue = r.ZName;
                bbiEtd.EditValue = r.EtdDte;
                bbiAff.EditValue = r.AffCode;
                bbiCustCode.EditValue = r.CustCode;
                bbiCustName.EditValue = r.CustName;
                bbiOrderBy.EditValue = r.CombInv;
                bbiRefInv.EditValue = r.RefNo;
                bbiInv.EditValue = r.RefInv;
            }
            bsiRecordsCount.Caption = "RECORDS : " + list.Count;
            npl = new SetPalletControllers().GetPartListCompletedDetail(inv);
            gridPalletControl.DataSource = npl;
        }

        string ReloadPlSet(SetPallatData obj, int p)
        {
            if (p > 0)
            {
                obj.PlSize = new GreeterFunction().GetPlSize(obj.PlSize, p);
                string plno = $"1P{(npl.Count + 1).ToString("D3")}";
                string sql = $"SELECT * FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY = '{obj.RefNo}' AND l.PALLETNO = '{plno}'";
                DataSet dr = new ConnDB().GetFill(sql);
                if (dr.Tables[0].Rows.Count < 1)
                {
                    string ins = $"INSERT INTO TXP_ISSPALLET(FACTORY, ISSUINGKEY, PALLETNO, PLTYPE, PLOUTSTS, UPDDTE, SYSDTE, PLTOTAL, BOOKED)\n" +
                        $"VALUES('{StaticFunctionData.Factory}', '{obj.RefNo}', '{plno}', '{obj.PlSize}', 0, current_timestamp, current_timestamp, {p}, 0)";
                    new ConnDB().ExcuteSQL(ins);
                }

                npl = new SetPalletControllers().GetPartListCompletedDetail(inv);
                gridPalletControl.BeginUpdate();
                gridPalletControl.DataSource = npl;
                gridPalletControl.EndUpdate();
                return plno;
            }
            return null;
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetPalletReportJobOrderPreview frm = new SetPalletReportJobOrderPreview(inv);
            frm.ShowDialog();
        }

        //void InsertPalletToPackingDetail(SetPallatData obj, string plno)
        //{
        //    string sql = $"UPDATE TXP_ISSPACKDETAIL SET SHIPPLNO = '{plno}'\n"+
        //                $"WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{obj.RefNo}' AND PONO = '{obj.OrderNo}' AND PARTNO = '{obj.PartNo}' AND ROWNUM< 2";
        //    new ConnDB().ExcuteSQL(sql);
        //}

        private void gridPartView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                SetPallatData obj = gridPartView.GetFocusedRow() as SetPallatData;
                int p = 0;
                if (obj.Ctn > 0)
                {
                    var x = XtraInputBox.Show("กรุณาระบุจำนวนต่อ 1 พาเลท", "XPW Alert!", $"{obj.Ctn}");
                    if (x != "")
                    {
                        p = int.Parse(x);
                        if (p > 0)
                        {
                            if (obj.Ctn < p)
                            {
                                XtraMessageBox.Show("กรุณาระบุจำนวนให้ถูกต้องด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            gridPartView.SetFocusedRowCellValue("Ctn", (obj.Ctn - p));
                            gridPartView.SetFocusedRowCellValue("CtnQty", (obj.CtnQty + p));
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("ไม่สามารถดำเนินการต่อได้\nเนื่องจากจำนวนเป็น 0", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                gridPartView.UpdateCurrentRow();
                gridPartView.UpdateSummary();
            }
            catch (Exception)
            {

                XtraMessageBox.Show("ไม่สามารถดำเนินการต่อได้\nกรุณาตรวจสอบความข้อมูลด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void gridPartView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                bool c = false;
                int x = 0;
                List<SetPallatData> list = gridPartView.DataSource as List<SetPallatData>;
                list.ForEach(i => {
                    c = true;
                    x += i.CtnQty;
                });
                bbiSetPallet.Enabled = c;
                bbiSetPallet.Caption = $"Set Pallet({x})";
                popupMenu1.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else
            {
                popupMenu1.HidePopup();
            }
        }

        private void bbiSetPallet_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<SetPallatData> list = gridPartControl.DataSource as List<SetPallatData>;
            List<SetPallatData> ob = new List<SetPallatData>();
            List<SetPallatData> n = new List<SetPallatData>();
            SetPallatData obj = new SetPallatData();
            int x = 0;
            foreach (var r in list)
            {
                if (r.CtnQty > 0)
                {
                    x += r.CtnQty;
                    obj = r;
                    n.Add(r);
                    if (r.Ctn > 0)
                    {
                        ob.Add(r);
                    }
                }
                else
                {
                    ob.Add(r);
                }
            }
            if (x > 0)
            {
                if (obj != null)
                {
                    string plno = ReloadPlSet(obj, x);
                    foreach (var j in n)
                    {
                        InsertPalletToPackingDetailAll(j, plno);
                    }
                    int ix = 1;
                    ob.ForEach(i =>
                    {
                        i.Id = ix;
                        i.CtnQty = 0;
                        ix++;
                    });
                    gridPartControl.DataSource = ob;
                }
            }
            else
            {
                XtraMessageBox.Show("กรุณาระบุจำนวนที่ต้องการด้วย", "ข้อความแจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void InsertPalletToPackingDetailAll(SetPallatData obj, string plno)
        {
            int i = 0;
            while (i < obj.CtnQty)
            {
                string sql = $"UPDATE TXP_ISSPACKDETAIL SET SHIPPLNO = '{plno}'\n" +
                        $"WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{obj.RefNo}' AND PONO = '{obj.OrderNo}' AND PARTNO = '{obj.PartNo}' AND ROWNUM < 2";
                new ConnDB().ExcuteSQL(sql);
                i++;
            }
        }

        private void gridPartView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            //switch (e.Column.FieldName)
            //{
            //    case "CtnQty":
            //        var x = XtraInputBox.Show("กรุณาระบุจำนวนที่ต้องการ", "ยืนยันจำนวนต่อพาเลท", "");
            //        if (x != "")
            //        {
            //            SetPallatData obj = gridPartView.GetFocusedRow() as SetPallatData;
            //            int p = int.Parse(x);
            //            if (p <= obj.Ctn)
            //            {
            //                gridPartView.SetFocusedRowCellValue("CtnQty", p);
            //            }
            //            else {
            //                XtraMessageBox.Show("กรุณาระบุจำนวนให้ถูกต้องด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //                return;
            //            }
            //        }
            //        break;
            //    default:
            //        break;
            //}
        }

        private void gridPartView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName)
            {
                case "Ctn":
                case "CtnQty":
                    if (e.Value.ToString() == "0")
                    {
                        e.DisplayText = "";
                    }
                    break;
                default:
                    break;
            }
        }

        private void gridPalletView_Click(object sender, EventArgs e)
        {
            try
            {
                SetPallatData x = gridPalletView.GetFocusedRow() as SetPallatData;
                gridPalleteDetailControl.DataSource = new SetPalletControllers().GetPallatePartList(x);
            }
            catch (Exception)
            {
            }
        }

        private void bbiResetQty_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetPallatData obj = gridPartView.GetFocusedRow() as SetPallatData;
            gridPartView.SetFocusedRowCellValue("Ctn", (obj.Ctn + obj.CtnQty));
            gridPartView.SetFocusedRowCellValue("CtnQty", 0);
        }

        private void gridPalletView_DoubleClick(object sender, EventArgs e)
        {
            SetPallatData x = gridPalletView.GetFocusedRow() as SetPallatData;
            var pl = XtraInputBox.Show("แก้ไขข้อมูลประเภทพาเลท", "ข้อความแจ้งเตือน", x.PlSize);
            if (pl != "")
            {
                x.PlSize = pl;
                if (new SetPalletControllers().UpdatePalletSize(x))
                {
                    XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
                    gridPalleteDetailControl.DataSource = new SetPalletControllers().GetPallatePartList(x);
                }
                else
                {
                    XtraMessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล", "ข้อความแจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void gridPartView_Click(object sender, EventArgs e)
        {
            gridPartView.UpdateSummary();
        }

        private void gridPalletView_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                popupMenu2.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else
            {
                popupMenu2.HidePopup();
            }
        }

        private void bbiDeletePallet_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetPallatData x = gridPalletView.GetFocusedRow() as SetPallatData;
            DialogResult r = XtraMessageBox.Show($"คุณต้องการที่จะลบพาเลท {x.ShipPlNo} ใช่หรือไม่?", "ข้อความแจ้งเตือน", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                if (new SetPalletControllers().UpdatePallet(x))
                {
                    XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
                }
                else
                {
                    XtraMessageBox.Show("เกิดข้อผิดพลาดในการบันทึกข้อมูล", "ข้อความแจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Reload();
        }
    }
}