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

namespace BookingApp
{
    public partial class BookingPreviewForm : DevExpress.XtraEditors.XtraForm
    {
        public BookingPreviewForm(string containerno)
        {
            InitializeComponent();
            this.Text = $"{containerno} BOOKING PREVIEW";
            BookingReport rp = new BookingReport();
            foreach (DevExpress.XtraReports.Parameters.Parameter i in rp.Parameters)
            {
                i.Visible = false;
            }
            rp.initData(containerno);
            rp.PaperKind = System.Drawing.Printing.PaperKind.A4;
            rp.Margins.Left = 1;
            rp.Margins.Right = 3;
            rp.Margins.Top = 10;
            rp.Margins.Bottom = 10;
            documentViewer1.DocumentSource = rp;
            rp.CreateDocument();
        }
    }
}