using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace InvoiceApp
{
    public partial class InvoiceJobCardForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        InvoiceBodyData ob;
        public InvoiceJobCardForm(InvoiceBodyData obj, bool all)
        {
            InitializeComponent();
            ob = obj;
            bbiJobCardOnly.Enabled = true;
            if (StaticFunctionData.Factory == "AW")
            {
                gridView.OptionsSelection.MultiSelect = false;
                bbiJobCardOnly.Enabled = false;
            }
            if (all)
            {
                ShowFTicketAll();
            }
            else
            {
                ShowFTicketByPart();
            }
        }

        void ShowFTicketByPart()
        {
            this.Text = $"PRINT JOBCARD {ob.RefInv} WITH {ob.PartNo}";
            string sql = $"SELECT d.ITEM,d.PONO,d.PARTNO,p.PARTNAME,d.FTICKETNO,d.ORDERQTY,b.LOTNO,d.CTNSN,d.UNIT,d.PLOUTNO,CASE WHEN d.PLOUTNO IS NULL THEN '' ELSE max(l.PALLETNO) END PALLETNO,c.SHELVE ,d.ISSUINGSTATUS FROM TXP_ISSPACKDETAIL d  \n" +
                "INNER JOIN TXP_ISSTRANSBODY b ON d.ISSUINGKEY = b.ISSUINGKEY AND b.PARTNO = d.PARTNO\n" +
                "LEFT JOIN TXP_ISSPALLET l ON b.ISSUINGKEY = l.ISSUINGKEY\n" +
                "LEFT JOIN TXP_CARTONDETAILS c ON d.PLOUTNO = c.PLOUTNO\n" +
                "LEFT JOIN TXP_PART p ON d.PARTNO = p.PARTNO\n" +
                $"WHERE d.PARTNO = '{ob.PartNo}' AND d.ISSUINGKEY = '{ob.RefInv}'\n" +
                $"GROUP BY d.ITEM,d.PONO,d.PARTNO,p.PARTNAME,d.FTICKETNO,d.ORDERQTY,b.LOTNO,d.CTNSN,d.UNIT,d.PLOUTNO,c.SHELVE ,d.ISSUINGSTATUS\n" +
                $"ORDER BY d.ITEM,d.SHIPPLNO,d.FTICKETNO  ";
            Console.WriteLine(sql);
            List<FTicketData> list = new List<FTicketData>();
            DataSet dr = new ConnDB().GetFill(sql);
            int iseq = ob.StartFticket;
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                iseq++;
                list.Add(new FTicketData()
                {
                    Id = list.Count + 1,
                    Seq = int.Parse(r["item"].ToString()),
                    OrderNo = r["pono"].ToString(),
                    PartName = r["partname"].ToString(),
                    PartNo = r["partno"].ToString(),
                    FTicketNo = r["fticketno"].ToString(),
                    LotNo = r["lotno"].ToString(),
                    OrderQty = int.Parse(r["orderqty"].ToString()),
                    SerialNo = r["ctnsn"].ToString(),
                    Unit = r["unit"].ToString(),
                    PlOutNo = r["ploutno"].ToString(),
                    PlNo = r["palletno"].ToString(),
                    Shelve = r["shelve"].ToString(),
                    Status = int.Parse(r["issuingstatus"].ToString()),
                    PrintFTicket = false,
                });
            }
            gridControl.DataSource = list;
            bsiRecordsCount.Caption = "RECORDS : " + list.Count;
        }

        void ShowFTicketAll()
        {
            this.Text = $"Packing List({ob.RefInv})";
            string sql = $"SELECT d.ITEM,d.PONO,d.PARTNO,p.PARTNAME,d.FTICKETNO,d.ORDERQTY,b.LOTNO,d.CTNSN,d.UNIT,d.PLOUTNO,d.SHIPPLNO PALLETNO,c.SHELVE ,d.ISSUINGSTATUS FROM TXP_ISSPACKDETAIL d  \n" +
                 "INNER JOIN TXP_ISSTRANSBODY b ON d.ISSUINGKEY = b.ISSUINGKEY AND b.PARTNO = d.PARTNO\n" +
                 "LEFT JOIN TXP_ISSPALLET l ON b.ISSUINGKEY = l.ISSUINGKEY\n" +
                 "LEFT JOIN TXP_CARTONDETAILS c ON d.PLOUTNO = c.PLOUTNO\n" +
                 "LEFT JOIN TXP_PART p ON d.PARTNO = p.PARTNO\n" +
                 $"WHERE d.ISSUINGKEY = '{ob.RefInv}'\n" +
                 $"GROUP BY d.ITEM,d.PONO,d.PARTNO,p.PARTNAME,d.FTICKETNO,d.ORDERQTY,b.LOTNO,d.CTNSN,d.UNIT,d.PLOUTNO,c.SHELVE ,d.ISSUINGSTATUS,d.SHIPPLNO\n" +
                 $"ORDER BY d.ITEM,d.SHIPPLNO,d.FTICKETNO ";
            Console.WriteLine(sql);
            List<FTicketData> list = new List<FTicketData>();
            DataSet dr = new ConnDB().GetFill(sql);
            int iseq = 0;
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                iseq++;
                list.Add(new FTicketData()
                {
                    Id = list.Count + 1,
                    Seq = int.Parse(r["item"].ToString()),
                    OrderNo = r["pono"].ToString(),
                    PartName = r["partname"].ToString(),
                    PartNo = r["partno"].ToString(),
                    FTicketNo = r["fticketno"].ToString(),
                    LotNo = r["lotno"].ToString(),
                    OrderQty = int.Parse(r["orderqty"].ToString()),
                    SerialNo = r["ctnsn"].ToString(),
                    Unit = r["unit"].ToString(),
                    PlOutNo = r["ploutno"].ToString(),
                    PlNo = r["palletno"].ToString(),
                    Shelve = r["shelve"].ToString(),
                    Status = int.Parse(r["issuingstatus"].ToString()),
                    PrintFTicket = false,
                });
            }
            gridControl.DataSource = list;
            bsiRecordsCount.Caption = "RECORDS : " + list.Count;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void bbiPrintJobCard_ItemClick(object sender, ItemClickEventArgs e)
        {
            //SplashScreenManager.ShowDefaultWaitForm();
            //bool checkinv = new InvoiceControllers().CheckInvoiceStatus(ob.RefInv);
            //if (checkinv)
            //{
            //    bool plabel = new InvoiceControllers().PrintFTicket(ob.RefInv, ob.PartNo, ob.OrderNo, ob.StartFticket, null);
            //    if (plabel)
            //    {
            //        XtraMessageBox.Show("ปริ้นข้อมูลเสร็จแล้ว");
            //        //this.Close();
            //    }
            //}
            //else
            //{
            //    XtraMessageBox.Show("กรุณาทำการยืนยัน Invoice ก่อน", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    this.Close();
            //}
        }

        private void gridView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                List<FTicketData> list = gridControl.DataSource as List<FTicketData>;
                if (list.Count < 0)
                {
                    bbiJobCardOnly.Caption = $"Print Label";
                    bbiDelete.Caption = $"Cut Short({gridView.GetFocusedRowCellValue("Seq").ToString()})";
                    popupMenu1.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
                }
                else
                {
                    popupMenu1.HidePopup();
                }
            }
        }

        private void gridView_CustomColumnDisplayText(object sender, DevExpress.XtraGrid.Views.Base.CustomColumnDisplayTextEventArgs e)
        {
            switch (e.Column.FieldName.ToString())
            {
                case "OrderQty":
                    e.DisplayText = "";
                    if (int.Parse(e.Value.ToString()) > 0)
                    {
                        e.DisplayText = string.Format("{0:n0}", int.Parse(e.Value.ToString()));
                    }
                    break;
                case "Status":
                    switch (e.Value.ToString())
                    {
                        case "0":
                            e.DisplayText = "None";
                            break;
                        case "1":
                            e.DisplayText = "Print F-Ticket";
                            break;
                        //case "2":
                        //    e.DisplayText = "";
                        //    break;
                        case "3":
                            e.DisplayText = "Cut Short";
                            break;
                        case "4":
                            e.DisplayText = "Prepared";
                            break;
                        default:
                            //e.DisplayText = "Unkonw";
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void bbiJobCardOnly_ItemClick(object sender, ItemClickEventArgs e)
        {
            bool checkinv = new InvoiceControllers().CheckInvoiceStatus(ob.RefInv);
            SplashScreenManager.ShowDefaultWaitForm();
            if (checkinv)
            {
                List<FTicketData> f = gridControl.DataSource as List<FTicketData>;
                List<FTicketData> list = new List<FTicketData>();
                var x = f.OrderByDescending(j => j.PrintFTicket).ToList();
                int i = 0;
                while (i < x.Count)
                {
                    FTicketData o = x[i];
                    if (o.Status < StaticFunctionData.StatusFTicket)
                    {
                        if (o.PrintFTicket)
                        {
                            Console.WriteLine(o);
                            list.Add(o);
                            //bool plabel = new InvoiceControllers().PrintFTicket(ob.RefInv, o.PartNo, o.OrderNo, o.Seq, o.Seq.ToString());
                            //bool plabel = new InvoiceControllers().PrintFTicket(ob.RefInv, o.FTicketNo, o.Id.ToString());
                            //if (plabel)
                            //{
                            //    //XtraMessageBox.Show("ปริ้นข้อมูลเสร็จแล้ว");
                            //    //this.Close();
                            //}
                        }
                    }
                    //else
                    //{
                    //    XtraMessageBox.Show("Label นี้ปริ้น/จัดเตรียมไปแล้ว", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    i++;
                }
                bool plabel = new InvoiceControllers().PrintFTicket(ob.RefInv, list);
                if (plabel)
                {
                    XtraMessageBox.Show("ปริ้นข้อมูลเสร็จแล้ว");
                }
            }
            else
            {
                XtraMessageBox.Show("กรุณาทำการยืนยัน Invoice ก่อน", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            try
            {
                SplashScreenManager.CloseDefaultWaitForm();
            }
            catch (Exception)
            {
            }
        }

        private void bbiDelete_ItemClick(object sender, ItemClickEventArgs e)
        {
            var fno = gridView.GetFocusedRowCellValue("FTicketNo");
            DialogResult r = XtraMessageBox.Show($"คุณต้องการตัด Short {fno.ToString()} นี้ใช่หรือไม่", "ยืนยันคำสั่ง", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                bool x = new ConnDB().ExcuteSQL($"UPDATE TXP_ISSPACKDETAIL d SET d.ISSUINGSTATUS = 3 WHERE d.FTICKETNO = '{fno}'");
                if (x)
                {
                    int ctn = int.Parse(gridView.GetFocusedRowCellValue("OrderQty").ToString());
                    int xn = new GreeterFunction().GetShortQty(gridView.GetFocusedRowCellValue("PartNo").ToString(), ob.RefInv, ctn);
                    new ConnDB().ExcuteSQL($"UPDATE TXP_ISSTRANSBODY d SET d.SHORDERQTY = {xn} WHERE d.PARTNO = '{gridView.GetFocusedRowCellValue("PartNo")}' and d.ISSUINGKEY = '{ob.RefInv}'");
                    gridView.SetFocusedRowCellValue("Status", 3);
                    XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
                }
            }
            return;
        }

        private void gridView_RowClick(object sender, DevExpress.XtraGrid.Views.Grid.RowClickEventArgs e)
        {
            if (StaticFunctionData.Factory != "AW")
            {
                FTicketData f = gridView.GetFocusedRow() as FTicketData;
                if (f.Status >= StaticFunctionData.StatusFTicket)
                {
                    XtraMessageBox.Show("Label นี้ปริ้น/จัดเตรียมไปแล้ว", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gridView.SetRowCellValue(e.RowHandle, "PrintFTicket", false);
                }
                else if (f.PlNo == "")
                {
                    XtraMessageBox.Show("ไม่สามารถปริ้น FTicket ได้\nเนื่องจากยังไม่ระบุเลขที่พาเลท", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    gridView.SetRowCellValue(e.RowHandle, "PrintFTicket", false);
                }
            }
        }

        private void gridView_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                if (StaticFunctionData.Factory != "AW")
                {
                    bbiJobCardOnly.Caption = $"Print Label";
                    bbiDelete.Caption = $"Cut Short({gridView.GetFocusedRowCellValue("Seq").ToString()})";
                    popupMenu1.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
                }
            }
            else
            {
                popupMenu1.HidePopup();
            }
        }
    }
}