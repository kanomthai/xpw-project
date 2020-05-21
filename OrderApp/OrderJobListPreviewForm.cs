﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using OrderApp.Reportings;

namespace OrderApp
{
    public partial class OrderJobListPreviewForm : DevExpress.XtraEditors.XtraForm
    {
        public OrderJobListPreviewForm(string refinv)
        {
            InitializeComponent();
            SplashScreenManager.ShowDefaultWaitForm();
            this.Text = $"Print JobList {refinv}";
            XtraJobListReport rp = new XtraJobListReport();
            foreach (DevExpress.XtraReports.Parameters.Parameter i in rp.Parameters)
            {
                i.Visible = false;
            }
            rp.InitData(refinv);
            jbDoc.DocumentSource = rp;
            rp.PaperKind = System.Drawing.Printing.PaperKind.A4;
            //rp.Margins.Left = 3;
            rp.Margins.Right = 1;
            rp.Margins.Top = 7;
            //rp.Margins.Bottom = 10;
            rp.CreateDocument();
        }
    }
}