using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using XPWLibrary.Controllers;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace SetPalletApp
{
    public partial class SetPalletNewInvoiceForm : DevExpress.XtraEditors.XtraForm
    {
        List<SetPallatData> ob = new List<SetPallatData>();
        string invno = null;
        string olderno = null;
        public SetPalletNewInvoiceForm(List<SetPallatData> obj)
        {
            InitializeComponent();
            ob = obj;
            var r = obj[0];
            olderno = r.RefNo;
            bbiEtdDate.EditValue = r.EtdDte;
            invno = new OrderControllers().GetRefInv(r.RefNo.Substring(1, 2), r.Factory, r.EtdDte);
            bbiShipType.EditValue = r.ShipType;
            bbiInvoiceNo.EditValue = invno;// new GreeterFunction().GetLastInvoice(r.RefNo);
            this.Text = $"REFNO: {invno}";
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            DialogResult r = XtraMessageBox.Show($"คุณต้องการที่จะสร้าง {invno} ใช่หรือไม่?", "ข้อความ", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (r == DialogResult.Yes)
            {
                SplashScreenManager.ShowDefaultWaitForm();
                List<SetPallatData> obj = new List<SetPallatData>();
                int i = 0;
                while (i < ob.Count)
                {
                    var b = ob[i];
                    b.Prefix = new OrderControllers().GetPrefix(b.RefNo);
                    b.RefInv = bbiInvoiceNo.EditValue.ToString();
                    b.RefNo = invno;
                    b.RefOldNo = olderno;
                    b.EtdDte = DateTime.Parse(bbiEtdDate.EditValue.ToString());
                    b.ShipType = bbiShipType.EditValue.ToString();
                    obj.Add(b);
                    i++;
                }
                if (new OrderControllers().CreateNewInvoice(obj))
                {
                    SplashScreenManager.CloseDefaultWaitForm();
                    r = XtraMessageBox.Show($"สร้างข้อมูล {invno} เรียบร้อยแล้ว", "ข้อความ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    if (r == DialogResult.OK)
                    {
                        this.Close();
                    }
                }
                else
                {
                    SplashScreenManager.CloseDefaultWaitForm();
                }
            }
        }

        private void bbiEtdDate_EditValueChanged(object sender, EventArgs e)
        {
            var r = ob[0];
            invno = new OrderControllers().GetRefInv(r.RefNo.Substring(1, 2), r.Factory, DateTime.Parse(bbiEtdDate.EditValue.ToString()));
            this.Text = $"REFNO: {invno}";
        }
    }
}