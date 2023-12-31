﻿using System;
using System.Windows.Forms;

namespace NAS
{
    internal class NasClientProgram
    {
        private NasClientProgram() { }

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(AuthForm.GetForm());
        }
    }
}
