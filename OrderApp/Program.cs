using System;
using System.Windows.Forms;
using XPWLibrary.Interfaces;

namespace OrderApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (new GreeterFunction().BeginingLoadApp())
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new OrderMainForm());
            }
            else
            {
                Console.WriteLine("error");
            }
        }
    }
}
