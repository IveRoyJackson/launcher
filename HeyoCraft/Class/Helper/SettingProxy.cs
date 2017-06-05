using Heyo.Class.Helper;
using HeyoCraft.Class.Data;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Media;
using static HeyoCraft.Properties.Settings;

namespace HeyoCraft.Class.Helper
{
    public class SettingProxy
    {
        public static string GetUuidByName(string playerName)
        {
            string result;
            if(Default.GUID.Count == PlayerNames.Count)
            {
                if(PlayerNames.Contains(playerName))
                {
                    int index = PlayerNames.IndexOf(playerName);
                    return Default.GUID[index];
                }
            }else
            {
                PlayerNames.Clear();
                Default.GUID.Clear();
            }
            result = Guid.NewGuid().ToString();
            PlayerNames.Add(playerName);
            Default.GUID.Add(result);
            Default.Save();
            return result;
            //Console.WriteLine(result);
        }
        public  static StringCollection PlayerNames
        {
            get
            {
                return Default.Name;
            }
            set
            {
                Default.Name = value;
                Default.Save();
            }
        }
        public static string Name
        {
            get
            {
                if (Default.Name.Count > 0)
                {
                    //Console.WriteLine(Default.Name[Default.Name.Count - 1]);
                    return Default.Name[Default.Name.Count - 1];
                }
                else
                {
                    return null;
                }
            }
            set
            {
                Default.Name.Add(value);
                Default.Save();
            }
        }
        /// <summary>
        /// 上次选择的Minecraft
        /// </summary>
        public static int ChooseIndex
        {
            get
            {
                return Default.LastChooseIndex;
            }
            set
            {
                Default.LastChooseIndex = value;
                Default.Save();
            }
        }
        public static string ChooseId
        {
            get
            {
                return Default.LastChooseId;
            }
            set
            {
                Default.LastChooseId = value;
                Default.Save();
            }
        }
            
        public static bool FirstTime
        {
            get
            {
                return Default.FirstTime;
            }
            set
            {
                Default.FirstTime = value;
                Default.Save();
            }
        }
        public static int MemorySize
        {
            get
            {
                return Default.MemorySize;
            }
            set
            {
                Default.MemorySize = value;
                Default.Save();
            }
        }
        public static int JavaSelectedIndex
        {
            get
            {
                return Default.JavaSelectedIndex;
            }
            set
            {
                Default.JavaSelectedIndex = value;
                Default.Save();
            }
        }
        public static StringCollection JavasPath
        {
            get
            {
                return Default.Javas;
            }
            set
            {
                Default.Javas = value;
                Default.Save();
            }
        }
        /// <summary>
        /// 是路径
        /// </summary>
        public static StringCollection MinecraftsPath
        {
            get
            {
                return Default.Minecrafts;
            }
            set
            {
                Default.Minecrafts = value;
                Default.Save();
            }
        }
        public static bool LiteMode
        {
            get
            {
                return Default.LiteMode;
            }
            set
            {
                Default.LiteMode = value;
                Default.Save();
            }
        }

        public static string MCID
        {
            get
            {
                return Default.MCID;
            }
            set
            {
                Default.MCID = value;
                Default.Save();
            }
        }
        public static Color ThemeColor
        {
            get
            {
                System.Drawing.Color c = Default.ThemeColor;
                return new Color() { A = c.A, R = c.R, G = c.G, B = c.B }; 
            }
            set
            {
                System.Drawing.Color c = System.Drawing.Color.FromArgb(value.A,value.R,value.G,value.B);
                Default.ThemeColor = c;
                Default.Save();
            }
        }
        public static string Password
        {
            get
            {
                return Default.Password;
            }
            set
            {
                Default.Password = value;
                Default.Save();
            }
        }
        public static bool RemPW
        {
            get
            {
                return Default.remPW;
            }
            set
            {
                Default.remPW = value;
                Default.Save();
            }
        }
        public static bool AutoLogIn
        {
            get
            {
                return Default.AutoLogIn;
            }
            set
            {
                Default.AutoLogIn = value;
                Default.Save();
            }
        }
        public static string DownloadPath
        {
            get
            {
                return Default.DownloadPath;
            }
            set
            {
                Default.DownloadPath = value;
                Default.Save();
            }
        }

        public static string FUCKINGSOARYUN = @"http://mc.soaryun.com/";
        /// <summary>
        /// 是Minecraft不是路径,对此项的任何更改需要调用save方法来保存
        /// </summary>
        public static List<Minecraft> Minecrafts = new List<Minecraft>();
        public static List<ModData> Mods = new List<ModData>();
        public static List<ResPackData> ResPack = new List<ResPackData>();
        public static List<ScreenshotData> Screenshot = new List<ScreenshotData>();
        public static List<ShaderPackData> ShaderPack = new List<ShaderPackData>();
        public static List<SaveData> Saves = new List<SaveData>();
        /// <summary>
        /// Version
        /// </summary>
        public static string Version { get; set; } = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        public static Dictionary<string, string> Javas { get; set; } = new Dictionary<string, string>();
        public static User User { get; set; } = new User();
        public static bool IsLogedIn { get; set; } = false;
        static SettingProxy()
        {
            try
            {
                Minecrafts = Searcher.SearchMC();
                Mods = Searcher.SearchItem<ModData>(Environment.CurrentDirectory + "\\.minecraft\\");
                ResPack = Searcher.SearchItem<ResPackData>(Environment.CurrentDirectory + "\\.minecraft\\");
                Screenshot = Searcher.SearchItem<ScreenshotData>(Environment.CurrentDirectory + "\\.minecraft\\");
                ShaderPack = Searcher.SearchItem<ShaderPackData>(Environment.CurrentDirectory + "\\.minecraft\\");
                Saves= Searcher.SearchItem<SaveData>(Environment.CurrentDirectory + "\\.minecraft\\");
                if (SettingProxy.Name?.Length > 0)
                {
                    User.Name = SettingProxy.Name;
                }
                else
                {
                    User.Name = "NAME";
                }

                Javas = Searcher.GetJavas();
                foreach (var item in JavasPath)
                {
                    if (!Javas.ContainsValue(item))
                    {
                        Javas.Add(item, item);
                    }
                }
                foreach (var item in Javas.Values)
                {
                    if (!JavasPath.Contains(item))
                    {
                        JavasPath.Add(item);
                    }
                }
            }
            catch(Exception ex)
            {
                LogHelper.WriteError(ex);
                throw ex;
            }
           
        }
        public static void Save()
        {
            StringCollection paths = new StringCollection();
            foreach (var item in Minecrafts)
            {
                paths.Add(item.AbsolutelyJsonDirectory);
            }
            MinecraftsPath = paths;
        }
        public static bool UseAnimation
        {
            get
            {
                return Default.UseAnimation;
            }
            set
            {
                Default.UseAnimation = value;
                Default.Save();
            }
        }
    }
}
