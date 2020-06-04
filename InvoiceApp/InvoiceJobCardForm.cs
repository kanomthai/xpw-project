using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace InvoiceApp
{
    public partial class InvoiceJobCardForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        InvoiceBodyData ob;
        public InvoiceJobCardForm(InvoiceBodyData obj)
        {
            InitializeComponent();
            ob = obj;
            this.Text = $"PRINT JOBCARD {obj.RefInv} WITH {obj.PartNo}";
            string sql = $"SELECT d.PONO,d.PARTNO,d.FTICKETNO,d.ORDERQTY,b.LOTNO,d.CTNSN,d.UNIT,d.PLOUTNO,d.ISSUINGSTATUS FROM TXP_ISSPACKDETAIL d \n" +
                "INNER JOIN TXP_ISSTRANSBODY b ON d.ISSUINGKEY = b.ISSUINGKEY AND b.PARTNO = d.PARTNO\n" +
                $"WHERE d.PARTNO = '{obj.PartNo}' AND d.ISSUINGKEY = '{obj.RefInv}'\n" +
                $"GROUP BY d.PONO,d.PARTNO,d.FTICKETNO,d.ORDERQTY,b.LOTNO,d.CTNSN,d.UNIT,d.PLOUTNO,d.ISSUINGSTATUS\n" +
                $"ORDER BY d.FTICKETNO ";
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
                    Seq = iseq,
                    OrderNo = r["pono"].ToString(),
                    PartNo = r["partno"].ToString(),
                    FTicketNo = r["fticketno"].ToString(),
                    LotNo = r["lotno"].ToString(),
                    OrderQty = int.Parse(r["orderqty"].ToString()),
                    SerialNo = r["ctnsn"].ToString(),
                    Unit = r["unit"].ToString(),
                    PlOutNo = r["ploutno"].ToString(),
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
            SplashScreenManager.ShowDefaultWaitForm();
            bool checkinv = new InvoiceControllers().CheckInvoiceStatus(ob.RefInv);
            if (checkinv)
            {
                bool plabel = new InvoiceControllers().PrintFTicket(ob.RefInv, ob.PartNo, ob.OrderNo, ob.StartFticket, null);
                if (plabel)
                {
                    XtraMessageBox.Show("ปริ้นข้อมูลเสร็จแล้ว");
                    this.Close();
                }
            }
            else
            {
                XtraMessageBox.Show("กรุณาทำการยืนยัน Invoice ก่อน", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
        }

        private void gridView_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (e.Button.ToString() == "Right")
            {
                bbiJobCardOnly.Caption = $"Print Label({gridView.GetFocusedRowCellValue("Seq").ToString()})";
                bbiDelete.Caption = $"Cut Short({gridView.GetFocusedRowCellValue("Seq").ToString()})";
                popupMenu1.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
            }
            else
            {
                popupMenu1.HidePopup();
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
                            e.DisplayText = "Printed";
                            break;
                        case "2":
                            e.DisplayText = "Prepare";
                            break;
                        case "3":
                            e.DisplayText = "Shorting";
                            break;
                        default:
                            e.DisplayText = "Unkonw";
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        private void bbiJobCardOnly_ItemClick(object sender, ItemClickEventArgs e)
        {
            SplashScreenManager.ShowDefaultWaitForm();
            bool checkinv = new InvoiceControllers().CheckInvoiceStatus(ob.RefInv);
            if (checkinv)
            {
                bool plabel = new InvoiceControllers().PrintFTicket(ob.RefInv, ob.PartNo, ob.OrderNo, ob.StartFticket, gridView.GetFocusedRowCellValue("Seq").ToString());
                if (plabel)
                {
                    XtraMessageBox.Show("ปริ้นข้อมูลเสร็จแล้ว");
                    this.Close();
                }
            }
            else
            {
                XtraMessageBox.Show("กรุณาทำการยืนยัน Invoice ก่อน", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
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
    }
}