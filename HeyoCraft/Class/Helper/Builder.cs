using Heyo.Class.Data;
using Heyo.Class.Helper;
using HeyoCraft.Class.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace HeyoCraft.Class.Helper
{
    class Builder
    {
        /// <summary>
        /// 通过jsons读取Minecraft
        /// </summary>
        /// <param name="jsonPaths"></param>
        /// <returns></returns>
        public static List<Minecraft> BuildMinecraftFromJsonFile(IEnumerable<string> jsonPaths , out IEnumerable<string> effectiveJsonPaths)
        {
            List<Minecraft> result = new List<Minecraft>();
            effectiveJsonPaths = new List<string>();

            Dictionary<string, List<Minecraft>> mcd = new Dictionary<string, List<Minecraft>>();
            foreach (var item in jsonPaths)
            {
                if (!File.Exists(item))
                    continue;
                if (item.Contains(Environment.GetFolderPath(Environment.SpecialFolder.System)) ||
                    item.Contains(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86)) ||
                    item.Contains(Environment.GetFolderPath(Environment.SpecialFolder.Windows)) ||
                    item.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles)) ||
                    item.Contains(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)) ||
                    item.Contains(Environment.GetFolderPath(Environment.SpecialFolder.Templates)))
                {

                    continue;
                }
                string str;
                try
                {
                    StreamReader sr = new StreamReader(item, Encoding.Default);
                    str = sr.ReadToEnd();
                }
                catch
                {
                    continue;
                }

                if (!str.Contains("minecraftArguments"))
                    continue;
                Minecraft mcv = JsonHelper.JsonDeserializeBySingleData<Minecraft>(str);
                try
                {
                    mcv.AbsolutelyJsonDirectory = item;
                }
                catch
                {
                    continue;
                }
                
                foreach (var lib in mcv.libraries)
                {
                    lib.Owner = mcv;
                }
                if (mcv.id == null)
                    continue;
                if (mcd.ContainsKey(mcv.id))
                    mcd[mcv.id].Add(mcv);
                else
                    mcd.Add(mcv.id, new List<Minecraft>() { mcv });

                (effectiveJsonPaths as List<string>).Add(item);
            }
            foreach (var mcList in mcd.Values)
            {
                foreach (var item in mcList)
                {

                    if (item.inheritsFrom == null)
                        result.Add(item);
                    else
                    {
                        if(mcd.ContainsKey(item.inheritsFrom))
                        {
                            Minecraft from = mcd[item.inheritsFrom][0];
                            item.libraries.AddRange(from.libraries);
                        }else if(SettingProxy.Minecrafts.Where(m=> m.id== item.inheritsFrom).Count()>0)
                        {
                            Minecraft from = SettingProxy.Minecrafts.Where(m => m.id == item.inheritsFrom).First();
                            item.libraries.AddRange(from.libraries);
                        }
                        result.Add(item);

                    }
                }
            }
            return result;
        }
    }
}
