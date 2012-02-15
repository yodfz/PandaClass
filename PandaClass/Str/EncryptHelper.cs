using System;
using System.Data;
using System.Configuration;
using System.Security.Cryptography;
using System.Text;
using System.Collections;
using System.Text.RegularExpressions;

namespace PandaClass
{
    /// <summary>
    /// 加密操作类
    /// </summary>
    public class EncryptHelper
    {
        #region MD5加密

        #region MD5算法加密字符串( 16位 )
        /// <summary>
        /// MD5算法加密字符串( 16位 )
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        public static string MD5By16(string text)
        {
            //如果字符串为空，则返回
            if (ValidationHelper.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            try
            {
                //创建MD5密码服务提供程序
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                //获取加密字符串
                string result = BitConverter.ToString(md5.ComputeHash(UTF8Encoding.Default.GetBytes(text)), 4, 8);

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                return result.Replace("-", "");
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region MD5算法加密字符串( 32位 )

        #region 重载1
        /// <summary>
        /// MD5算法加密字符串( 32位 )
        /// </summary>
        /// <param name="text">要加密的字符串</param>    
        /// <param name="encoding">字符编码</param>    
        public static string MD5By32(string text, Encoding encoding)
        {
            //如果字符串为空，则返回
            if (ValidationHelper.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            try
            {
                //创建MD5密码服务提供程序
                MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();

                //计算传入的字节数组的哈希值
                byte[] hashCode = md5.ComputeHash(encoding.GetBytes(text));

                //释放资源
                md5.Clear();

                //返回MD5值的字符串表示
                string temp = "";
                for (int i = 0, len = hashCode.Length; i < len; i++)
                {
                    temp += hashCode[i].ToString("x").PadLeft(2, '0');
                }
                return temp;
            }
            catch
            {
                return string.Empty;
            }
        }
        #endregion

        #region 重载2
        /// <summary>
        /// MD5算法加密字符串( 32位 )
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        public static string MD5By32(string text)
        {
            return MD5By32(text, Encoding.UTF8);
        }
        #endregion

        #region 重载3
        /// <summary>
        /// MD5算法加密字符串( 支付宝专用 )
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        public static string MD5ByAlipay(string text)
        {
            return MD5By32(text, Encoding.GetEncoding("gb2312"));
        }
        #endregion

        #endregion

        #endregion

        #region Base64加密
        /// <summary>
        /// Base64加密
        /// </summary>
        /// <param name="text">要加密的字符串</param>
        /// 
        /// <returns></returns>
        public static string EncodeBase64(string text,int code_type)
        {
            //如果字符串为空，则返回
            if (ValidationHelper.IsNullOrEmpty<string>(text))
            {
                return "";
            }

            try
            {
                char[] Base64Code = new char[]{'A','B','C','D','E','F','G','H','I','J','K','L','M','N','O','P','Q','R','S','T',
											'U','V','W','X','Y','Z','a','b','c','d','e','f','g','h','i','j','k','l','m','n',
											'o','p','q','r','s','t','u','v','w','x','y','z','0','1','2','3','4','5','6','7',
											'8','9','+','/','='};
                byte empty = (byte)0;
                ArrayList byteMessage = new ArrayList(Encoding.GetEncoding(code_type).GetBytes(text));
                StringBuilder outmessage;
                int messageLen = byteMessage.Count;
                int page = messageLen / 3;
                int use = 0;
                if ((use = messageLen % 3) > 0)
                {
                    for (int i = 0; i < 3 - use; i++)
                        byteMessage.Add(empty);
                    page++;
                }
                outmessage = new System.Text.StringBuilder(page * 4);
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[3];
                    instr[0] = (byte)byteMessage[i * 3];
                    instr[1] = (byte)byteMessage[i * 3 + 1];
                    instr[2] = (byte)byteMessage[i * 3 + 2];
                    int[] outstr = new int[4];
                    outstr[0] = instr[0] >> 2;
                    outstr[1] = ((instr[0] & 0x03) << 4) ^ (instr[1] >> 4);
                    if (!instr[1].Equals(empty))
                        outstr[2] = ((instr[1] & 0x0f) << 2) ^ (instr[2] >> 6);
                    else
                        outstr[2] = 64;
                    if (!instr[2].Equals(empty))
                        outstr[3] = (instr[2] & 0x3f);
                    else
                        outstr[3] = 64;
                    outmessage.Append(Base64Code[outstr[0]]);
                    outmessage.Append(Base64Code[outstr[1]]);
                    outmessage.Append(Base64Code[outstr[2]]);
                    outmessage.Append(Base64Code[outstr[3]]);
                }
                return outmessage.ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Base64解密
        /// <summary>
        /// Base64解密
        /// </summary>
        /// <param name="text">要解密的字符串</param>
        public static string DecodeBase64(string text,int code_type)
        {
            //如果字符串为空，则返回
            if (ValidationHelper.IsNullOrEmpty<string>(text))
            {
                return "";
            }

            //将空格替换为加号
            text = text.Replace(" ", "+");

            try
            {
                if ((text.Length % 4) != 0)
                {
                    return "包含不正确的BASE64编码";
                }
                if (!Regex.IsMatch(text, "^[A-Z0-9/+=]*$", RegexOptions.IgnoreCase))
                {
                    return "包含不正确的BASE64编码";
                }
                string Base64Code = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";
                int page = text.Length / 4;
                ArrayList outMessage = new ArrayList(page * 3);
                char[] message = text.ToCharArray();
                for (int i = 0; i < page; i++)
                {
                    byte[] instr = new byte[4];
                    instr[0] = (byte)Base64Code.IndexOf(message[i * 4]);
                    instr[1] = (byte)Base64Code.IndexOf(message[i * 4 + 1]);
                    instr[2] = (byte)Base64Code.IndexOf(message[i * 4 + 2]);
                    instr[3] = (byte)Base64Code.IndexOf(message[i * 4 + 3]);
                    byte[] outstr = new byte[3];
                    outstr[0] = (byte)((instr[0] << 2) ^ ((instr[1] & 0x30) >> 4));
                    if (instr[2] != 64)
                    {
                        outstr[1] = (byte)((instr[1] << 4) ^ ((instr[2] & 0x3c) >> 2));
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    if (instr[3] != 64)
                    {
                        outstr[2] = (byte)((instr[2] << 6) ^ instr[3]);
                    }
                    else
                    {
                        outstr[2] = 0;
                    }
                    outMessage.Add(outstr[0]);
                    if (outstr[1] != 0)
                        outMessage.Add(outstr[1]);
                    if (outstr[2] != 0)
                        outMessage.Add(outstr[2]);
                }
                byte[] outbyte = (byte[])outMessage.ToArray(Type.GetType("System.Byte"));
                return Encoding.GetEncoding(code_type).GetString(outbyte);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region HMAC加密
        /// <summary>
        /// HMAC加密
        /// </summary>
        /// <param name="text">要加密的文本</param>
        /// <param name="key">键</param>
        public static String HMAC32(string text, string key)
        {
            //如果字符串为空，则返回
            if (ValidationHelper.IsNullOrEmpty(text))
            {
                return string.Empty;
            }

            string k_ipad, k_opad, temp;
            string ipad = "6666666666666666666666666666666666666666666666666666666666666666";
            string opad = @"\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\\";
            k_ipad = fun_MD5(strXor(key, ipad) + text);

            k_opad = strXor(key, opad);

            byte[] Test = hexstr2array(k_ipad);
            temp = "";

            char[] b = Encoding.GetEncoding(1252).GetChars(Test);
            for (int i = 0; i < b.Length; i++)
            {
                temp += b[i];
            }
            temp = k_opad + temp;
            return fun_MD5(temp).ToLower();
        }
        private static String fun_MD5(string str)
        {
            byte[] b = System.Text.Encoding.GetEncoding(1252).GetBytes(str);
            b = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(b);
            string ret = "";
            for (int i = 0; i < b.Length; i++)
                ret += b[i].ToString("x").PadLeft(2, '0');
            return ret;
        }
        private static Byte[] hexstr2array(string HexStr)
        {
            string HEX = "0123456789ABCDEF";
            string str = HexStr.ToUpper();
            int len = str.Length;
            byte[] RetByte = new byte[len / 2];
            for (int i = 0; i < len / 2; i++)
            {
                int NumHigh = HEX.IndexOf(str[i * 2]);
                int NumLow = HEX.IndexOf(str[i * 2 + 1]);
                RetByte[i] = Convert.ToByte(NumHigh * 16 + NumLow);
            }
            return RetByte;
        }
        private static string strXor(String password, String pad)
        {
            String iResult = "";
            int KLen = password.Length;

            for (int i = 0; i < 64; i++)
            {
                if (i < KLen)
                    iResult += Convert.ToChar(pad[i] ^ password[i]);
                else
                    iResult += Convert.ToChar(pad[i]);
            }
            return iResult;
        }
        #endregion
    }
}
