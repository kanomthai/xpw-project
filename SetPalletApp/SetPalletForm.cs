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
using XPWLibrary.Controllers;
using XPWLibrary.Models;
using XPWLibrary.Interfaces;
using DevExpress.XtraSplashScreen;

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
            SplashScreenManager.ShowDefaultWaitForm();
            //new SetPalletControllers().CheckPalletSetSeq(issuekey);
            Reload();
            SplashScreenManager.CloseDefaultWaitForm();
        }

        void Reload()
        {
            gridPartControl.DataSource = null;
            gridPalletControl.DataSource = null;
            gridPalleteDetailControl.DataSource = null;
            List<SetPallatData> list = new SetPalletControllers().GetPartListDetail(inv);
            List<SetPallatData> ord = new List<SetPallatData>();
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
                list.ForEach(i => {
                    if (i.Ctn > 0)
                    {
                        ord.Add(i);
                    }
                });
            }
            gridPartControl.DataSource = ord;
            bsiRecordsCount.Caption = "RECORDS : " + ord.Count;
            npl = new SetPalletControllers().GetPartListCompletedDetail(inv);
            gridPalletControl.DataSource = npl;
        }

        string ReloadPlSet(SetPallatData obj, int p)
        {
            if (p > 0)
            {
                obj.PlSize = null;
                string plno = null;
                if (obj.Factory == "INJ")
                {
                    plno = new SetPalletControllers().GetLastPallet(obj.RefNo);
                    obj.PlSize = new GreeterFunction().GetPlSize(obj.PlSize, p);
                }
                else
                {
                    plno = new SetPalletControllers().GetLastPallet(obj);
                    obj.PlSize = new GreeterFunction().GetPalletWireSize(p);
                }

                string sql = $"SELECT * FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY = '{obj.RefNo}' AND l.PALLETNO = '{plno}'";
                string w = "0";
                string ll = "0";
                string hh = "0";
                if (obj.PlSize != "MIX")
                {
                    w = obj.PlSize.Substring(0, obj.PlSize.IndexOf("x"));
                    string l = obj.PlSize.Substring(w.Length + 1);
                    ll = obj.PlSize.Substring(w.Length + 1, l.IndexOf("x"));

                    string h = obj.PlSize.Substring(ll.Length + 2);
                    hh = h.Substring(h.IndexOf("x") + 1);

                }

                DataSet dr = new ConnDB().GetFill(sql);
                if (dr.Tables[0].Rows.Count < 1)
                {
                    string ins = $"INSERT INTO TXP_ISSPALLET(FACTORY, ISSUINGKEY, PALLETNO, PLTYPE, PLOUTSTS, UPDDTE, SYSDTE, PLTOTAL, BOOKED,PLWIDE,PLLENG,PLHIGHT)\n" +
                        $"VALUES('{StaticFunctionData.Factory}', '{obj.RefNo}', '{plno}', '{obj.PlSize}', 0, current_timestamp, current_timestamp, {p}, '0', '{w}', '{ll}' ,'{hh}')";
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
            List<SetPallatData> list = gridPartControl.DataSource as List<SetPallatData>;
            if (list.Count < 1)
            {
                SetPalletReportJobOrderPreview frm = new SetPalletReportJobOrderPreview(inv);
                frm.ShowDialog();
            }
            else
            {
                XtraMessageBox.Show("กรุณา Set Pallet ให้ครบด้วย", "ข้อความแจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
                bbiSetCarton.Enabled = false;
                int x = 0;
                List<SetPallatData> list = gridPartView.DataSource as List<SetPallatData>;
                list.ForEach(i => {
                    x += i.CtnQty;
                });
                if (x > 0)
                {
                    c = true;
                }
                if (list[0].Factory == "AW")
                {
                    bbisendToPallet.Enabled = true;
                    if (x == 2)
                    {
                        bbiSetCarton.Enabled = true;
                        bbiSetPallet.Enabled = false;
                    }
                    else if (x > 17)
                    {
                        bbiSetCarton.Enabled = false;
                        bbiSetPallet.Enabled = true;
                    }
                    else
                    {
                        bbiSetCarton.Enabled = false;
                        bbiSetPallet.Enabled = false;
                    }
                }
                else
                {
                    if (x < 1)
                    {
                        bbiSetCarton.Enabled = true;
                    }
                    bbisendToPallet.Enabled = c;
                    bbiSetPallet.Enabled = c;
                }
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
            Reload();
        }

        private void InsertPalletToPackingDetailAll(SetPallatData obj, string plno)
        {
            //int i = obj.CtnQty + 1;
            //string xsql = $"SELECT * FROM TXP_ISSPACKDETAIL \n" +
            //           $"WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{obj.RefNo}' AND PONO = '{obj.OrderNo}' AND PARTNO = '{obj.PartNo}' AND ROWNUM < {i}";
            //Console.WriteLine(xsql);
            //string sql = $"UPDATE TXP_ISSPACKDETAIL SET SHIPPLNO = '{plno.ToUpper()}'\n" +
            //            $"WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{obj.RefNo}' AND PONO = '{obj.OrderNo}' AND PARTNO = '{obj.PartNo}' AND ROWNUM < {i}";
            //new ConnDB().ExcuteSQL(sql);
            new SetPlControllers().InsertPalletToPackingDetailAll(obj, plno);
            new SetPalletControllers().CheckPalletSetSeq(obj.RefNo);
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
                SetPallatData x = gridPalletView.GetFocusedRow() as SetPallatData;
                bbiDeletePallet.Enabled = true;
                bbiPrintCarton.Enabled = true;
                bbiDeletePallet.Caption = $"Delete {x.ShipPlNo}";
                if (x.PlOutNo != "")
                {
                    bbiDeletePallet.Enabled = false;
                }
                if (x.ShipPlNo.IndexOf("P") >= 0)
                {
                    bbiPrintCarton.Enabled = false;
                }
                List<SetPallatData> obj = gridPalletControl.DataSource as List<SetPallatData>;
                bool xinv = false;
                obj.ForEach(j => {
                    if (j.NewInvoice)
                    {
                        xinv = true;
                    }
                });
                bbiNewInvoice.Enabled = xinv;
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

        bool ReloadCartonSet(SetPallatData obj, string plno)
        {
            string sql = $"SELECT * FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY = '{obj.RefNo}' AND l.PALLETNO = '{plno}'";
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count < 1)
            {
                string ins = $"INSERT INTO TXP_ISSPALLET(FACTORY, ISSUINGKEY, PALLETNO, PLTYPE, PLOUTSTS, UPDDTE, SYSDTE, PLTOTAL, BOOKED,PLWIDE,PLLENG,PLHIGHT)\n" +
                    $"VALUES('{StaticFunctionData.Factory}', '{obj.RefNo}', '{plno}', 'BOX', 0, current_timestamp, current_timestamp, 1, '0', '0', '0' ,'0')";
                new ConnDB().ExcuteSQL(ins);
            }

            npl = new SetPalletControllers().GetPartListCompletedDetail(inv);
            gridPalletControl.BeginUpdate();
            gridPalletControl.DataSource = npl;
            gridPalletControl.EndUpdate();
            return true;
        }



        private void bbiSetCarton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetPallatData obj = gridPartView.GetFocusedRow() as SetPallatData;
            if (obj.Factory == "AW")
            {
                DialogResult r = XtraMessageBox.Show($"คุณต้องการที่จะ Set Carton({obj.PName}) นี้ใช่หรือไม่?", "ข้อความแจ้งเตือน", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (r == DialogResult.Yes)
                {
                    List<SetPallatData> list = gridPartControl.DataSource as List<SetPallatData>;
                    string plno = new SetPalletControllers().GetLastCarton(list[0]);
                    int x = 0;
                    while (x < list.Count)
                    {
                        if (list[x].CtnQty > 0)
                        {
                            if (ReloadCartonSet(list[x], plno))
                            {
                                InsertPalletToPackingDetailAll(list[x], plno);
                            }
                        }
                        x++;
                    }
                }
                Reload();
            }
            else
            {
                if (obj.CtnQty <= 0)
                {
                    DialogResult r = XtraMessageBox.Show($"คุณต้องการที่จะ Set Carton({obj.PName}) นี้ใช่หรือไม่?", "ข้อความแจ้งเตือน", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (r == DialogResult.Yes)
                    {
                        obj.CtnQty += 1;
                        int x = obj.Ctn -= 1;
                        if (obj.CtnQty > 0)
                        {
                            string plno = new SetPalletControllers().GetLastCarton(obj.RefNo);

                            if (ReloadCartonSet(obj, plno))
                            {
                                InsertPalletToPackingDetailAll(obj, plno);
                                if (x > 0)
                                {
                                    gridPartView.SetFocusedRowCellValue("Ctn", x);
                                    gridPartView.SetFocusedRowCellValue("CtnQty", 0);
                                }
                                else
                                {
                                    gridPartView.DeleteSelectedRows();
                                }
                                gridPartView.UpdateCurrentRow();
                                gridPartView.UpdateTotalSummary();
                            }
                        }
                    }
                }
            }
        }

        private void bbisendToPallet_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetPallatData obj = gridPartView.GetFocusedRow() as SetPallatData;
            SetPalletAddToForm frm = new SetPalletAddToForm(obj);
            frm.ShowDialog();
            Reload();
        }

        private void bbiRefresh_ItemClick(object sender, ItemClickEventArgs e)
        {
            Reload();
        }

        private void bbiDelPartDetail_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetPalletListData obj = gridPalleteDetailView.GetFocusedRow() as SetPalletListData;
            DialogResult r = XtraMessageBox.Show($"ยืนยันคำสั่งลบ {obj.FTicket} ข้อมูล", "", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
            if (r == DialogResult.Yes)
            {
                if (new SetPlControllers().UpdateSetPallet(obj))
                {
                    gridPalleteDetailView.DeleteSelectedRows();
                    gridPalleteDetailView.UpdateCurrentRow();
                    gridPalleteDetailView.UpdateTotalSummary();

                    npl = new SetPalletControllers().GetPartListCompletedDetail(inv);
                    gridPalletControl.BeginUpdate();
                    gridPalletControl.DataSource = npl;
                    gridPalletControl.EndUpdate();

                    List<SetPallatData> list = new SetPalletControllers().GetPartListDetail(inv);
                    gridPartControl.DataSource = list;
                }
            }
        }

        private void gridPalleteDetailView_MouseUp(object sender, MouseEventArgs e)
        {
            List<SetPalletListData> o = gridPalleteDetailControl.DataSource as List<SetPalletListData>;
            if (o.Count > 0)
            {
                try
                {
                    if (e.Button.ToString() == "Right")
                    {
                        SetPalletListData obj = gridPalleteDetailView.GetFocusedRow() as SetPalletListData;
                        bbiDelPartDetail.Caption = $"Delete {obj.FTicket}";
                        bbiPrintCarton.Caption = $"Print Carton({obj.ShipPlNo})";
                        popupMenu3.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
                    }
                    else
                    {
                        popupMenu3.HidePopup();
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private void bbiPrintCarton_ItemClick(object sender, ItemClickEventArgs e)
        {
            SetPallatData obj = gridPalletView.GetFocusedRow() as SetPallatData;
            bool x = new InvoiceControllers().PrintWireLabelQR(obj.RefNo, obj.ShipPlNo);
            if (x)
            {
                XtraMessageBox.Show("ปริ้นข้อมูลเสร็จแล้ว", "ข้อความแจ้งเตือน", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void bbiNewInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {
            List<SetPallatData> l = gridPalletControl.DataSource as List<SetPallatData>;
            List<SetPallatData> obj = new List<SetPallatData>();
            int i = 0;
            while (i < l.Count)
            {
                var r = l[i];
                if (r.NewInvoice) {
                    r.Factory = bbiFactory.EditValue.ToString();
                    r.EtdDte = DateTime.Parse(bbiEtd.EditValue.ToString());
                    r.ShipType = bbiShip.EditValue.ToString();
                    obj.Add(r);
                }
                i++;
            }
            if (obj.Count > 0)
            {
                SetPalletNewInvoiceForm frm = new SetPalletNewInvoiceForm(obj);
                frm.ShowDialog();
                Reload();
            }
        }

        private void btnSetNewInvoice_ItemClick(object sender, ItemClickEventArgs e)
        {

        }
    }
}