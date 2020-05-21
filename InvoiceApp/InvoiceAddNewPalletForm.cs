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
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace InvoiceApp
{
    public partial class InvoiceAddNewPalletForm : DevExpress.XtraEditors.XtraForm
    {
        string invno;
        public InvoiceAddNewPalletForm(string refinv)
        {
            InitializeComponent();
            invno = refinv;
            this.Text = $"ADD PALLET";
            List<INJPlData> obj = new GreeterFunction().GetPalletData(invno);
            foreach (var r in obj)
            {
                cbPlSize.Properties.Items.Add(r.PSize);
            }
        }

        private void sbPallet_Click(object sender, EventArgs e)
        {
            var a = bbiPlNo.EditValue.ToString();
            var sstr = "";
            var estr = "";
            if (a.IndexOf("-") >= 0)
            {
                sstr = $"1P{int.Parse(a.Substring(0, a.IndexOf("-"))).ToString("D3")}";
                Console.WriteLine(a.Substring(a.IndexOf("-"), a.Length - 2));
                estr = $"1P{int.Parse((a.Substring(a.IndexOf("-"), a.Length - sstr.Length)).Replace("-", "")).ToString("D3")}";
            }
            else
            {
                sstr = $"1P{int.Parse(a.Replace(" ", "")).ToString("D3")}";
            }
            Console.WriteLine(a);
            Console.WriteLine(sstr);
            Console.WriteLine(estr);
        }
    }
}