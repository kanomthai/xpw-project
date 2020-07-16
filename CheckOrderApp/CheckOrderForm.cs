using DevExpress.XtraBars;
using System;
using System.Collections.Generic;
using XPWLibrary.Controllers;
using XPWLibrary.Models;

namespace CheckOrderApp
{
    public partial class CheckOrderForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public CheckOrderForm()
        {
            InitializeComponent();
            //gridControl.DataSource = GetDataSource();
            //BindingList<Customer> dataSource = GetDataSource();
            //gridControl.DataSource = dataSource;
            //bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }

        private void bbiEtd_EditValueChanged(object sender, EventArgs e)
        {
            ReloadData();
        }

        void ReloadData()
        {
            DateTime etd = DateTime.Parse(bbiEtd.EditValue.ToString());
            List<OrderCheckData> list = new OrderCheckControllers().GetOrderList(etd);
            gridControl.DataSource = list;
            bsiRecordsCount.Caption = "RECORDS : " + list.Count;
        }
    }
}