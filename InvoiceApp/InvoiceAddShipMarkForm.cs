using System;

namespace InvoiceApp
{
    public partial class InvoiceAddShipMarkForm : DevExpress.XtraEditors.XtraForm
    {
        string inv = null;
        public InvoiceAddShipMarkForm(string invoiceno)
        {
            InitializeComponent();
            this.Text = $"PRINT SHIPMARK {invoiceno}";
            inv = invoiceno;
        }

        private void bbiPrintShipMark_Click(object sender, EventArgs e)
        {
            InvoiceShippingMarkPreviewForm frm = new InvoiceShippingMarkPreviewForm(inv, null);
            frm.ShowDialog();
        }
    }
}