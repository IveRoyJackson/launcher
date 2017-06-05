using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Windows;

namespace Heyo.Class.Helper
{
    public class PermissionHelper
    {
        /// <summary>
        /// 检查程序是否在管理员身份下运行
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator()
        {
            WindowsIdentity current = WindowsIdentity.GetCurrent();
            WindowsPrincipal windowsPrincipal = new WindowsPrincipal(current);
            return windowsPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        /// <summary>
        /// 以管理员身份重启程序
        /// </summary>
        public static void RestartByAdministrator()
        {
            ProcessStartInfo psi = new ProcessStartInfo();
            psi.FileName = System.Windows.Forms.Application.ExecutablePath;
            psi.Verb = "runas";
            try
            {
                Process.Start(psi);
                Application.Current.Shutdown();
            }
            catch
            {

            }
        }
    }
}
