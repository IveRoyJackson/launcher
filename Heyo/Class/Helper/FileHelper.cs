using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;

namespace Heyo.Class.Helper
{
    class FileHelper
    {
        /// Return Type: UINT->unsigned int
        ///lpszFile: LPCWSTR->WCHAR*
        ///nIconIndex: int
        ///phiconLarge: HICON*
        ///phiconSmall: HICON*
        ///nIcons: UINT->unsigned int
        [System.Runtime.InteropServices.DllImportAttribute("shell32.dll", EntryPoint = "ExtractIconExW", CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
        public static extern uint ExtractIconExW([System.Runtime.InteropServices.InAttribute()] [System.Runtime.InteropServices.MarshalAsAttribute(System.Runtime.InteropServices.UnmanagedType.LPWStr)] string lpszFile, int nIconIndex, ref System.IntPtr phiconLarge, ref System.IntPtr phiconSmall, uint nIcons);
        /// <summary>  
        /// 通过扩展名得到图标和描述  
        /// </summary>  
        /// <param name="ext">扩展名(如“.txt”)</param>  
        /// <param name="largeIcon">得到大图标</param>  
        /// <param name="smallIcon">得到小图标</param>  
        /// <param name="description">得到类型描述或者空字符串</param>  
        public static void GetExtsIconAndDescription(string ext, out Icon largeIcon, out Icon smallIcon, out string description)
        {
            GetDefaultIcon(out largeIcon, out smallIcon);   //得到缺省图标  
            description = "";                               //缺省类型描述  
            RegistryKey extsubkey = Registry.ClassesRoot.OpenSubKey(ext);   //从注册表中读取扩展名相应的子键  
            if (extsubkey == null) return;

            string extdefaultvalue = extsubkey.GetValue(null) as string;     //取出扩展名对应的文件类型名称  
            if (extdefaultvalue == null) return;

            if (extdefaultvalue.Equals("exefile", StringComparison.OrdinalIgnoreCase))  //扩展名类型是可执行文件  
            {
                RegistryKey exefilesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue);  //从注册表中读取文件类型名称的相应子键  
                if (exefilesubkey != null)
                {
                    string exefiledescription = exefilesubkey.GetValue(null) as string;   //得到exefile描述字符串  
                    if (exefiledescription != null) description = exefiledescription;
                }
                IntPtr exefilePhiconLarge = new IntPtr();
                IntPtr exefilePhiconSmall = new IntPtr();
                ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 2, ref exefilePhiconLarge, ref exefilePhiconSmall, 1);
                if (exefilePhiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(exefilePhiconLarge);
                if (exefilePhiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(exefilePhiconSmall);
                return;
            }

            RegistryKey typesubkey = Registry.ClassesRoot.OpenSubKey(extdefaultvalue);  //从注册表中读取文件类型名称的相应子键  
            if (typesubkey == null) return;

            string typedescription = typesubkey.GetValue(null) as string;   //得到类型描述字符串  
            if (typedescription != null) description = typedescription;

            RegistryKey defaulticonsubkey = typesubkey.OpenSubKey("DefaultIcon");  //取默认图标子键  
            if (defaulticonsubkey == null) return;

            //得到图标来源字符串  
            string defaulticon = defaulticonsubkey.GetValue(null) as string; //取出默认图标来源字符串  
            if (defaulticon == null) return;
            string[] iconstringArray = defaulticon.Split(',');
            int nIconIndex = 0; //声明并初始化图标索引  
            if (iconstringArray.Length > 1)
                if (!int.TryParse(iconstringArray[1], out nIconIndex))
                    nIconIndex = 0;     //int.TryParse转换失败，返回0  

            //得到图标  
            IntPtr phiconLarge = new IntPtr();
            IntPtr phiconSmall = new IntPtr();
            ExtractIconExW(iconstringArray[0].Trim('"'), nIconIndex, ref phiconLarge, ref phiconSmall, 1);
            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
            if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
        }

        /// <summary>  
        /// 获取缺省图标  
        /// </summary>  
        /// <param name="largeIcon"></param>  
        /// <param name="smallIcon"></param>  
        private static void GetDefaultIcon(out Icon largeIcon, out Icon smallIcon)
        {
            largeIcon = smallIcon = null;
            IntPtr phiconLarge = new IntPtr();
            IntPtr phiconSmall = new IntPtr();
            ExtractIconExW(Path.Combine(Environment.SystemDirectory, "shell32.dll"), 0, ref phiconLarge, ref phiconSmall, 1);
            if (phiconLarge.ToInt32() > 0) largeIcon = Icon.FromHandle(phiconLarge);
            if (phiconSmall.ToInt32() > 0) smallIcon = Icon.FromHandle(phiconSmall);
        }
    }
}
