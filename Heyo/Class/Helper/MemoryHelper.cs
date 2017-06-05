using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualBasic.Devices;
using Microsoft.VisualBasic.CompilerServices;
using System.Diagnostics;

namespace Heyo.Class.Helper
{
    /// <summary>
    /// 内存控制
    /// </summary>
    [OptionText, StandardModule]
    public class MemoryHelper
    {
        [DllImport("kernel32.dll", EntryPoint = "SetProcessWorkingSetSize")]
        public static extern int SetProcessWorkingSetSize(IntPtr process, int minSize, int maxSize);
        /// <summary>
        /// 清理自身内存
        /// </summary>
        internal static void CleanSelfMemory()
        {
            MemoryHelper.SetProcessWorkingSetSize(Process.GetCurrentProcess().Handle, -1, -1);
        }
        /// <summary>
        /// 清理全局内存
        /// </summary>
        public static void CleanAllMemory()
        {
            Process[] processes = Process.GetProcesses();
            checked
            {
                for (int i = 0; i < processes.Length; i++)
                {
                    Process process = processes[i];
                    try
                    {
                        SetProcessWorkingSetSize(process.Handle, -1, -1);
                    }
                    catch (Exception)
                    {
                        //ProjectData.SetProjectError(expr_3C);
                        //ProjectData.ClearProjectError();
                    }
                }
            }
        }
        /// <summary>
        /// 获取总内存大小
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static double GetTotalPhysicalMemory(Unit unit = Unit.MB)
        {
            ComputerInfo ci = new ComputerInfo();
            double value = ci.TotalPhysicalMemory;
            if (unit == Unit.MB)
            {
                return value / 1024 / 1024;
            }
            else if (unit == Unit.GB)
            {
                return value / 1024 / 1024 / 1024;
            }
            else if (unit == Unit.KB)
            {
                return value / 1024;
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// 获取剩余内存
        /// </summary>
        /// <param name="unit"></param>
        /// <returns></returns>
        public static double GetAvailablePhysicalMemory(Unit unit=Unit.MB)
        {
            ComputerInfo ci = new ComputerInfo();
            double value = ci.AvailablePhysicalMemory;
            if (unit== Unit.MB)
            {
                return value / 1024 / 1024;
            }else if(unit==Unit.GB)
            {
                return value / 1024 / 1024 / 1024;
            }
            else if(unit == Unit.KB)
            {
                return value / 1024;
            }
            else
            {
                return value;
            }
        }
        /// <summary>
        /// 内存单位
        /// </summary>
        public enum Unit
        {
            B,KB,MB,GB
        }
    }
}
