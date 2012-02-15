using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Drawing;
using System.Web.SessionState;

namespace PandaClass
{
    #region 验证码2
    /// <summary>
    /// Session:CheckCode 
    /// </summary>
    public sealed class VerifyCodeMSG : IHttpHandler, IRequiresSessionState
    {
        public VerifyCodeMSG()
        {
        }
        public void ProcessRequest(HttpContext context)
        {
            if (context != null)
            {
                Res.VerifyCode.CreateCheckCodeImage(Res.VerifyCode.GenerateCheckCode(context), context);
            }
        }

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }

    }

    #endregion
    /// <summary>
    /// 各类资源
    /// </summary>
    public class Res
    {
        #region 验证码
        /// <summary>
        /// 验证码
        /// </summary>
        public partial class VerifyCode : System.Web.UI.Page
        {
            private int _length;
            private int _fontsize;
            private int _padding;
            private bool _chaos = true;
            public static string GenerateCheckCode(HttpContext context)
            {
                int number;
                char code;
                string checkCode = String.Empty;

                System.Random random = new Random();

                for (int i = 0; i < 5; i++)
                {
                    number = random.Next();

                    if (number % 2 == 0)
                        code = (char)('0' + (char)(number % 10));
                    else
                        code = (char)('A' + (char)(number % 26));

                    checkCode += code.ToString();
                }
                context.Session["CheckCode"] = checkCode;
                //context.Response.Cookies.Add(new HttpCookie("CheckCode", checkCode));
                return checkCode;
            }
            public static void CreateCheckCodeImage(string checkCode, HttpContext context)
            {
                if (checkCode == null || checkCode.Trim() == String.Empty)
                    return;

                System.Drawing.Bitmap image = new System.Drawing.Bitmap((int)Math.Ceiling((checkCode.Length * 13.5)), 24);
                Graphics g = Graphics.FromImage(image);

                try
                {
                    //生成随机生成器
                    Random random = new Random();

                    //清空图片背景色
                    g.Clear(Color.White);

                    //画图片的背景噪音线
                    for (int i = 0; i < 25; i++)
                    {
                        int x1 = random.Next(image.Width);
                        int x2 = random.Next(image.Width);
                        int y1 = random.Next(image.Height);
                        int y2 = random.Next(image.Height);

                        g.DrawLine(new Pen(Color.Pink), x1, y1, x2, y2);
                    }

                    Font font = new System.Drawing.Font("Verdana", 12, (System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic));
                    System.Drawing.Drawing2D.LinearGradientBrush brush = new System.Drawing.Drawing2D.LinearGradientBrush(new Rectangle(0, 0, image.Width, image.Height), Color.Blue, Color.OrangeRed, 1f, true);
                    g.DrawString(checkCode, font, brush, 2, 2);

                    //画图片的前景噪音点
                    for (int i = 0; i < 80; i++)
                    {
                        int x = random.Next(image.Width);
                        int y = random.Next(image.Height);

                        image.SetPixel(x, y, Color.FromArgb(random.Next()));
                    }

                    //画图片的边框线
                    g.DrawRectangle(new Pen(Color.White), 0, 0, image.Width - 1, image.Height - 1);

                    System.IO.MemoryStream ms = new System.IO.MemoryStream();
                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);

                    context.Response.ClearContent();
                    context.Response.ContentType = "image/Gif";
                    context.Response.BinaryWrite(ms.ToArray());
                }
                finally
                {
                    g.Dispose();
                    image.Dispose();
                }
            }
            #region 验证码长度(默认6个验证码的长度)
            /// <summary>
            /// 验证码长度(默认6个验证码的长度) 
            /// </summary>
            public int Length
            {
                get
                {
                    return _length;
                }
                set
                {
                    _length = value;
                }
            }
            #endregion

            #region 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
            /// <summary>
            /// 验证码字体大小(为了显示扭曲效果，默认40像素，可以自行修改)
            /// </summary>
            public int FontSize
            {
                get
                {
                    return _fontsize;
                }
                set
                {
                    _fontsize = value;
                }
            }
            #endregion

            #region 边框补(默认1像素)
            /// <summary>
            /// 边框补(默认1像素)
            /// </summary>
            public int Padding
            {
                get
                {
                    return _padding;
                }
                set
                {
                    _padding = value;
                }
            }
            #endregion

            #region 是否输出燥点(默认不输出)
            /// <summary>
            /// 是否输出燥点(默认不输出)
            /// </summary>
            public bool Chaos
            {
                get
                {
                    return _chaos;
                }
                set
                {
                    _chaos = value;
                }
            }
            #endregion

            #region 输出燥点的颜色(默认灰色)
            /// <summary>
            /// 输出燥点的颜色(默认灰色)
            /// </summary>
            Color _chaoscolor = Color.LightGray;
            public Color ChaosColor
            {
                get
                {
                    return _chaoscolor;
                }
                set
                {
                    _chaoscolor = value;
                }
            }
            #endregion

            #region 自定义背景色(默认白色)
            /// <summary>
            /// 自定义背景色(默认白色)
            /// </summary>
            Color _backgroundcolor = Color.White;
            public Color BackgroundColor
            {
                get
                {
                    return _backgroundcolor;
                }
                set
                {
                    _backgroundcolor = value;
                }
            }
            #endregion

            #region 自定义随机颜色数组
            /// <summary>
            /// 自定义随机颜色数组
            /// </summary>
            Color[] _colors = { Color.Black, Color.Red, Color.DarkBlue, Color.Green, Color.Orange, Color.Brown, Color.DarkCyan, Color.Purple };
            public Color[] Colors
            {
                get
                {
                    return _colors;
                }
                set
                {
                    _colors = value;
                }
            }
            #endregion

            #region 自定义字体数组
            /// <summary>
            /// 自定义字体数组
            /// </summary>
            string[] _fonts = { "Arial", "Georgia" };
            public string[] Fonts
            {
                get
                {
                    return _fonts;
                }
                set
                {
                    _fonts = value;
                }
            }
            #endregion

            #region 自定义随机码字符串序列(使用逗号分隔)
            /// <summary>
            /// 自定义随机码字符串序列(使用逗号分隔)
            /// </summary>
            string _codeserial = "0,1,2,3,4,5,6,7,8,9";//,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z,A,B,C,D,E,F,G,H,I,J,K,L,M,N,O,P,Q,R,S,T,U,V,W,X,Y,Z";
            public string CodeSerial
            {
                get
                {
                    return _codeserial;
                }
                set
                {
                    _codeserial = value;
                }
            }
            #endregion

            #region 产生波形滤镜效果
            private const double PI = 3.1415926535897932384626433832795;
            private const double PI2 = 6.283185307179586476925286766559;
            /// <summary>
            /// 正弦曲线Wave扭曲图片
            /// </summary>
            /// <param name="srcBmp">图片路径</param>
            /// <param name="bXDir">如果扭曲则选择为True</param>
            /// <param name="dMultValue">波形的幅度倍数，越大扭曲的程度越高，一般为3</param>
            /// <param name="dPhase">波形的起始相位，取值区间[0-2*PI)</param>
            /// <returns></returns>
            public System.Drawing.Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
            {
                System.Drawing.Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
                // 将位图背景填充为白色
                System.Drawing.Graphics graph = System.Drawing.Graphics.FromImage(destBmp);
                graph.FillRectangle(new SolidBrush(System.Drawing.Color.White), 0, 0, destBmp.Width, destBmp.Height);
                graph.Dispose();
                double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
                for (int i = 0; i < destBmp.Width; i++)
                {
                    for (int j = 0; j < destBmp.Height; j++)
                    {
                        double dx = 0;
                        dx = bXDir ? (PI2 * (double)j) / dBaseAxisLen : (PI2 * (double)i) / dBaseAxisLen;
                        dx += dPhase;
                        double dy = Math.Sin(dx);
                        // 取得当前点的颜色
                        int nOldX = 0, nOldY = 0;
                        nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                        nOldY = bXDir ? j : j + (int)(dy * dMultValue);
                        System.Drawing.Color color = srcBmp.GetPixel(i, j);
                        if (nOldX >= 0 && nOldX < destBmp.Width
                         && nOldY >= 0 && nOldY < destBmp.Height)
                        {
                            destBmp.SetPixel(nOldX, nOldY, color);
                        }
                    }
                }
                return destBmp;
            }
            #endregion

            #region 生成校验码图片
            /// <summary>
            /// 生成校验码图片
            /// </summary>
            /// <param name="code"></param>
            /// <returns></returns>
            public Bitmap CreateImageCode(string code)
            {
                int fSize = FontSize;
                int fWidth = fSize + Padding;
                int imageWidth = (int)(code.Length * fWidth) + 4 + Padding * 2;
                int imageHeight = fSize * 2 + Padding;
                System.Drawing.Bitmap image = new System.Drawing.Bitmap(imageWidth, imageHeight);
                Graphics g = Graphics.FromImage(image);
                g.Clear(BackgroundColor);
                Random rand = new Random();
                //给背景添加随机生成的燥点
                if (this.Chaos)
                {
                    Pen pen = new Pen(ChaosColor, 0);
                    int c = Length * 10;
                    for (int i = 0; i < c; i++)
                    {
                        int x = rand.Next(image.Width);
                        int y = rand.Next(image.Height);
                        g.DrawRectangle(pen, x, y, 1, 1);
                    }
                }
                int left = 0, top = 0, top1 = 1, top2 = 1;
                int n1 = (imageHeight - FontSize - Padding * 2);
                int n2 = n1 / 4;
                top1 = n2;
                top2 = n2 * 2;
                Font f;
                Brush b;
                int cindex, findex;
                //随机字体和颜色的验证码字符
                for (int i = 0; i < code.Length; i++)
                {
                    cindex = rand.Next(Colors.Length - 1);
                    findex = rand.Next(Fonts.Length - 1);
                    f = new System.Drawing.Font(Fonts[findex], fSize, System.Drawing.FontStyle.Bold);
                    b = new System.Drawing.SolidBrush(Colors[cindex]);
                    if (i % 2 == 1)
                    {
                        top = top2;
                    }
                    else
                    {
                        top = top1;
                    }
                    left = i * fWidth;
                    g.DrawString(code.Substring(i, 1), f, b, left, top);
                }
                //画一个边框 边框颜色为Color.Black
                // g.DrawRectangle(new Pen(Color.Black, 0), 0, 0, image.Width - 1, image.Height - 1);
                g.Dispose();
                //产生波形
                image = TwistImage(image, true, 3, 0);
                return image;
            }
            #endregion

            #region 将创建好的图片输出到页面
            /// <summary>
            /// 将创建好的图片输出到页面
            /// </summary>
            /// <param name="code"></param>
            /// <param name="context"></param>
            public void CreateImageOnPage(string code, HttpContext context)
            {
                System.IO.MemoryStream ms = new System.IO.MemoryStream();
                Bitmap image = this.CreateImageCode(code);
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                context.Response.ClearContent();
                context.Response.ContentType = "image/Jpeg";
                context.Response.BinaryWrite(ms.GetBuffer());
                ms.Close();
                ms = null;
                image.Dispose();
                image = null;
            }
            #endregion

            #region 生成随机字符码
            /// <summary>
            /// 生成随机字符码
            /// </summary>
            /// <param name="codeLen"></param>
            /// <returns></returns>
            public string CreateVerifyCode(int codeLen)
            {
                if (codeLen == 0)
                {
                    codeLen = Length;
                }

                string[] arr = CodeSerial.Split(',');

                string code = "";

                int randValue = -1;

                Random rand = new Random(unchecked((int)DateTime.Now.Ticks));

                for (int i = 0; i < codeLen; i++)
                {
                    randValue = rand.Next(0, arr.Length - 1);

                    code += arr[randValue];
                }

                return code;
            }
            public string CreateVerifyCode()
            {
                return CreateVerifyCode(0);
            }
            #endregion

            #region 生成汉字字符
            /// <summary>
            /// 生成汉字字符
            /// </summary>
            /// <returns></returns>
            public char CreateZhChar()
            {
                //若提供了汉字集，查询汉字集选取汉字
                //if (ChineseChars.Length > 0)
                //{
                //    return ChineseChars[rnd.Next(0, ChineseChars.Length)];
                //}
                //若没有提供汉字集，则根据《GB2312简体中文编码表》编码规则构造汉字
                //else
                //{
                Random rnd = new Random();
                byte[] bytes = new byte[2];

                //第一个字节值在0xb0, 0xf7之间
                bytes[0] = (byte)rnd.Next(0xb0, 0xf8);
                //第二个字节值在0xa1, 0xfe之间
                bytes[1] = (byte)rnd.Next(0xa1, 0xff);

                //根据汉字编码的字节数组解码出中文汉字
                string str1 = System.Text.Encoding.GetEncoding("gb2312").GetString(bytes);

                return str1[0];
                //}
            }
            #endregion
        }
        #endregion

        
        
    }
}
