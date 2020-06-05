namespace XPWLibrary.Interfaces
{
    public class StaticFunctionData
    {
        public static string fticketprinter { get; set; }
        public static string cartonticketprinter { get; set; }
        public static int aw_totalpallet { get; set; }
        public static int aw_boxbigsize { get; set; }
        public static int aw_palletlimit { get; set; }
        public static int aw_netweight { get; set; }
        public static string aw_plprinter { get; set; }
        public static string inj_plprinter { get; set; }
        public static bool confirmlotno { get; set; }
        public static string Factory { get; set; }
        public static string ConnString { get; set; }
        public static string DBname { get; set; }
        public static string AppVersion { get; set; }
        public static bool AllWeek { get; set; }
        public static bool EditShip { get; set; }
        public static bool EditOrder { get; set; }
        public static string PathExcute { get; set; }
        public static int ReloadGrid { get; set; }

        //add Langures
        public static string JobListTilte { get; internal set; }
        public static string JobListInformation { get; internal set; }
        public static string JoblistConfirmInv { get; set; }
        public static string JoblistPrintJobList { get; set; }
        public static string JoblistLabelShort { get; set; }
        public static string JoblistShippingList { get; set; }
        public static string JoblistShippingListByPart { get; set; }
        public static string JoblistShippingListByAll { get; set; }
        public static string JoblistPalletList { get; set; }
        public static string JoblistSplitPart { get; set; }
        public static string JoblistEditOrder { get; set; }
        public static string JoblistSetMultiLot { get; set; }
        public static string JoblistPartShort { get; set; }
        public static string JoblistOrderHold { get; set; }
        public static string JoblistOrderCancel { get; set; }
    }
}
