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
    public class OrderControllers
    {
        List<OrderData> AddOrderList(DataRow ob, string sql)
        {
            List<OrderData> obj = new List<OrderData>();
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                if (int.Parse(r["item"].ToString()) > 0)
                {
                    obj.Add(new OrderData()
                    {
                        Id = obj.Count,
                        Factory = ob["factory"].ToString(),
                        Etd = DateTime.Parse(ob["etdtap"].ToString()),
                        Ship = ob["shiptype"].ToString(),
                        Affcode = ob["affcode"].ToString(),
                        Custcode = ob["bishpc"].ToString(),
                        Custname = ob["bisafn"].ToString(),
                        CustPoType = ob["potype"].ToString(),
                        PoType = r["custpono"].ToString(),
                        Zone = ob["zname"].ToString(),
                        OrderCtn = int.Parse(r["orderctn"].ToString()),
                        ItemCtn = int.Parse(r["item"].ToString()),
                        RefInv = r["invoceno"].ToString(),
                        RefNo = r["curinv"].ToString(),
                        OrderRewrite = r["rewrite"].ToString(),
                        Status = int.Parse(r["orderstatus"].ToString()),
                        Combinv = ob["combinv"].ToString(),
                    });
                }
            }
            return obj;
        }

        public List<OrderData> GetOrderData(string orderid)
        {
            List<OrderData> obj = new List<OrderData>();
            string sql = $"SELECT p.FACTORY,p.ETDTAP,p.SHIPTYPE,get_zone(p.FACTORY, p.BIOABT) zname,p.AFFCODE,p.BISHPC,p.BISAFN,'' custpono,m.POTYPE,0 item,0 orderctn ,'' CURINV,'' invoceno,0 ORDERSTATUS,max(m.combinv) combinv,max(p.REASONCD) rewrite\n" +
                        "FROM TXP_ORDERPLAN p\n" +
                        "INNER JOIN TXM_CUSTOMER m ON p.FACTORY = m.FACTORY  AND p.AFFCODE = m.AFFCODE AND p.BISHPC = m.BISHPC AND p.BISAFN = m.CUSTNM AND m.POTYPE IS NOT NULL\n" +
                        $"WHERE p.ORDERID LIKE  '%@{orderid}%'\n" +
                        "GROUP BY p.FACTORY,p.ETDTAP,p.SHIPTYPE,p.BIOABT,p.AFFCODE,p.BISHPC,p.BISAFN,m.POTYPE\n" +
                        "ORDER BY p.ETDTAP";
            //Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            int i = 0;
            while (i < dr.Tables[0].Rows.Count)
            {
                Console.WriteLine($"++++++++++++++++++++++++ START ++++++++++++++++++++++++++++++++");
                DataRow r = dr.Tables[0].Rows[i];
                string sqlbody;
                DateTime d = DateTime.Parse(r["etdtap"].ToString());
                switch (r["potype"].ToString())
                {
                    case "3END":
                        sqlbody = $"SELECT SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0\n" +
                              $"GROUP BY SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3)";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                    case "3FRD":
                        sqlbody = $"SELECT SUBSTR(p.PONO,1, 3) custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0\n" +
                              $"GROUP BY SUBSTR(p.PONO,1, 3)";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                    case "A_TMW":
                        sqlbody = $"SELECT 'ALL' custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0 AND \n" +
                              $"SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) != 'TMW'";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }

                        sqlbody = $"SELECT 'TMW' custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0 AND\n" +
                              $"SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) = 'TMW'";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                    case "ALL":
                        sqlbody = $"SELECT 'ALL' custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                    default:
                        sqlbody = $"SELECT p.PONO custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0\n" +
                              $"GROUP BY p.PONO";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                }
                i++;
            }
            return obj;
        }

        public string CreatedJobList(OrderData b)
        {
            string refinvoice = b.RefNo;
            new ConnDB().ExcuteSQL($"DELETE TXP_ISSTRANSENT e WHERE e.ISSUINGKEY = '{b.RefNo}'");
            List<OrderBody> ord = GetOrderDetail(b);
            if (ord.Count > 0)
            {
                //create head
                OrderBody j = ord[0];
                refinvoice = GetRefInv(j.Prefix, j.Factory, j.Etd);
                string Note1 = new GreeterFunction().GetNote(1, j.BioABT, j.Ship, j.Factory);
                string Note2 = new GreeterFunction().GetNote(2, j.BioABT, j.Ship, j.Factory);
                string Note3 = new GreeterFunction().GetNote(3, j.BioABT, j.Ship, j.Factory);
                string zonecode = $"TO_CHAR(SYSDATE,'YYMMDD')||'{j.Prefix}'|| LPAD(substr('{refinvoice}',-5),5)";
                //SplashScreenManager.Default.SetWaitFormCaption($"{ninv}");
                string sqlhead;
                //check issue
                List<string> h = GetOrderRefinvoice(b);
                if (h.Count > 0)
                {
                    string refkey = "'" + string.Join("','", h) + "'";
                    refinvoice = h[0];
                    zonecode = $"TO_CHAR(SYSDATE,'YYMMDD')||'{j.Prefix}'|| LPAD(substr('{refinvoice}',-5),5)";
                    sqlhead = $"update txp_isstransent set etddte=to_date('{j.Etd.ToString("dd-MM-yyyy")}','DD-MM-YYYY'),issuingkey='{refinvoice}',refinvoice='{refinvoice}',issuingmax='{ord.Count()}',zonecode={zonecode},upddte=sysdate where issuingkey='{refinvoice}'";
                    int i = 0;
                    while (i < ord.Count)
                    {
                        string uukey = Guid.NewGuid().ToString();
                        DataSet drh = new ConnDB().GetFill($"select count(*) ctn from txp_isstransent where issuingkey='{b.RefNo}'");
                        if (int.Parse(drh.Tables[0].Rows[0]["ctn"].ToString()) < 1)
                        {
                            sqlhead = $"insert into txp_isstransent(issuingkey,refinvoice,issuingstatus,etddte,factory,affcode,bishpc,custname,comercial,zoneid,shiptype,\n" +
                             "combinv,pc,zonecode,note1,note2,upddte,sysdte,uuid,createdby,modifiedby,containertype,issuingmax)\n" +
                             $"values('{refinvoice}','{refinvoice}',0,to_date('{j.Etd.ToString("dd-MM-yyyy")}','DD-MM-YYYY'),'{j.Factory}','{j.Affcode}','{j.Custcode}','{j.Custname}','{j.Commercial}','{j.BioABT}','{j.Ship}',\n" +
                             $"'{j.Combinv}','{j.Pc}',{zonecode},'{Note1}','{Note2}',sysdate,sysdate,'{uukey}','SYS','SYS','{Note3}','{ord.Count()}')";
                        }
                        new ConnDB().ExcuteSQL(sqlhead);
                        OrderBody r = ord[i];
                        Guid g = Guid.NewGuid();
                        string sqlcheckbody = $"SELECT b.PARTNO,round(b.ORDERQTY/b.STDPACK) ctn FROM TXP_ISSTRANSBODY b WHERE b.PARTNO = '{r.PartNo}' AND b.ISSUINGKEY IN ({refkey})";
                        DataSet dr = new ConnDB().GetFill(sqlcheckbody);
                        string upb;
                        if (dr.Tables[0].Rows.Count > 0)
                        {
                            //having data
                            upb = $"update txp_isstransbody set ISSUINGKEY='{refinvoice}',ORDERQTY={r.BalQty}  WHERE PARTNO = '{r.PartNo}' AND ISSUINGKEY IN ({refkey})";
                        }
                        else
                        {
                            //insert body
                            upb = $"insert into txp_isstransbody(issuingkey,issuingseq,pono,tagrp,partno,stdpack,orderqty,issueokqty,shorderqty,prepareqty,revisedqty,issuedqty,issuingstatus,bwide,bleng,bhight,neweight,gtweight,upddte,sysdte,parttype,partname,shiptype,edtdte,uuid,createdby,modifiedby,ordertype,lotno,refinv)\n" +
                                    $"values('{refinvoice}',{i},'{r.OrderNo}','C','{r.PartNo}','{r.BiSTDP}','{r.BalQty}',0,0,0,0,0,0,{r.BiWidt},{r.BiLeng},{r.BiHigh},{r.BiNetW},{r.BiGrwt},sysdate,sysdate,'{r.OrderType}','{r.PartName}','{r.Ship}'," +
                                    $"to_date('{r.Etd.ToString("yyyy/MM/dd")}', 'yyyy/MM/dd'),'{g.ToString()}','SYS','SYS','{r.PartType}','{r.LotNo}','{r.Uuid}')";
                        }
                        new ConnDB().ExcuteSQL(upb);
                        int rn = 0;
                        while (rn < r.Ctn)
                        {
                            int nums = (rn + 1);
                            string sqldetail = $"SELECT count(*) ctn FROM TXP_ISSPACKDETAIL d WHERE d.PARTNO = '{r.PartNo}' AND d.ITEM = '{nums}' AND d.ISSUINGKEY IN ({refkey})";
                            dr = new ConnDB().GetFill(sqldetail);
                            if (dr.Tables[0].Rows.Count < 1)
                            {
                                Guid gid = Guid.NewGuid();
                                string fkey = new GreeterFunction().getFTicket(r.Factory);
                                string insql = $"insert into txp_isspackdetail(issuingkey,pono,tagrp,partno,fticketno,orderqty,issuedqty,unit,issuingstatus,upddte,sysdte,uuid,createdby,modifedby,ITEM,splorder)\n" +
                                    $"values('{refinvoice}','{r.OrderNo}','C','{r.PartNo}','{fkey}','{r.BiSTDP}',0,'PCS',0,sysdate,sysdate,'{gid.ToString()}','SYS','SYS',{nums},'{g.ToString()}')";
                                new ConnDB().ExcuteSQL(insql);
                                GreeterFunction.updateFTicket(r.Factory);
                            }
                            rn++;
                        }
                        string updateorder = $"update txp_orderplan set curinv = '{refinvoice}',orderstatus=1,upddte = sysdate where uuid = '{r.Uuid}'";
                        new ConnDB().ExcuteSQL(updateorder);
                        i++;
                    }
                }
                else
                {
                    sqlhead = $"insert into txp_isstransent(issuingkey,refinvoice,issuingstatus,etddte,factory,affcode,bishpc,custname,comercial,zoneid,shiptype,\n" +
                             "combinv,pc,zonecode,note1,note2,upddte,sysdte,uuid,createdby,modifiedby,containertype,issuingmax)\n" +
                             $"values('{refinvoice}','{refinvoice}',0,to_date('{j.Etd.ToString("dd-MM-yyyy")}','DD-MM-YYYY'),'{j.Factory}','{j.Affcode}','{j.Custcode}','{j.Custname}','{j.Commercial}','{j.BioABT}','{j.Ship}',\n" +
                             $"'{j.Combinv}','{j.Pc}',{zonecode},'{Note1}','{Note2}',sysdate,sysdate,'{Guid.NewGuid().ToString()}','SYS','SYS','{Note3}','{ord.Count()}')";
                    new ConnDB().ExcuteSQL(sqlhead);
                    //create body
                    int i = 0;
                    while (i < ord.Count)
                    {
                        OrderBody r = ord[i];
                        Guid g = Guid.NewGuid();
                        string sql = $"insert into txp_isstransbody(issuingkey,issuingseq,pono,tagrp,partno,stdpack,orderqty,issueokqty,shorderqty,prepareqty,revisedqty,issuedqty,issuingstatus,bwide,bleng,bhight,neweight,gtweight,upddte,sysdte,parttype,partname,shiptype,edtdte,uuid,createdby,modifiedby,ordertype,lotno,refinv)\n" +
                                $"values('{refinvoice}',{i},'{r.OrderNo}','C','{r.PartNo}','{r.BiSTDP}','{r.BalQty}',0,0,0,0,0,0,{r.BiWidt},{r.BiLeng},{r.BiHigh},{r.BiNetW},{r.BiGrwt},sysdate,sysdate,'{r.OrderType}','{r.PartName}','{r.Ship}'," +
                                $"to_date('{r.Etd.ToString("yyyy/MM/dd")}', 'yyyy/MM/dd'),'{g.ToString()}','SYS','SYS','{r.PartType}','{r.LotNo}','{r.Uuid}')";
                        new ConnDB().ExcuteSQL(sql);
                        Console.WriteLine(g.ToString());
                        Console.WriteLine(r.Ctn);
                        Console.WriteLine(r.Uuid);
                        //create issue detail
                        int rn = 0;
                        while (rn < r.Ctn)
                        {
                            Console.WriteLine($"create packing detail {rn} {r.OrderCtn}");
                            Guid gid = Guid.NewGuid();
                            string fkey = new GreeterFunction().getFTicket(r.Factory);
                            string sqldetail = $"insert into txp_isspackdetail(issuingkey,pono,tagrp,partno,fticketno,orderqty,issuedqty,unit,issuingstatus,upddte,sysdte,uuid,createdby,modifedby,ITEM,splorder)\n" +
                                $"values('{refinvoice}','{r.OrderNo}','C','{r.PartNo}','{fkey}','{r.BiSTDP}',0,'PCS',0,sysdate,sysdate,'{gid.ToString()}','SYS','SYS',{(rn + 1)},'{g.ToString()}')";
                            new ConnDB().ExcuteSQL(sqldetail);
                            if (GreeterFunction.updateFTicket(r.Factory))
                            {
                                rn++;
                            }
                        }
                        string updateorder = $"update txp_orderplan set curinv = '{refinvoice}',orderstatus=1,upddte = sysdate where uuid = '{r.Uuid}'";
                        new ConnDB().ExcuteSQL(updateorder);
                        i++;
                    }
                    new GreeterFunction().SumPallet(refinvoice);
                }
            }
            return refinvoice;
        }

        private string GetRefInv(string prefix, string factory, DateTime etd)
        {
            string sql = $"SELECT count(*) +1 ctn FROM TXP_ISSTRANSENT e WHERE TO_CHAR(etddte, 'yyyy-MM-dd') = '{etd.ToString("yyyy-MM-dd")}' and factory = '{factory}'";
            DataSet dr = new ConnDB().GetFill(sql);
            string fac = $"I{prefix}-{etd.ToString("yyMMdd")}-";
            if (factory == "AW")
            {
                fac = $"A{prefix}-{etd.ToString("yyMMdd")}-";
            }
            string inv = fac + int.Parse(dr.Tables[0].Rows[0]["ctn"].ToString()).ToString("D5");
            return inv;
        }

        public List<OrderData> GetOrderData(string factory, DateTime etd, bool onlyday)
        {
            List<OrderData> obj = new List<OrderData>();
            string dte = etd.ToString("ddMMyyy");
            string fdte = $"AND p.ETDTAP = to_date('{dte}', 'ddMMyyyy')";
            int wnum = 7;
            if (onlyday != true)
            {
                fdte = $"AND p.ETDTAP between TRUNC(to_date('{dte}', 'ddMMyyyy') - 1, 'DY') AND(TRUNC(to_date('{dte}', 'ddMMyyyy'), 'DY') + {wnum})";
            }
            string sql = $"SELECT p.FACTORY,p.ETDTAP,p.SHIPTYPE,get_zone(p.FACTORY, p.BIOABT) zname,p.AFFCODE,p.BISHPC,p.BISAFN,'' custpono,m.POTYPE,0 item,0 orderctn ,'' CURINV,'' invoceno,max(p.ORDERSTATUS) ORDERSTATUS,max(m.combinv) combinv,max(p.REASONCD) rewrite\n" +
                        "FROM TXP_ORDERPLAN p\n" +
                        "INNER JOIN TXM_CUSTOMER m ON p.FACTORY = m.FACTORY  AND p.AFFCODE = m.AFFCODE AND p.BISHPC = m.BISHPC AND p.BISAFN = m.CUSTNM AND m.POTYPE IS NOT NULL\n" +
                        $"WHERE p.STATUS = 1 AND p.FACTORY = '{factory}' {fdte}\n" +
                        "GROUP BY p.FACTORY,p.ETDTAP,p.SHIPTYPE,p.BIOABT,p.AFFCODE,p.BISHPC,p.BISAFN,m.POTYPE\n" +
                        "ORDER BY p.ETDTAP";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            int i = 0;
            while (i < dr.Tables[0].Rows.Count)
            {
                Console.WriteLine($"++++++++++++++++++++++++ START ++++++++++++++++++++++++++++++++");
                DataRow r = dr.Tables[0].Rows[i];
                string sqlbody;
                DateTime d = DateTime.Parse(r["etdtap"].ToString());
                switch (r["potype"].ToString())
                {
                    case "3END":
                        sqlbody = $"SELECT SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0\n" +
                              $"GROUP BY SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3)";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                    case "3FRD":
                        sqlbody = $"SELECT SUBSTR(p.PONO,1, 3) custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0\n" +
                              $"GROUP BY SUBSTR(p.PONO,1, 3)";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                    case "A_TMW":
                        sqlbody = $"SELECT 'ALL' custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0 AND \n" +
                              $"SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) != 'TMW'";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }

                        sqlbody = $"SELECT 'TMW' custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0 AND\n" +
                              $"SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) = 'TMW'";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                    case "ALL":
                        sqlbody = $"SELECT 'ALL' custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                    default:
                        sqlbody = $"SELECT p.PONO custpono,count(p.PARTNO) item,sum(round(p.BALQTY/p.BISTDP)) orderctn ,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,max(p.ORDERSTATUS) ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                              "FROM TXP_ORDERPLAN p\n" +
                              $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                              $"p.AFFCODE = '{r["affcode"]}' AND " +
                              $"p.BISHPC = '{r["bishpc"]}' AND " +
                              $"p.BISAFN = '{r["bisafn"]}' AND " +
                              $"p.SHIPTYPE = '{r["shiptype"]}' AND p.BALQTY > 0\n" +
                              $"GROUP BY p.PONO";
                        Console.WriteLine(sqlbody);
                        foreach (OrderData od in AddOrderList(r, sqlbody))
                        {
                            od.Id = obj.Count + 1;
                            obj.Add(od);
                        }
                        break;
                }
                i++;
            }
            return obj;
        }


        List<OrderBody> AddOrderBodyList(OrderData b, string sql)
        {
            Console.WriteLine($"##################### {b.RefNo} #########################");
            Console.WriteLine(sql);
            List<OrderBody> obj = new List<OrderBody>();
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                obj.Add(new OrderBody()
                {
                    Id = obj.Count,
                    Factory = b.Factory,
                    Etd = b.Etd,
                    Ship = b.Ship,
                    Affcode = b.Affcode,
                    Custcode = b.Custcode,
                    Custname = b.Custname,
                    CustPoType = b.CustPoType,
                    PoType = b.PoType,
                    Zone = b.Zone,
                    OrderCtn = b.OrderCtn,
                    ItemCtn = b.ItemCtn,
                    RefInv = r["invoceno"].ToString(),
                    RefNo = b.RefNo,
                    Status = int.Parse(r["orderstatus"].ToString()),
                    ReasonCD = r["rewrite"].ToString(),
                    Combinv = b.Combinv,
                    OrderNo = r["custpono"].ToString(),
                    PartNo = r["partno"].ToString(),
                    PartName = r["partname"].ToString(),
                    PartType = r["bicomd"].ToString(),
                    LotNo = r["lotno"].ToString(),
                    BalQty= int.Parse(r["balqty"].ToString()),
                    Ctn = int.Parse(r["orderctn"].ToString()),
                    LotSeq = 0,
                    Prefix = r["biivpx"].ToString(),
                    BioABT = r["bioabt"].ToString(),
                    Commercial = r["commercial"].ToString(),
                    Pc = r["pc"].ToString(),
                    Uuid = r["uuid"].ToString(),
                    BiSTDP = int.Parse(r["bistdp"].ToString()),
                    BiWidt = int.Parse(r["biwidt"].ToString()),
                    BiLeng = int.Parse(r["bileng"].ToString()),
                    BiNetW = int.Parse(r["binewt"].ToString()),
                    BiHigh = int.Parse(r["bihigh"].ToString()),
                    BiGrwt = int.Parse(r["bigrwt"].ToString()),
                    OrderType = r["ordertype"].ToString(),
                    BiComd = r["bicomd"].ToString(),
                    LastUpdate = DateTime.Parse(r["upddte"].ToString()),
                });
            }
            return obj;
        }

        string CheckOrderGroup(OrderData b)
        {
            string sql;
            switch (b.CustPoType.ToString())
            {
                case "3END":
                    sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{b.Affcode}' AND " +
                          $"p.BISHPC = '{b.Custcode}' AND " +
                          $"p.BISAFN = '{b.Custname}' AND " +
                          $"p.SHIPTYPE = '{b.Ship}' AND " +
                          $"SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) = '{b.PoType}' AND get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    break;
                case "3FRD":
                    sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{b.Affcode}' AND " +
                          $"p.BISHPC = '{b.Custcode}' AND " +
                          $"p.BISAFN = '{b.Custname}' AND " +
                          $"p.SHIPTYPE = '{b.Ship}' AND " +
                          $"SUBSTR(p.PONO,1, 3) = '{b.PoType}' AND get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    break;
                case "A_TMW":
                    if (b.PoType.ToString() == "ALL")
                    {
                        sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{b.Affcode}' AND " +
                          $"p.BISHPC = '{b.Custcode}' AND " +
                          $"p.BISAFN = '{b.Custname}' AND " +
                          $"p.SHIPTYPE = '{b.Ship}' AND " +
                          $"SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) != 'TMW' AND get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    }
                    else
                    {
                        sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{b.Affcode}' AND " +
                          $"p.BISHPC = '{b.Custcode}' AND " +
                          $"p.BISAFN = '{b.Custname}' AND " +
                          $"p.SHIPTYPE = '{b.Ship}' AND " +
                          $"SUBSTR(p.PONO,LENGTH(p.PONO) - 2, 3) = 'TMW' AND get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    }
                    break;

                case "ALL":
                    sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{b.Affcode}' AND " +
                          $"p.BISHPC = '{b.Custcode}' AND " +
                          $"p.BISAFN = '{b.Custname}' AND " +
                          $"p.SHIPTYPE = '{b.Ship}' AND get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    break;

                default:
                    sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{b.Affcode}' AND " +
                          $"p.BISHPC = '{b.Custcode}' AND " +
                          $"p.BISAFN = '{b.Custname}' AND " +
                          $"p.SHIPTYPE = '{b.Ship}' AND \n" +
                          $"p.PONO = '{b.PoType}' AND get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    break;
            }
            return sql;
        }

        public List<OrderBody> GetOrderDetail(OrderData b)
        {
            string sql = $"SELECT p.PONO custpono,p.PARTNO,p.PARTNAME,p.LOTNO,p.BALQTY,round(p.BALQTY/p.BISTDP) orderctn,p.BIIVPX,p.BIOABT,p.COMMERCIAL,p.PC,p.UUID," +
                $"p.BISTDP,p.BIWIDT,p.BILENG,p.BIHIGH,p.BIGRWT,p.BINEWT,p.ORDERTYPE,p.BICOMD,p.CURINV,'' invoceno,p.ORDERSTATUS," +
                $"CASE WHEN p.FACTORY = '{b.Factory}' THEN substr(p.REASONCD, 1, 1) ELSE substr(p.REASONCD, 2, 1) END rewrite,p.upddte,p.bicomd FROM TXP_ORDERPLAN p\n" +
                "--LEFT JOIN TXP_ISSTRANSBODY b ON p.CURINV = b.ISSUINGKEY AND p.PARTNO = b.PARTNO\n" +
                "--LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n";
            List<OrderBody> obj = new List<OrderBody>();
            sql += CheckOrderGroup(b);
            foreach (OrderBody od in AddOrderBodyList(b, sql))
            {
                od.Id = obj.Count + 1;
                obj.Add(od);
            }
            return obj;
        }

        public List<string> GetOrderRefinvoice(OrderData b)
        {
            string sql = $"SELECT p.CURINV FROM TXP_ORDERPLAN p\n" +
                "LEFT JOIN TXP_ISSTRANSBODY b ON p.CURINV = b.ISSUINGKEY AND p.PARTNO = b.PARTNO\n" +
                "LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n";
            sql += CheckOrderGroup(b);
            sql += "AND p.CURINV IS NOT NULL AND p.BALQTY > 0 \n";
            sql += "GROUP BY p.CURINV";
            sql += "\nORDER BY p.CURINV";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            List<string> list = new List<string>();
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                list.Add(r["curinv"].ToString());
            }
            return list;
        }

        public bool GetOrderBodyRefinvoice(OrderData b)
        {
            string sql = $"SELECT p.CURINV,COUNT(p.CURINV) FROM TXP_ORDERPLAN p\n" +
                "LEFT JOIN TXP_ISSTRANSBODY b ON p.CURINV = b.ISSUINGKEY AND p.PARTNO = b.PARTNO\n" +
                "LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n";
            sql += CheckOrderGroup(b);
            sql += "AND p.BALQTY > 0 \n";
            sql += "\nGROUP BY p.CURINV";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 1)
            {
                return true;
            }
            return false;
        }
    }
}
