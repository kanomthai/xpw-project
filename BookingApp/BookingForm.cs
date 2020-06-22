using DevExpress.XtraBars;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BookingApp
{
    public partial class BookingForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        public BookingForm()
        {
            InitializeComponent();
            gridControl.DataSource = GetDataSource();
            BindingList<Customer> dataSource = GetDataSource();
            gridControl.DataSource = dataSource;
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }
        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }
        public BindingList<Customer> GetDataSource()
        {
            BindingList<Customer> result = new BindingList<Customer>();
            result.Add(new Customer()
            {
                ID = 1,
                Name = "ACME",
                Address = "2525 E El Segundo Blvd",
                City = "El Segundo",
                State = "CA",
                ZipCode = "90245",
                Phone = "(310) 536-0611"
            });
            result.Add(new Customer()
            {
                ID = 2,
                Name = "Electronics Depot",
                Address = "2455 Paces Ferry Road NW",
                City = "Atlanta",
                State = "GA",
                ZipCode = "30339",
                Phone = "(800) 595-3232"
            });
            return result;
        }
        public class Customer
        {
            [Key, Display(AutoGenerateField = false)]
            public int ID { get; set; }
            [Required]
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public string State { get; set; }
            [Display(Name = "Zip Code")]
            public string ZipCode { get; set; }
            public string Phone { get; set; }
        }

        private void bbiNew_ItemClick(object sender, ItemClickEventArgs e)
        {
            DateTime etd = DateTime.Now;
            string conno = null;
            BookingAddForm frm = new BookingAddForm(etd, conno);
            frm.ShowDialog();
        }
    }
}