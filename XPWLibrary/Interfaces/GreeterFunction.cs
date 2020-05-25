using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using XPWLibrary.Models;

namespace XPWLibrary.Interfaces
{
    public class GreeterFunction
    {
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
            return true;
        }

        public string GetLastInvoice(string refinv)
        {
            string txt = "TO";
            string sql = "SELECT e.FACTORY,e.AFFCODE,e.BISHPC,e.CUSTNAME,count(e.CUSTNAME) + 1 rnum FROM TXP_ISSTRANSENT e  \n" +
                $"WHERE e.ISSUINGKEY LIKE '{refinv.Substring(0, 3)}%' AND TO_CHAR(e.ETDDTE, 'yyyy') = TO_CHAR(SYSDATE, 'yyyy') AND LENGTH(e.REFINVOICE) = 10\n" +
                "GROUP BY e.FACTORY,e.AFFCODE,e.BISHPC,e.CUSTNAME";
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
                            PlNo = 0,
                            Total = ctn,
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
                            PlNo = a,
                            Total = 20,
                        });
                        var b = ctn - (a * 20);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "TOTAL",
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
                            PlSize = "TOTAL",
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
                                PlSize = "TOTAL",
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
                            PlSize = "TOTAL",
                            PlNo = 0,
                            Total = ctn,
                        });
                    }
                    break;
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
                                PlSize = "TOTAL",
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
                            PlSize = "TOTAL",
                            PlNo = 0,
                            Total = ctn,
                        });
                    }
                    break;
                case "38x56x39":
                    plgroup = "3";
                    if (ctn < 18)
                    {
                        obj.Add(new INJPlData()
                        {
                            PlGroup = plgroup,
                            PSize = partsize,
                            PlSize = "TOTAL",
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
                            PlNo = a,
                            Total = 30,
                        });
                        var b = ctn - (a*30);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "",
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
                            PlSize = "",
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
                            PlNo = a,
                            Total = 36,
                        });
                        var b = ctn - (a * 36);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "",
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
                            PlSize = "",
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
                            PlNo = a,
                            Total = 30,
                        });
                        var b = ctn - (a * 30);
                        if (b > 0)
                        {
                            obj.Add(new INJPlData()
                            {
                                PlGroup = plgroup,
                                PSize = partsize,
                                PlSize = "",
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

        public int GetInvoiceStatus(string refNo)
        {
            if (refNo == "")
            {
                return 0;
            }
            //string sql = $"SELECT e.ISSUINGSTATUS + 1 FROM TXP_ISSTRANSENT e WHERE e.ISSUINGKEY = '{refNo}'";
            //DataSet dr = new ConnDB().GetFill(sql);
            //if (dr.Tables[0].Rows.Count < 1)
            //{
            //    sql = $"SELECT p.orderstatus FROM TXP_ORDERPLAN p WHERE p.curinv = '{refNo}'";
            //    dr = new ConnDB().GetFill(sql);
            //    if (dr.Tables[0].Rows.Count < 1)
            //    {
            //        return 0;
            //    }
            //    return int.Parse(dr.Tables[0].Rows[0][0].ToString());
            //}
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
                    switch ($"{abt}{factory}")
                    {
                        case "4INJ":
                        case "1AW":
                            n = "LOAD AT CK2";
                            break;
                        case "2AW":
                        case "2INJ":
                            n = "LOAD AT NESS";
                            break;
                        case "3AW":
                        case "3INJ":
                            n = "LOAD AT ICAM";
                            break;
                        case "1INJ":
                        case "4AW":
                            n = "LOAD AT CK1";
                            break;
                        default:
                            n = "";
                            break;
                    }
                    break;
                case 2:
                    switch ($"{abt}{factory}")
                    {
                        case "1INJ":
                            n = "DOMESTIC";
                            break;
                        case "4INJ":
                            n = "BONDED";
                            break;
                        case "1AW":
                            n = "BONDED";
                            break;
                        default:
                            n = "";
                            break;
                    }
                    break;
                default:
                    switch ($"{shiptype}{abt}{factory}")
                    {
                        case "B4INJ":
                        case "B1AW":
                            n = "FCL"; break;
                        case "B2AW":
                        case "B3AW":
                        case "B4AW":
                        case "B1INJ":
                        case "B2INJ":
                        case "B3INJ":
                            n = "LCL";
                            break;
                        default:
                            n = "";
                            break;
                    }
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

        public bool SumPlInj(string invoice)
        {
            bool x = false;
            string sql = $"SELECT sum(round(b.ORDERQTY/b.STDPACK)) ctn,p.BIWIDT||'x'||p.BILENG||'x'||p.BIHIGH dm FROM TXP_ISSTRANSBODY b\n"+
                        $"INNER JOIN TXP_ORDERPLAN p ON b.ISSUINGKEY = p.CURINV AND b.PARTNO = p.PARTNO\n"+
                        $"WHERE b.ISSUINGKEY = '{invoice}'\n"+
                        $"GROUP BY p.BIWIDT || 'x' || p.BILENG || 'x' || p.BIHIGH";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);

            //================================>
            List<INJPlData> ob = new List<INJPlData>();
            List<INJPlData> opl = new List<INJPlData>();
            int plnum = 1;
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                List<INJPlData> obj = PlInjSize(r["dm"].ToString(), int.Parse(r["ctn"].ToString()));
                int i = 0;
                while (i < obj.Count)
                {
                    Console.WriteLine($"P => {obj[i].PlNo}");
                    if (obj[i].PlNo > 0)
                    {
                        int j = 0;
                        while (j < obj[i].PlNo)
                        {
                            obj[i].PlName = $"1P{plnum.ToString("D3")}";
                            ob.Add(obj[i]);
                            Console.WriteLine($"PARTSIZE: {obj[i].PSize} PLSIZE: {obj[i].PlSize} PLKEY: {obj[i].PlName}");
                            plnum++;
                            j++;
                        }
                    }
                    else
                    {
                        opl.Add(obj[i]);
                    }
                    i++;
                }
            }
            Console.WriteLine($"FULLPL: {ob.Count} BBOX: {opl.Count}");
            int bbctn = 1;
            if (opl.Count > 0)
            {
                List<INJPlData> b = opl.OrderBy(rd => rd.PlGroup).ToList<INJPlData>();
                int r = 0;
                int sumpl = 0;
                string plg = null;
                string psize = null;
                while (r < b.Count)
                {
                    Console.WriteLine(b[r].PlGroup);
                    if (plg == null)
                    {
                        plg = b[r].PlGroup;
                        sumpl = b[r].Total;
                        psize = b[r].PSize;
                        Console.WriteLine($"START NEW GRP:{plg} PSIZE: {psize} SUM:{sumpl}");
                    }
                    else
                    {
                        if (plg != b[r].PlGroup)
                        {
                            //Start new PL
                            List<INJPlData> obj = PlInjSize(psize, sumpl);
                            int i = 0;
                            while (i < obj.Count)
                            {
                                Console.WriteLine($"P => {obj[i].PlNo}");
                                if (obj[i].PlNo > 0)
                                {
                                    int j = 0;
                                    while (j < obj[i].PlNo)
                                    {
                                        obj[i].PlName = $"1P{plnum.ToString("D3")}";
                                        Console.WriteLine($"PARTSIZE: {obj[i].PSize} PLSIZE: {obj[i].PlSize} PLKEY: {obj[i].PlName}");
                                        plnum++;
                                        j++;
                                    }
                                }
                                else
                                {
                                    //opl.Add(obj[i]);
                                    int j = 0;
                                    while (j < obj[i].Total)
                                    {
                                        obj[i].PlName = $"1C{bbctn.ToString("D3")}";
                                        Console.WriteLine($"PARTSIZE: {obj[i].PSize} PLSIZE: {obj[i].PlSize} PLKEY: {obj[i].PlName}");
                                        bbctn++;
                                        j++;
                                    }
                                }
                                i++;
                            }

                            plg = b[r].PlGroup;
                            sumpl = b[r].Total;
                            psize = b[r].PSize;
                            Console.WriteLine($"START NEW GRP:{plg} PSIZE: {psize} SUM:{sumpl}");
                        }
                        else
                        {
                            sumpl += b[r].Total;
                        }
                    }
                    Console.WriteLine($"GROUP: {plg} SUM: {sumpl}");
                    r++;
                    if (r >= b.Count)
                    {
                        //start pl ending
                        List<INJPlData> obj = PlInjSize(psize, sumpl);
                        int i = 0;
                        while (i < obj.Count)
                        {
                            Console.WriteLine($"P => {obj[i].PlNo}");
                            if (obj[i].PlNo > 0)
                            {
                                int j = 0;
                                while (j < obj[i].PlNo)
                                {
                                    obj[i].PlName = $"1P{plnum.ToString("D3")}";
                                    ob.Add(obj[i]);
                                    Console.WriteLine($"PARTSIZE: {obj[i].PSize} PLSIZE: {obj[i].PlSize} PLKEY: {obj[i].PlName}");
                                    plnum++;
                                    j++;
                                }
                            }
                            else
                            {
                                //opl.Add(obj[i]);
                                int j = 0;
                                while (j < obj[i].Total)
                                {
                                    obj[i].PlName = $"1C{bbctn.ToString("D3")}";
                                    Console.WriteLine($"PARTSIZE: {obj[i].PSize} PLSIZE: {obj[i].PlSize} PLKEY: {obj[i].PlName}");
                                    bbctn++;
                                    j++;
                                }
                            }
                            i++;
                        }
                    }
                }
            }
            return x;
        }

        public bool SumPallet(string inv)
        {
            try
            {
                SplashScreenManager.Default.SetWaitFormDescription($"SUMMARY PL.");
                string fac = "AW";
                if (inv.Substring(0, 1) == "I")
                {
                    fac = "INJ";
                    return true;
                }
                else
                {
                    BindingList<PlListData> olkey = GetPlData(inv);
                    bool repl = CountPl(inv);
                    //// Console.WriteLine(repl);
                    //string sql = $"SELECT SUBSTR(d.PARTNO, 0, 3) partype,count(d.PARTNO) seqctn FROM TXP_ISSPACKDETAIL d  WHERE d.ISSUINGKEY = '{inv}' GROUP BY SUBSTR(d.PARTNO, 0, 3) order by seqctn desc";
                    string sql = $"SELECT partype,sum(ctn) seqctn FROM (\n" +
                                $"    SELECT SUBSTR(p.PARTNO, 0, 3) partype,p.BALQTY / p.BISTDP ctn FROM TXP_ORDERPLAN p WHERE p.CURINV = '{inv}' AND p.STATUS = 1\n" +
                                $") GROUP BY partype\n" +
                                $"ORDER BY seqctn DESC";
                    // Console.WriteLine(sql);
                    DataSet dr = new ConnDB().GetFill(sql);
                    int xpl = 0;
                    int laspl = 0;
                    int i;
                    int final = 0;
                    List<PlCountData> x = new List<PlCountData>();//จำนวนเศษ
                    string pltypename = "MIX";
                    foreach (DataRow r in dr.Tables[0].Rows)
                    {
                        // Console.WriteLine(r["seqctn"].ToString());
                        int[] pl_total = countPallet(int.Parse(r["seqctn"].ToString()));
                        // Console.WriteLine(pl_total);
                        pltypename = r["partype"].ToString();
                        if (int.Parse(r["seqctn"].ToString()) > 44)
                        {
                            i = 0;
                            while (i < pl_total[0])
                            {
                                xpl = laspl + (i + 1);
                                // Console.WriteLine($"TYPE: {r["partype"].ToString()} PALLET: 1P" + (xpl).ToString("D3"));
                                string plnumber = "1P" + (xpl).ToString("D3");
                                var plnokey = olkey.Where(plx => plx.PlNo.Contains(plnumber)).FirstOrDefault();
                                // Console.WriteLine(plnokey);
                                string ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}', '{pltypename}',45,sysdate,sysdate)";
                                if (plnokey != null)
                                {
                                    ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno,containerno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}','{plnokey.PlKey}', '{plnokey.ContNo}', '{pltypename}',45,sysdate,sysdate)";
                                }
                                bool xup = new ConnDB().ExcuteSQL(ins_pl);
                                if (xup)
                                {
                                    i++;
                                }
                            }
                            x.Add(new PlCountData()
                            {
                                pltype = r["partype"].ToString(),
                                plcount = pl_total[1]
                            });
                            laspl = xpl;
                        }
                        else
                        {
                            if (pl_total[2] == 1)
                            {
                                string plnumber = "1P" + (laspl + 1).ToString("D3");
                                var plnokey = olkey.Where(plx => plx.PlNo.Contains(plnumber)).FirstOrDefault();
                                // Console.WriteLine(plnokey);
                                string ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}', '{pltypename}',{int.Parse(r["seqctn"].ToString())},sysdate,sysdate)";
                                if (plnokey != null)
                                {
                                    ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno, containerno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}','{plnokey.PlKey}', '{plnokey.ContNo}', '{pltypename}',{int.Parse(r["seqctn"].ToString())},sysdate,sysdate)";
                                }
                                new ConnDB().ExcuteSQL(ins_pl);
                                final = pl_total[0];
                            }
                            else
                            {
                                x.Add(new PlCountData()
                                {
                                    pltype = r["partype"].ToString(),
                                    plcount = int.Parse(r["seqctn"].ToString())
                                });
                            }
                        }
                    }
                    // Console.WriteLine(xpl);
                    // Console.WriteLine(x);
                    int total_pl = 0;
                    foreach (var ii in x)
                    {
                        total_pl += ii.plcount;
                        if (ii.plcount > 0)
                        {
                            pltypename = ii.pltype;
                        }
                    }
                    if (x.Count > 1)
                    {
                        if (x[0].plcount > 0 && x[1].plcount > 0)
                        {
                            pltypename = "MIX";
                        }
                    }
                    else
                    {
                        if (x[0].plcount > 0)
                        {
                            pltypename = "MIX";
                        }
                    }
                    int[] xmix = countPallet(total_pl);
                    // Console.WriteLine(xmix[0]);//จำนวนเต็ม
                    i = 0;
                    while (i < xmix[0])
                    {
                        xpl = laspl + (i + 1);
                        int tt = 45;
                        if (xmix[2] == 0)
                        {
                            tt = total_pl;
                        }
                        // Console.WriteLine($"TYPE: MIX PALLET: 1P" + (xpl).ToString("D3"));
                        string plnumber = "1P" + (xpl).ToString("D3");
                        var plnokey = olkey.Where(plx => plx.PlNo.Contains(plnumber)).FirstOrDefault();
                        string ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}', '{pltypename}',{tt},sysdate,sysdate)";
                        if (plnokey != null)
                        {
                            ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno,containerno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}','{plnokey.PlKey}','{plnokey.ContNo}', '{pltypename}',{tt},sysdate,sysdate)";
                        }
                        bool xup = new ConnDB().ExcuteSQL(ins_pl);
                        //bool xup = new ConnDB().ExcuteSQL($"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '1P{(xpl).ToString("D3")}', '{pltypename}',{tt},sysdate,sysdate)");
                        if (xup)
                        {
                            //xpl += 1;
                            i++;
                        }
                    }
                    // Console.WriteLine(xmix[1]);//จำนวนเศษ
                    if (xmix[1] > 17)
                    {
                        // Console.WriteLine($"TYPE: MIX PALLET: 1P" + (xpl + 1).ToString("D3"));
                        string plnumber = "1P" + (xpl + 1).ToString("D3");
                        var plnokey = olkey.Where(plx => plx.PlNo.Contains(plnumber)).FirstOrDefault();
                        string ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}', 'MIX',{xmix[1]},sysdate,sysdate)";
                        if (plnokey != null)
                        {
                            ins_pl = $"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,ploutno,containerno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '{plnumber}','{plnokey.PlKey}','{plnokey.ContNo}', 'MIX',{xmix[1]},sysdate,sysdate)";
                        }
                        bool xup = new ConnDB().ExcuteSQL(ins_pl);
                        //bool xup = new ConnDB().ExcuteSQL($"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '1P{(xpl + 1).ToString("D3")}', 'MIX',{xmix[1]},sysdate,sysdate)");
                        return xup;
                    }
                    else
                    {
                        int bb_a = xmix[1] / 2;
                        int bb_b = xmix[1] - (bb_a * 2);
                        i = 0;
                        while (i < bb_a)
                        {
                            // Console.WriteLine($"TYPE: BIGBOX PALLET: 1C" + (i + 1).ToString("D3"));
                            bool xup = new ConnDB().ExcuteSQL($"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '1C{(i + 1).ToString("D3")}', 'BBOX',2,sysdate,sysdate)");
                            if (xup)
                            {
                                i++;
                            }
                        }
                        if (bb_b > 0)
                        {
                            if (final == 0)
                            {
                                // Console.WriteLine($"TYPE: SMALLBOX PALLET: 1C" + (bb_b + i).ToString("D3"));
                                new ConnDB().ExcuteSQL($"insert into TXP_ISSPALLET(Factory,issuingkey,Palletno,Pltype,pltotal,Sysdte,Upddte)values('{fac}', '{inv}', '1C{(bb_b + i).ToString("D3")}', 'SBOX',1,sysdate,sysdate)");
                            }
                        }
                    }
                }    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return true;
        }

        private BindingList<PlListData> GetPlData(string inv)
        {
            BindingList<PlListData> list = new BindingList<PlListData>();
            string sql = $"select l.palletno,l.ploutno,l.containerno from txp_isspallet l where l.issuingkey = '{inv}' and l.ploutno is not null";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow i in dr.Tables[0].Rows)
            {
                list.Add(new PlListData()
                {
                    PlNo = i["palletno"].ToString(),
                    PlKey = i["ploutno"].ToString(),
                    ContNo = i["containerno"].ToString(),
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
    }
}
