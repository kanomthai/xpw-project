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
using DevExpress.XtraBars;
using System.ComponentModel.DataAnnotations;
using XPWLibrary.Models;
using XPWLibrary.Interfaces;

namespace InvoiceApp
{
    public partial class InvoicePalletDetailForm : DevExpress.XtraBars.Ribbon.RibbonForm
    {
        string plno = "";
        public InvoicePalletDetailForm(string pl)
        {
            InitializeComponent();
            plno = pl;
            this.Text = $"{plno} DETAIL";
            ReBuildingPallet();
        }

        void ReloadData()
        {
            BindingList<PlDetailData> dataSource = GetDataSource(plno);
            gridControl.DataSource = dataSource;
            gridView.ClearSelection();
            bsiRecordsCount.Caption = "RECORDS : " + dataSource.Count;
        }

        void bbiPrintPreview_ItemClick(object sender, ItemClickEventArgs e)
        {
            gridControl.ShowRibbonPrintPreview();
        }
        public BindingList<PlDetailData> GetDataSource(string plno)
        {
            BindingList<PlDetailData> list = new BindingList<PlDetailData>();
            string sql = $"SELECT c.PONO,c.PARTNO,p.PARTNAME,c.LOTNO,c.RUNNINGNO,c.RVMANAGINGNO,c.SHELVE,l.ISSUINGKEY FROM TXP_CARTONDETAILS c \n" +
                $"INNER JOIN TXP_PART p ON c.PARTNO = p.PARTNO\n" +
                "LEFT JOIN TXP_ISSPALLET l ON c.PLOUTNO = l.PLOUTNO\n" +
                $"WHERE c.PLOUTNO = '{plno}' \n" +
                "GROUP BY c.PONO,c.PARTNO,p.PARTNAME,c.LOTNO,c.RUNNINGNO,c.RVMANAGINGNO,c.SHELVE,l.ISSUINGKEY\n" +
                $"ORDER BY c.PONO,p.PARTNAME,c.LOTNO,c.RUNNINGNO";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new PlDetailData()
                {
                    Id = list.Count + 1,
                    OrderNo = r["pono"].ToString(),
                    PartNo = r["partno"].ToString(),
                    PartName = r["partname"].ToString(),
                    LotNo = r["lotno"].ToString(),
                    SerialNo = r["runningno"].ToString(),
                    Shelve = r["shelve"].ToString(),
                    RefInv = r["shelve"].ToString(),
                });
            }
            return list;
        }

        void ReBuildingPallet()
        {
            ReloadData();
            //if (invno.Substring(0, 1) == "I")
            //{
            //    if (new GreeterFunction().SumPlInj(invno))
            //    {
            //        ReloadData();
            //    }
            //}
            //else
            //{
            //    if (new GreeterFunction().SumPallet(invno))
            //    {
            //        ReloadData();
            //    }
            //}
        }
    }
}