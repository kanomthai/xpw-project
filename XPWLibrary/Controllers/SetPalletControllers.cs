using DevExpress.XtraEditors.Filtering.Templates;
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
            string sql = $"SELECT * FROM TBT_SETPALLET l WHERE l.ISSUINGKEY = '{issuekey}' AND CTN > 0 ORDER BY PLSIZE ";
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
                    CtnQty = 0,
                    ShipPlNo = r["shipplno"].ToString(),//SHIPPLNO
                });
            }
            return obj;
        }

        public List<SetPallatData> GetPartListCompletedDetail(string issuekey)
        {
            List<SetPallatData> obj = new List<SetPallatData>();
            string sql = $"SELECT * FROM TXP_ISSPALLET l WHERE l.ISSUINGKEY = '{issuekey}' AND l.PALLETNO LIKE '%P%'\n"+
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
                    RefNo = r["issuingkey"].ToString(),//ISSUINGKEY
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

        public List<SetPalletListData> GetJobListPallet(string refinv)
        {
            string sql = $"SELECT * FROM TBT_PALLETREPORT e WHERE e.ISSUINGKEY = '{refinv}'";
            List<SetPalletListData> obj = new List<SetPalletListData>();
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new SetPalletListData()
                {
                    Id = obj.Count + 1,
                    EtdDte = DateTime.Parse(r["etddte"].ToString()),
                    RefInv = r["refinvoice"].ToString(),
                    RefNo = r["issuingkey"].ToString(),
                    OrderNo = r["pono"].ToString(),
                    ShipPlNo = r["shipplno"].ToString().Substring(2),
                    PartNo = r["partno"].ToString(),
                    ContainerType = r["containertype"].ToString(),
                    Qty = int.Parse(r["orderqty"].ToString()),
                    StdPack = int.Parse(r["stdpack"].ToString()),
                    Ctn = int.Parse(r["ctn"].ToString()),
                    ITem = int.Parse(r["seq"].ToString()),
                    PlSize = r["plsize"].ToString(),//PLSIZE
                    CombInv = r["potype"].ToString(),//POTYPE
                    LotNo = r["lotno"].ToString(),//LOTNO
                    AffCode = r["affcode"].ToString(),//AFFCODE
                    CustCode = r["bishpc"].ToString(),//BISHPC
                    CustName = r["custname"].ToString(),//CUSTNAME
                    ShipType = r["shiptype"].ToString(),//SHIPTYPE
                    Factory = r["factory"].ToString(),//FACTORY
                    Note1 = r["note1"].ToString(),//NOTE1
                    Note2 = r["note2"].ToString(),//NOTE2
                    Note3 = r["note3"].ToString(),//NOTE3
                    ZCode = r["zonecode"].ToString(),//ZONECODE
                });
            }
            return obj;
        }

        public List<SetPalletListData> GetPallatePartList(SetPallatData x)
        {
            List<SetPalletListData> obj = new List<SetPalletListData>();
            string sql = $"SELECT * FROM TBT_PALLETVIEWER p WHERE p.ISSUINGKEY = '{x.RefNo}' AND p.SHIPPLNO = '{x.ShipPlNo}'";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new SetPalletListData()
                {
                    Id = obj.Count + 1,
                    Factory = r["factory"].ToString(),
                    ShipType = r["shiptype"].ToString(),
                    ZName = r["zname"].ToString(),
                    EtdDte = DateTime.Parse(r["etddte"].ToString()),
                    AffCode = r["affcode"].ToString(),
                    CustCode = r["custcode"].ToString(),
                    CustName = r["custname"].ToString(),
                    CombInv = r["combinv"].ToString(),
                    RefInv = r["refinvoice"].ToString(),
                    RefNo = r["issuingkey"].ToString(),
                    OrderNo = r["orderno"].ToString(),
                    PName = r["pname"].ToString(),
                    ShipPlNo = r["shipplno"].ToString(),
                    PlOutNo = r["ploutno"].ToString(),
                    PlSize = r["plsize"].ToString(),
                    PartNo = r["partno"].ToString(),
                    PartName = r["partname"].ToString(),
                    FTicket = r["fticketno"].ToString(),
                    SerialNo = r["ctnsn"].ToString(),
                    LotNo = r["lotno"].ToString(),
                    Qty = int.Parse(r["orderqty"].ToString()),
                    StdPack = int.Parse(r["stdpack"].ToString()),
                    Ctn = int.Parse(r["ctn"].ToString()),
                    PrePareCtn = int.Parse(r["preparectn"].ToString()),
                    ShCtn = int.Parse(r["shctn"].ToString()),
                    WaitCtn = int.Parse(r["waitctn"].ToString()),
                    ITem = int.Parse(r["item"].ToString()),
                    Status = int.Parse(r["ploutsts"].ToString()),
                });
            }
            return obj;
        }

        public bool UpdatePalletSize(SetPallatData obj)
        {
            string sql = $"UPDATE TXP_ISSPALLET l SET l.PLTYPE='{obj.PlSize}' WHERE l.ISSUINGKEY = '{obj.RefNo}' AND l.PALLETNO = '{obj.ShipPlNo}'";
            return new ConnDB().ExcuteSQL(sql);
        }

        public bool UpdatePallet(SetPallatData obj)
        {
            bool x = false;
            string ssql = $"SELECT PLOUTSTS status FROM TXP_ISSPALLET WHERE ISSUINGKEY = '{obj.RefNo}' AND PALLETNO = '{obj.ShipPlNo}' AND BOOKED = 0";
            DataSet dr = new ConnDB().GetFill(ssql);
            if (dr.Tables.Count > 0)
            {
                if (dr.Tables[0].Rows[0]["status"].ToString() == "0")
                {
                    string sql = $"DELETE TXP_ISSPALLET l  WHERE l.ISSUINGKEY = '{obj.RefNo}' AND l.PALLETNO = '{obj.ShipPlNo}'";
                    new ConnDB().ExcuteSQL($"UPDATE TXP_ISSPACKDETAIL d SET d.SHIPPLNO = '' WHERE d.ISSUINGKEY ='{obj.RefNo}' AND d.SHIPPLNO = '{obj.ShipPlNo}'");
                    x = new ConnDB().ExcuteSQL(sql);
                }
            }
            return x;
        }
    }
}
