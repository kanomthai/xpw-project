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
using XPWLibrary.Models;
using XPWLibrary.Interfaces;

namespace OrderApp
{
    public partial class OrderEditCustomerForm : DevExpress.XtraEditors.XtraForm
    {
        InvoiceBodyData ob;
        public OrderEditCustomerForm(List<InvoiceBodyData> obj)
        {
            InitializeComponent();
            string potype = "ALL";
            ob = obj[0];
            switch (obj[0].Combinv)
            {
                case "E":
                    potype = "3 END";
                    break;
                case "F":
                    potype = "3 FRIST";
                    break;
                default:
                    break;
            }
            bbiPoType.EditValue = potype;
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            string potype = "N";
            switch (bbiPoType.EditValue.ToString())
            {
                case "3 END":
                    potype = "E";
                    break;
                case "3 FRIST":
                    potype = "F";
                    break;
                default:
                    break;
            }
            string sql = $"update txm_customer set combinv='{potype}' where factory='{ob.Factory}' and affcode='{ob.Affcode}' and bishpc='{ob.Bishpc}' and custnm= '{ob.Custname}'";
            if (new ConnDB().ExcuteSQL(sql))
            {
                XtraMessageBox.Show("บันทึกข้อมูลเสร็จแล้ว");
            }
        }
    }
}