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
            //gridControl.ShowRibbonPrintPreview();
        }

        void InsertPalletToPackingDetail(SetPallatData obj, string plno)
        {
            string sql = $"UPDATE TXP_ISSPACKDETAIL SET SHIPPLNO = '{plno}'\n"+
                        $"WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{obj.RefNo}' AND PONO = '{obj.OrderNo}' AND PARTNO = '{obj.PartNo}' AND ROWNUM< 2";
            new ConnDB().ExcuteSQL(sql);
        }

        private void gridPartView_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                SetPallatData obj = gridPartView.GetFocusedRow() as SetPallatData;
                int p = 0;
                if (obj.Ctn > 0)
                {
                    var x = XtraInputBox.Show("กรุณาระบุจำนวนต่อ 1 พาเลท", "XPW Alert!", "");
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
                            if ((obj.Ctn - p) <= 0)
                            {
                                gridPartView.DeleteRow(gridPartView.FocusedRowHandle);
                            }
                            else
                            {
                                gridPartView.SetFocusedRowCellValue("Ctn", (obj.Ctn - p));
                            }
                            //update issuepack
                            string plno = ReloadPlSet(obj, p);
                            int seq = 0;
                            while (seq < p)
                            {
                                InsertPalletToPackingDetail(obj, plno);
                                seq++;
                            }
                        }
                    }
                }
                else
                {
                    XtraMessageBox.Show("ไม่สามารถดำเนินการต่อได้\nเนื่องจากจำนวนเป็น 0", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
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
                List<SetPallatData> list = gridPartControl.DataSource as List<SetPallatData>;
                list.ForEach(i => {
                    if (i.MgrPl)
                    {
                        c = true;
                        x += i.Ctn;
                    }
                });
                if (c)
                {
                    bbiSetPallet.Caption = $"Set Pallet({x})";
                    popupMenu1.ShowPopup(new Point(MousePosition.X, MousePosition.Y));
                }
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
                if (r.MgrPl)
                {
                    x += r.Ctn;
                    obj = r;
                    n.Add(r);
                }
                else
                {
                    ob.Add(r);
                }
            }

            if (obj != null)
            {
                string plno  = ReloadPlSet(obj, x);
                int ix = 1;
                ob.ForEach(i => {
                    i.Id = ix;
                    ix++;
                });

                foreach (var j in n)
                {
                    InsertPalletToPackingDetailAll(j, plno);
                }
                gridPartControl.DataSource = ob;
            }
        }

        private void InsertPalletToPackingDetailAll(SetPallatData obj, string plno)
        {
            string sql = $"UPDATE TXP_ISSPACKDETAIL SET SHIPPLNO = '{plno}'\n" +
                        $"WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{obj.RefNo}' AND PONO = '{obj.OrderNo}' AND PARTNO = '{obj.PartNo}'";
            new ConnDB().ExcuteSQL(sql);
        }
    }
}