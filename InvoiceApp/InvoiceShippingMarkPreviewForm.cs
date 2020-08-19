using DevExpress.XtraSplashScreen;
using InvoiceApp.Reportings;

namespace InvoiceApp
{
    public partial class InvoiceShippingMarkPreviewForm : DevExpress.XtraEditors.XtraForm
    {
        public InvoiceShippingMarkPreviewForm(string inv, string plout)
        {
            InitializeComponent();
            SplashScreenManager.ShowDefaultWaitForm();
            ShiipingMarkReporting rp = new ShiipingMarkReporting();
            foreach (DevExpress.XtraReports.Parameters.Parameter i in rp.Parameters)
            {
                i.Visible = false;
            }
            rp.initData(inv, plout);
            rp.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //rp.Margins.Left = 1;
            //rp.Margins.Right = 3;
            //rp.Margins.Top = 10;
            //rp.Margins.Bottom = 10;
            documentViewer1.DocumentSource = rp;
            rp.CreateDocument();
            SplashScreenManager.CloseDefaultWaitForm();
        }
    }
}