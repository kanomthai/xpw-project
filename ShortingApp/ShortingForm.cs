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
using DevExpress.XtraBars;
using System.ComponentModel.DataAnnotations;
using XPWLibrary.Models;

namespace ShortingApp
{
    public partial class ShortingForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        OrderData obj = new OrderData();
        public ShortingForm(OrderData ob)
        {
            InitializeComponent();
            obj = ob;
            if (obj.Custname == null) {
                Console.WriteLine(obj);
            }
            //gridControl.DataSource = GetDataSource();
            //BindingList<Customer> dataSource = GetDataSource();
            //gridControl.DataSource = dataSource;
            //bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            //gridControl.ShowRibbonPrintPreview();
        }
    }
}