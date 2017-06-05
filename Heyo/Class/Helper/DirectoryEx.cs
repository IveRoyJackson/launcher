using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Heyo.Class.Helper
{
    public static class DirectoryEx
    {
        /// <summary>
        /// 拷贝文件夹
        /// </summary>
        /// <param name="srcdir"></param>
        /// <param name="desdir"></param>
        public static void CopyDirectory(string srcdir, string desdir)
        {
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);

            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }
                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfileName = desfolderdir + "\\" + srcfileName;

                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
                    if(!File.Exists(srcfileName))
                    {
                        File.Copy(file, srcfileName, true);
                    }
                }
            }
        }
    }
}
