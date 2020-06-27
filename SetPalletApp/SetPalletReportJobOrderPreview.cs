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
            SetPalletReporting rp = new SetPalletReporting();
            foreach (DevExpress.XtraReports.Parameters.Parameter i in rp.Parameters)
            {
                i.Visible = false;
            }
            rp.InitData(refinv);
            documentViewer1.DocumentSource = rp;
            rp.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //rp.Margins.Left = 3;
            rp.Margins.Right = 1;
            rp.Margins.Top = 7;
            //rp.Margins.Bottom = 10;
            rp.CreateDocument();
        }
    }
}