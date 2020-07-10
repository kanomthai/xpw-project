using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace XPWLibrary.Controllers
{
    public class BookingControllers
    {
        public List<BookingCustomerData> GetCustomer(DateTime etd)
        {
            List<BookingCustomerData> obj = new List<BookingCustomerData>();
            string sql = "SELECT e.CUSTNAME FROM TXP_ISSTRANSENT e\n" +
                        $"WHERE e.ETDDTE = TO_DATE('{etd.ToString("dd/MM/yyyy")}', 'dd/MM/yyyy')\n" +
                        "GROUP BY e.CUSTNAME\n" +
                        "ORDER BY e.CUSTNAME";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new BookingCustomerData()
                {
                    id = obj.Count,
                    custname = r["custname"].ToString(),
                });
            }
            return obj;
        }

        public List<Bookings> GetContainerList(DateTime d)
        {
            string sql = $"select '' custname,c.etddte,c.containerno,c.sealno,c.containersize,'' invoice,0 grossweight,0 netweight,0 plcount," +
                $"min(l.PLOUTSTS) loadsts,0 closed,c.RELEASEDTE from txp_loadcontainer c\n" +
                "INNER JOIN TXP_ISSPALLET l ON c.CONTAINERNO = l.CONTAINERNO \n" +
                $"where c.etddte = to_date('{d.ToString("dd/MM/yyyy")}', 'dd/MM/yyyy')\n" +
                $"GROUP BY c.etddte,c.containerno,c.sealno,c.containersize,c.RELEASEDTE";
            DataSet dr = new ConnDB().GetFill(sql);
            Console.WriteLine(sql);
            Console.WriteLine("========== GetLoadContainer ===========");
            List<Bookings> list = new List<Bookings>();
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new Bookings()
                {
                    Id = list.Count + 1,
                    ContainerNo = r["containerno"].ToString(),
                    ContainerSize = r["containersize"].ToString(),
                    Invoice = r["invoice"].ToString(),
                    SealNo = r["sealno"].ToString(),
                    //Custname = r["custname"].ToString(),
                    Pallet = GetCountContainer(r["containerno"].ToString()),
                    LoadStatus = int.Parse(r["loadsts"].ToString()),
                    CloseStatus = int.Parse(r["closed"].ToString()),
                    GrossWeight = int.Parse(r["grossweight"].ToString()),
                    NetWeight = int.Parse(r["netweight"].ToString()),
                    Etd = DateTime.Parse(r["etddte"].ToString()),
                    ReleaseDate = DateTime.Parse(r["releasedte"].ToString()),
                });
            }
            return list;
        }

        private int GetCountContainer(string v)
        {
            string sql = $"SELECT count(*) ponocount FROM TXP_ISSPALLET l WHERE l.CONTAINERNO = '{v}'";
            Console.WriteLine(sql);
            Console.WriteLine($"============ GetCountContainer(string {v}) ==================");
            DataSet dr = new ConnDB().GetFill(sql);
            int x = 0;
            if (dr.Tables.Count < 1)
            {
                return x;
            }
            if (dr.Tables[0].Rows.Count > 0)
            {
                x = int.Parse(dr.Tables[0].Rows[0]["ponocount"].ToString());
            }
            return x;
        }

        public List<Bookings> GetContainerListDetail(string containerno)
        {
            string sql = $"SELECT l.PALLETNO,l.PLOUTNO,c.etddte,l.issuingkey,l.custname,c.containersize FROM TXP_LOADCONTAINER c\n" +
                        $"INNER JOIN TXP_ISSPALLET l ON c.CONTAINERNO = l.CONTAINERNO\n" +
                        $"WHERE l.containerno = '{containerno}' and l.booked = 6\n" +
                        $"ORDER BY l.custname,l.issuingkey,l.PALLETNO";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            List<Bookings> list = new List<Bookings>();
            List<string> cust = new List<string>();
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                List<string> x = GetPonoListByPl(r["ploutno"].ToString());
                string l = string.Join(",", x);
                if (l.Length > 50)
                {
                    l = l.Substring(0, 20) + "....";
                }
                list.Add(new Bookings()
                {
                    Id = list.Count + 1,
                    Etd = DateTime.Parse(r["etddte"].ToString()),
                    Invoice = r["issuingkey"].ToString(),
                    ContainerSize = r["containersize"].ToString(),
                    Custname = r["custname"].ToString(),
                    PlNo = r["palletno"].ToString(),
                    PlOutNo = r["ploutno"].ToString(),
                    OrderNo = l,
                });
                var c = cust.FindAll(i => i.Contains(r["custname"].ToString()));
                if (c.Count < 1)
                {
                    cust.Add(r["custname"].ToString());
                }
            }
            list[0].Custname = string.Join(",", cust);
            return list;
        }

        private List<string> GetPonoListByPl(string plno)
        {
            string sql = $"SELECT pono FROM TXP_CARTONDETAILS c WHERE c.PLOUTNO = '{plno}' GROUP BY pono ORDER BY pono";
            DataSet dr = new ConnDB().GetFill(sql);
            List<string> x = new List<string>();
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                //x[i] = r["orderid"].ToString();
                //i++;
                Console.WriteLine(r["pono"].ToString());
                x.Add(r["pono"].ToString());
            }
            return x;
        }

        public List<BookingPlData> GetPalletInvoice(DateTime etd, string custname)
        {
            string sql = $"SELECT e.ISSUINGKEY,e.REFINVOICE,bb.pono,CASE WHEN l.pl IS NULL THEN 0 ELSE l.pl END pl,CASE WHEN bl.cctn IS NULL THEN 0 ELSE bl.cctn END booked,CASE WHEN ll.cctn IS NULL THEN 0 ELSE ll.cctn END nbook FROM TXP_ISSTRANSENT e \n" +
                        "INNER JOIN(\n"+
                        "    SELECT b.ISSUINGKEY, count(b.PONO) pono FROM TXP_ISSTRANSBODY b GROUP BY b.ISSUINGKEY\n"+
                        ") bb ON e.ISSUINGKEY = bb.ISSUINGKEY\n"+
                        "LEFT JOIN(\n"+
                        "    SELECT l.ISSUINGKEY, count(l.CONTAINERNO) cctn FROM TXP_ISSPALLET l WHERE l.CONTAINERNO IS NOT NULL GROUP BY l.ISSUINGKEY\n"+
                        ") bl ON e.ISSUINGKEY = bl.ISSUINGKEY\n"+
                        "LEFT JOIN(\n"+
                        "    SELECT l.ISSUINGKEY, count(l.CONTAINERNO) cctn FROM TXP_ISSPALLET l WHERE l.CONTAINERNO IS NULL GROUP BY l.ISSUINGKEY\n"+
                        ") ll ON e.ISSUINGKEY = ll.ISSUINGKEY\n"+
                        "LEFT JOIN(\n"+
                        "    SELECT tl.ISSUINGKEY, count(tl.PALLETNO) pl FROM TXP_ISSPALLET tl  WHERE tl.PALLETNO LIKE '%P%' GROUP BY tl.ISSUINGKEY\n" +
                        ")l ON e.ISSUINGKEY = l.ISSUINGKEY\n"+
                        $"WHERE e.ETDDTE = TO_DATE('{etd.ToString("dd/MM/yyyy")}', 'dd/MM/yyyy') AND e.CUSTNAME = '{custname}'";
            Console.WriteLine(sql);
            List<BookingPlData> obj = new List<BookingPlData>();
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new BookingPlData()
                {
                    id = obj.Count + 1,
                    issuekey = r["issuingkey"].ToString(),
                    refinv = r["refinvoice"].ToString(),
                    poctn = int.Parse(r["pono"].ToString()),
                    plctn = int.Parse(r["pl"].ToString()),
                    booked = int.Parse(r["booked"].ToString()),
                    ubook = int.Parse(r["pl"].ToString()) - int.Parse(r["booked"].ToString()),
                    custname = custname,
                });
            }
            return obj;
        }

        public List<BookingInvoicePallet> GetInvoicePl(string issuekey)
        {
            string sql = $"SELECT * FROM TBT_BOOKINGLIST WHERE ISSUINGKEY = '{issuekey}' AND BOOKED < 4";
            List<BookingInvoicePallet> obj = new List<BookingInvoicePallet>();
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new BookingInvoicePallet()
                {
                    id = obj.Count + 1,
                    issuekey = r["issuingkey"].ToString(),
                    pltype = r["pltype"].ToString(),
                    plno = r["palletno"].ToString(),
                    pltotal = int.Parse(r["pltotal"].ToString()),
                    ploutno = r["ploutno"].ToString(),
                    plstatus = int.Parse(r["ploutsts"].ToString()),
                    custname = r["custname"].ToString(),
                });
            }
            return obj;
        }

        public List<BookingInvoicePallet> GetInvoicePlBooked(string issuekey)
        {
            string sql = $"SELECT * FROM TBT_BOOKINGLIST WHERE ISSUINGKEY = '{issuekey}' AND BOOKED > 3";
            List<BookingInvoicePallet> obj = new List<BookingInvoicePallet>();
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new BookingInvoicePallet()
                {
                    id = obj.Count + 1,
                    issuekey = r["issuingkey"].ToString(),
                    pltype = r["pltype"].ToString(),
                    plno = r["palletno"].ToString(),
                    pltotal = int.Parse(r["pltotal"].ToString()),
                    ploutno = r["ploutno"].ToString(),
                    plstatus = int.Parse(r["ploutsts"].ToString()),
                    custname = r["custname"].ToString(),
                });
            }
            return obj;
        }

        public bool AddBooking(BookingInvoicePallet obj)
        {
            return true;
        }

        public List<BookingInvoicePallet> GetContainerDeaitl(string conns)
        {
            List<BookingInvoicePallet> list = new List<BookingInvoicePallet>();
            string sql = $"SELECT e.FACTORY,e.ISSUINGKEY,l.PALLETNO,l.PONO,l.CUSTNAME,l.PLTYPE,l.PLOUTNO,l.PLOUTSTS,l.PLTOTAL,l.CONTAINERNO,l.BOOKED,l.PLWIDE,l.PLLENG,l.PLHIGHT,e.CUSTNAME,e.REFINVOICE FROM txp_isspallet l\n" +
                        $"INNER JOIN TXP_ISSTRANSENT e ON l.ISSUINGKEY = e.ISSUINGKEY WHERE l.containerno = '{conns}'\n" +
                        $"order by l.PLOUTNO,l.PALLETNO,l.PONO";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(new BookingInvoicePallet()
                {
                    id = list.Count + 1,
                    issuekey = r["issuingkey"].ToString(),
                    refinv = r["refinvoice"].ToString(),
                    pltype = r["pltype"].ToString(),
                    plno = r["palletno"].ToString(),
                    pltotal = int.Parse(r["pltotal"].ToString()),
                    ploutno = r["ploutno"].ToString(),
                    plstatus = int.Parse(r["ploutsts"].ToString()),
                    custname = r["custname"].ToString(),
                });
            }
            return list;
        }
    }
}
