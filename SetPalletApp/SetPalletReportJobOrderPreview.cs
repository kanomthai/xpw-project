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

namespace SetPalletApp
{
    public partial class SetPalletReportJobOrderPreview : DevExpress.XtraEditors.XtraForm
    {
        public SetPalletReportJobOrderPreview(string refinv)
        {
            InitializeComponent();
            this.Text = $"{refinv} REPORT SUMMARY";
        }
    }
}