using DevExpress.XtraSplashScreen;
using NiceLabel.SDK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace XPWLibrary.Controllers
{
    public class InvoiceControllers
    {
        public List<InvoiceData> GetInvoiceData(DateTime etd, string zname)
        {
            string etddate = $"t.ETDDTE = to_date('{etd.ToString("dd/MM/yyyy")}', 'dd/MM/yyyy')";
            //if (StaticFunctionData.AllWeek)
            //{
            //    etddate = $"t.ETDDTE BETWEEN (TRUNC(to_date('{etd.ToString("ddMMyyyy")}', 'ddMMyyyy'), 'DY') + 0) AND (TRUNC(to_date('{etd.ToString("ddMMyyyy")}', 'ddMMyyyy'), 'DY') + 7)";
            //}
            string sql = $"SELECT * FROM TBT_ISSUELIST t WHERE t.ZNAME = '{zname}' AND t.FACTORY = '{StaticFunctionData.Factory}' AND {etddate}";
            if (zname == "AIR" || zname == "TRUCK")
            {
                sql = $"SELECT * FROM TBT_ISSUELIST t WHERE t.SHIPTYPE = '{zname.Substring(1, 1)}' AND t.FACTORY = '{StaticFunctionData.Factory}' AND {etddate}";
            }
            Console.WriteLine(sql);
            List<InvoiceData> list = new List<InvoiceData>();
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new InvoiceData()
                {
                    Id = list.Count + 1,
                    Factory = r["factory"].ToString(),
                    Zname = r["zname"].ToString(),
                    Etddte = DateTime.Parse(r["etddte"].ToString()),
                    Affcode = r["affcode"].ToString(),
                    Bishpc = r["bishpc"].ToString(),
                    Custname = r["custname"].ToString(),
                    Ship = r["shiptype"].ToString(),
                    RefInv = r["issuingkey"].ToString(),
                    Invoice = r["refinvoice"].ToString(),
                    Zoneid = int.Parse(r["zoneid"].ToString()),
                    Ord = r["ord"].ToString(),
                    Potype = r["potype"].ToString(),
                    Itm = int.Parse(r["itm"].ToString()),
                    Ctn = int.Parse(r["ctn"].ToString()),
                    Issue = int.Parse(r["issue"].ToString()),
                    RmCtn = int.Parse(r["ctn"].ToString()) - int.Parse(r["issue"].ToString()),
                    Pl = int.Parse(r["pl"].ToString()),
                    Plno = int.Parse(r["plno"].ToString()),
                    RmCon = int.Parse(r["pl"].ToString()) - int.Parse(r["plno"].ToString()),
                    ShCtn = int.Parse(r["shctn"].ToString()),
                    Conn = int.Parse(r["conn"].ToString()),
                    Status = int.Parse(r["status"].ToString()),
                    Upddte = DateTime.Parse(r["upddte"].ToString()),
                });
            }
            return list;
        }
        public List<InvoiceData> GetInvoiceData(DateTime etd)
        {
            string etddate = $"t.ETDDTE = to_date('{etd.ToString("dd/MM/yyyy")}', 'dd/MM/yyyy')";
            if (StaticFunctionData.AllWeek)
            {
                etddate = $"t.ETDDTE BETWEEN (TRUNC(to_date('{etd.ToString("ddMMyyyy")}', 'ddMMyyyy'), 'DY') + 0) AND (TRUNC(to_date('{etd.ToString("ddMMyyyy")}', 'ddMMyyyy'), 'DY') + 7)";
            }
            string sql = $"SELECT * FROM TBT_ISSUELIST t WHERE t.FACTORY = '{StaticFunctionData.Factory}' AND {etddate}";
            Console.WriteLine(sql);
            List<InvoiceData> list = new List<InvoiceData>();
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new InvoiceData()
                { 
                    Id = list.Count + 1,
                    Factory = r["factory"].ToString(),
                    Zname = r["zname"].ToString(),
                    Etddte = DateTime.Parse(r["etddte"].ToString()),
                    Affcode = r["affcode"].ToString(),
                    Bishpc = r["bishpc"].ToString(),
                    Custname = r["custname"].ToString(),
                    Ship = r["shiptype"].ToString(),
                    RefInv = r["issuingkey"].ToString(),
                    Invoice = r["refinvoice"].ToString(),
                    Zoneid = int.Parse(r["zoneid"].ToString()),
                    Ord = r["ord"].ToString(),
                    Potype = r["potype"].ToString(),
                    Itm = int.Parse(r["itm"].ToString()),
                    Ctn = int.Parse(r["ctn"].ToString()),
                    Issue = int.Parse(r["issue"].ToString()),
                    RmCtn = int.Parse(r["ctn"].ToString()) - int.Parse(r["issue"].ToString()),
                    Pl = int.Parse(r["pl"].ToString()),
                    Plno = int.Parse(r["plno"].ToString()),
                    RmCon = int.Parse(r["pl"].ToString()) - int.Parse(r["plno"].ToString()),
                    ShCtn = int.Parse(r["shctn"].ToString()),
                    Conn = int.Parse(r["conn"].ToString()),
                    Status = int.Parse(r["status"].ToString()),
                    Upddte = DateTime.Parse(r["upddte"].ToString()),
                });
            }
            return list;
        }

        public object GetInvoiceWeek(DateTime d, int bweek, int eweek)
        {
            string sql = "SELECT to_char(pp.ETD, 'DD/MM/YYYY')ETD,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '1' AND PP.shiptype = 'B'  THEN 1 else 0 END) CK2,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '2'                         THEN 1 else 0 END) NESS,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '3'                         THEN 1 else 0 END) ICAM,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '4'                         THEN 1 else 0 END) CK1,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '1' AND PP.shiptype = 'T'  THEN 1 else 0 END) TRUCK,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '1' AND PP.shiptype = 'A'  THEN 1 else 0 END) AIR\n" +
                        "FROM(\n" +
                        "       select  e.etddte ETD, to_number(to_char(e.etddte, 'WW'))  WEEK, e.FACTORY, e.affcode, e.shiptype, e.zoneid\n" +
                        "        from txp_isstransent e\n" +
                        $"       WHERE  e.FACTORY = '{StaticFunctionData.Factory.ToUpper()}' and " +
                        $"e.etddte between next_day(to_date('{d.ToString("dd-MM-yyyy")}','DD-MM-YYYY') + {bweek}, 'SUNDAY') and " +
                        $"next_day(to_date('{d.ToString("dd-MM-yyyy")}','DD-MM-YYYY') + {eweek}, 'SUNDAY')\n" +
                        "    )  PP\n" +
                        "GROUP BY pp.ETD\n" +
                        "order by PP.ETD";
            if (StaticFunctionData.Factory.ToUpper() != "AW")
            {
                sql = ("SELECT to_char(pp.ETD, 'DD/MM/YYYY')ETD,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '4' AND PP.shiptype = 'B'  THEN 1 else 0 END) CK2,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '2'                         THEN 1 else 0 END) NESS,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '3'                         THEN 1 else 0 END) ICAM,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '1'                         THEN 1 else 0 END) CK1,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '4' AND PP.shiptype = 'T'  THEN 1 else 0 END) TRUCK,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '4' AND PP.shiptype = 'A'  THEN 1 else 0 END) AIR\n" +
                       "FROM(\n" +
                        "       select  e.etddte ETD, to_number(to_char(e.etddte, 'WW'))  WEEK, e.FACTORY, e.affcode, e.shiptype, e.zoneid\n" +
                        "        from txp_isstransent e\n" +
                        $"       WHERE  e.FACTORY = '{StaticFunctionData.Factory.ToUpper()}' and " +
                        $"e.etddte between next_day(to_date('{d.ToString("dd-MM-yyyy")}','DD-MM-YYYY') + {bweek}, 'SUNDAY') and " +
                        $"next_day(to_date('{d.ToString("dd-MM-yyyy")}','DD-MM-YYYY') + {eweek}, 'SUNDAY')\n" +
                        "    )  PP\n" +
                        "GROUP BY pp.ETD\n" +
                        "order by PP.ETD");
            }
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            BindingList<InvoiceMasterData> list = new BindingList<InvoiceMasterData>();
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new InvoiceMasterData()
                {
                    Id = list.Count + 1,
                    Etd = DateTime.Parse(r["etd"].ToString()),
                    Ck2 = int.Parse(r["ck2"].ToString()),
                    Ness = int.Parse(r["ness"].ToString()),
                    Icam = int.Parse(r["icam"].ToString()),
                    Ck1 = int.Parse(r["ck1"].ToString()),
                    Truck = int.Parse(r["truck"].ToString()),
                    Air = int.Parse(r["air"].ToString()),
                });
            }
            return list;
        }

        public BindingList<InvoiceMasterData> GetInvoiceToWeek(DateTime d)
        {
            string sql = "SELECT to_char(pp.ETD, 'DD/MM/YYYY')ETD,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '1' AND PP.shiptype = 'B'  THEN 1 else 0 END) CK2,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '2'                         THEN 1 else 0 END) NESS,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '3'                         THEN 1 else 0 END) ICAM,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '4'                         THEN 1 else 0 END) CK1,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '1' AND PP.shiptype = 'T'  THEN 1 else 0 END) TRUCK,\n" +
                        "         SUM(CASE WHEN PP.zoneid = '1' AND PP.shiptype = 'A'  THEN 1 else 0 END) AIR\n" +
                        "FROM(\n" +
                        "       select  e.etddte ETD, to_number(to_char(e.etddte, 'WW'))  WEEK, e.FACTORY, e.affcode, e.shiptype, e.zoneid\n" +
                        "        from txp_isstransent e\n" +
                        $"       WHERE  e.FACTORY = '{StaticFunctionData.Factory.ToUpper()}' and " +
                        $"e.etddte between next_day(to_date('{d.ToString("dd-MM-yyyy")}','DD-MM-YYYY') - 7, 'SUNDAY') and " +
                        $"next_day(to_date('{d.ToString("dd-MM-yyyy")}','DD-MM-YYYY'), 'SUNDAY')\n" +
                        "    )  PP\n" +
                        "GROUP BY pp.ETD\n" +
                        "order by PP.ETD";
            if (StaticFunctionData.Factory.ToUpper() != "AW")
            {
                sql = ("SELECT    pp.ETD,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '4' AND PP.shiptype = 'B'  THEN 1 else 0 END) CK2,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '2'                         THEN 1 else 0 END) NESS,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '3'                         THEN 1 else 0 END) ICAM,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '1'                         THEN 1 else 0 END) CK1,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '4' AND PP.shiptype = 'T'  THEN 1 else 0 END) TRUCK,\n" +
                       "          SUM(CASE WHEN PP.zoneid = '4' AND PP.shiptype = 'A'  THEN 1 else 0 END) AIR\n" +
                       "FROM(\n" +
                        "       select  e.etddte ETD, to_number(to_char(e.etddte, 'WW'))  WEEK, e.FACTORY, e.affcode, e.shiptype, e.zoneid\n" +
                        "        from txp_isstransent e\n" +
                        $"       WHERE  e.FACTORY = '{StaticFunctionData.Factory.ToUpper()}' and " +
                        $"e.etddte between next_day(to_date('{d.ToString("dd-MM-yyyy")}','DD-MM-YYYY') - 7, 'SUNDAY') and " +
                        $"next_day(to_date('{d.ToString("dd-MM-yyyy")}','DD-MM-YYYY'), 'SUNDAY')\n" +
                        "    )  PP\n" +
                        "GROUP BY pp.ETD\n" +
                        "order by PP.ETD");
            }
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            BindingList<InvoiceMasterData> list = new BindingList<InvoiceMasterData>();
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                Console.WriteLine(r["etd"].ToString());
                list.Add(new InvoiceMasterData()
                {
                    Id = list.Count + 1,
                    Etd = DateTime.Parse(r["etd"].ToString()),
                    Ck2 = int.Parse(r["ck2"].ToString()),
                    Ness = int.Parse(r["ness"].ToString()),
                    Icam = int.Parse(r["icam"].ToString()),
                    Ck1 = int.Parse(r["ck1"].ToString()),
                    Truck = int.Parse(r["truck"].ToString()),
                    Air = int.Parse(r["air"].ToString()),
                });
            }
            return list;
        }

        public bool CheckInvoiceStatus(string refInv)
        {
            bool x = false;
            string sql = $"SELECT count(*) ctn FROM txp_isstransent e WHERE e.ISSUINGKEY = '{refInv}' AND LENGTH(e.REFINVOICE) = 10";
            DataSet dr = new ConnDB().GetFill(sql);
            if ((dr.Tables[0].Rows[0][0]).ToString() != "0")
            {
                x = true;
            }
            return x;
        }

        public List<InvoiceBodyData> GetInvoiceBody(InvoiceData ob)
        {
            List<InvoiceBodyData> list = new List<InvoiceBodyData>();
            string sql = $"SELECT * FROM TBT_ISSUEDETAIL where issuingkey = '{ob.RefInv}' ORDER BY PONO,KIDS,SIZES,LOTNO,SEQ ,CTN";
            if (StaticFunctionData.Factory == "INJ")
            {
                sql = $"SELECT * FROM TBT_ISSUEDETAIL where issuingkey = '{ob.RefInv}' ORDER BY PARTNO,PONO,CTN";
            }
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            int i = 0;
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                int seq = 0;
                try
                {
                    seq = int.Parse(r["seq"].ToString());
                }
                catch (Exception)
                {
                }
                i += int.Parse(r["ctn"].ToString());
                int sctn = (i - int.Parse(r["ctn"].ToString()));
                int ectn = sctn + int.Parse(r["ctn"].ToString());
                Console.WriteLine($"{r["partno"].ToString()} CTN => {int.Parse(r["ctn"].ToString())} => START {sctn} TO {ectn}");
                list.Add(new InvoiceBodyData()
                {
                    Id = list.Count + 1,
                    Factory = ob.Factory,// = r["factory"].ToString(),
                    Zname = ob.Zname,// = r["zname"].ToString(),
                    Etddte = ob.Etddte,// = DateTime.Parse(r["etddte"].ToString()),
                    Affcode = ob.Affcode,// = r["affcode"].ToString(),
                    Bishpc = ob.Bishpc,// = r["bishpc"].ToString(),
                    Custname = ob.Custname,// = r["custname"].ToString(),
                    Ship = ob.Ship,// = r["shiptype"].ToString(),
                    RefInv = ob.RefInv,// = r["issuingkey"].ToString(),
                    Invoice = ob.Invoice,// = r["refinvoice"].ToString(),
                    Zoneid = ob.Zoneid,// = int.Parse(r["zoneid"].ToString()),
                    Ord = ob.Ord,// = r["ord"].ToString(),
                    Potype = ob.Potype,// = r["potype"].ToString(),
                    Itm = ob.Itm,// = int.Parse(r["itm"].ToString()),
                    Ctn = ob.Ctn,// = int.Parse(r["ctn"].ToString()),
                    Issue = ob.Issue,// = int.Parse(r["issue"].ToString()),
                    RmCtn = ob.RmCtn,// = int.Parse(r["ctn"].ToString()) - int.Parse(r["issue"].ToString()),
                    Pl = ob.Pl,// = int.Parse(r["pl"].ToString()),
                    Plno = ob.Plno,// = int.Parse(r["plno"].ToString()),
                    RmCon = ob.RmCon,// = int.Parse(r["pl"].ToString()) - int.Parse(r["plno"].ToString()),
                    Conn = ob.Conn,// = int.Parse(r["conn"].ToString()),
                    Status = int.Parse(r["status"].ToString()),
                    Upddte = ob.Upddte,
                    OrderNo = r["pono"].ToString(),
                    PartNo = r["partno"].ToString(),
                    PartName = r["partname"].ToString(),
                    LotNo = r["lotno"].ToString(),
                    LotSeq = seq,
                    BalQty = int.Parse(r["orderqty"].ToString()),
                    BalCtn = int.Parse(r["ctn"].ToString()),
                    ShCtn = int.Parse(r["shctn"].ToString()),
                    PartRmCtn = int.Parse(r["rm"].ToString()),
                    StartFticket = sctn
                });
            }
            return list;
        }

        public List<PalletData> GetPalletDetail(string refinv)
        {
            string sql = $"SELECT l.PALLETNO,l.PLOUTNO,l.PLTYPE,l.CONTAINERNO,l.PLTOTAL,CASE WHEN cc.ctn IS NULL THEN 0 ELSE cc.ctn END total,case when l.PLOUTSTS is null then '0' else l.PLOUTSTS end PLOUTSTS FROM TXP_ISSPALLET l\n" +
                        "LEFT JOIN(SELECT c.PLOUTNO, count(c.PLOUTNO) ctn FROM TXP_CARTONDETAILS c GROUP BY c.PLOUTNO) cc ON l.PLOUTNO = cc.PLOUTNO\n" +
                        $"WHERE l.ISSUINGKEY = '{refinv}'";
            List<PalletData> obj = new List<PalletData>();
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new PalletData()
                {
                    PlNo = r["palletno"].ToString(),
                    PlOut = r["ploutno"].ToString(),
                    PlType = r["pltype"].ToString(),
                    ContainerNo = r["containerno"].ToString(),
                    PlSize = int.Parse(r["pltotal"].ToString()),
                    PlTotal = int.Parse(r["total"].ToString()),
                    PlStatus = int.Parse(r["ploutsts"].ToString()),
                });
            }
            return obj;
        }

        public bool PrintFTicket(string RefInv, string Partno, string pono, int seq, string snum)
        {
            string prname = StaticFunctionData.cartonticketprinter;
            string labeltemp = $"{AppDomain.CurrentDomain.BaseDirectory}Labels\\CK2_1DCTN.nlbl";
            if (RefInv.Substring(0, 1) != "A")
            {
                SplashScreenManager.Default.SetWaitFormCaption("Printing Job Card");
                labeltemp = $"{AppDomain.CurrentDomain.BaseDirectory}Labels\\CK2_1DCTN.nlbl";
                prname = StaticFunctionData.fticketprinter;
                IPrintEngine NL_PrintEngine = PrintEngineFactory.PrintEngine;
                NL_PrintEngine.Initialize();
                ILabel NL_Label = NL_PrintEngine.OpenLabel(labeltemp);
                DataSet dr = this.GetFTicker(RefInv, Partno, pono);
                var prn = NL_PrintEngine.Printers.ToList().Find(i => i.Name == prname);
                if (prn != null)
                {
                    var r = dr.Tables[0].Rows;
                    if (dr.Tables[0].Rows.Count > 0)
                    {
                        int i = 0;
                        while (i < r.Count)
                        {
                            if (r[i]["fticketno"].ToString() != "")
                            {
                                string invno = null;
                                if (snum != null)
                                {
                                    invno = $"{r[i]["issuingkey"].ToString()}/{snum}";
                                    NL_Label.Variables["PARTNO"].SetValue(Partno);
                                    NL_Label.Variables["CUSTPARTNO"].SetValue(r[i]["custname"].ToString());
                                    NL_Label.Variables["ORDERNO"].SetValue(r[i]["pono"].ToString());
                                    // NL_Label.Variables["CUSTNAME"].SetValue(r.CustName);
                                    NL_Label.Variables["CUSTNAME"].SetValue("");
                                    NL_Label.Variables["INVOICENO"].SetValue(invno);
                                    NL_Label.Variables["BARCODE"].SetValue($"{r[i]["fticketno"].ToString().ToUpper()}");
                                    NL_Label.Variables["txtbarcode"].SetValue($"*{r[i]["fticketno"].ToString().ToUpper()}*");
                                    NL_Label.Variables["QRCODE"].SetValue($"06P{r[i]["partno"].ToString().ToUpper()};17Q{r[i]["qty"].ToString().ToUpper()};30T{r[i]["pono"].ToString().ToUpper()};32T{r[i]["fticketno"].ToString().ToUpper()};");
                                    NL_Label.Variables["PREFIX"].SetValue("P01");
                                    NL_Label.PrintSettings.PrinterName = prname;// GetPrinterName(r["factory"].ToString());
                                    NL_Label.PrintSettings.JobName = $"NiceLabel Printing {r[i]["issuingkey"].ToString()}";
                                    i = r.Count;
                                }
                                else
                                {
                                    seq++;
                                    invno = $"{r[i]["issuingkey"].ToString()}/{seq}";
                                    NL_Label.Variables["PARTNO"].SetValue(Partno);
                                    NL_Label.Variables["CUSTPARTNO"].SetValue(r[i]["custname"].ToString());
                                    NL_Label.Variables["ORDERNO"].SetValue(r[i]["pono"].ToString());
                                    // NL_Label.Variables["CUSTNAME"].SetValue(r.CustName);
                                    NL_Label.Variables["CUSTNAME"].SetValue("");
                                    NL_Label.Variables["INVOICENO"].SetValue(invno);
                                    NL_Label.Variables["BARCODE"].SetValue($"{r[i]["fticketno"].ToString().ToUpper()}");
                                    NL_Label.Variables["txtbarcode"].SetValue($"*{r[i]["fticketno"].ToString().ToUpper()}*");
                                    NL_Label.Variables["QRCODE"].SetValue($"06P{r[i]["partno"].ToString().ToUpper()};17Q{r[i]["qty"].ToString().ToUpper()};30T{r[i]["pono"].ToString().ToUpper()};32T{r[i]["fticketno"].ToString().ToUpper()};");
                                    NL_Label.Variables["PREFIX"].SetValue("P01");
                                    NL_Label.PrintSettings.PrinterName = prname;// GetPrinterName(r["factory"].ToString());
                                    NL_Label.PrintSettings.JobName = $"NiceLabel Printing {r[i]["issuingkey"].ToString()}";
                                }
                                SplashScreenManager.Default.SetWaitFormDescription($"F_Ticket {invno}");
                                //update detail
                                new ConnDB().ExcuteSQL($"UPDATE TXP_ISSPACKDETAIL d SET d.ISSUINGSTATUS = 1 WHERE d.FTICKETNO = '{r[i]["fticketno"].ToString()}'");
                                NL_Label.Print(1);
                            }
                            i++;
                        }
                    }
                    else
                    {
                        Console.WriteLine("ไม่พบข้อมูลที่จะปริ้น");
                        return false;
                    }
                    NL_Label.Dispose();
                    NL_PrintEngine.Shutdown();
                }
                else
                {
                    Console.WriteLine("ไม่พบเครื่องปริ้นที่ต้องการ");
                    return false;
                }
            }
            return true;
        }

        private DataSet GetFTicker(string inv, string partno, string pono)
        {
            string sql = $"SELECT d.partno,d.pono,e.refinvoice issuingkey,e.custname,d.fticketno,d.orderqty,d.orderqty qty FROM TXP_ISSPACKDETAIL d\n" +
                        $"inner join txp_isstransent e on d.ISSUINGKEY = e.issuingkey\n" +
                        $"WHERE d.ISSUINGKEY = '{inv}' AND d.partno = '{partno}' AND d.PONO = '{pono}'\n" +
                        $"order by d.fticketno";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            return dr;
        }
    }
}
