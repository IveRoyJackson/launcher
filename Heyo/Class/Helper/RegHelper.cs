using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Heyo.Class.Helper
{
    class RegHelper
    {
        #region 32位程序读写64注册表

        static UIntPtr HKEY_CLASSES_ROOT = (UIntPtr)0x80000000;
        static UIntPtr HKEY_CURRENT_USER = (UIntPtr)0x80000001;
        static UIntPtr HKEY_LOCAL_MACHINE = (UIntPtr)0x80000002;
        static UIntPtr HKEY_USERS = (UIntPtr)0x80000003;
        static UIntPtr HKEY_CURRENT_CONFIG = (UIntPtr)0x80000005;

        // 关闭64位（文件系统）的操作转向
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);
        // 开启64位（文件系统）的操作转向
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        [DllImport("advapi32.dll")]
        extern private static int RegQueryInfoKey(
            IntPtr hkey,
            StringBuilder lpClass,
            uint lpcbClass,
            IntPtr lpReserved,
            out uint lpcSubKeys,
            IntPtr lpcbMaxSubKeyLen,
            IntPtr lpcbMaxClassLen,
            out uint lpcValues,
            IntPtr lpcbMaxValueNameLen,
            IntPtr lpcbMaxValueLen,
            IntPtr lpcbSecurityDescriptor,
            IntPtr lpftLastWriteTime
        );
        //枚举指定项下方的子项
        [DllImport("advapi32.dll", CharSet = CharSet.Auto , EntryPoint = "RegEnumKeyEx")]
        extern private static int RegEnumKeyEx(IntPtr hkey,
            uint index,
            StringBuilder lpName,
            ref uint lpcbName,
            IntPtr reserved,
            IntPtr lpClass,
            IntPtr lpcbClass,
            out long lpftLastWriteTime);
        // 获取操作Key值句柄
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern uint RegOpenKeyEx(UIntPtr hKey, string lpSubKey, uint ulOptions, int samDesired, out IntPtr phkResult);
        //关闭注册表转向（禁用特定项的注册表反射）
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long RegDisableReflectionKey(IntPtr hKey);
        //使能注册表转向（开启特定项的注册表反射）
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern long RegEnableReflectionKey(IntPtr hKey);
        //获取Key值（即：Key值句柄所标志的Key对象的值）
        [DllImport("Advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int RegQueryValueEx(IntPtr hKey, string lpValueName, int lpReserved,
                                                  out uint lpType, System.Text.StringBuilder lpData,
                                                  ref uint lpcbData);

        public enum ParentKeyName
        {
            HKEY_CLASSES_ROOT,
            HKEY_CURRENT_USER,
            HKEY_LOCAL_MACHINE,
            HKEY_USERS,
            HKEY_CURRENT_CONFIG,
        }
        private static UIntPtr TransferKeyName(string keyName)
        {
            switch (keyName)
            {
                case "HKEY_CLASSES_ROOT":
                    return HKEY_CLASSES_ROOT;
                case "HKEY_CURRENT_USER":
                    return HKEY_CURRENT_USER;
                case "HKEY_LOCAL_MACHINE":
                    return HKEY_LOCAL_MACHINE;
                case "HKEY_USERS":
                    return HKEY_USERS;
                case "HKEY_CURRENT_CONFIG":
                    return HKEY_CURRENT_CONFIG;
            }

            return HKEY_CLASSES_ROOT;
        }

        public static string Get64BitRegistryKey(ParentKeyName parentKeyName, string subKeyName, string keyName)
        {
            int KEY_QUERY_VALUE = (0x0001);
            int KEY_WOW64_64KEY = (0x0100);
            int KEY_ALL_WOW64 = (KEY_QUERY_VALUE | KEY_WOW64_64KEY);

            try
            {
                //将Windows注册表主键名转化成为不带正负号的整形句柄（与平台是32或者64位有关）
                UIntPtr hKey = TransferKeyName(parentKeyName.ToString());

                //声明将要获取Key值的句柄
                IntPtr pHKey = IntPtr.Zero;

                //记录读取到的Key值
                StringBuilder result = new StringBuilder("".PadLeft(1024));
                uint resultSize = 1024;
                uint lpType = 0;

                //关闭文件系统转向 
                IntPtr oldWOW64State = new IntPtr();
                if (Wow64DisableWow64FsRedirection(ref oldWOW64State))
                {
                    //获得操作Key值的句柄
                    RegOpenKeyEx(hKey, subKeyName, 0, KEY_ALL_WOW64, out pHKey);
                    //关闭注册表转向（禁止特定项的注册表反射）
                    RegDisableReflectionKey(pHKey);
                    //获取访问的Key值
                    RegQueryValueEx(pHKey, keyName, 0, out lpType, result, ref resultSize);
                    //打开注册表转向（开启特定项的注册表反射）
                    RegEnableReflectionKey(pHKey);
                }
                //打开文件系统转向
                Wow64RevertWow64FsRedirection(oldWOW64State);
                //返回Key值
                return result.ToString().Trim();
            }
            catch
            {
                return null;
            }
        }

        public static string[] Enum64BitRegistryKey(ParentKeyName parentKeyName, string subKeyName)
        {
            uint numSubKeys = 0, numValues;
            UIntPtr hKey = TransferKeyName(parentKeyName.ToString());
            uint s = 0;
            IntPtr pHKey = IntPtr.Zero;
            int KEY_QUERY_VALUE = (0x0001);
            int KEY_WOW64_64KEY = (0x0100);
            int KEY_ENUMERATE_SUB_KEYS = (0x0008);
            int KEY_ALL_WOW64 = (KEY_QUERY_VALUE | KEY_WOW64_64KEY | KEY_ENUMERATE_SUB_KEYS);
            // Create an array to hold the names.
            string[] names = new string[0]; ;

            IntPtr oldWOW64State = new IntPtr();
            //关闭文件系统转向 
            if (Wow64DisableWow64FsRedirection(ref oldWOW64State))
            {
                RegOpenKeyEx(hKey, subKeyName, 0, KEY_ALL_WOW64, out pHKey);
                //关闭注册表转向（禁止特定项的注册表反射）
                RegDisableReflectionKey(pHKey);

                RegQueryInfoKey(pHKey, null, s, IntPtr.Zero, out numSubKeys, IntPtr.Zero, IntPtr.Zero, out numValues, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
                names = new string[numSubKeys];
                uint index = 0;
                long writeTime;
                while (index < numSubKeys)
                {
                    uint MAX_REG_KEY_SIZE = 2048;
                    StringBuilder sb = new StringBuilder();
                    int ret = RegEnumKeyEx(pHKey, index, sb, ref MAX_REG_KEY_SIZE, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero, out writeTime);
                    if (ret != 0)
                    {
                        break;
                    }
                    Console.WriteLine(sb.ToString());
                    names[(int)(index++)] = sb.ToString();
                }

                //开启注册表转向
                RegEnableReflectionKey(pHKey);
            }
            //打开文件系统转向
            Wow64RevertWow64FsRedirection(oldWOW64State);

            return names;
        }

        #endregion
    }
}
