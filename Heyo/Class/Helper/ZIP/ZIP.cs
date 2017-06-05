using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;
using System.Diagnostics;
using ICSharpCode.SharpZipLib.Core;
using System.Text.RegularExpressions;

namespace Heyo.Class.Helper.ZIP
{
    public class ZIP
    {
        /// <summary>
        /// 将文件从Zip中提取到内存流中
        /// </summary>
        /// <param name="zipFile">要提取的ZIP路径</param>
        /// <param name="extractFileName">ZIP内文件名</param>
        /// <param name="extractFileDirectory">指定ZIP内路径</param>
        /// <returns>内存流</returns>
        public static MemoryStream ExtractFile(string zipFile,Regex extractFileName, string extractFileDirectory = null)
        {
            MemoryStream ms = new MemoryStream();

            if (!File.Exists(zipFile))
                return ms;
            ZipInputStream s = new ZipInputStream(File.OpenRead(zipFile));
            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {

                if(((Path.GetDirectoryName(theEntry.Name) == extractFileDirectory)|| extractFileDirectory==null)&& extractFileName.IsMatch(Path.GetFileName(theEntry.Name)))
                {
                    int size = 2048;
                    byte[] data = new byte[2048];
                    while (true)
                    {
                        size = s.Read(data, 0, data.Length);
                        if (size > 0)
                        {
                            ms.Write(data, 0, size);
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            return ms;
        }
        /// <summary>
        /// 解压ZIP
        /// </summary>
        /// <param name="zipFile">需解压的ZIP文件</param>
        /// <param name="targetDir">解压到</param>
        public static void UnZip(string zipFile, string targetDir)
        {
            if (!File.Exists(zipFile))
                return;

            ZipInputStream s = new ZipInputStream(File.OpenRead(zipFile));

            ZipEntry theEntry;
            while ((theEntry = s.GetNextEntry()) != null)
            {

                string relaDir = Path.GetDirectoryName(theEntry.Name);
                string fileName = Path.GetFileName(theEntry.Name);
                string absoDir = Path.Combine(targetDir,relaDir);
                string absoFile = Path.Combine(absoDir, fileName);
                if (File.Exists(absoFile))
                    return;
                // create directory
                if (!Directory.Exists(absoDir))
                {
                    Directory.CreateDirectory(absoDir);
                }

                if (fileName != String.Empty)
                {
                    using (FileStream streamWriter = File.Create(absoFile))
                    {

                        int size = 2048;
                        byte[] data = new byte[2048];
                        while (true)
                        {
                            size = s.Read(data, 0, data.Length);
                            if (size > 0)
                            {
                                streamWriter.Write(data, 0, size);
                            }
                            else {
                                break;
                            }
                        }
                    }
                }
            }
        }
    }
}
