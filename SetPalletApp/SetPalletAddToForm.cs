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
using XPWLibrary.Controllers;

namespace SetPalletApp
{
    public partial class SetPalletAddToForm : DevExpress.XtraEditors.XtraForm
    {
        SetPallatData ob = new SetPallatData();
        public SetPalletAddToForm(SetPallatData obj)
        {
            InitializeComponent();
            ob = obj;
            List<SetPallatData> list = new SetPalletControllers().GetPartListCompletedDetail(ob.RefNo);
            list.ForEach(i => {
                cbPalletObj.Properties.Items.Add(i.ShipPlNo);
            });
        }

        private void bbiSave_Click(object sender, EventArgs e)
        {
            try
            {
                string plno = cbPalletObj.EditValue.ToString();
                if (new SelPlControllers().InsertPalletToPackingDetailAll(ob, plno))
                {
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                XtraMessageBox.Show(ex.Message);
            }
        }

        private void bbiCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}