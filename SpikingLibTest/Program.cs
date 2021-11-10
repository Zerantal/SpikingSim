﻿using System;
using System.Windows.Forms;
using System.Diagnostics;

namespace SpikingLibTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            TextWriterTraceListener debugFile = new TextWriterTraceListener("debugLog.txt");
            Debug.Listeners.Add(debugFile);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());           
        }
    }
}
