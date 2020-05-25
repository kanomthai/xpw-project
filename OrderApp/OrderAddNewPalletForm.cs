using DevExpress.XtraEditors;
using System;
using System.Data;
using System.Windows.Forms;
using XPWLibrary.Interfaces;

namespace OrderApp
{
    public partial class OrderAddNewPalletForm : DevExpress.XtraEditors.XtraForm
    {
        string refinvno;
        public OrderAddNewPalletForm(string refinv)
        {
            InitializeComponent();
            refinvno = refinv;
            ReloadData();
        }

        void ReloadData()
        {
            bbiPlNo.EditValue = "";
            bbiPlSize.EditValue = "WxLxH";
            bbiPlTotal.EditValue = "0";
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string plpref = "C";
            if (bbiPallet.Checked)
            {
                plpref = "P";
            }
            string p = bbiPlNo.EditValue.ToString();
            if (p == "")
            {
                XtraMessageBox.Show("กรุณาระบุเลขที่ PALLET ด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                bbiPlNo.Focus();
            }
            else
            {
                Console.WriteLine(p.IndexOf("-"));
                if (p.IndexOf("-") >= 0)
                {
                    string a = p.Substring(0, p.IndexOf("-"));
                    string b = p.Substring(a.Length + 1, p.Length - (a.Length + 1));
                    Console.WriteLine($"START: {a} TO {b}");
                    int i = 0;
                    int x = int.Parse(a);
                    while (i <= (int.Parse(b) - int.Parse(a)))
                    {
                        string plno = $"1{plpref}{x.ToString("D3")}";
                        Console.WriteLine(plno);
                        string sql = $"SELECT count(*) FROM TXP_ISSPALLET l WHERE l.PALLETNO = '{plno}' AND l.ISSUINGKEY = '{refinvno}'";
                        DataSet dr = new ConnDB().GetFill(sql);
                        if (dr.Tables[0].Rows.Count > 0)
                        {
                            string ins = $"INSERT INTO TXP_ISSPALLET(FACTORY, ISSUINGKEY, PALLETNO, PLTYPE, PLOUTSTS, UPDDTE, SYSDTE, PLTOTAL, BOOKED)\n" +
                        $"VALUES('{StaticFunctionData.Factory}', '{refinvno}', '{plno}', '{bbiPlSize.EditValue.ToString()}', 0, current_timestamp, current_timestamp, {bbiPlTotal.EditValue.ToString()}, 0)";
                            if (new ConnDB().ExcuteSQL(ins))
                            {
                                x++;
                                i++;
                            }
                        }
                    }
                }
                else
                {
                    try
                    {
                        string plno = $"1{plpref}{int.Parse(p).ToString("D3")}";
                        Console.WriteLine(plno);
                        string ins = $"INSERT INTO TXP_ISSPALLET(FACTORY, ISSUINGKEY, PALLETNO, PLTYPE, PLOUTSTS, UPDDTE, SYSDTE, PLTOTAL, BOOKED)\n" +
                        $"VALUES('{StaticFunctionData.Factory}', '{refinvno}', '{plno}', '{bbiPlSize.EditValue.ToString()}', 0, current_timestamp, current_timestamp, {bbiPlTotal.EditValue.ToString()}, 0)";
                        string sql = $"SELECT count(*) FROM TXP_ISSPALLET l WHERE l.PALLETNO = '{plno}' AND l.ISSUINGKEY = '{refinvno}'";
                        DataSet dr = new ConnDB().GetFill(sql);
                        if (dr.Tables[0].Rows.Count > 0)
                        {
                            new ConnDB().ExcuteSQL(ins);
                        }
                    }
                    catch (Exception)
                    {
                        XtraMessageBox.Show("กรุณาระบุเลขที่ PALLET ให้ถูกต้องด้วย", "XPW Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        bbiPlNo.EditValue = "";
                        bbiPlNo.Focus();
                    }
                }
            }
            this.Close();
        }
    }
}