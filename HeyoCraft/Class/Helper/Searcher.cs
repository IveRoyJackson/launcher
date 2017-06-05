using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;
using System.IO;
using System.Text.RegularExpressions;
using Heyo.Class.Data;
using HeyoCraft.Class.Data;

namespace HeyoCraft.Class.Helper
{
    class Searcher
    {
        public static Dictionary<string,string> GetJavas()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            RegistryKey pregkey = Registry.LocalMachine.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Uninstall");
            Dictionary<string, RegistryKey> nameKeys=new Dictionary<string, RegistryKey>();
            foreach (var item in pregkey.GetSubKeyNames())
            {
                if(!nameKeys.ContainsKey(item))
                    nameKeys.Add(item, pregkey);
            }
            
            if (Environment.Is64BitOperatingSystem && !Environment.Is64BitProcess)
            {
                RegistryKey localMachine = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                RegistryKey pregkey64 = localMachine.OpenSubKey(@"Software\\Microsoft\Windows\CurrentVersion\Uninstall");
                foreach (var item in pregkey64.GetSubKeyNames())
                {
                    if (!nameKeys.ContainsKey(item))
                        nameKeys.Add(item, pregkey64);
                }
            }

            
            RegistryKey currentKey = null;
            foreach (var item in nameKeys.Keys)
            {
                currentKey = nameKeys[item].OpenSubKey(item);
                string displayName = currentKey.GetValue("DisplayName") as string;           //获取显示名称
                if(displayName!=null && displayName.ToLower().Contains("java"))
                {
                    Console.WriteLine(displayName);
                    string installSource = currentKey.GetValue("InstallLocation") as string;   //获取路径
                    int lenght = installSource.LastIndexOf(@"\");
                    if (lenght < 1)
                        continue;
                    installSource = installSource.Substring(0, installSource.LastIndexOf(@"\")) + "\\bin\\javaw.exe";
                    result.Add(displayName, installSource);
                }
            }
            return result;
        }
        [Obsolete]
        public static List<string> SearchMCJson()
        {
            //List<string> result = new List<string>();
            Regex r = new Regex(@".json", RegexOptions.IgnoreCase);
            List<string> result = GetFileName(Environment.CurrentDirectory+"\\.minecraft\\versions\\", r);

            return result;
        }
        [Obsolete]
        public static List<Minecraft> SearchMC()
        {
            List<Minecraft> result=new List<Minecraft>();
            var jsons = SearchMCJson();
            IEnumerable<string> effective = new List<string>();
            result = Builder.BuildMinecraftFromJsonFile(jsons,out effective);
            return result;
        }
        public static List<T> SearchItem<T>(string path) where T : ItemData, new()
        {
            List<T> result = new List<T>();
            T t= new T();
            path = path + t.FileLocation + "\\";
            if (!Directory.Exists(path))
                return result;
            IEnumerable<string> files = Directory.EnumerateFiles(path);
            
            foreach (var item in files)
            {
                T t2 = new T();
                t2.AbsolutelyFilePath = item;
                result.Add(t2);
            }
            return result;
        }
        public static List<string> GetFileName(string DirName, Regex regex)
        {
            //文件夹信息
            DirectoryInfo dir = new DirectoryInfo(DirName);
            List<string> result = new List<string>();
            //如果非根路径且是系统文件夹则跳过
            if (null != dir.Parent && dir.Attributes.ToString().IndexOf("System") > -1)
            {
                return result;
            }
            if (!dir.Exists)
                return result;
            //取得所有文件
            FileInfo[] finfo = dir.GetFiles();
            string fname = string.Empty;
            for (int i = 0; i < finfo.Length; i++)
            {
                fname = finfo[i].Name;
                //判断文件是否包含查询名
                if(regex.Match(fname).Success)
                {
                    if(!result.Contains(finfo[i].DirectoryName + @"\" + fname))
                        result.Add(finfo[i].DirectoryName + @"\" + fname);
                }
            }
            //取得所有子文件夹
            DirectoryInfo[] dinfo = dir.GetDirectories();
            for (int i = 0; i < dinfo.Length; i++)
            {
                //查找子文件夹中是否有符合要求的文件
                result.AddRange(GetFileName(dinfo[i].FullName, regex));
            }
            return result;
        }
    }
}
