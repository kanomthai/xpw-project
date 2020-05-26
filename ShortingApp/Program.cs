using System;
using System.Windows.Forms;
using XPWLibrary.Interfaces;
using XPWLibrary.Models;

namespace ShortingApp
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
                OrderData obj = new OrderData();
                Application.Run(new ShortingForm(obj));
            }
            else
            {
                Console.WriteLine("error");
            }
        }
    }
}
