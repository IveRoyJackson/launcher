using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Heyo.Class.Helper.Encryption
{
    public class MD5
    {
        /// <summary>
        /// 将字符串进行MD5加密，返回8位无符号数组
        /// </summary>
        /// <param name="str"></param>
        /// <returns>返回8位无符号数组</returns>
        public static byte[] EncryptToMD5(string str)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] str1 = System.Text.Encoding.UTF8.GetBytes(str);
            byte[] str2 = md5.ComputeHash(str1, 0, str1.Length);
            md5.Clear();
            (md5 as IDisposable).Dispose();
            return str2;
        }
        /// <summary>
        /// 将字符串进行MD5加密，返回字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns>返回字符串</returns>
        public static string EncryptToMD5string(string str)
        {
            byte[] bytHash = EncryptToMD5(str);
            string sTemp = "";
            for (int i = 0; i < bytHash.Length; i++)
            {
                sTemp += bytHash[i].ToString("X").PadLeft(2, '0');
            }
            return sTemp.ToLower();
        }
    }
}
