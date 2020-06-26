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
    public class SetPalletControllers
    {
        public List<SetPallatData> GetPartListDetail(string issuekey)
        {
            List<SetPallatData> obj = new List<SetPallatData>();
            string sql = $"SELECT * FROM TBT_SETPALLET l WHERE l.ISSUINGKEY = '{issuekey}' ORDER BY PLSIZE ";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new SetPallatData()
                { 
                    Id = obj.Count + 1,
                    Factory=r["factory"].ToString(),//FACTORY
                    ShipType = r["shiptype"].ToString(),//SHIPTYPE
                    ZName = r["zname"].ToString(),//ZNAME
                    EtdDte = DateTime.Parse(r["etddte"].ToString()),//ETDDTE
                    AffCode = r["affcode"].ToString(),//AFFCODE
                    CustCode = r["bishpc"].ToString(),//BISHPC
                    CustName = r["custname"].ToString(),//CUSTNAME
                    CombInv = r["combinv"].ToString(),//COMBINV
                    RefInv = r["refinvoice"].ToString(),//REFINVOICE
                    RefNo = r["issuingkey"].ToString(),//ISSUINGKEY
                    OrderNo = r["orderno"].ToString(),//ORDERNO
                    PName = r["pname"].ToString(),//PNAME
                    PartNo = r["partno"].ToString(),//PARTNO
                    PartName = r["partname"].ToString(),//PARTNAME
                    PlSize = r["plsize"].ToString(),//PLSIZE
                    Ctn = int.Parse(r["ctn"].ToString()),//CTN
                    ShipPlNo = r["shipplno"].ToString(),//SHIPPLNO
                });
            }
            return obj;
        }

        public List<SetPallatData> GetPartListCompletedDetail(string issuekey)
        {
            List<SetPallatData> obj = new List<SetPallatData>();
            string sql = $"SELECT l.PALLETNO,l.PLTOTAL,l.PLTYPE FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY = '{issuekey}' AND l.PALLETNO LIKE '%P%'\n"+
                           "ORDER BY l.PALLETNO ";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new SetPallatData()
                {
                    Id = obj.Count + 1,
                    //Factory = r["factory"].ToString(),//FACTORY
                    //ShipType = r["shiptype"].ToString(),//SHIPTYPE
                    //ZName = r["zname"].ToString(),//ZNAME
                    //EtdDte = DateTime.Parse(r["etddte"].ToString()),//ETDDTE
                    //AffCode = r["affcode"].ToString(),//AFFCODE
                    //CustCode = r["bishpc"].ToString(),//BISHPC
                    //CustName = r["custname"].ToString(),//CUSTNAME
                    //CombInv = r["combinv"].ToString(),//COMBINV
                    //RefInv = r["refinvoice"].ToString(),//REFINVOICE
                    //RefNo = r["issuingkey"].ToString(),//ISSUINGKEY
                    //OrderNo = r["orderno"].ToString(),//ORDERNO
                    //PName = r["pname"].ToString(),//PNAME
                    //PartNo = r["partno"].ToString(),//PARTNO
                    //PartName = r["partname"].ToString(),//PARTNAME
                    PlSize = r["pltype"].ToString(),//PLSIZE
                    Ctn = int.Parse(r["pltotal"].ToString()),//CTN
                    ShipPlNo = r["palletno"].ToString(),//SHIPPLNO
                });
            }
            return obj;
        }
    }
}
