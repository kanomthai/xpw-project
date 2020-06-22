using System;
using System.Collections.Generic;
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
                });
            }
            return obj;
        }

        public List<BookingInvoicePallet> GetInvoicePl(string issuekey)
        {
            string sql = $"SELECT l.ISSUINGKEY,l.PLTYPE,l.PALLETNO,l.PLTOTAL,l.PLOUTNO,l.PLOUTSTS FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY = '{issuekey}'";
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
                });
            }
            return obj;
        }

        public bool AddBooking(BookingInvoicePallet obj)
        {
            return true;
        }
    }
}
