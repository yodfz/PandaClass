using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Net;
using System.IO;
using System.Net.Security;
using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;

namespace PandaClass
{
    public class Web
    {
        #region 文件上传
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="Files">上传对象</param>
        /// <param name="Floder">保存文件夹</param>
        /// <param name="FileType">文件类型</param>
        /// <param name="FileSize">文件大小:字节</param>
        /// <returns></returns>
        public static Boolean UpLoadFile(HttpFileCollection Files, String Floder, String FileType, int FileSize, out String WebFileName)
        {
            if (Files.Count >= 1)
            {
                String FileName = Files[0].FileName.ToString();
                int _FileSize = Files[0].ContentLength;
                FileName = FileName.Substring(FileName.LastIndexOf("."), FileName.Length - FileName.LastIndexOf("."));
                FileName = FileName.Replace(".", "");
                Random X = new Random();
                IO.CreateDiretory(Floder);
                String TmpFileName = DateTime.Now.ToString("yyyyMMdd") + Guid.NewGuid().ToString() + "." + FileName;
                if (FileType.ToUpper().IndexOf(FileName.ToUpper()) > -1)
                {
                    if (_FileSize <= FileSize)
                    {
                        Files[0].SaveAs(Floder + TmpFileName);
                        WebFileName = TmpFileName;
                        return true;
                    }
                    else
                    {
                        WebFileName = "文件超出大小";
                        return false;
                    }
                }
                else
                {
                    WebFileName = "类型错误";
                    return false;
                }
            }
            else
            {
                WebFileName = "上传文件参数错误!";
                return false;
            }


        }

        #endregion
        #region 提交数据
        /// <summary>
        /// 提交数据
        /// </summary>
        /// <param name="Url">提交的地址</param>
        /// <param name="PostKey">提交的数据(&a=值&b=值)</param>
        /// <param name="Html">返回的页面代码</param>
        /// <param name="PostCookie">Cookie对象(小甜心)</param>
        /// <param name="PostEncoding">编码格式</param>
        /// <param name="RefferUrl">来源地址</param>
        /// <param name="host">主机名称</param>
        /// <param name="proxy">代理IP</param>
        /// <param name="proxyport">代理端口</param>
        /// <param name="TimeOut">超时时间</param>
        /// <param name="ConnectionLimit">最大线程</param>
        /// <returns></returns>
        public static CookieContainer Post(string Url, string PostKey, out String Html, CookieContainer PostCookie = null, string PostEncoding = "gb2312", string RefferUrl = "", string Host = "", string proxy = "", string proxyport = "", int TimeOut = 30, int ConnectionLimit = 255, string Accept = "")
        {
            //TODO:注释无法正常显示
            CookieContainer TempCookie = new CookieContainer();
            if (PostCookie == null)
            {
                PostCookie = TempCookie;
            }
            String Address = Url;
            String PostData = PostKey;
            Byte[] PostBytes = Encoding.ASCII.GetBytes(PostData);
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(Address);
            req.Method = "POST";
            req.CookieContainer = PostCookie;
            req.Referer = RefferUrl;
            if (!Accept.IsNullOrEmpty())
            {
                req.Accept = Accept;
            }
            req.Timeout = TimeOut * 1000;
            req.ContentType = "application/x-www-form-urlencoded" + (string.IsNullOrWhiteSpace(PostEncoding) ? "" : ";charset=" + PostEncoding);

            if (!proxy.IsNullOrEmpty())
            {
                req.Proxy = new WebProxy(proxy, proxyport.toString(0));
            }

            req.ContentLength = PostBytes.Length;

            req.UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0)";

            if (!Host.IsNullOrEmpty())
            {
                req.Host = Host;
            }
            req.ServicePoint.ConnectionLimit = ConnectionLimit;
            req.ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation;
            //发送POST数据
            using (Stream reqStream = req.GetRequestStream())
            {
                reqStream.Write(PostBytes, 0, PostBytes.Length);
            }
            //接受数据回来
            using (HttpWebResponse wr = (HttpWebResponse)req.GetResponse())
            {

                Stream R = wr.GetResponseStream();
                Encoding Enc;
                //转换失败 直接使用默认编码
                if (string.IsNullOrEmpty(PostEncoding))
                {
                    Enc = Encoding.Default;
                }
                else
                {
                    try
                    {
                        Enc = Encoding.GetEncoding(PostEncoding);
                    }
                    catch
                    {
                        Enc = Encoding.Default;
                    }
                }
                StreamReader SR = new StreamReader(R, Enc);
                String _Html = SR.ReadToEnd();
                TempCookie.Add(wr.Cookies);
                Html = _Html;
                R.Dispose();
                SR.Dispose();
                req.Abort();
            }
            return TempCookie;
        }

