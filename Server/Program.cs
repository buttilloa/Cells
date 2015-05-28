using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Server
{

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>  private static NetServer server;
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ServerForm());

        }

    }
}
