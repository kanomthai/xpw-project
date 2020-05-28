using DevExpress.Skins;
using System;
using System.Windows.Forms;
using XPWLibrary.Interfaces;

namespace InvoiceApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        [Obsolete]
        static void Main()
        {
            if (new GreeterFunction().BeginingLoadApp())
            {
                //enable load skin
                DevExpress.UserSkins.BonusSkins.Register();
                DevExpress.UserSkins.OfficeSkins.Register();
                SkinManager.EnableFormSkins();
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new InvoiceMainForm());
            }
            else
            {
                Console.WriteLine("error");
            }
        }
    }
}