        #endregion
        #region 获取数据
        /// <summary>
        /// 抓取页面
        /// </summary>
        /// <param name="Url">被抓取的网页地址</param>
        /// <param name="PageCookie">当前地址的COOKIE</param>
        /// <param name="Enc">地址编码格式</param>
        /// <returns></returns>
        // public static CookieContainer Post(string Url,string PostKey,out String Html,CookieContainer PostCookie = null,string PostEncoding = "gb2312",string RefferUrl = "",string host = "",string proxy = "",string proxyport = "",int TimeOut = 30,int ConnectionLimit=255)
        public static CookieContainer Get(
            string Url,
            out string Html,
            CookieContainer GetCookie = null,
            string cookiestr = "",
            string Accept = "text/html, application/xhtml+xml, */*",
            string PostEncoding = "gb2312",
            string RefferUrl = "",
            string Host = "",
            string proxy = "",
            string proxyport = "",
            int TimeOut = 30,
            int ConnectionLimit = 255,
            string ContentType = "application/x-www-form-urlencoded;charset=",
            string UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0)",
            bool KeepAlive = true,
            bool PreAuthenticate = true,
            System.Security.Principal.TokenImpersonationLevel ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation,
            System.Net.Security.AuthenticationLevel AuthenticationLevel = System.Net.Security.AuthenticationLevel.None



            )
        {
            String _Html = "";
            GetCookie = GetCookie == null ? new CookieContainer() : GetCookie;
            if (Url.Length < 7)
            {
                Html = "";
                return GetCookie;
            }
            String address = Url;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(address);
            req.Method = "GET";
            req.Timeout = TimeOut * 1000;
            if (!proxy.IsNullOrEmpty())
            {
                req.Proxy = new WebProxy(proxy, proxyport.toString(80));
            }
            req.ContentType = ContentType + PostEncoding;
            req.Accept = Accept;

            if (!RefferUrl.IsNullOrEmpty())
            {
                req.Referer = RefferUrl;
            }
            req.UserAgent = UserAgent;
            req.KeepAlive = true;
            req.ServicePoint.ConnectionLimit = 255;
            req.AllowAutoRedirect = true;
            req.ImpersonationLevel = ImpersonationLevel;
            req.KeepAlive = KeepAlive;
            req.PreAuthenticate = PreAuthenticate;
            req.AuthenticationLevel = AuthenticationLevel;
            if (!cookiestr.IsNullOrEmpty())
            {
                req.Headers.Add(HttpRequestHeader.Cookie, cookiestr);
            }
            if (!Host.IsNullOrEmpty())
            {
                req.Host = Host;
            }
            req.CookieContainer = GetCookie;

            //接受数据回来

            using (HttpWebResponse wr = (HttpWebResponse)req.GetResponse())
            {

                //string[] cookie = wr.Headers.GetValues("Set-Cookie");
                //for (int i = 0; i < cookie.Length-1; i++)
                //{
                //    GetCookie.SetCookies(new Uri(address), cookie[i].Replace(";", ","));
                //}
                //foreach (var item in wr.Headers.GetValues("Set-Cookie"))
                //{
                //    GetCookie.SetCookies(new Uri("http://www.baidu.com"), item.Replace(";", ","));
                //}
                Stream R = wr.GetResponseStream();
                StreamReader SR;
                SR = new StreamReader(R, Encoding.GetEncoding(PostEncoding));
                _Html = SR.ReadToEnd();
                GetCookie.Add(wr.Cookies);
                R.Dispose();
                SR.Dispose();
                req.Abort();
                Html = _Html;
            }
            return GetCookie;
        }
        #endregion
        #region 获取数据
        /// <summary>
        /// 抓取页面
        /// </summary>
        /// <param name="Url">被抓取的网页地址</param>
        /// <param name="PageCookie">当前地址的COOKIE</param>
        /// <param name="Enc">地址编码格式</param>
        /// <returns></returns>
        // public static CookieContainer Post(string Url,string PostKey,out String Html,CookieContainer PostCookie = null,string PostEncoding = "gb2312",string RefferUrl = "",string host = "",string proxy = "",string proxyport = "",int TimeOut = 30,int ConnectionLimit=255)
        public static CookieContainer GetSSL(
            string Url,
            out string Html,
            CookieContainer GetCookie = null,
            string cookiestr = "",
            string Accept = "text/html, application/xhtml+xml, */*",
            string PostEncoding = "gb2312",
            string RefferUrl = "",
            string Host = "",
            string proxy = "",
            string proxyport = "",
            int TimeOut = 30,
            int ConnectionLimit = 255,
            string ContentType = "application/x-www-form-urlencoded;charset=",
            string UserAgent = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Win64; x64; Trident/5.0)",
            bool KeepAlive = true,
            bool PreAuthenticate = true,
            System.Security.Principal.TokenImpersonationLevel ImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Delegation,
            System.Net.Security.AuthenticationLevel AuthenticationLevel = System.Net.Security.AuthenticationLevel.None



            )
        {
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);

            String _Html = "";
            GetCookie = GetCookie == null ? new CookieContainer() : GetCookie;
            if (Url.Length < 7)
            {
                Html = "";
                return GetCookie;
            }
            String address = Url;
            HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(address);
            req.Method = "GET";
            req.Timeout = TimeOut * 1000;
            if (!proxy.IsNullOrEmpty())
            {
                req.Proxy = new WebProxy(proxy, proxyport.toString(80));
            }
            req.ContentType = ContentType + PostEncoding;
            req.Accept = Accept;

            if (!RefferUrl.IsNullOrEmpty())
            {
                req.Referer = RefferUrl;
            }
            req.UserAgent = UserAgent;
            req.KeepAlive = true;
            req.ServicePoint.ConnectionLimit = 255;
            req.AllowAutoRedirect = true;
            req.ImpersonationLevel = ImpersonationLevel;
            req.KeepAlive = KeepAlive;
            req.PreAuthenticate = PreAuthenticate;
            req.AuthenticationLevel = AuthenticationLevel;

            if (!cookiestr.IsNullOrEmpty())
            {
                req.Headers.Add(HttpRequestHeader.Cookie, cookiestr);
            }
            if (!Host.IsNullOrEmpty())
            {
                req.Host = Host;
            }
            req.CookieContainer = GetCookie;

            //接受数据回来

            using (HttpWebResponse wr = (HttpWebResponse)req.GetResponse())
            {

                //string[] cookie = wr.Headers.GetValues("Set-Cookie");
                //for (int i = 0; i < cookie.Length-1; i++)
                //{
                //    GetCookie.SetCookies(new Uri(address), cookie[i].Replace(";", ","));
                //}
                //foreach (var item in wr.Headers.GetValues("Set-Cookie"))
                //{
                //    GetCookie.SetCookies(new Uri("http://www.baidu.com"), item.Replace(";", ","));
                //}
                Stream R = wr.GetResponseStream();
                StreamReader SR;
                SR = new StreamReader(R, Encoding.GetEncoding(PostEncoding));
                _Html = SR.ReadToEnd();
                GetCookie.Add(wr.Cookies);
                R.Dispose();
                SR.Dispose();
                req.Abort();
                Html = _Html;
            }
            return GetCookie;
        }
        #endregion
        #region 下载图片
        /// <summary>
        /// 下载图片到本地地址   image/png
        /// </summary>
        /// <param name="Url">被下载的图片地址</param>
        /// <param name="PageCookie">当前地址的COOKIE</param>
        /// <returns></returns>
        public static Boolean DownLoadImage(String Url, String FileName, String reffurl, CookieContainer PageCookie)
        {
            bool Value = false;
            WebResponse response = null;
            Stream stream = null;
            //try
            //{
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.ServicePoint.ConnectionLimit = 255;
                request.Referer = reffurl;
                request.CookieContainer = PageCookie;
                response = request.GetResponse();
                stream = response.GetResponseStream();
                if (!response.ContentType.ToLower().StartsWith("text/"))
                {
                    //Value = SaveBinaryFile(response , FileName);
                    byte[] buffer = new byte[1024];
                    try
                    {
                        if (File.Exists(FileName))
                        {
                            File.Delete(FileName);
                        }
                        else
                        {
                            Stream outStream = System.IO.File.Create(FileName);
                            Stream inStream = response.GetResponseStream();

                            int l;
                            do
                            {
                                l = inStream.Read(buffer, 0, buffer.Length);
                                if (l > 0)
                                {
                                    outStream.Write(buffer, 0, l);
                                }
                            }
                            while (l > 0);

                            outStream.Close();
                            inStream.Close();
                        }
                    }
                    catch(Exception err)
                    {
                        throw new Exception(err.ToString());
                        Value = false;
                    }
                    return Value;

                }
                else
                {
                    return false;
                }
            //}
            //catch (Exception err)
            //{
            //    throw new Exception(err.ToString());
            //}

        }
        /// <summary>
        /// 获取一个网络图像
        /// </summary>
        /// <param name="Url">网络图像界面</param>
        /// <param name="PageCookie">提交的COOKIE</param>
        /// <param name="ContentType">类型</param>
        /// <param name="Proxy">代理</param>
        /// <returns></returns>
        public static Image GetImage(String Url, CookieContainer PageCookie, string ContentType = "image/png", string Proxy = "", string ProxyPort = "")
        {

            WebResponse response = null;
            Stream stream = null;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.CookieContainer = PageCookie;
            request.ServicePoint.ConnectionLimit = 255;
            request.ContentType = ContentType;

            if (!Proxy.IsNullOrEmpty())
            {
                request.Proxy = new WebProxy(Proxy, ProxyPort.toString(0));
            }

            response = request.GetResponse();

            stream = response.GetResponseStream();
            Image img = Image.FromStream(stream);
            request.Abort();
            response.Close();
            return img;
        }

        /// <summary>
        /// 获取一个网络图像
        /// </summary>
        /// <param name="Url">网络图像界面</param>
        /// <param name="PageCookie">提交的COOKIE</param>
        /// <param name="ContentType">类型</param>
        /// <param name="Proxy">代理</param>
        /// <returns></returns>
        public static Image GetImageSSL(String Url, CookieContainer PageCookie, string ContentType = "image/png", string Proxy = "", string ProxyPort = "")
        {

            WebResponse response = null;
            Stream stream = null;
            ServicePointManager.ServerCertificateValidationCallback = new System.Net.Security.RemoteCertificateValidationCallback(CheckValidationResult);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.CookieContainer = PageCookie;
            request.ServicePoint.ConnectionLimit = 255;
            request.ContentType = ContentType;

            if (!Proxy.IsNullOrEmpty())
            {
                request.Proxy = new WebProxy(Proxy, ProxyPort.toString(0));
            }

            response = request.GetResponse();

            stream = response.GetResponseStream();
            Image img = Image.FromStream(stream);
            request.Abort();
            response.Close();
            return img;
        }
        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        #endregion
    }

    public class Net
    {
        public static string GetRealIP()
        {
            HttpRequest request = HttpContext.Current.Request;
            string userIP = request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            if (userIP == null || userIP == "")
            {
                //没有代理服务器,如果有代理服务器获取的是代理服务器的IP
                userIP = request.ServerVariables["REMOTE_ADDR"];
            }
            return userIP;

            //string ip;
            //try
            //{
            //    HttpRequest request = HttpContext.Current.Request;

            //    if (request.ServerVariables["HTTP_VIA"] != null)
            //    {
            //        ip = request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Split(',')[0].Trim();
            //    }
            //    else
            //    {
            //        ip = request.UserHostAddress;
            //    }
            //}
            //catch (Exception e)
            //{
            //    throw e;
            //}

            //return ip;
        }
        public static long IP2Long(string ip)
        {
            try
            {
                System.Net.IPAddress a = System.Net.IPAddress.Parse(ip);
                byte[] b = a.GetAddressBytes();
                int[] iparr = new int[4];
                b.CopyTo(iparr, 0);
                string result = "";
                foreach (int i in iparr)
                {
                    result += i.ToString();
                }
                return result.toString64(0);
            }
            catch
            {
                return 0;
            }
        }
    }

}
