using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XPWLibrary.Models
{
    public class PlListData
    {
        public int Id { get; set; }
        public string PlNo { get; set; }
        public string PlKey { get; set; }
        public string ContNo { get; set; }
        public string PlType { get; set; }
        public int PlSize { get; set; }
        public int PlTotal { get; set; }
        public int PlStatus { get; set; }
    }

    public class PlObjData : PlListData
    {
        public string RefInv { get; set; }
        public string PlStr { get; set; }
    }

    public class PlSizeData
    {
        public string PlStr { get; set; }
        public int FullTotal { get; set; }
    }
}
