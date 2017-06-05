using Heyo.Class;
using Heyo.Class.Helper;
using static Heyo.Class.Helper.ZIP.ZIP;
using Heyo.Class.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Heyo.Workers;
using HeyoCraft.Class.Data;
using HeyoCraft.Workers;

namespace HeyoCraft.Class.Helper
{
    public class Launcher
    {
        public Minecraft Game { get; set; }
        public string JavaPath { get; set; }
        public int MaxMem { get; set; }
        public User User { get; set; }
        public event DataReceivedEventHandler OutputDataReceived;
        public event DataReceivedEventHandler ErrorDataReceived;

        /// <param name="mc">MC版本</param>
        /// <param name="javaPath">JAVA路径</param>
        /// <param name="maxMem">最大内存</param>
        /// <param name="user">玩家数据</param>
        /// <param name="outputDataReceived">标准输出事件</param>
        /// <param name="errorDataReceived">错误输出事件</param>
        public Launcher(Minecraft mc, string javaPath, int maxMem, User user, DataReceivedEventHandler outputDataReceived = null, DataReceivedEventHandler errorDataReceived = null)
        {
            Game = mc;
            JavaPath = javaPath;
            User = user;
            MaxMem = maxMem;
            OutputDataReceived += outputDataReceived;
            ErrorDataReceived += errorDataReceived;
        }
        /// <summary>
        /// 启动MC
        /// </summary>
        public void Launch()
        {

            Process p = new Process();
            p.StartInfo.FileName = JavaPath;
            string arg= BuildArguments();
            if (arg == null)
                return;
            p.StartInfo.Arguments = arg;
            p.StartInfo.WorkingDirectory = Game.PublicDirectory;
            //将工作目录设置为版本文件夹，强制隔离各个版本
            //如logs,mods,assets等文件夹都会被放在 "/.minecraft/versions/版本号/" 下
            //如果取消将上一行代码替换为↓(不建议取消，会爆炸)
            //p.StartInfo.WorkingDirectory = Environment.CurrentDirectory + "\\.minecraft";
            /*string publicPath = Game.PublicDirectory;
            foreach (var item in Directory.EnumerateDirectories(publicPath))
            {
                string name = item.Substring(item.LastIndexOf('\\') + 1);
                if (name == "assets" || name == "libraries" || name == "versions" || name == "natives" || name == "logs")
                {
                    continue;
                }

                DirectoryEx.CopyDirectory(item, Game.AbsolutelyDirectory);
            }
                
            string publicAssetsPath = Path.Combine(publicPath, "assets\\");
            if (File.Exists(Path.Combine(publicAssetsPath, "indexes\\" + Game.assets + ".json")))
            {
                if(!File.Exists(Path.Combine(Game.AbsolutelyDirectory, "assets\\indexes\\" + Game.assets + ".json")))
                {
                    DirectoryEx.CopyDirectory(publicAssetsPath, Path.Combine(Game.AbsolutelyDirectory, "assets\\"));
                }
            }*/

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = true;//接受来自调用程序的输入信息
            p.StartInfo.RedirectStandardOutput = true;//由调用程序获取输出信息
            p.StartInfo.RedirectStandardError = true;//重定向标准错误输出
            p.Start();
            if(OutputDataReceived!=null)
            {
                p.OutputDataReceived += OutputDataReceived;
                p.BeginOutputReadLine();
            }
            if(ErrorDataReceived!=null)
            {
                p.ErrorDataReceived += ErrorDataReceived;
                p.BeginErrorReadLine();
            }
            //p.StandardError
        }
        //bool wait = true;
        /// <summary>
        /// 生成启动参数
        /// </summary>
        /// <returns></returns>
        public string BuildArguments()
        {

            string nativeDir = string.Format("{0}\\versions\\{1}\\{1}-natives", Game.PublicDirectory, Game.id);//生成natives文件夹,放那些DLL用
            StringBuilder libBuilder = new StringBuilder();
            foreach (var item in Game.libraries)
            {
                libBuilder.Append(item.AbsolutelyJarPath).Append(";");
                if (!File.Exists(item.AbsolutelyJarPath))
                {
                    //Console.WriteLine(item.AbsolutelyJarPath);
                    HttpDownloader hdl = new HttpDownloader(item.BmclUri, item.AbsolutelyJarPath);
                    hdl.Work(null, new System.ComponentModel.DoWorkEventArgs(null));
                    
                    //while (wait) ;
                    //wait = true;
                }
                if (item.natives != null)
                {
                    UnZip(item.AbsolutelyJarPath, nativeDir); //通通解压到natives文件夹
                }
            }
            libBuilder.Append(Game.AbsolutelyJarPath).Append(";");
            string libDirs = libBuilder.ToString();

            StringBuilder gameArgBuilder = new StringBuilder(Game.minecraftArguments);

            string guid = User.SID == null ? SettingProxy.GetUuidByName(User.Name) : User.SID;

            gameArgBuilder.Replace("${auth_player_name}", User.Name)
                .Replace("${version_name}", Game.id)
                .Replace("${game_directory}", ".")
                .Replace("${game_assets}", "assets")
                .Replace("${assets_root}", "assets")
                .Replace("${user_type}", "Legacy")
                .Replace("${assets_index_name}", Game.assets)
                .Replace("${auth_uuid}", guid)
                .Replace("${auth_access_token}", User.accessToken ?? guid)
                .Replace("${user_properties}", "{}");

            string gameArg = gameArgBuilder.ToString();

            string arguments = string.Format(Game.RefArguments, MaxMem, nativeDir, libDirs, Game.mainClass, gameArg);

            //FileStream fs = new FileStream(@"E:\\1.txt", FileMode.Create);
            //byte[] buffer = arguments.ToBytes();
            //fs.Write(buffer,0,buffer.Length);
            //fs.Close();
            return arguments;
        }
    }
}
