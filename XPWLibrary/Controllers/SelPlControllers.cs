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
    public class SelPlControllers
    {
        public List<SetPlData> GetPlData(string refinv)
        {
            List<SetPlData> obj = new List<SetPlData>();
            string sql = $"SELECT * FROM TBT_PLDIM d where d.issuingkey = '{refinv}'";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                string pltype = new GreeterFunction().GetPlSize(r["pldim"].ToString(), int.Parse(r["ctn"].ToString()));
                string plno = new GreeterFunction().GetPlList(r["issuingkey"].ToString(), pltype);
                int plqty = new GreeterFunction().GetPlQty(r["issuingkey"].ToString(), plno);
                obj.Add(new SetPlData()
                {
                    id = obj.Count + 1,
                    RefInv = r["refinvoice"].ToString(),
                    RefNo = r["issuingkey"].ToString(),
                    plno = plno,
                    pldim = r["pldim"].ToString(),
                    pltype = pltype,
                    qty = plqty,
                    ctn = int.Parse(r["ctn"].ToString()),
                    unit = r["unit"].ToString(),
                });
            }
            return obj;
        }
    }
}
