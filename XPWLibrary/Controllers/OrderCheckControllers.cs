using System;
using System.Collections.Generic;
using System.Data;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace XPWLibrary.Controllers
{
    public class OrderCheckControllers
    {
        public List<OrderCheckData> GetOrderList(DateTime etd)
        {
            List<OrderCheckData> obj = new List<OrderCheckData>();
            string sql = $"";
            DataSet dr = new ConnDB().GetFill(sql);
            foreach (DataRow r in dr.Tables[0].Rows)
            {

            }
            return obj;
        }
    }
}
