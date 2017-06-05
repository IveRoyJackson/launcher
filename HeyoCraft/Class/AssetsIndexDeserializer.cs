using HeyoCraft.Class.Data;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace HeyoCraft.Class.Helper
{
    class AssetsIndexDeserializer
    {
        /// <summary>
        /// 反序列化assets\indexes目录下指示资源文件的json数据的内容
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns>带有哈希值、文件名称、文件大小的List类型</returns>
        public static List<Asset> Deserialize(string json)
        {
            List<Asset> result=new List<Asset>();
            Regex hashRegex=new Regex("(?<=\")[a-z0-9]{40}(?=\")");
            Regex nameRegex = new Regex(@"\w+\.\w+");
            Regex sizeRegex = new Regex(@"(?<=:\s?)\d+");

            MatchCollection hashMaches = hashRegex.Matches(json);
            MatchCollection nameMaches = nameRegex.Matches(json);
            MatchCollection sizeMaches = sizeRegex.Matches(json);
            if(hashMaches.Count==nameMaches.Count && nameMaches.Count == sizeMaches.Count)
            {
                for (int i = 0; i < hashMaches.Count; i++)
                {
                    result.Add(new Asset() { hash = hashMaches[i].Value, name = nameMaches[i].Value, size = int.Parse(sizeMaches[i].Value) });
                }
            }
            return result;
        }
    }
}
