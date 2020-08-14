using DevExpress.XtraSplashScreen;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace XPWLibrary.Controllers
{
    public class OrderControllers
    {
        List<OrderData> AddOrderList(DataRow ob, string sql)
        {
            List<OrderData> obj = new List<OrderData>();
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                try
                {
                    SplashScreenManager.Default.SetWaitFormDescription($"{r["custpono"].ToString()} {r["item"].ToString()}");
                }
                catch (Exception)
                {
                }
                int st = int.Parse(r["orderstatus"].ToString());
                if (int.Parse(r["orderctn"].ToString()) < 1)
                {
                    st = 10;
                }
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
                        RefInv = ob["invoceno"].ToString(),
                        RefNo = r["curinv"].ToString(),
                        OrderRewrite = r["rewrite"].ToString(),
                        Status = st,
                        Combinv = ob["combinv"].ToString(),
                        Commercial = ob["commercial"].ToString(),
                        Pc = ob["pc"].ToString(),
                        BioABT = ob["bioabt"].ToString(),
                        //BiComb = ob["bicomd"].ToString(),
                        LastUpdate = DateTime.Parse(ob["upddte"].ToString())
                    });
                }
            }
            return obj;
        }

        public List<OrderData> GetOrderData(string orderid)
        {
            List<OrderData> obj = new List<OrderData>();
            string sql = $"SELECT p.FACTORY,p.ETDTAP,p.SHIPTYPE,get_zone(p.FACTORY, p.BIOABT) zname,p.AFFCODE,p.BISHPC,p.BISAFN,'' custpono,'' POTYPE,0 item,0 orderctn ," +
                        $"min(p.CURINV) CURINV,max(e.refinvoice) invoceno,CASE WHEN max(p.ORDERSTATUS) IS NULL THEN 0 ELSE max(p.ORDERSTATUS) END ORDERSTATUS," +
                        $"m.combinv,p.COMMERCIAL,p.PC,p.BIOABT,max(p.REASONCD) rewrite,max(p.upddte) upddte\n" +
                        "FROM TXP_ORDERPLAN p\n" +
                        "INNER JOIN TXM_CUSTOMER m ON p.FACTORY = m.FACTORY  AND p.AFFCODE = m.AFFCODE AND p.BISHPC = m.BISHPC AND p.BISAFN = m.CUSTNM \n" +
                        "LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n" +
                         $"WHERE p.FACTORY = '{StaticFunctionData.Factory}' AND p.ORDERID LIKE  '%{orderid.ToString().Trim().ToUpper()}%'\n" +
                        "GROUP BY p.ETDTAP,p.FACTORY,p.AFFCODE,p.BISHPC,p.BISAFN,p.COMMERCIAL,p.PC,p.SHIPTYPE,p.BIOABT,m.COMBINV\n" +
                        "ORDER BY p.ETDTAP,p.FACTORY,p.AFFCODE,p.BISHPC,p.BISAFN,p.COMMERCIAL,p.PC,p.SHIPTYPE,p.BIOABT,m.COMBINV";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            int i = 0;
            while (i < dr.Tables[0].Rows.Count)
            {
                Console.WriteLine($"++++++++++++++++++++++++ START ++++++++++++++++++++++++++++++++");
                DataRow r = dr.Tables[0].Rows[i];
                List<OrderData> obb = GetOrderBodyDetail(r);
                if (obb.Count > 0)
                {
                    foreach (OrderData od in obb)
                    {
                        od.Id = obj.Count + 1;
                        obj.Add(od);
                    }
                }
                i++;
            }
            return obj;
        }

        public List<OrderData> GetLotNoData(string v)
        {
            List<OrderData> obj = new List<OrderData>();
            string sql = $"SELECT p.FACTORY,p.ETDTAP,p.SHIPTYPE,get_zone(p.FACTORY, p.BIOABT) zname,p.AFFCODE,p.BISHPC,p.BISAFN,'' custpono,'' POTYPE,0 item,0 orderctn ," +
                        $"min(p.CURINV) CURINV,max(e.refinvoice) invoceno,CASE WHEN max(p.ORDERSTATUS) IS NULL THEN 0 ELSE max(p.ORDERSTATUS) END ORDERSTATUS," +
                        $"m.combinv,p.COMMERCIAL,p.PC,p.BIOABT,max(p.REASONCD) rewrite,max(p.upddte) upddte\n" +
                        "FROM TXP_ORDERPLAN p\n" +
                        "INNER JOIN TXM_CUSTOMER m ON p.FACTORY = m.FACTORY  AND p.AFFCODE = m.AFFCODE AND p.BISHPC = m.BISHPC AND p.BISAFN = m.CUSTNM \n" +
                        "LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n" +
                         $"WHERE p.FACTORY = '{StaticFunctionData.Factory}' AND p.LOTNO LIKE  '%{v.ToString().Trim().ToUpper()}%'\n" +
                        "GROUP BY p.ETDTAP,p.FACTORY,p.AFFCODE,p.BISHPC,p.BISAFN,p.COMMERCIAL,p.PC,p.SHIPTYPE,p.BIOABT,m.COMBINV\n" +
                        "ORDER BY p.ETDTAP,p.FACTORY,p.AFFCODE,p.BISHPC,p.BISAFN,p.COMMERCIAL,p.PC,p.SHIPTYPE,p.BIOABT,m.COMBINV";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            int i = 0;
            while (i < dr.Tables[0].Rows.Count)
            {
                Console.WriteLine($"++++++++++++++++++++++++ START ++++++++++++++++++++++++++++++++");
                DataRow r = dr.Tables[0].Rows[i];
                List<OrderData> obb = GetOrderBodyDetail(r);
                if (obb.Count > 0)
                {
                    foreach (OrderData od in obb)
                    {
                        od.Id = obj.Count + 1;
                        obj.Add(od);
                    }
                }
                i++;
            }
            return obj;
        }

        public string CreatedJobList(OrderData b)
        {
            string refinvoice = b.RefNo;
            string refno;
            new ConnDB().ExcuteSQL($"DELETE TXP_ISSTRANSENT e WHERE e.ISSUINGKEY = '{b.RefNo}'");
            List<OrderBody> ord = GetOrderDetail(b);
            if (ord.Count > 0)
            {
                //create head
                OrderBody j = ord[0];
                refno = GetRefInv(j.Prefix, j.Factory, j.Etd);
                string Note1 = new GreeterFunction().GetNote(1, j.BioABT, j.Ship, j.Factory);
                string Note2 = new GreeterFunction().GetNote(2, j.BioABT, j.Ship, j.Factory);
                string Note3 = new GreeterFunction().GetNote(3, j.BioABT, j.Ship, j.Factory);
                string zonecode = $"TO_CHAR(SYSDATE,'YYMMDD')||'{j.Prefix}'|| LPAD(substr('{refinvoice}',-5),5)";
                SplashScreenManager.Default.SetWaitFormCaption($"{refno}");
                string sqlhead;
                //check issue
                List<string> h = GetOrderRefinvoice(b);
                if (h.Count > 0)
                {
                    string refkey = "'" + string.Join("','", h) + "'";
                    refinvoice = h[0];
                    zonecode = $"TO_CHAR(SYSDATE,'YYMMDD')||'{j.Prefix}'|| LPAD(substr('{refinvoice}',-5),5)";
                    sqlhead = $"update txp_isstransent set etddte=to_date('{j.Etd.ToString("dd-MM-yyyy")}','DD-MM-YYYY'),issuingkey='{refinvoice}',refinvoice='{refinvoice}',issuingmax='{ord.Count()}',zonecode={zonecode},upddte=sysdate where issuingkey='{refinvoice}'";
                    string uukey = Guid.NewGuid().ToString();
                    Console.WriteLine($"select count(*) ctn from txp_isstransent where issuingkey='{refinvoice}'");
                    DataSet drh = new ConnDB().GetFill($"select count(*) ctn from txp_isstransent where issuingkey='{refinvoice}'");
                    if (int.Parse(drh.Tables[0].Rows[0]["ctn"].ToString()) < 1)
                    {
                        sqlhead = $"insert into txp_isstransent(issuingkey,refinvoice,issuingstatus,etddte,factory,affcode,bishpc,custname,comercial,zoneid,shiptype,\n" +
                         "combinv,pc,zonecode,note1,note2,upddte,sysdte,uuid,createdby,modifiedby,containertype,issuingmax)\n" +
                         $"values('{refinvoice}','{refinvoice}',0,to_date('{j.Etd.ToString("dd-MM-yyyy")}','DD-MM-YYYY'),'{j.Factory}','{j.Affcode}','{j.Custcode}','{j.Custname}','{j.Commercial}','{j.BioABT}','{j.Ship}',\n" +
                         $"'{j.Combinv}','{j.Pc}',{zonecode},'{Note1}','{Note2}',sysdate,sysdate,'{uukey}','SYS','SYS','{Note3}','{ord.Count()}')";
                    }
                    new ConnDB().ExcuteSQL(sqlhead);

                    SplashScreenManager.Default.SetWaitFormCaption($"CHECK BODY");
                    int i = 0;
                    while (i < ord.Count)
                    {
                        OrderBody r = ord[i];
                        Guid g = Guid.NewGuid();
                        Console.WriteLine($"{i} => {r.PartNo} ORDER => {r.OrderNo}");
                        string sqlcheckbody = $"SELECT b.PARTNO,round(b.ORDERQTY/b.STDPACK) ctn FROM TXP_ISSTRANSBODY b WHERE b.PARTNO = '{r.PartNo}' AND b.PONO = '{r.OrderNo}' AND b.ISSUINGKEY IN ({refkey})";
                        //Console.WriteLine(sqlcheckbody);
                        DataSet dr = new ConnDB().GetFill(sqlcheckbody);
                        string upb;
                        if (dr.Tables[0].Rows.Count > 0)
                        {
                            //having data
                            upb = $"update txp_isstransbody set ISSUINGKEY='{refinvoice}',ORDERQTY={r.BalQty}  WHERE PARTNO = '{r.PartNo}' AND PONO = '{r.OrderNo}' AND ISSUINGKEY IN ({refkey})";
                        }
                        else
                        {
                            //insert body
                            upb = $"insert into txp_isstransbody(issuingkey,issuingseq,pono,tagrp,partno,stdpack,orderqty,issueokqty,shorderqty,prepareqty,revisedqty,issuedqty,issuingstatus,bwide,bleng,bhight,neweight,gtweight,upddte,sysdte,parttype,partname,shiptype,edtdte,uuid,createdby,modifiedby,ordertype,lotno,refinv)\n" +
                                    $"values('{refinvoice}',{i},'{r.OrderNo}','C','{r.PartNo}','{r.BiSTDP}','{r.BalQty}',0,0,0,0,0,0,{r.BiWidt},{r.BiLeng},{r.BiHigh},{r.BiNetW},{r.BiGrwt},sysdate,sysdate,'{r.OrderType}','{r.PartName}','{r.Ship}'," +
                                    $"to_date('{r.Etd.ToString("yyyy/MM/dd")}', 'yyyy/MM/dd'),'{g.ToString()}','SYS','SYS','{r.PartType}','{r.LotNo}','{r.Uuid}')";
                        }
                        SplashScreenManager.Default.SetWaitFormCaption($"{refinvoice}");
                        SplashScreenManager.Default.SetWaitFormDescription($"CREATE DETAIL {r.PartNo}");
                        new ConnDB().ExcuteSQL(upb);
                        int rn = 0;
                        while (rn < r.Ctn)
                        {
                            int nums = (rn + 1);
                            string sqldetail = $"SELECT count(*) ctn FROM TXP_ISSPACKDETAIL d WHERE d.PARTNO = '{r.PartNo}' AND d.pono='{r.OrderNo}' AND d.ITEM = '{nums}' AND d.ISSUINGKEY IN ({refkey})";
                            dr = new ConnDB().GetFill(sqldetail);
                            if (dr.Tables[0].Rows.Count > 0)
                            {
                                if (int.Parse((dr.Tables[0].Rows[0][0]).ToString()) <= 0)
                                {
                                    Guid gid = Guid.NewGuid();
                                    string fkey = new GreeterFunction().getFTicket(r.Factory);
                                    SplashScreenManager.Default.SetWaitFormDescription(fkey);
                                    string insql = $"insert into txp_isspackdetail(issuingkey,pono,tagrp,partno,fticketno,orderqty,issuedqty,unit,issuingstatus,upddte,sysdte,uuid,createdby,modifedby,ITEM,splorder)\n" +
                                        $"values('{refinvoice}','{r.OrderNo}','C','{r.PartNo}','{fkey}','{r.BiSTDP}',0,'PCS',0,sysdate,sysdate,'{gid.ToString()}','SYS','SYS',{nums},'{g.ToString()}')";
                                    new ConnDB().ExcuteSQL(insql);
                                    GreeterFunction.updateFTicket(r.Factory);
                                }
                            }
                            rn++;
                        }
                        string updateorder = $"update txp_orderplan set curinv = '{refinvoice}',orderstatus=1,upddte = sysdate where uuid = '{r.Uuid}'";
                        new ConnDB().ExcuteSQL(updateorder);
                        SplashScreenManager.Default.SetWaitFormCaption($"UPDATE ORDER STATUS");
                        i++;
                    }
                }
                else
                {
                    refinvoice = refno;
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
                        Console.WriteLine($"{i} => {r.PartNo} ORDER => {r.OrderNo}");
                        string sqlcheckbody = $"SELECT b.PARTNO,round(b.ORDERQTY/b.STDPACK) ctn FROM TXP_ISSTRANSBODY b WHERE b.PARTNO = '{r.PartNo}' AND b.PONO = '{r.OrderNo}' AND b.ISSUINGKEY IN ('{refinvoice}')";
                        //Console.WriteLine(sqlcheckbody);
                        DataSet dr = new ConnDB().GetFill(sqlcheckbody);
                        string sql;
                        if (dr.Tables[0].Rows.Count < 1)
                        {
                            sql = $"insert into txp_isstransbody(issuingkey,issuingseq,pono,tagrp,partno,stdpack,orderqty,issueokqty,shorderqty,prepareqty,revisedqty,issuedqty,issuingstatus,bwide,bleng,bhight,neweight,gtweight,upddte,sysdte,parttype,partname,shiptype,edtdte,uuid,createdby,modifiedby,ordertype,lotno,refinv)\n" +
                                  $"values('{refinvoice}',{i},'{r.OrderNo}','C','{r.PartNo}','{r.BiSTDP}','{r.BalQty}',0,0,0,0,0,0,{r.BiWidt},{r.BiLeng},{r.BiHigh},{r.BiNetW},{r.BiGrwt},sysdate,sysdate,'{r.OrderType}','{r.PartName}','{r.Ship}'," +
                                  $"to_date('{r.Etd.ToString("yyyy/MM/dd")}', 'yyyy/MM/dd'),'{g.ToString()}','SYS','SYS','{r.PartType}','{r.LotNo}','{r.Uuid}')";
                        }
                        else
                        {
                            sql = $"update txp_isstransbody set ISSUINGKEY='{refinvoice}',ORDERQTY={r.BalQty}  WHERE PARTNO = '{r.PartNo}' AND PONO = '{r.OrderNo}' AND ISSUINGKEY IN ('{refinvoice}')";
                        }
                        SplashScreenManager.Default.SetWaitFormCaption($"{refinvoice}");
                        SplashScreenManager.Default.SetWaitFormDescription($"CREATE DETAIL {r.PartNo}");
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
                        SplashScreenManager.Default.SetWaitFormCaption($"UPDATE ORDER STATUS");
                        i++;
                    }
                }
            }
            if (b.Factory == "AW")
            {
                //if (b.Custname == StaticFunctionData.specialcustomer)
                //{
                //    new GreeterFunction().SumPalletSpecial(refinvoice);
                //}
                //else
                //{
                //    new GreeterFunction().SumPallet(refinvoice);
                //}
                new GreeterFunction().SumPallet(refinvoice);
            }
            return refinvoice;
        }


        public bool CreateNewInvoice(SetPallatData obj)
        {
            bool x = true;
            //update packing detail
            string sql = $"UPDATE TXP_ISSPACKDETAIL SET ISSUINGKEY ='{obj.RefNo}' WHERE ISSUINGKEY = '{obj.RefOldNo}' AND SHIPPLNO = '{obj.ShipPlNo}'";
            new ConnDB().ExcuteSQL(sql);

            //copy iss body
            string sql_body = $"SELECT ISSUINGKEY,PONO,PARTNO,sum(1) ctn,ORDERQTY FROM  TXP_ISSPACKDETAIL WHERE ISSUINGKEY = '{obj.RefNo}' AND SHIPPLNO = '{obj.ShipPlNo}'\n"+
                   "GROUP BY ISSUINGKEY,PONO,PARTNO,ORDERQTY";
            Console.WriteLine(sql_body);
            //create body 
            DataSet dr = new ConnDB().GetFill(sql_body);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                int xctn = int.Parse(r["orderqty"].ToString()) * int.Parse(r["ctn"].ToString());
                string ins_body = $"insert into txp_isstransbody(issuingkey,issuingseq,pono,tagrp,partno,stdpack,orderqty,issueokqty,shorderqty,prepareqty,revisedqty,issuedqty,issuingstatus,bwide,bleng,bhight,neweight,gtweight,upddte,sysdte,parttype,partname,shiptype,edtdte,uuid,createdby,modifiedby,ordertype,lotno,refinv)\n" +
                                  $"select '{obj.RefNo}',{r["ctn"].ToString()},'{r["pono"].ToString()}',tagrp,partno,stdpack,{xctn},issueokqty,shorderqty,prepareqty,revisedqty,issuedqty,issuingstatus,bwide,bleng,bhight,neweight,gtweight,sysdate,sysdate,parttype,partname,shiptype,edtdte,uuid,createdby,modifiedby,ordertype,lotno,'{obj.RefInv}' from txp_isstransbody where \n" +
                                  $"issuingkey = '{obj.RefNo}' and pono = '{r["pono"].ToString()}' and partno = '{r["partno"].ToString()}'";
                Console.WriteLine(ins_body);
                new ConnDB().ExcuteSQL(ins_body);
            }
            //create issue ent
            string sqlent = $"SELECT e.ISSUINGKEY FROM TXP_ISSTRANSENT e WHERE e.ISSUINGKEY = '{obj.RefNo}'";
            dr = new ConnDB().GetFill(sqlent);
            if (dr.Tables[0].Rows.Count <= 0)
            {
                string sqlhead = $"insert into txp_isstransent(issuingkey,refinvoice,issuingstatus,etddte,factory,affcode,bishpc,custname,comercial,zoneid,shiptype,combinv,pc,zonecode,note1,note2,upddte,sysdte,uuid,createdby,modifiedby,containertype,issuingmax)\n"+
                                 $"SELECT '{obj.RefNo}', '{obj.RefInv}', 0, etddte, factory, affcode, bishpc, custname, comercial, zoneid, shiptype, combinv, pc, zonecode, note1, note2, upddte, sysdte, uuid, createdby, modifiedby, containertype, 0 FROM txp_isstransent where issuingkey = '{obj.RefNo}'";
                Console.WriteLine(sqlhead);
                new ConnDB().ExcuteSQL(sqlhead);
            }
            //update issent 
            string upolder = $"UPDATE TXP_ISSTRANSENT SET ISSUINGMAX = (SELECT count(*) FROM TXP_ISSTRANSBODY WHERE ISSUINGKEY = '{obj.RefOldNo}') WHERE ISSUINGKEY = '{obj.RefOldNo}'";
            string upnewiss = $"UPDATE TXP_ISSTRANSENT SET ISSUINGMAX = (SELECT count(*) FROM TXP_ISSTRANSBODY WHERE ISSUINGKEY = '{obj.RefNo}') WHERE ISSUINGKEY = '{obj.RefNo}'";
            new ConnDB().ExcuteSQL(upolder);
            new ConnDB().ExcuteSQL(upnewiss);
            new ConnDB().ExcuteSQL($"UPDATE TXP_ISSPALLET SET ISSUINGKEY ='{obj.RefNo}' WHERE ISSUINGKEY ='{obj.RefOldNo}' AND PALLETNO ='{obj.ShipPlNo}'");
            return x;
        }

        public string GetRefInv(string prefix, string factory, DateTime etd)
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

        List<OrderData> GetOrderBodyDetail(DataRow r)
        {
            string sqlbody;
            DateTime d = DateTime.Parse(r["etdtap"].ToString());
            List<OrderData> obb;
            switch (r["combinv"].ToString())
            {
                case "E":
                    sqlbody = $"SELECT SUBSTR(p.ORDERID,LENGTH(p.ORDERID) - 2, 3) custpono,count(p.PARTNO) item,CASE WHEN sum(round(p.BALQTY/p.BISTDP)) IS NULL THEN 0 ELSE sum(round(p.BALQTY/p.BISTDP)) END orderctn,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,CASE WHEN max(p.ORDERSTATUS) IS NULL THEN 0 ELSE max(p.ORDERSTATUS) END ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                          "FROM TXP_ORDERPLAN p\n" +
                          $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{r["affcode"]}' AND " +
                          $"p.BISHPC = '{r["bishpc"]}' AND " +
                          $"p.BISAFN = '{r["bisafn"]}' AND " +
                          $"p.SHIPTYPE = '{r["shiptype"]}' " +
                          $"AND p.COMMERCIAL = '{r["commercial"]}' " +
                          $"AND p.PC = '{r["pc"]}' " +
                          $"AND p.BIOABT = '{r["bioabt"]}' AND " +
                          $"get_zone(p.FACTORY,p.BIOABT) = '{r["zname"]}'\n" +
                          $"GROUP BY SUBSTR(p.ORDERID,LENGTH(p.ORDERID) - 2, 3)";
                    //Console.WriteLine(sqlbody);
                    obb = AddOrderList(r, sqlbody);
                    break;
                case "F":
                    sqlbody = $"SELECT SUBSTR(p.orderid,1, 3) custpono,count(p.PARTNO) item,CASE WHEN sum(round(p.BALQTY/p.BISTDP)) IS NULL THEN 0 ELSE sum(round(p.BALQTY/p.BISTDP)) END orderctn,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,CASE WHEN max(p.ORDERSTATUS) IS NULL THEN 0 ELSE max(p.ORDERSTATUS) END ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                          "FROM TXP_ORDERPLAN p\n" +
                          $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{r["affcode"]}' AND " +
                          $"p.BISHPC = '{r["bishpc"]}' AND " +
                          $"p.BISAFN = '{r["bisafn"]}' AND " +
                          $"p.SHIPTYPE = '{r["shiptype"]}' " +
                          $"AND p.COMMERCIAL = '{r["commercial"]}' " +
                          $"AND p.PC = '{r["pc"]}' " +
                          $"AND p.BIOABT = '{r["bioabt"]}' AND " +
                          $"get_zone(p.FACTORY,p.BIOABT) = '{r["zname"]}'\n" +
                          $"GROUP BY SUBSTR(p.orderid,1, 3)";
                    //Console.WriteLine(sqlbody);
                    obb = AddOrderList(r, sqlbody);
                    break;
                case "N":
                    sqlbody = $"SELECT 'ALL' custpono,count(p.PARTNO) item,CASE WHEN sum(round(p.BALQTY/p.BISTDP)) IS NULL THEN 0 ELSE sum(round(p.BALQTY/p.BISTDP)) END orderctn,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,CASE WHEN max(p.ORDERSTATUS) IS NULL THEN 0 ELSE max(p.ORDERSTATUS) END ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                          "FROM TXP_ORDERPLAN p\n" +
                          $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                         $"p.AFFCODE = '{r["affcode"]}' AND " +
                          $"p.BISHPC = '{r["bishpc"]}' AND " +
                          $"p.BISAFN = '{r["bisafn"]}' AND " +
                          $"p.SHIPTYPE = '{r["shiptype"]}' " +
                          $"AND p.COMMERCIAL = '{r["commercial"]}' " +
                          $"AND p.PC = '{r["pc"]}' " +
                          $"AND p.BIOABT = '{r["bioabt"]}' AND " +
                          $"get_zone(p.FACTORY,p.BIOABT) = '{r["zname"]}'";
                    //Console.WriteLine(sqlbody);
                    obb = AddOrderList(r, sqlbody);
                    break;
                default:
                    sqlbody = $"SELECT p.PONO custpono,count(p.PARTNO) item,CASE WHEN sum(round(p.BALQTY/p.BISTDP)) IS NULL THEN 0 ELSE sum(round(p.BALQTY/p.BISTDP)) END orderctn,max(p.CURINV) CURINV,MIN(p.CURINV) invoceno,CASE WHEN max(p.ORDERSTATUS) IS NULL THEN 0 ELSE max(p.ORDERSTATUS) END ORDERSTATUS,CASE WHEN max(p.REASONCD) IS NOT NULL THEN '1' ELSE '0' end rewrite\n" +
                          "FROM TXP_ORDERPLAN p\n" +
                          $"WHERE p.STATUS = 1 AND p.FACTORY = '{r["factory"]}' AND p.ETDTAP = to_date('{d.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{r["affcode"]}' AND " +
                          $"p.BISHPC = '{r["bishpc"]}' AND " +
                          $"p.BISAFN = '{r["bisafn"]}' AND " +
                          $"p.SHIPTYPE = '{r["shiptype"]}' " +
                          $"AND p.COMMERCIAL = '{r["commercial"]}' " +
                          $"AND p.PC = '{r["pc"]}' " +
                          $"AND p.BIOABT = '{r["bioabt"]}' AND " +
                          $"get_zone(p.FACTORY,p.BIOABT) = '{r["zname"]}'\n" +
                          $"GROUP BY p.PONO";
                    //Console.WriteLine(sqlbody);
                    obb = AddOrderList(r, sqlbody);
                    break;
            }
            return obb;
        }

        List<OrderData> GetOrderList(string sql)
        {
            List<OrderData> obj = new List<OrderData>();
            try
            {
                SplashScreenManager.Default.SetWaitFormCaption("START ENT");
            }
            catch (Exception)
            {
            }
            Console.WriteLine($"START ---> HEAD");
            DataSet dr = new ConnDB().GetFill(sql);
            int i = 0;
            while (i < dr.Tables[0].Rows.Count)
            {
                //Console.WriteLine($"++++++++++++++++++++++++ START ++++++++++++++++++++++++++++++++");
                DataRow r = dr.Tables[0].Rows[i];
                try
                {
                    SplashScreenManager.Default.SetWaitFormCaption($"{r["bisafn"]}");
                }
                catch (Exception)
                {
                }
                List<OrderData> obb = GetOrderBodyDetail(r);
                if (obb.Count > 0)
                {
                    foreach (OrderData od in obb)
                    {
                        od.Id = obj.Count + 1;
                        obj.Add(od);
                    }
                }
                i++;
            }
            return obj;
        }

        public List<OrderData> GetOrderData(string factory, DateTime etd, bool onlyday)
        {
            string dte = etd.ToString("ddMMyyy");
            string fdte = $"AND ETDTAP = to_date('{dte}', 'ddMMyyyy')";
            int wnum = 7;
            if (onlyday != true)
            {
                fdte = $"AND ETDTAP between TRUNC(to_date('{dte}', 'ddMMyyyy') - 1, 'DY') AND(TRUNC(to_date('{dte}', 'ddMMyyyy'), 'DY') + {wnum})";
            }
            string sql = $"select distinct * from TBT_ORDERLIST where FACTORY = '{factory}' {fdte} AND CURINV IS NULL ORDER BY affcode,bishpc,bisafn";
            //string sql = $"SELECT p.FACTORY,p.ETDTAP,p.SHIPTYPE,get_zone(p.FACTORY, p.BIOABT) zname,p.AFFCODE,p.BISHPC,p.BISAFN,'' custpono,'' POTYPE,0 item,0 orderctn ," +
            //            $"min(p.CURINV) CURINV,max(e.refinvoice) invoceno,CASE WHEN max(p.ORDERSTATUS) IS NULL THEN 0 ELSE max(p.ORDERSTATUS) END ORDERSTATUS," +
            //            $"m.combinv,p.COMMERCIAL,p.PC,p.BIOABT,max(p.REASONCD) rewrite,max(p.upddte) upddte\n" +
            //            "FROM TXP_ORDERPLAN p\n" +
            //            "INNER JOIN TXM_CUSTOMER m ON p.FACTORY = m.FACTORY  AND p.AFFCODE = m.AFFCODE AND p.BISHPC = m.BISHPC AND p.BISAFN = m.CUSTNM \n" +
            //            "LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n"+
            //            $"WHERE p.STATUS = 1 AND p.FACTORY = '{factory}' {fdte} AND p.CURINV IS NULL\n" +
            //            "GROUP BY p.ETDTAP,p.FACTORY,p.AFFCODE,p.BISHPC,p.BISAFN,p.COMMERCIAL,p.PC,p.SHIPTYPE,p.BIOABT,m.COMBINV\n" +
            //            "ORDER BY p.ETDTAP,p.FACTORY,p.AFFCODE,p.BISHPC,p.BISAFN,p.COMMERCIAL,p.PC,p.SHIPTYPE,p.BIOABT,m.COMBINV";
            Console.WriteLine(sql);
            List<OrderData> obj = GetOrderList(sql);
            return obj;
        }

        List<OrderBody> AddOrderJobList(OrderData b, string sql)
        {
            Console.WriteLine($"##################### {b.RefNo} #########################");
            Console.WriteLine(sql);
            List<OrderBody> obj = new List<OrderBody>();
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {
                if (int.Parse(r["balqty"].ToString()) > 0)
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
                        BalQty = int.Parse(r["balqty"].ToString()),
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
            string rescd = "";
            //if (b.Factory == "AW")
            //{
            //    rescd = "AND p.REASONCD != '30' \n";
            //}
            string sql;
            switch (b.Combinv.ToString())
            {
                case "E":
                   sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                         $"p.AFFCODE = '{b.Affcode}' AND " +
                         $"p.BISHPC = '{b.Custcode}' AND " +
                         $"p.BISAFN = '{b.Custname}' AND " +
                         $"p.SHIPTYPE = '{b.Ship}' " +
                         $"{rescd}"+
                         $"AND p.COMMERCIAL = '{b.Commercial}' " +
                         $"AND p.PC = '{b.Pc}' " +
                         $"AND p.BIOABT = '{b.BioABT}' AND " +
                         $"SUBSTR(p.ORDERID,LENGTH(p.ORDERID) - 2, 3) = '{b.PoType}' AND get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    break;
                case "F":
                    sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{b.Affcode}' AND " +
                          $"p.BISHPC = '{b.Custcode}' AND " +
                          $"p.BISAFN = '{b.Custname}' AND " +
                          $"p.SHIPTYPE = '{b.Ship}' " +
                          $"{rescd}" +
                          $"AND p.COMMERCIAL = '{b.Commercial}' " +
                          $"AND p.PC = '{b.Pc}' " +
                          $"AND p.BIOABT = '{b.BioABT}' AND " +
                          $"SUBSTR(p.orderid,1, 3) = '{b.PoType}' AND get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    break;

                case "N":
                    sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                         $"p.AFFCODE = '{b.Affcode}' AND " +
                         $"p.BISHPC = '{b.Custcode}' AND " +
                         $"p.BISAFN = '{b.Custname}' AND " +
                         $"p.SHIPTYPE = '{b.Ship}' " +
                         $"{rescd}" +
                         $"AND p.COMMERCIAL = '{b.Commercial}' " +
                         $"AND p.PC = '{b.Pc}' " +
                         $"AND p.BIOABT = '{b.BioABT}' AND " +
                         $"get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    break;

                default:
                    sql = $"WHERE p.STATUS = 1 AND p.FACTORY = '{b.Factory}' AND p.ETDTAP = to_date('{b.Etd.ToString("ddMMyyyy")}', 'ddMMyyyy') AND " +
                          $"p.AFFCODE = '{b.Affcode}' AND " +
                          $"p.BISHPC = '{b.Custcode}' AND " +
                          $"p.BISAFN = '{b.Custname}' AND " +
                          $"p.SHIPTYPE = '{b.Ship}' {rescd} AND " +
                          $"p.COMMERCIAL = '{b.Commercial}' " +
                          $"AND p.PC = '{b.Pc}' " +
                          $"AND p.BIOABT = '{b.BioABT}' AND " +
                          $"p.PONO = '{b.PoType}' AND " +
                          $"get_zone(p.FACTORY,p.BIOABT) = '{b.Zone}'";
                    break;
            }
            return sql;
        }

        public List<OrderBody> GetReportJobList(OrderData b)
        {
            string sql = $"SELECT p.PONO custpono,p.PARTNO,CASE WHEN p.FACTORY = 'INJ' THEN p.PARTNO ELSE p.PARTNAME END PARTNAME,p.LOTNO,b.ORDERQTY BALQTY,round(b.ORDERQTY/b.STDPACK) orderctn,p.BIIVPX,p.BIOABT,p.COMMERCIAL,p.PC,p.UUID," +
                $"p.BISTDP,p.BIWIDT,p.BILENG,p.BIHIGH,p.BIGRWT,p.BINEWT,p.ORDERTYPE,p.CURINV,e.refinvoice invoceno,p.ORDERSTATUS," +
                $"substr(p.REASONCD, 1, 1) rewrite,p.upddte,p.bicomd FROM TXP_ORDERPLAN p\n" +
                "INNER JOIN TXP_ISSTRANSBODY b ON p.ORDERID = b.PONO AND p.PARTNO = b.PARTNO\n" +
                "LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n" +
                "LEFT JOIN TXP_PART m ON p.FACTORY = m.VENDORCD AND p.PARTNO = m.PARTNO\n";
            List<OrderBody> obj = new List<OrderBody>();
            sql += CheckOrderGroup(b);
            string ordby = "\nORDER BY p.PARTNO,round(p.BALQTY/p.BISTDP),p.PONO";
            if (b.Factory == "AW")
            {
                ordby = "\nORDER BY m.KIDS,m.SIZES ,p.ORDERID,p.LOTNO,round(p.BALQTY/p.BISTDP)";
            }
            sql += ordby;
            foreach (OrderBody od in AddOrderJobList(b, sql))
            {
                od.Id = obj.Count + 1;
                obj.Add(od);
            }
            return obj;
        }

        public List<OrderBody> GetOrderJobList(OrderData b)
        {
            string sql = $"SELECT p.PONO custpono,p.PARTNO,CASE WHEN p.FACTORY = 'INJ' THEN p.PARTNO ELSE p.PARTNAME END PARTNAME,p.LOTNO,p.BALQTY,round(p.BALQTY/p.BISTDP) orderctn,p.BIIVPX,p.BIOABT,p.COMMERCIAL,p.PC,p.UUID," +
                $"p.BISTDP,p.BIWIDT,p.BILENG,p.BIHIGH,p.BIGRWT,p.BINEWT,p.ORDERTYPE,p.CURINV,e.refinvoice invoceno,p.ORDERSTATUS," +
                $"substr(p.REASONCD, 1, 1) rewrite,p.upddte,p.bicomd FROM TXP_ORDERPLAN p\n" +
                "LEFT JOIN TXP_ISSTRANSBODY b ON p.ORDERID = b.PONO AND p.PARTNO = b.PARTNO\n" +
                "LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n" +
                "LEFT JOIN TXP_PART m ON p.FACTORY = m.VENDORCD AND p.PARTNO = m.PARTNO\n";
            List<OrderBody> obj = new List<OrderBody>();
            sql += CheckOrderGroup(b);
            string ordby = "\nORDER BY p.PARTNO,round(p.BALQTY/p.BISTDP),p.PONO";
            if (b.Factory == "AW")
            {
                ordby = "\nORDER BY m.KIDS,m.SIZES ,p.ORDERID,p.LOTNO,round(p.BALQTY/p.BISTDP)";
            }
            sql += ordby;
            foreach (OrderBody od in AddOrderJobList(b, sql))
            {
                od.Id = obj.Count + 1;
                obj.Add(od);
            }
            return obj;
        }

        public List<OrderBody> GetOrderDetail(OrderData b)
        {
            string sql = $"SELECT p.PONO custpono,p.PARTNO,CASE WHEN p.FACTORY = 'INJ' THEN p.PARTNO ELSE p.PARTNAME END PARTNAME,p.LOTNO,p.BALQTY,round(p.BALQTY/p.BISTDP) orderctn,p.BIIVPX,p.BIOABT,p.COMMERCIAL,p.PC,p.UUID," +
                $"p.BISTDP,p.BIWIDT,p.BILENG,p.BIHIGH,p.BIGRWT,p.BINEWT,p.ORDERTYPE,p.CURINV,e.refinvoice invoceno,p.ORDERSTATUS," +
                $"substr(p.REASONCD, 1, 1) rewrite,p.upddte,p.bicomd FROM TXP_ORDERPLAN p\n" +
                "LEFT JOIN TXP_ISSTRANSBODY b ON p.ORDERID = b.PONO AND p.PARTNO = b.PARTNO\n" +
                "LEFT JOIN TXP_ISSTRANSENT e ON p.CURINV = e.ISSUINGKEY\n" +
                "LEFT JOIN TXP_PART m ON p.FACTORY = m.VENDORCD AND p.PARTNO = m.PARTNO\n";
            List<OrderBody> obj = new List<OrderBody>();
            sql += CheckOrderGroup(b);
            string ordby = "\nORDER BY p.PARTNO,round(p.BALQTY/p.BISTDP),p.PONO";
            if (b.Factory == "AW")
            {
                ordby = "\nORDER BY m.KIDS,m.SIZES ,p.ORDERID,p.LOTNO,round(p.BALQTY/p.BISTDP)";
            }
            sql += ordby;

            foreach (OrderBody od in AddOrderBodyList(b, sql))
            {
                od.Id = obj.Count + 1;
                obj.Add(od);
            }
            return obj;
        }

        public List<string> GetOrderRefinvoice(OrderData b)
        {
            try
            {
                SplashScreenManager.Default.SetWaitFormDescription($"CHECK REFNO.");
            }
            catch (Exception ex)
            {
                GreeterFunction.Logs(ex.Message);
            }
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
            sql += "GROUP BY p.CURINV";
            Console.WriteLine(sql);
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 1)
            {
                return true;
            }
            return false;
        }

        public int GetOrderNotCreateJobList(DateTime etd)
        {
            string sql = $"SELECT p.ORDERID FROM TXP_ORDERPLAN p WHERE p.FACTORY= '{StaticFunctionData.Factory}' AND" +
                $" p.ETDTAP = to_date('{etd.ToString("dd/MM/yyyy")}', 'dd/MM/yyyy') AND " +
                $"p.CURINV IS NULL AND p.CURINV IS NULL AND p.BALQTY > 0\n" +
                $"GROUP BY p.ORDERID";
            Console.WriteLine(sql);
            int x = 0;
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 0)
            {
                return dr.Tables[0].Rows.Count;
            }
            return x;
        }

        public int GetLastPalletCtn(string issno, string pltype)
        {
            int x = 0;
            if (pltype == "B")
            {
                pltype = "C";
            }
            string sql = $"SELECT SUBSTR(PALLETNO, 3) ctn FROM TXP_ISSPALLET l WHERE ISSUINGKEY = '{issno}' AND PALLETNO  LIKE '1{pltype}%' ORDER BY PALLETNO DESC";
            DataSet dr = new ConnDB().GetFill(sql);
            if (dr.Tables[0].Rows.Count > 0)
            {
                x = int.Parse(dr.Tables[0].Rows[0]["ctn"].ToString());
            }
            else
            {
                sql = $"SELECT SUBSTR(PALLETNO, 3) ctn FROM TXP_ISSPALLET l WHERE SUBSTR(ISSUINGKEY, 0, 10) = '{issno.Substring(0,10)}' AND PALLETNO  LIKE '1{pltype}%' ORDER BY PALLETNO DESC";
                dr = new ConnDB().GetFill(sql);
                if (dr.Tables[0].Rows.Count > 0)
                {
                    x = int.Parse(dr.Tables[0].Rows[0]["ctn"].ToString());
                }
            }
            return x;
        }
    }
}
