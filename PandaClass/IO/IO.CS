using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Web;
using System.Web.Security;
using System.Text.RegularExpressions;

namespace PandaClass
{
    public class IO
    {
        /// <summary>
        /// 获取文件夹大小
        /// </summary>
        /// <param name="dirPath">路径</param>
        /// <returns></returns>
        public static long GetDirectoryLength(string dirPath)
        {
            if (!Directory.Exists(dirPath))
                return 0;
            long len = 0;
            DirectoryInfo di = new DirectoryInfo(dirPath);
            foreach (FileInfo fi in di.GetFiles())
            {
                len += fi.Length;
            }
            DirectoryInfo[] dis = di.GetDirectories();
            if (dis.Length > 0)
            {
                for (int i = 0; i < dis.Length; i++)
                {
                    len += GetDirectoryLength(dis[i].FullName);
                }
            }
            return len;
        } 
        /// <summary>
        /// 获取指定驱动器的空间总大小(单位为B)
        /// </summary>
        /// <param name="str_HardDiskName">只需输入代表驱动器的字母即可</param>
        /// <returns></returns>
        public static long GetHardDiskSpace(string str_HardDiskName)
        {
            long totalSize = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    totalSize = drive.TotalSize;
                }
            }
            return totalSize;
        }

        /// <summary>
        /// 获取指定驱动器的剩余空间总大小(单位为B)
        /// </summary>
        /// <param name="str_HardDiskName">只需输入代表驱动器的字母即可</param>
        /// <returns></returns>
        public static long GetHardDiskFreeSpace(string str_HardDiskName)
        {
            long freeSpace = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace;
                }
            }
            return freeSpace;
        }
        #region 序列化与反序列化
        /// <summary>
        /// 读取序列化至硬盘的文件
        /// </summary>
        /// <param name="FileName">文件绝对地址</param>
        /// <returns></returns>
        public static object BinaryRead (string FileName)
        {
            using (FileStream Fs = new FileStream(FileName, FileMode.Open))
            {
                BinaryFormatter Xs = new BinaryFormatter();
                object ReadObj = (object) Xs.Deserialize(Fs);
                Fs.Close();
                return ReadObj;
            }
        }
        /// <summary>
        /// 序列化数据至硬盘文件
        /// </summary>
        /// <param name="obj">被序列化对象</param>
        /// <param name="FileName">地址</param>
        public static void BinaryWrite (object obj, string FileName)
        {
            using (FileStream Fs = new FileStream(FileName, FileMode.Create))
            {
                BinaryFormatter Xs = new BinaryFormatter();
                Xs.Serialize(Fs, obj);
                Fs.Close();

            }
        }
        #endregion
        #region 生成文件夹
        /// <summary>
        /// 生成文件夹
        /// </summary>
        /// <param name="fname">所要生成文件所在地址</param>
        /// <returns></returns>
        public static string CreateDiretory (String DFname)
        {
            if (!Directory.Exists(DFname))
            {
                Directory.CreateDirectory(DFname);
            }
            return DFname;
        }
        #endregion
        #region 文件删除
        /// <summary>
        /// 文件删除
        /// </summary>
        /// <param name="FileName">文件所在位置</param>
        /// <returns></returns>
        public static Boolean FileDelete (String FileName)
        {
            File.Delete(FileName);
            if (File.Exists(FileName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }


    /// <summary>
    ///IO 的摘要说明
    /// </summary>
    public class IOController
    {


        /// <summary>
        /// 建立者:赵逸风
        /// 建立时间:2009-4-12
        /// 修改时间:2009-4-14
        /// 说明:
        /// IO文件操作集合
        /// </summary>
        public class File
        {
            #region 静态函数


            public static String UpLoadFile (HttpFileCollection Files, String Floder, String FileType, int FileSize)
            {
                String FileName = Files [0].FileName.ToString();
                int _FileSize = Files [0].ContentLength;
                FileName = FileName.Substring(FileName.LastIndexOf("."), FileName.Length - FileName.LastIndexOf("."));
                Random X = new Random();
                Floder = Floder + DateTime.Now.ToString("/yyyyMMdd/MMdd/");
                CreateDiretory(Floder);
                Floder += DateTime.Now.ToString("yyyyMMddhhmmss") + X.Next(0, 99999) + FileName;
                FileName = FileName.Replace(".", "");
                if (FileType.ToUpper().IndexOf(FileName.ToUpper()) > -1)
                {
                    if (_FileSize <= FileSize)
                    {
                        Files [0].SaveAs(HttpContext.Current.Server.MapPath(Floder));
                        return Floder;
                    }
                    else
                    {
                        return "文件超出大小";
                    }
                }
                else
                {
                    return "类型错误";
                }


            }
            /// <summary>
            /// 返回调用程序所在真实基目录
            /// </summary>
            /// <returns></returns>
            public static String GetBaseDirectory ()
            {

                return System.AppDomain.CurrentDomain.BaseDirectory.ToString();


            }
            /// <summary>
            /// 复制文件
            /// </summary>
            /// <param name="SourceFile">源文件</param>
            /// <param name="SaveFile">目标文件</param>
            public static void Copy (String SourceFile, String SaveFile)
            {

                System.IO.File.Copy(SourceFile, SaveFile, true);

            }
            /// <summary>
            /// 返回虚拟路径真实地址
            /// </summary>
            /// <param name="FileName"></param>
            /// <returns></returns>
            public static String Mappath (String FileName)
            {
                return System.Web.HttpContext.Current.Server.MapPath(FileName);
            }
            /// <summary>
            /// 检查文件夹是否存在
            /// </summary>
            /// <param name="DirectoryName">文件夹路径</param>
            /// <returns></returns>
            public static Boolean IsDirectoryExists (String DirectoryName)
            {
                return Directory.Exists(DirectoryName);
            }
            /// <summary>
            /// 判断文件是否存在
            /// </summary>
            /// <param name="FileProcedures">文件路径</param>
            /// <returns>是否存在</returns>
            public static Boolean IsExists (string FileProcedures)
            {
                if (System.IO.File.Exists(FileProcedures))
                {
                    return true;
                }
                return false;
            }

            /// <summary>
            /// 生成文件夹
            /// </summary>
            /// <param name="fname">所要生成文件所在地址</param>
            /// <returns></returns>
            public static string CreateDiretory (String DFname)
            {
                if (!IsDirectoryExists(System.Web.HttpContext.Current.Server.MapPath(DFname)))
                {
                    Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(DFname));
                }
                return DFname;
            }

            /// <summary>
            /// 生成文件夹
            /// </summary>
            /// <param name="fname">所要生成文件所在地址</param>
            /// <returns></returns>
            public static string CreateDiretory_WinForm (String DFname)
            {
                if (!IsDirectoryExists((DFname)))
                {
                    Directory.CreateDirectory((DFname));
                }
                return DFname;
            }
            /// <summary>
            /// 写文件
            /// </summary>
            /// <param name="Data">欲写入的数据字符串</param>
            /// <param name="FileProcedures">欲写入文件路径</param>
            /// <param name="Append">写入方式。如果文件存在，Append为true则追加至文件结尾，Append为false则重写文件。文件不存在则创建新文件</param>
            public static void Write (string Data, string FileProcedures, Boolean Append)
            {
                CreateDiretory_WinForm(FileProcedures.Substring(0, FileProcedures.LastIndexOf("\\")));
                StreamWriter TempLateWrite = new StreamWriter(FileProcedures, Append, System.Text.Encoding.UTF8);
                TempLateWrite.Write(Data);
                TempLateWrite.Close();
                TempLateWrite.Dispose();
            }
            /// <summary>
            /// 读取文件内容
            /// </summary>
            /// <param name="FileProcedures">欲读取文件的路径</param>
            /// <returns>以字符串形式返回文件内容</returns>
            public static String Read (string FileProcedures, System.Text.Encoding encoding)
            {
                String TempLateCode = "";
                StreamReader ReadTempLate = new StreamReader(FileProcedures, encoding);
                TempLateCode = ReadTempLate.ReadToEnd();
                ReadTempLate.Close();
                ReadTempLate.Dispose();
                return TempLateCode;
            }
            /// <summary>
            /// 以系统默认编码方式，读取文件内容
            /// </summary>
            /// <param name="FileProcedures">欲读取文件的路径</param>
            /// <returns>以字符串形式返回文件内容</returns>
            public static String Read (string FileProcedures)
            {
                return Read(FileProcedures, System.Text.Encoding.UTF8);
            }


            ///// <summary>
            ///// 上传文本框处理组件
            ///// </summary>
            ///// <param name="FilePath">服务存放地址</param>
            ///// <param name="Fileobj">控件</param>
            ///// <param name="FileSize">文件大小</param>
            ///// <returns></returns>
            //public static String UpFile(String FilePath , FileUpload Fileobj , int FileSize)
            //{
            //    if (Fileobj.HasFile)
            //    {
            //        //获取MIEM基本类型
            //        String FileType = Fileobj.PostedFile.ContentType;
            //        //判断类型
            //        if (FileType == "image/bmp" || FileType == "image/gif" || FileType == "image/pjpeg" || FileType == "image/jpeg")
            //        {
            //            //判断大小
            //            if (( Fileobj.PostedFile.ContentLength / 1024 ) <= FileSize)
            //            {
            //                String TypeFile = Fileobj.FileName;
            //                TypeFile = TypeFile.Substring (TypeFile.LastIndexOf ('.') , TypeFile.Length - TypeFile.LastIndexOf ('.'));
            //                //保存文件
            //                Random RD = new Random ();
            //                String FileName = FilePath + DateTime.Now.ToString ("yyyyMMddhhmmss") + RD.Next (0 , 999) + TypeFile;
            //                Fileobj.SaveAs (HttpContext.Current.Server.MapPath (FileName));
            //                return FileName;
            //            }
            //            else
            //            {
            //                return "";
            //            }
            //        }
            //        else
            //        {
            //            return "";
            //        }
            //        //    //检查文件扩展名
            //        //    string[] _fx = FileEx.Split('|');
            //        //    string fex = Fileobj.FileName.Substring(Fileobj.FileName.LastIndexOf(".") + 1).ToLower();
            //        //    bool isex = false;
            //        //    for (int i = 0; i < _fx.Length; i++)
            //        //    {
            //        //        if (_fx[i].ToString().ToLower() == fex)
            //        //        {
            //        //            isex = true;
            //        //            break;
            //        //        }
            //        //    }
            //        //    if (!isex)
            //        //    {
            //        //        //文件类型不正确
            //        //        //HttpContext.Current.Response.Write("<font color=red>文件类型不正确</font>");
            //        //        //HttpContext.Current.Response.End();
            //        //        return false;
            //        //    }
            //        //    //检查上传目录
            //        //    if (!MyFile.DirectoryExists(FilePath))
            //        //    {
            //        //        //上传目录不存在
            //        //        return false;
            //        //        //HttpContext.Current.Response.Write("<font color=red>上传目录不存在</font>");
            //        //        //HttpContext.Current.Response.End();
            //        //    }
            //        //    //检查文件大小
            //        //    if (Fileobj.PostedFile.ContentLength / 1024 > FileSize)
            //        //    {
            //        //        //上传文件过大
            //        //        return false;
            //        //        //HttpContext.Current.Response.Write("<font color=red>上传文件过大</font>");
            //        //        //HttpContext.Current.Response.End();
            //        //    }
            //        //    string Fname = MyFile.CFileName(Fileobj.FileName);
            //        //    Fileobj.SaveAs(HttpContext.Current.Server.MapPath(FilePath + "/" + Fname));
            //        //    //return FilePath + "/" + Fname;
            //        //    return true;
            //        //}
            //        //return false;
            //    }
            //    else
            //    {
            //        return "";
            //    }
            #endregion
            //}
            /// <summary>
            /// 用正则来取文章中的图片
            /// </summary>
            /// <param name="Str">数据</param>
            /// <param name="Exp">图片类型</param>
            /// <returns></returns>
            public static string LoadPic (string Str, string Exp)
            {
                string Result = "";
                System.Text.RegularExpressions.Regex Re = new System.Text.RegularExpressions.Regex(@Exp);
                System.Text.RegularExpressions.MatchCollection m = Re.Matches(Str);
                foreach (Match k in m)
                {
                    Result += k.Groups [0].Value + "|";
                }
                return Result;
            }
            /// <summary>
            /// 删除文件
            /// </summary>
            /// <param name="FilePath">文件所在的相对位置</param>
            public static void DelFile (string FilePath)
            {
                try
                {
                    System.IO.File.Delete((FilePath));
                }
                catch
                {
                    //Cstr.JsMsg("一个或多个文件未被删除！");
                }
            }
            /// <summary>
            /// 删除FCK里的文件，必须是上传到UpFile目录里的
            /// </summary>
            /// <param name="FckStr">FCK的字符串</param>
            public static void DelFckFile (string FckStr)
            {
                string _str = FckStr.Replace("|", "｜");
                string _t = LoadPic(_str, "/UpFile/[^>]*.(?:jpg|bmp|gif|png|swf|rar|doc|jpeg|swf|rar|doc|zip|txt)");
                string [] y = _t.Split('|');
                for (int i = 0; i < y.Length - 1; i++)
                {
                    try
                    {
                        System.IO.File.Delete(HttpContext.Current.Server.MapPath(y [i].ToString()));
                    }
                    catch
                    {
                    }
                }
            }



        }
    }


}
