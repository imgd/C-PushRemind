using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Web;

namespace Sucool.PushRemind
{
    //---------------------------------------------------------------
    //   缓存 扩展方法类                                                   
    // —————————————————————————————————————————————————             
    // | varsion 1.0                                   |             
    // | creat by gd 2014.7.31                         |             
    // | 联系我:@大白2013 http://weibo.com/u/2239977692 |            
    // —————————————————————————————————————————————————             
    //                                                               
    // *使用说明：                                                    
    //    使用当前扩展类添加引用: using Extensions.CacheExtension;                      
    //    使用所有扩展类添加引用: using Extensions;                         
    // -------------------------------------------------------------- 
    public static class FileExtension
    {
        /// <summary>
        /// 创建文件夹
        /// </summary>
        /// <param name="path">必须是物理路径</param>
        public static void CreateFile(this string path)
        {
            if (!Directory.Exists(path))//如果不存在文件夹，则创建
            {
                try
                {
                    Directory.CreateDirectory(path);
                    DirectorySecurity security = new DirectorySecurity(path, AccessControlSections.Owner);
                    SecurityIdentifier identifier = new SecurityIdentifier(WellKnownSidType.CreatorOwnerSid, null);
                    security.SetAccessRule(new FileSystemAccessRule(identifier, FileSystemRights.FullControl, AccessControlType.Allow));
                }
                catch (FileNotFoundException e) { throw e; }
            }
        }

        /// <summary>
        /// 写入信息到记事本
        /// </summary>
        /// <param name="path">写入信息的屋里路径</param>
        /// <param name="msg">写入信息</param>
        public static void WriteStream(this string msg, string path)
        {
            //GC.Collect(); 强制垃圾回收
            try
            {
                using (StreamWriter sw = File.AppendText(path))
                {
                    sw.WriteLine(msg);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 判断文件是否存在
        /// </summary>
        /// <param name="path">文件的绝对路径</param>
        /// <returns></returns>
        public static bool IsFileExists(this string path)
        {
            return path.IsNullOrEnpty() ?
                false : File.Exists(path);
        }
    }
}
