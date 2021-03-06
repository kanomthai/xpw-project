﻿using DevExpress.XtraEditors;
using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Script.Serialization;
using System.Windows.Forms;
using System.Xml;
using XPWLibrary.Controllers;
using XPWLibrary.Models;

namespace XPWLibrary.Interfaces
{
    public class GreeterFunction
    {
        public string GetStatus(int x, int status)
        {
            string txtpath = "";
            string txt = status.ToString();
            switch (x)
            {
                case 1:
                    txtpath = $"{AppDomain.CurrentDomain.BaseDirectory}Configures\\invoice.json";
                    using (StreamReader r = new StreamReader(txtpath))
                    {
                        string json = r.ReadToEnd();
                        JavaScriptSerializer jss = new JavaScriptSerializer();
                        var Items = jss.Deserialize<JSONClass>(json);
                    }
                    break;
                default:
                    break;
            }
            return txt;
        }
        public void CheckGitHubVersionAsync()
        {
            try
            {
                string fromdir = StaticFunctionData.PathSource;
                string targetdir = $"{AppDomain.CurrentDomain.BaseDirectory}";
                foreach (string f in Directory.GetFiles(targetdir))
                {
                    string fn = Path.GetFileName(f);
                    DateTime modification = File.GetLastWriteTime(fn);
                    string adate = modification.ToString("ddMMyyyyHHmmss");
                    LoopCheckFileDiff(fromdir, targetdir, fn, adate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool CheckVersionAsync()
        {
            try
            {
                string fromdir = StaticFunctionData.PathSource;
                string targetdir = $"{AppDomain.CurrentDomain.BaseDirectory}";
                foreach (string f in Directory.GetFiles(targetdir))
                {
                    string fn = Path.GetFileName(f);
                    DateTime modification = File.GetLastWriteTime(fn);
                    string adate = modification.ToString("ddMMyyyyHHmmss");
                    return LoopCheckDiff(fromdir, targetdir, fn, adate);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return false;
        }

        bool LoopCheckDiff(string fromdir, string targetdir, string fn, string adate)
        {
            foreach (string file in Directory.GetFiles(fromdir))
            {
                string fname = Path.GetFileName(file);
                DateTime modif = File.GetCreationTime(fname);
                string bdate = modif.ToString("ddMMyyyyHHmmss");
                if (fname == fn)
                {
                    Console.WriteLine($"MATCH  A: {fn} B: {fname}");
                    Console.WriteLine($"A: {adate} B: {bdate}");
                    if (adate != bdate)
                    {
                        Console.WriteLine($"COPY B TO A");
                        File.Copy(fromdir + "\\" + fname, targetdir + "\\" + fname, true);
                        return true;
                    }
                }
            }
            return false;
        }

        void LoopCheckFileDiff(string fromdir, string targetdir, string fn, string adate)
        {
            foreach (string file in Directory.GetFiles(fromdir))
            {
                string fname = Path.GetFileName(file);
                DateTime modif = File.GetLastWriteTime(fname);
                string bdate = modif.ToString("ddMMyyyyHHmmss");
                if (fname == fn)
                {
                    try
                    {
                        SplashScreenManager.Default.SetWaitFormDescription($"{fname}");
                    }
                    catch (Exception)
                    {
                    }
                    Console.WriteLine($"MATCH  A: {fn} B: {fname}");
                    Console.WriteLine($"A: {adate} B: {bdate}");
                    if (adate != bdate)
                    {
                        Console.WriteLine($"COPY B TO A");
                        File.Copy(fromdir + "\\" + fname, targetdir + "\\" + fname, true);
                        return;
                    }
                }
            }
        }


        private void GetLangure()
        {
            string docdir = $"{AppDomain.CurrentDomain.BaseDirectory}Configures\\invoicedetail.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(docdir);
            XmlNodeList node = doc.DocumentElement.ChildNodes;
            StaticFunctionData.JobListTilte = node[0].InnerText;
            StaticFunctionData.JobListInformation = node[1].InnerText;
            StaticFunctionData.JoblistConfirmInv = node[2].InnerText;
            StaticFunctionData.JoblistPrintJobList = node[3].InnerText;
            StaticFunctionData.JoblistLabelShort = node[4].InnerText;
            StaticFunctionData.JoblistShippingList = node[5].InnerText;
            StaticFunctionData.JoblistShippingListByPart = node[6].InnerText;
            StaticFunctionData.JoblistShippingListByAll = node[7].InnerText;
            StaticFunctionData.JoblistPalletList = node[8].InnerText;
            StaticFunctionData.JoblistSplitPart = node[9].InnerText;
            StaticFunctionData.JoblistEditOrder = node[10].InnerText;
            StaticFunctionData.JoblistSetMultiLot = node[11].InnerText;
            StaticFunctionData.JoblistPartShort = node[12].InnerText;
            StaticFunctionData.JoblistOrderHold = node[13].InnerText;
            StaticFunctionData.JoblistOrderCancel = node[14].InnerText;
            StaticFunctionData.JoblistOrderShorting = node[15].InnerText;
        }
        public bool BeginingLoadApp()
        {
            string docdir = $"{AppDomain.CurrentDomain.BaseDirectory}Configures\\settings.xml";
            XmlDocument doc = new XmlDocument();
            doc.Load(docdir);
            XmlNodeList node = doc.DocumentElement.ChildNodes;
            StaticFunctionData.Factory = node[0].InnerText;
            StaticFunctionData.fticketprinter = node[1].InnerText;
            StaticFunctionData.cartonticketprinter = node[2].InnerText;
            StaticFunctionData.aw_totalpallet = int.Parse(node[3].InnerText);
            StaticFunctionData.aw_palletlimit = int.Parse(node[4].InnerText);
            StaticFunctionData.aw_boxbigsize = int.Parse(node[5].InnerText);
            StaticFunctionData.aw_netweight = int.Parse(node[6].InnerText);
            StaticFunctionData.aw_plprinter = node[11].InnerText;
            StaticFunctionData.inj_plprinter = node[12].InnerText;
            StaticFunctionData.confirmlotno = bool.Parse(node[13].InnerText);
            StaticFunctionData.EditOrder = bool.Parse(node[14].InnerText);
            StaticFunctionData.EditShip = bool.Parse(node[15].InnerText);
            StaticFunctionData.ConnString = $"Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST={node[7].InnerText})(PORT=1521))" +
                                       $"(CONNECT_DATA=(SERVICE_NAME={node[8].InnerText})));" +
                                       $"User Id={node[9].InnerText};Password={node[10].InnerText};Min Pool Size=50;Connection Lifetime = 120;" +
                                          "Connection Timeout = 60; Incr Pool Size=15;Decr Pool Size=12;";
            StaticFunctionData.PathExcute = node[16].InnerText;
            StaticFunctionData.ReloadGrid = int.Parse(node[17].InnerText);
            StaticFunctionData.onWeek = int.Parse(node[18].InnerText);
            StaticFunctionData.nextWeek = int.Parse(node[19].InnerText);
            StaticFunctionData.StatusFTicket = int.Parse(node[20].InnerText);
            StaticFunctionData.StatusSendGEDI = int.Parse(node[21].InnerText);
            StaticFunctionData.PathSource = node[22].InnerText;
            StaticFunctionData.PathTemplate = node[23].InnerText;
            StaticFunctionData.specialcustomer = node[24].InnerText;
            StaticFunctionData.shiping_label = node[25].InnerText.Split(',').ToList();
            StaticFunctionData.enable_etd = bool.Parse(node[26].InnerText.ToString());
            StaticFunctionData.enable_ship = bool.Parse(node[27].InnerText.ToString());
            StaticFunctionData.enable_note1 = bool.Parse(node[28].InnerText.ToString());
            StaticFunctionData.enable_note2 = bool.Parse(node[29].InnerText.ToString());
            StaticFunctionData.enable_note3 = bool.Parse(node[30].InnerText.ToString());
            StaticFunctionData.enable_container = bool.Parse(node[31].InnerText.ToString());
            StaticFunctionData.enable_container_date = bool.Parse(node[32].InnerText.ToString());

            Version version = Assembly.GetExecutingAssembly().GetName().Version;

            if (node[9].InnerText.ToString() == "sktsys")
            {
                StaticFunctionData.DBname = "TEST";
                StaticFunctionData.AppVersion = $"XPW TEST V.{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            else if (node[9].InnerText.ToString() == "expsys")
            {
                StaticFunctionData.DBname = "CK";
                StaticFunctionData.AppVersion = $"XPW V.{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            else
            {
                StaticFunctionData.DBname = "unknow";
                StaticFunctionData.AppVersion = $"XPW UNKNOW V.{version.Major}.{version.Minor}.{version.Build}.{version.Revision}";
            }
            GetLangure();
            CopyTemplateFile();
            return true;
        }

        public void RestoreTemplate()
        {
            try
            {
                string fromdir = StaticFunctionData.PathTemplate;
                string targetdir = $"{AppDomain.CurrentDomain.BaseDirectory}Templates";
                foreach (string file in Directory.GetFiles(fromdir))
                {
                    string fname = Path.GetFileName(file);
                    Console.WriteLine(fromdir + "\\" + fname);
                    Console.WriteLine(targetdir + "\\" + fname);
                    File.Copy(fromdir + "\\" + fname, targetdir + "\\" + fname, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void CopyTemplateFile()
        {
            try
            {
                string fromdir = StaticFunctionData.PathTemplate;
                string targetdir = $"{AppDomain.CurrentDomain.BaseDirectory}Configures\\xpw-templates";
                foreach (string file in Directory.GetFiles(fromdir))
                {
                    string fname = Path.GetFileName(file);
                    Console.WriteLine(fromdir + "\\" + fname);
                    Console.WriteLine(targetdir + "\\" + fname);
                    File.Copy(fromdir + "\\" + fname, targetdir + "\\" + fname, true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public bool CheckUpdateInvoice(DateTime etd)
        {
            bool x = true;
            string sql = $"SELECT nextweek FROM (" +
                $"SELECT(TRUNC(SYSDATE, 'DY') + {StaticFunctionData.onWeek}) onweek,(TRUNC(SYSDATE, 'DY') + {StaticFunctionData.nextWeek}) nextweek FROM dual" +
                $") WHERE nextweek >= TO_DATE('{etd.ToString("ddMMyyyy")}', 'ddMMyyyy')";
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 0)
            {
                x = false;
            }
            return x;
        }

        public string GetLastInvoice(string refinv)
        {
            string txt = "TW";
            string sql = "SELECT e.FACTORY,e.AFFCODE,e.BISHPC,e.CUSTNAME,TO_NUMBER(SUBSTR(max(e.REFINVOICE), 5, 5)) + 1 rnum FROM TXP_ISSTRANSENT e  \n" +
                $"WHERE SUBSTR(e.ISSUINGKEY, 1, 3) = '{refinv.Substring(0, 3)}' AND TO_CHAR(e.ETDDTE, 'yyyy') = TO_CHAR(SYSDATE, 'yyyy') AND LENGTH(e.REFINVOICE) = 10\n" +
                "GROUP BY e.FACTORY,e.AFFCODE,e.BISHPC,e.CUSTNAME\n" +
                "ORDER BY TO_NUMBER(SUBSTR(max(e.REFINVOICE), 5, 5)) DESC";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            if (refinv.Substring(0, 1) == "I")
            {
                txt = "TI";
            }
            int x = 1;
            if (dr.Tables[0].Rows.Count > 0)
            { 
                x = int.Parse(dr.Tables[0].Rows[0]["rnum"].ToString()); 
            }
            dr = new ConnDB().GetFill($"SELECT e.shiptype  FROM TXP_ISSTRANSENT e WHERE e.ISSUINGKEY = '{refinv}' GROUP BY e.shiptype");
            return txt + refinv.Substring(1, 2) + x.ToString("D5") + dr.Tables[0].Rows[0]["shiptype"].ToString();
        }

        public object GetOrderGroupBy(string poType, string custPoType)
        {
            string txt;
            switch (poType)
            {
                case "3END":
                    txt = "E";
                    break;
                case "3FRD":
                    txt = "F";
                    break;
                case "A_TMW":
                    txt = "N";
                    if (custPoType == "TMW")
                    {
                        txt = "T";
                    }
                    break;
                case "ALL":
                    txt = "N";
                    break;
                case "CONS":
                    txt = "c";
                    break;
                default:
                    txt = "";
                    break;
            }
            return txt;
        }

        public string GetPalletWireSize(int x)
        {
            string xpl;
            if (x >= 46 && x <= 54)
            {
                xpl = "110x110x13.5";
            }
            else if (x >= 37 && x <= 45)
            {
                xpl = "110x110x82.5";
            }
            else if (x >= 28 && x <= 36)
            {
                xpl = "110x110x69";
            }
            else if (x >= 19 && x <= 27)
            {
                xpl = "110x110x55.5";
            }
            else if (x == 18)
            {
                xpl = "110x110x42";
            }
            else
            {
                xpl = "35x35x13.5";
            }
            return xpl;
        }

        public string GetPlSize(string partsize, int ctn)
        {
            string PlSize ="";
            switch (partsize)
            {
                case "63x64x15":
                case "63x64x18":
                case "63x64x17":
                    if (ctn < 20 && ctn >= 16)
                    {
                        PlSize = "65x129x159";

                    }
                    else if (ctn >= 20)//20
                    {
                        PlSize = "65x129x195";
                    }
                    else
                    {
                        //PlSize = "BOX";
                        PlSize = "65x129x159";
                    }
                    break;
                case "27x27x50":
                case "27x27x51":
                    //if (ctn >= 24)
                    //{
                    //    PlSize = "111x115x94";
                    //}
                    //else
                    //{
                    //    PlSize = "BOX";
                    //}
                    PlSize = "111x115x94";
                    break;
                case "43x27x14":
                case "26x43x14":
                case "27x43x14":
                    //if (ctn >= 64)
                    //{
                    //    PlSize = "111x115x125";
                    //}
                    //else
                    //{
                    //    PlSize = "BOX";
                    //}
                    PlSize = "111x115x125";
                    break;
                case "38x56x39":
                case "56x38x39":
                case "39x56x40":
                    if (ctn < 18)
                    {
                        //PlSize = "BOX";
                        PlSize = "111x115x133";
                    }
                    else if (ctn < 24 && ctn > 17)//18
                    {
                        PlSize = "111x115x133";
                    }
                    else if (ctn < 30 && ctn > 23)//24
                    {
                        PlSize = "111x115x213";
                    }
                    else if (ctn >= 30)
                    {
                        PlSize = "111x115x213";
                    }
                    break;
                case "36x54x24":
                case "37x54x24":
                case "38x56x23":
                    //if (ctn < 36)
                    //{
                    //    PlSize = "BOX";
                    //}
                    //else
                    //{
                    //    PlSize = "111x115x163";
                    //}
                    PlSize = "111x115x163";
                    break;
                case "36x55x39":
                    //if (ctn < 30)
                    //{
                    //    PlSize = "BOX";
                    //}
                    //else
                    //{
                    //    PlSize = "111x115x130";
                    //}
                    PlSize = "111x115x130";
                    break;
                default:
                    PlSize = "MIX";
                    break;
            }
            return PlSize;
        }

        List<INJPlData> PlInjSize(string partsize, int ctn)
        {
            string plgroup;
            List<INJPlData> obj = new List<INJPlData>();
            switch (partsize)
            {
                case "63x64x15":
                case "63x64x18":
                case "63x64x17":
                    plgroup = "0";
                    if (ctn < 20 && ctn >= 16)
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "65x129x159",
                            PlNo = ctn,
                            Total = 1,
                        });

                    }
                    else if (ctn >= 20)//20
                    {
                        var a = ctn / 20;
                        Console.WriteLine(a);//pl qty
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "65x129x195",
                            PlNo = 20,
                            Total = a,
                        });
                        var b = ctn - (a * 20);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "BOX",
                                PlNo = 0,
                                Total = b,
                            });
                        }
                    }
                    else
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "BOX",
                            PlNo = 0,
                            Total = ctn,
                        });
                    }
                    break;
                case "27x27x50":
                case "27x27x51":
                    plgroup = "1";
                    if (ctn >= 24)
                    {
                        var a = ctn / 24;
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "111x115x94",
                            PlNo = 24,
                            Total = a,
                        });
                        var b = ctn - (a * 24);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "BOX",
                                PlNo = 0,
                                Total = b,
                            });
                        }
                    }
                    else 
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "BOX",
                            PlNo = 0,
                            Total = ctn,
                        });
                    }
                    break;
                case "43x27x14":
                case "26x43x14":
                case "27x43x14":
                    plgroup = "2";
                    if (ctn >= 64)
                    {
                        var a = ctn / 64;
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "111x115x125",
                            PlNo = 64,
                            Total = a,
                        });
                        var b = ctn - (a * 64);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "BOX",
                                PlNo = 0,
                                Total = b,
                            });
                        }
                    }
                    else
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "BOX",
                            PlNo = 0,
                            Total = ctn,
                        });
                    }
                    break;
                case "38x56x39":
                case "56x38x39":
                case "39x56x40":
                    plgroup = "3";
                    if (ctn < 18)
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "BOX",
                            PlNo = 0,
                            Total = ctn,
                        });
                    }
                    else if (ctn < 24 && ctn > 17)//18
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "111x115x133",
                            PlNo = 1,
                            Total = ctn,
                        });
                    }
                    else if (ctn < 30 && ctn > 23)//24
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "111x115x213",
                            PlNo = 1,
                            Total = ctn,
                        });
                    }
                    else if (ctn >= 30)
                    {
                        var a = ctn / 30;
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "111x115x213",
                            PlNo = 30,
                            Total = a,
                        });
                        var b = ctn - (a*30);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "BOX",
                                PlNo = 0,
                                Total = b,
                            });
                        }
                    }
                    break;
                case "36x54x24":
                case "37x54x24":
                case "38x56x23":
                    plgroup = "4";
                    if (ctn < 36)
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "BOX",
                            PlNo = 0,
                            Total = ctn,
                        });
                    }
                    else
                    {
                        var a = ctn / 36;
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "111x115x163",
                            PlNo = 36,
                            Total = a,
                        });
                        var b = ctn - (a * 36);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "BOX",
                                PlNo = 0,
                                Total = b,
                            });
                        }
                    }
                    break;
                case "36x55x39":
                    plgroup = "5";
                    if (ctn < 30)
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "BOX",
                            PlNo = 0,
                            Total = ctn,
                        });
                    }
                    else
                    {
                        var a = ctn / 30;
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "111x115x130",
                            PlNo = 30,
                            Total = a,
                        });
                        var b = ctn - (a * 30);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "BOX",
                                PlNo = 0,
                                Total = b,
                            });
                        }
                    }
                    break;
                default:
                    obj.Add(new INJPlData()
                    {
                        PlGroup = "6",
                        PSize = partsize,
                        PlSize = "MIX",
                        PlNo = 0,
                        Total = ctn,
                    });
                    break;
            }
            return obj;
        }

        public void CreateLogSearch(string v)
        {
            throw new NotImplementedException();
        }

        public bool CheckPlDuplicate(string refno, string plnum)
        {
            bool x = false;
            string sql = $"SELECT * FROM TXP_ISSPALLET l WHERE l.PALLETNO = '{plnum}' AND l.ISSUINGKEY = '{refno}'";
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 0)
            {
                x = true;
            }
            return x;
        }

        public int GetPlQty(string refno, string plno)
        {
            string sql = $"SELECT l.PLTOTAL  FROM TXP_ISSPALLET l \n" +
                $"WHERE l.ISSUINGKEY = '{refno}' AND l.PALLETNO = '{plno}'";
            Console.WriteLine(sql);
            int x = 0;
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 0)
            {
                x = int.Parse(dr.Tables[0].Rows[0]["pltotal"].ToString());
            }
            return x;
        }

        public string GetPlList(string refno, string pltype)
        {
            string sql = $"SELECT \n" +
                $"CASE WHEN min(l.PALLETNO) = max(l.PALLETNO) THEN min(l.PALLETNO) ELSE min(l.PALLETNO)||'-'||max(l.PALLETNO) END  plno,l.PLTYPE \n" +
                $"FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY = '{refno}' AND l.PLTYPE = '{pltype}' GROUP BY l.PLTYPE";
            string x = "";
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 0)
            {
                x = dr.Tables[0].Rows[0]["plno"].ToString();
            }
            return x;
        }

        public int GetInvoiceStatus(string refNo)
        {
            if (refNo == "")
            {
                return 0;
            }
            string sql = $"SELECT p.orderstatus FROM TXP_ORDERPLAN p WHERE p.curinv = '{refNo}'";
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count < 1)
            {
                return 0;
            }
            return int.Parse(dr.Tables[0].Rows[0][0].ToString());
        }

        internal static bool updateFTicket(object fac)
        {
            string sql = $"update TXM_SHIPLABELSN set lastsnno = lastsnno + 1 where factory = '{fac}'";
            return new ConnDB().ExcuteSQL(sql);
        }

        internal string getFTicket(object fac)
        {
            string sql = $"select fprefix || to_char(sysdate, 'MM')|| substr('0000000000000000'|| (lastsnno + 1),-10) from TXM_SHIPLABELSN where factory = '{fac}'";
            DataSet dr = new ConnDB().GetFill(sql);
            return dr.Tables[0].Rows[0][0].ToString();
        }

        internal string GetNote(int x, string abt, string shiptype, string factory)
        {
            string n;
            switch (x)
            {
                case 1:
                    n = "";
                    break;
                case 2:
                    switch ($"{abt}{factory}")
                    {
                        case "1AW":
                            n = "";
                            break;
                        case "1INJ":
                            n = "LOADING AT CK-1 LCL";
                            break;
                        case "2INJ":
                            n = "LOADING AT NESC";
                            break;
                        case "3INJ":
                            n = "LOADING AT ICAM";
                            break;
                        case "4INJ":
                            n = "LOADING AT CK-2";
                            break;
                        default:
                            n = "";
                            break;
                    }
                    break;
                case 3:
                    switch ($"{abt}{factory}")
                    {
                        case "4INJ":
                            n = "BONDED";
                            break;
                        case "1AW":
                            n = "BONDED(LOADING AT CK-2)";
                            break;
                        case "1INJ":
                            n = "DOMESTIC";
                            break;
                        default:
                            n = "";
                            break;
                    }
                    break;
                case 4:
                    n = "";
                    break;
                default:
                    n = "";
                    break;
            }
            return n;
        }

        public List<INJPlData> GetPalletData(string invoice)
        {
            string sql = $"SELECT sum(round(b.ORDERQTY/b.STDPACK)) ctn,p.BIWIDT||'x'||p.BILENG||'x'||p.BIHIGH dm FROM TXP_ISSTRANSBODY b\n" +
                        $"INNER JOIN TXP_ORDERPLAN p ON b.ISSUINGKEY = p.CURINV AND b.PARTNO = p.PARTNO\n" +
                        $"WHERE b.ISSUINGKEY = '{invoice}'\n" +
                        $"GROUP BY p.BIWIDT || 'x' || p.BILENG || 'x' || p.BIHIGH\n" +
                        $"ORDER BY  p.BIWIDT || 'x' || p.BILENG || 'x' || p.BIHIGH";
            Console.WriteLine(sql);
            List<INJPlData> ob = new List<INJPlData>();
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows) {
                ob.Add(new INJPlData()
                { 
                    Id = ob.Count,
                    PSize = r["dm"].ToString(),
                });
            }
            return ob;
        }

        bool CheckPalleteDuplicate(INJPlData ob, string refinv)
        {
            string sql = $"SELECT count(*) FROM TXP_ISSPALLET l WHERE l.PALLETNO = '{ob.PlName}' AND l.ISSUINGKEY = '{refinv}'";
            DataSet dr = new ConnDB().GetFill(sql);
            if (int.Parse(dr.Tables[0].Rows[0][0].ToString()) < 1)
            {
                int t = ob.PlNo;
                if (ob.PlName.IndexOf("C") >= 0)
                {
                    t = 1;
                }
                string ins = $"INSERT INTO TXP_ISSPALLET(FACTORY, ISSUINGKEY, PALLETNO, PLTYPE, PLOUTSTS, UPDDTE, SYSDTE, PLTOTAL, BOOKED)\n" +
                         $"VALUES('{StaticFunctionData.Factory}', '{refinv}', '{ob.PlName}', '{ob.PlSize}', 0, current_timestamp, current_timestamp, {t}, 0)";
                return new ConnDB().ExcuteSQL(ins);
            }
            return true;
        }

        public bool SumPlInj(string invoice)
        {
            bool x = true;
            return x;
        }

        private int GetPartWireCtn(string inv, string ptype)
        {
            int x = 0;
            string sql = $"SELECT CASE WHEN sum(round(b.ORDERQTY/b.STDPACK)) IS NULL THEN 0 ELSE sum(round(b.ORDERQTY/b.STDPACK)) END ctn " +
                $"FROM TXP_ISSTRANSBODY b WHERE b.ISSUINGKEY = '{inv}' AND b.PARTNO LIKE '{ptype}%'";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                x = int.Parse(r["ctn"].ToString());
            }
            return x;
        }

        private int[] AvgPlWireCount(int x, string pltype)
        {
            int[] i = new int[5];
            int a_a;//{ 0 } = pallet no
            int a_b;//{ 1 } = กล่องใหญ่
            int a_c;//{ 2 } = เศษ
            int a_d = 0; //{ 3 } = กล่องเล็ก
            int a_e; //{ 4 } = จำนวนต่อหน่วย

            int a;

            switch (pltype)
            {
                case "PL":
                    if (x > 44)
                    {
                        a = x / 45;
                        if ((x - (a * 45)) < 1)
                        {
                            a_a = a;
                            a_b = 0;
                            a_c = (x - (a * 45));
                            a_e = 45;
                        }
                        else
                        {
                            a_a = a;
                            a_b = 0;
                            a_c = (x - (a * 45));
                            a_e = 0;
                            if (a > 0)
                            {
                                a_e = 45;
                            }
                        }
                    }
                    else
                    {
                        a_a = 0;
                        a_b = 0;
                        a_c = x;
                        a_e = 0;
                    }
                    break;
                default:
                    a = x / 2;
                    a_a = 0;
                    a_b = a;
                    a_c = 0;
                    a_d = (x - (a * 2));
                    a_e = 2;
                    break;
            }

            i[0] = a_a;
            i[1] = a_b;
            i[2] = a_c;
            i[3] = a_d;
            i[4] = a_e;
            return i;
        }

        private int WrPl(List<PlListData> olkey, int i, string inv, string pltype,int lastpl, int total)
        {
            try
            {
                if (i > 0 && total > 0)
                {
                    bool xcust = false;
                    string sql_check_cust = $"SELECT * FROM TXP_ISSTRANSENT WHERE ISSUINGKEY = '{inv}' AND CUSTNAME = '{StaticFunctionData.specialcustomer}'";
                    SplashScreenManager.Default.SetWaitFormDescription($"CHECK CUTOMER");
                    DataSet dr = new ConnDB().GetFill(sql_check_cust);
                    if (dr.Tables[0].Rows.Count > 0)
                    {
                        xcust = true;
                    }
                    int j = 0;
                    while (j < i)
                    {
                        string plpref = "P";
                        if (pltype == "BOX")
                        {
                            plpref = "C";
                        }
                        string plnum = $"1{plpref}{lastpl.ToString("D3")}";
                        SplashScreenManager.Default.SetWaitFormDescription($"{plnum.ToString()}");
                        var b = olkey.FindIndex(x => x.PlNo == plnum);
                        string PlKey = "";
                        string ContNo = "";
                        int PlStatus = 0;
                        if (b >= 0)
                        {
                            PlKey = olkey[b].PlKey;
                            ContNo = olkey[b].ContNo;
                            PlStatus = olkey[b].PlStatus;
                        }
                        //StaticFunctionData.specialcustomer
                        int[] y = new int[3] { 0, 0, 0 };
                        if (xcust)
                        {
                            y = AWCheckPalletSpecialDim(total);
                        }
                        else
                        {
                            y = AWCheckPalletDim(total);
                        }
                        dr = new ConnDB().GetFill($"SELECT * FROM TXP_ISSPACKDETAIL WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{inv}'");
                        if (dr.Tables[0].Rows.Count > 0)
                        {
                            string ptype = $"{y[0]}X{y[1]}X{y[2]}";
                            string sql_coubnt = $"SELECT * FROM TXP_ISSPALLET WHERE Palletno ='{plnum}' AND issuingkey ='{inv}'";
                            dr = new ConnDB().GetFill(sql_coubnt);
                            Console.WriteLine($"{inv} PL=>{plnum} TYPE=>{pltype} TOTAL=>{total} SIZE: {ptype}");
                            string sql = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno,containerno,Pltype,pltotal,ploutsts,Sysdte,Upddte,PLWIDE,PLLENG,PLHIGHT)" +
                                $"values" +
                                $"('{StaticFunctionData.Factory}', '{inv}', '{plnum}','{PlKey}', '{ContNo}', '{pltype}',{total},{PlStatus},sysdate,sysdate, {y[0]}, {y[1]}, {y[2]})";
                            if (dr.Tables[0].Rows.Count > 0)
                            {
                                sql = $"UPDATE TXP_ISSPALLET set Pltype='{pltype}',pltotal='{total}',PLWIDE={y[0]},PLLENG={y[1]},PLHIGHT={y[2]} WHERE Palletno ='{plnum}' AND issuingkey ='{inv}'";
                            }
                            if (new ConnDB().ExcuteSQL(sql))
                            {
                                //string sql_q = $"SELECT UUID FROM TXP_ISSPACKDETAIL WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{inv}' AND PARTNO LIKE '{pltype}%' AND ROWNUM <= {total} ORDER BY FTICKETNO,ITEM";
                                //if (pltype == "MIX")
                                //{
                                //    sql_q = $"SELECT UUID FROM TXP_ISSPACKDETAIL WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{inv}' AND ROWNUM <= {total} ORDER BY FTICKETNO,ITEM";
                                //}
                                //if (plnum.ToUpper().Substring(1, 1) == "C")
                                //{
                                //    sql_q = $"SELECT UUID FROM TXP_ISSPACKDETAIL WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{inv}' AND ROWNUM <= {total} ORDER BY FTICKETNO,ITEM";
                                //}
                                //DataSet ds = new ConnDB().GetFill(sql_q);
                                //if (ds.Tables[0].Rows.Count > 0)
                                //{
                                //    //string upsql = $"UPDATE TXP_ISSPACKDETAIL SET SHIPPLNO = '{plnum.ToUpper()}',PLOUTNO='{PlKey}'  " +
                                //    //    $"WHERE SHIPPLNO IS NULL AND ISSUINGKEY = '{inv}' AND ROWNUM <= {total}";
                                //    //new ConnDB().ExcuteSQL(sql);

                                //    //int run = 0;
                                //    //foreach (DataRow r in ds.Tables[0].Rows)
                                //    //{
                                //    //    sql = $"UPDATE TXP_ISSPACKDETAIL SET SHIPPLNO = '{plnum.ToUpper()}',PLOUTNO='{PlKey}' WHERE UUID='{r["uuid"].ToString()}'";
                                //    //    if (new ConnDB().ExcuteSQL(sql))
                                //    //    {
                                //    //        run++;
                                //    //        Console.WriteLine($"NEXT => {run}");
                                //    //        SplashScreenManager.Default.SetWaitFormDescription($"{plnum.ToString()} SEQ => {run}");
                                //    //    }
                                //    //}
                                //}
                                Console.WriteLine("==================================================");
                                lastpl++;
                                j++;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                GreeterFunction.Logs(ex.Message);
            }
            return lastpl;
        }

        private int[] AWCheckPalletSpecialDim(int total)
        {
            int[] x = new int[3];
            int w = 35;
            int l = 35;
            int h = 0;
            if (total <= 54 && total >= 46)
            {
                w = 110;
                l = 110;
                h = 96;
            }
            else if (total <= 45 && total >= 37)
            {
                w = 110;
                l = 110;
                h = 82;
            }
            else if (total <= 36 && total >= 28)
            {
                w = 110;
                l = 110;
                h = 69;
            }
            else if (total <= 27 && total >= 19)
            {
                w = 110;
                l = 110;
                h = 55;
            }
            else if (total == 18)
            {
                w = 110;
                l = 110;
                h = 42;
            }
            else if (total <= 17)
            {
                h = 13;
            }

            x[0] = w;
            x[1] = l;
            x[2] = h;
            return x;
        }

        private int[] AWCheckPalletDim(int total)
        {
            int[] x = new int[3];
            int w = 35;
            int l = 35;
            int h = 13;
            if (total <= 45 && total >= 37)
            {
                w = 110;
                l = 110;
                h = 70;
            }
            else if (total <= 36 && total >= 28)
            {
                w = 110;
                l = 110;
                h = 56;
            }
            else if (total <= 27 && total >= 19)
            {
                w = 110;
                l = 110;
                h = 42;
            }
            else if (total == 18)
            {
                w = 110;
                l = 110;
                h = 28;
            }
            else if (total == 2)
            {
                h = 23;
            }

            x[0] = w;
            x[1] = l;
            x[2] = h;
            return x;
        }

        //public bool SumPalletSpecial(string inv)
        //{
        //    bool x = false;

        //    return x;
        //}

        public bool SumPallet(string inv, string custname)
        {
            bool success = false;
            try
            {
                List<PlListData> olkey = GetPlData(inv);
                if (CountPl(inv))
                {
                    int lastpl = GetPalletCount(inv, "P", custname);//get Last PL
                    int ctn_180 = GetPartWireCtn(inv, "180");
                    int ctn_181 = GetPartWireCtn(inv, "181");

                    int[] a_180 = AvgPlWireCount(ctn_180, "PL");
                    int[] a_181 = AvgPlWireCount(ctn_181, "PL");

                    if (a_180[4] > a_181[4])
                    {
                        //180
                        lastpl = WrPl(olkey, a_180[0], inv, "180", lastpl, a_180[4]);

                        //181
                        lastpl = WrPl(olkey, a_181[0], inv, "181", lastpl, a_181[4]);
                    }
                    else
                    {
                        //181
                        lastpl = WrPl(olkey, a_181[0], inv, "181", lastpl, a_181[4]);

                        //180
                        lastpl = WrPl(olkey, a_180[0], inv, "180", lastpl, a_180[4]);
                    }

                    //mix
                    Console.WriteLine("START MIX==========================");
                    int mpl = a_180[2] + a_181[2];
                    int[] mmpl = AvgPlWireCount(mpl, "PL");
                    lastpl = WrPl(olkey, mmpl[0], inv, "MIX", lastpl, 45);

                    //lastmix
                    if (mmpl[2] >= 18)
                    {
                        lastpl = WrPl(olkey, 1, inv, "MIX", lastpl, mmpl[2]);
                    }
                    else
                    {
                        Console.WriteLine("START BOX ==========================");
                        lastpl = GetPalletCount(inv, "B", custname);//get Last box
                        int[] bbc = AvgPlWireCount(mmpl[2], "BX");
                        lastpl = WrPl(olkey, bbc[1], inv, "BOX", lastpl, 2);
                        if (bbc[3] > 0)
                        {
                            lastpl = WrPl(olkey, bbc[3], inv, "BOX", lastpl, 1);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                GreeterFunction.Logs(ex.Message);
                success = false;
            }
            return success;
        }

        private int GetPalletCount(string inv, string v, string custname)
        {
            int x = 1;
            string refinv = inv.Substring(0, 12) + (int.Parse(inv.Substring(13).ToString()) - 1).ToString("D4");
            if ((int.Parse(inv.Substring(13).ToString()) - 1) < 1)
            {
                refinv = inv;
            }
            x += new OrderControllers().GetLastPalletCtn(refinv, v, inv, custname);
            return x;
        }

        public static void Logs(string message)
        {
            string txtpath = $"{AppDomain.CurrentDomain.BaseDirectory}Logs\\errors.txt";
            TextWriter tw = new StreamWriter(txtpath);
            tw.WriteLine(message);
            tw.Close();
        }

        //public bool SumPallet(string inv)
        //{
        //    try
        //    {
        //        try
        //        {
        //            SplashScreenManager.Default.SetWaitFormDescription($"SUMMARY PL.");
        //        }
        //        catch (Exception)
        //        {
        //        }
        //        string fac = "AW";
        //        if (inv.Substring(0, 1) == "I")
        //        {
        //            fac = "INJ";
        //            return true;
        //        }
        //        else
        //        {
        //            BindingList<PlListData> olkey = GetPlData(inv);
        //            bool repl = CountPl(inv);
        //            //// Console.WriteLine(repl);
        //            //string sql = $"SELECT SUBSTR(d.PARTNO, 0, 3) partype,count(d.PARTNO) seqctn FROM TXP_ISSPACKDETAIL d  WHERE d.ISSUINGKEY = '{inv}' GROUP BY SUBSTR(d.PARTNO, 0, 3) order by seqctn desc";
        //            string sql = $"SELECT partype,sum(ctn) seqctn FROM (\n" +
        //                        $"    SELECT SUBSTR(p.PARTNO, 0, 3) partype,p.BALQTY / p.BISTDP ctn FROM TXP_ORDERPLAN p WHERE p.CURINV = '{inv}' AND p.STATUS = 1\n" +
        //                        $") GROUP BY partype\n" +
        //                        $"ORDER BY seqctn DESC";
        //            // Console.WriteLine(sql);
        //            string lastissueno = null;
        //            DataSet dr = new ConnDB().GetFill(sql);
        //            string scust = $"SELECT e.FACTORY,get_zone(e.FACTORY, e.ZONEID) zname, to_char(e.ETDDTE, 'dd/MM/yyyy') etd,e.CUSTNAME FROM TXP_ISSTRANSENT e WHERE e.ISSUINGKEY = '{inv}'";
        //            DataSet rx = new ConnDB().GetFill(scust);
        //            foreach (DataRow rr in rx.Tables[0].Rows)
        //            {
        //                string ssql = $"SELECT ISSUINGKEY,SUBSTR(ISSUINGKEY, 12) issctn FROM TXP_ISSTRANSENT e WHERE " +
        //                    $"e.FACTORY = '{rr["factory"].ToString()}' AND " +
        //                    $"get_zone(e.FACTORY, e.ZONEID) = '{rr["zname"].ToString()}' " +
        //                    $"AND e.ETDDTE = TO_DATE('{rr["etd"].ToString()}', 'dd/MM/yyyy') " +
        //                    $"AND e.CUSTNAME = '{rr["custname"].ToString()}' " +
        //                    $"AND rownum < 3 " +
        //                    $"ORDER BY SUBSTR(ISSUINGKEY, 12)  DESC";
        //                Console.WriteLine(ssql);
        //                DataSet rq = new ConnDB().GetFill(ssql);
        //                //int row = 0;
        //                //while (row < rq.Tables[0].Rows.Count)
        //                //{
        //                //    var o = rq.Tables[0].Rows[row];
        //                //    Console.WriteLine(o["issuingkey"]);
        //                //    if 
        //                //    row++;
        //                //}
        //                lastissueno = rq.Tables[0].Rows[0]["issuingkey"].ToString();
        //                //if (rq.Tables[0].Rows.Count >= 2)
        //                //{
        //                //    lastissueno = rq.Tables[0].Rows[1]["issuingkey"].ToString();
        //                //    Console.WriteLine(rq.Tables[0].Rows[2]["issuingkey"].ToString());
        //                //}
        //            }


        //            int xpl = 0;
        //            int laspl = 0;
        //            int i;
        //            int final = 0;
        //            List<PlCountData> x = new List<PlCountData>();//จำนวนเศษ
        //            string pltypename = "MIX";
        //            foreach (DataRow r in dr.Tables[0].Rows)
        //            {
        //                // Console.WriteLine(r["seqctn"].ToString());
        //                int[] pl_total = countPallet(int.Parse(r["seqctn"].ToString()));
        //                // Console.WriteLine(pl_total);
        //                pltypename = r["partype"].ToString();
        //                if (int.Parse(r["seqctn"].ToString()) > 44)
        //                {
        //                    i = 0;
        //                    while (i < pl_total[0])
        //                    {
        //                        laspl = new OrderControllers().GetLastPalletCtn(lastissueno, "P");
        //                        xpl = laspl + (i + 1);
        //                        // Console.WriteLine($"TYPE: {r["partype"].ToString()} PALLET: 1P" + (xpl).ToString("D3"));
        //                        string plnumber = "1P" + (xpl).ToString("D3");
        //                        var plnokey = olkey.Where(plx => plx.PlNo.Contains(plnumber)).FirstOrDefault();
        //                        // Console.WriteLine(plnokey);
        //                        string ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}', '{pltypename}',45,sysdate,sysdate)";
        //                        if (plnokey != null)
        //                        {
        //                            ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno,containerno,Pltype,pltotal,ploutsts,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}','{plnokey.PlKey}', '{plnokey.ContNo}', '{pltypename}',45,{plnokey.PlStatus},sysdate,sysdate)";
        //                        }
        //                        bool xup = new ConnDB().ExcuteSQL(ins_pl);
        //                        if (xup)
        //                        {
        //                            i++;
        //                        }
        //                    }
        //                    x.Add(new PlCountData()
        //                    {
        //                        pltype = r["partype"].ToString(),
        //                        plcount = pl_total[1]
        //                    });
        //                    laspl = new OrderControllers().GetLastPalletCtn(lastissueno, "P");
        //                }
        //                else
        //                {
        //                    if (pl_total[2] == 1)
        //                    {
        //                        laspl = new OrderControllers().GetLastPalletCtn(lastissueno, "P");
        //                        string plnumber = "1P" + (laspl + 1).ToString("D3");
        //                        var plnokey = olkey.Where(plx => plx.PlNo.Contains(plnumber)).FirstOrDefault();
        //                        // Console.WriteLine(plnokey);
        //                        string ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}', '{pltypename}',{int.Parse(r["seqctn"].ToString())},sysdate,sysdate)";
        //                        if (plnokey != null)
        //                        {
        //                            ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno, containerno,Pltype,pltotal,ploutsts,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}','{plnokey.PlKey}', '{plnokey.ContNo}', '{pltypename}',{int.Parse(r["seqctn"].ToString())},{plnokey.PlStatus},sysdate,sysdate)";
        //                        }
        //                        new ConnDB().ExcuteSQL(ins_pl);
        //                        final = pl_total[0];
        //                    }
        //                    else
        //                    {
        //                        x.Add(new PlCountData()
        //                        {
        //                            pltype = r["partype"].ToString(),
        //                            plcount = int.Parse(r["seqctn"].ToString())
        //                        });
        //                    }
        //                }
        //            }
        //            // Console.WriteLine(xpl);
        //            // Console.WriteLine(x);
        //            int total_pl = 0;
        //            foreach (var ii in x)
        //            {
        //                total_pl += ii.plcount;
        //                if (ii.plcount > 0)
        //                {
        //                    pltypename = ii.pltype;
        //                }
        //            }
        //            if (x.Count > 1)
        //            {
        //                if (x[0].plcount > 0 && x[1].plcount > 0)
        //                {
        //                    pltypename = "MIX";
        //                }
        //            }
        //            else
        //            {
        //                if (x[0].plcount > 0)
        //                {
        //                    pltypename = "MIX";
        //                }
        //            }
        //            int[] xmix = countPallet(total_pl);
        //            // Console.WriteLine(xmix[0]);//จำนวนเต็ม
        //            i = 0;
        //            while (i < xmix[0])
        //            {
        //                laspl = new OrderControllers().GetLastPalletCtn(lastissueno, "P");
        //                xpl = laspl + 1;
        //                int tt = 45;
        //                if (xmix[2] == 0)
        //                {
        //                    tt = total_pl;
        //                }
        //                // Console.WriteLine($"TYPE: MIX PALLET: 1P" + (xpl).ToString("D3"));
        //                string plnumber = "1P" + (xpl).ToString("D3");
        //                var plnokey = olkey.Where(plx => plx.PlNo.Contains(plnumber)).FirstOrDefault();
        //                string ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}', '{pltypename}',{tt},sysdate,sysdate)";
        //                if (plnokey != null)
        //                {
        //                    ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno,containerno,Pltype,pltotal,ploutsts,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}','{plnokey.PlKey}','{plnokey.ContNo}', '{pltypename}',{tt},{plnokey.PlStatus},sysdate,sysdate)";
        //                }
        //                bool xup = new ConnDB().ExcuteSQL(ins_pl);
        //                //bool xup = new ConnDB().ExcuteSQL($"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '1P{(xpl).ToString("D3")}', '{pltypename}',{tt},sysdate,sysdate)");
        //                if (xup)
        //                {
        //                    //xpl += 1;
        //                    i++;
        //                }
        //            }
        //            // Console.WriteLine(xmix[1]);//จำนวนเศษ
        //            if (xmix[1] > 17)
        //            {
        //                // Console.WriteLine($"TYPE: MIX PALLET: 1P" + (xpl + 1).ToString("D3"));
        //                laspl = new OrderControllers().GetLastPalletCtn(lastissueno, "P");
        //                string plnumber = "1P" + (laspl + 1).ToString("D3");
        //                var plnokey = olkey.Where(plx => plx.PlNo.Contains(plnumber)).FirstOrDefault();
        //                string ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}', 'MIX',{xmix[1]},sysdate,sysdate)";
        //                if (plnokey != null)
        //                {
        //                    ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno,containerno,Pltype,pltotal,ploutsts,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}','{plnokey.PlKey}','{plnokey.ContNo}', 'MIX',{xmix[1]}, {plnokey.PlStatus},sysdate,sysdate)";
        //                }
        //                bool xup = new ConnDB().ExcuteSQL(ins_pl);
        //                //bool xup = new ConnDB().ExcuteSQL($"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '1P{(xpl + 1).ToString("D3")}', 'MIX',{xmix[1]},sysdate,sysdate)");
        //                return xup;
        //            }
        //            else
        //            {
        //                int bb_a = xmix[1] / 2;
        //                int bb_b = xmix[1] - (bb_a * 2);
        //                i = 0;
        //                while (i < bb_a)
        //                {
        //                    // Console.WriteLine($"TYPE: BIGBOX PALLET: 1C" + (i + 1).ToString("D3"));
        //                    laspl = new OrderControllers().GetLastPalletCtn(lastissueno, "C");
        //                    bool xup = new ConnDB().ExcuteSQL($"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '1C{(laspl + 1).ToString("D3")}', 'BBOX',2,sysdate,sysdate)");
        //                    if (xup)
        //                    {
        //                        i++;
        //                    }
        //                }
        //                if (bb_b > 0)
        //                {
        //                    if (final == 0)
        //                    {
        //                        // Console.WriteLine($"TYPE: SMALLBOX PALLET: 1C" + (bb_b + i).ToString("D3"));
        //                        laspl = new OrderControllers().GetLastPalletCtn(lastissueno, "C");
        //                        new ConnDB().ExcuteSQL($"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '1C{(laspl + 1).ToString("D3")}', 'SBOX',1,sysdate,sysdate)");
        //                    }
        //                }
        //            }
        //        }    
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    return true;
        //}

        private List<PlListData> GetPlData(string inv)
        {
            List<PlListData> list = new List<PlListData>();
            string sql = $"select l.palletno,l.ploutno,l.containerno,CASE WHEN l.PLOUTSTS IS NULL THEN '0' ELSE l.PLOUTSTS END PLOUTSTS from txp_isspallet l where l.issuingkey = '{inv}' and l.ploutno is not null";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow i in dr.Tables[0].Rows)
            {
                list.Add(new PlListData()
                {
                    PlNo = i["palletno"].ToString(),
                    PlKey = i["ploutno"].ToString(),
                    ContNo = i["containerno"].ToString(),
                    PlStatus = int.Parse(i["ploutsts"].ToString()),
                });
            }
            return list;
        }

        private int[] countPallet(int pl)
        {
            int[] x = new int[3];
            if (pl > 44)
            {
                int pl_b = pl / 45;
                x[0] = pl_b;
                // Console.WriteLine(pl_b);//จำนวน pallet
                //createpallet(pl_b, type);
                int pl_c = pl_b * 45;
                x[1] = pl - pl_c;//จำนวนเศษ
                x[2] = 1;
            }
            else if (pl > 17)
            {
                //set 1P001]
                x[0] = 1;
                x[1] = 0;
                x[2] = 0;
            }
            else
            {
                x[0] = 0;
                x[1] = pl;
                x[2] = 2;
            }
            return x;
        }

        private bool CountPl(string inv)
        {
            string sql = $"delete TXP_ISSPALLET where issuingkey = '{inv}'";
            return new ConnDB().ExcuteSQL(sql);
        }

        public int GetShortQty(string partno, string fno, int ctn)
        {
            int x = 1;
            string sql = $"SELECT shorderqty ctn FROM TXP_ISSTRANSBODY WHERE partno = '{partno}' AND ISSUINGKEY = '{fno}'";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                x = int.Parse(r["ctn"].ToString()) + ctn;
            }
            return x;
        }

        public static string GetRewrite(string runno)
        {
            string txt = "";
            switch (runno.Substring(0, 1))
            {
                case "P":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkOrange;
                    txt = "ADD/REM.";
                    break;
                case "M":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkBlue;
                    txt = "SHIP.";
                    break;
                case "D":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkCyan;
                    txt = "ETD.";
                    break;
                case "Q":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkRed;
                    txt = "QTY.";
                    break;
                case "L":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkKhaki;
                    txt = "LOCAT.";
                    break;
                //AW
                case "0":
                    switch (runno)
                    {
                        case "00":
                            txt = "UPDATE";
                            break;
                        default:
                            break;
                    }
                    break;
                case "1":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkGreen;
                    switch (runno)
                    {
                        case "10":
                            txt = "CANCEL";
                            break;
                        case "11":
                            txt = "REMOVE";
                            break;
                        case "12":
                            txt = "REDUCE";
                            break;
                        default:
                            break;
                    }
                    break;
                case "2":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkBlue;
                    switch (runno)
                    {
                        case "20":
                            txt = "CHANGE";
                            break;
                        case "21":
                            txt = "ETD DATE";
                            break;
                        case "22":
                            txt = "SHIP";
                            break;
                        case "23":
                            txt = "SPLIT";
                            break;
                        case "24":
                            txt = "ETD. CON.";
                            break;
                        default:
                            break;
                    }
                    break;
                case "3":
                    //e.Column.AppearanceCell.ForeColor = Color.DarkOrange;
                    switch (runno)
                    {
                        case "30":
                            txt = "REP.";
                            break;
                        case "31":
                            txt = "CLAIM";
                            break;
                        case "32":
                            txt = "SHIPPING";
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            return txt + " => " + runno;
        }

        public void ErrorHadler(string msg)
        {
            XtraMessageBox.Show(msg, "Alert!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
