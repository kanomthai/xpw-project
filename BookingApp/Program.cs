﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XPWLibrary.Interfaces;

namespace BookingApp
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
                Application.Run(new BookingForm());
            }
            else
            {
                Console.WriteLine("error");
            }
        }
    }
}
