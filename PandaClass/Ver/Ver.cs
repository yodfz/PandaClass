using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace PandaClass
{
    /// <summary>
    /// 用于验证的公共类
    /// </summary>
    public class ValidationHelper
    {
        /*一些常用的正则表达式
         * 
         * 
         * 
            ^\d+$　　//匹配非负整数（正整数 + 0） 
            ^[0-9]*[1-9][0-9]*$　　//匹配正整数 
            ^((-\d+)|(0+))$　　//匹配非正整数（负整数 + 0） 
            ^-[0-9]*[1-9][0-9]*$　　//匹配负整数 
            ^-?\d+$　　　　//匹配整数 
            ^\d+(\.\d+)?$　　//匹配非负浮点数（正浮点数 + 0） 
            ^(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*))$　　//匹配正浮点数 
            ^((-\d+(\.\d+)?)|(0+(\.0+)?))$　　//匹配非正浮点数（负浮点数 + 0） 
            ^(-(([0-9]+\.[0-9]*[1-9][0-9]*)|([0-9]*[1-9][0-9]*\.[0-9]+)|([0-9]*[1-9][0-9]*)))$　　//匹配负浮点数 
            ^(-?\d+)(\.\d+)?$　　//匹配浮点数 
            ^[A-Za-z]+$　　//匹配由26个英文字母组成的字符串 
            ^[A-Z]+$　　//匹配由26个英文字母的大写组成的字符串 
            ^[a-z]+$　　//匹配由26个英文字母的小写组成的字符串 
            ^[A-Za-z0-9]+$　　//匹配由数字和26个英文字母组成的字符串 
            ^\w+$　　//匹配由数字、26个英文字母或者下划线组成的字符串 
            ^[\w-]+(\.[\w-]+)*@[\w-]+(\.[\w-]+)+$　　　　//匹配email地址 
            ^[a-zA-z]+://匹配(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$　　//匹配url 

            匹配中文字符的正则表达式： [\u4e00-\u9fa5] 
            匹配双字节字符(包括汉字在内)：[^\x00-\xff] 
            匹配空行的正则表达式：\n[\s| ]*\r 
            匹配HTML标记的正则表达式：/<(.*)>.*<\/>|<(.*) \/>/ 
            匹配首尾空格的正则表达式：(^\s*)|(\s*$) 
            匹配Email地址的正则表达式：\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)* 
            匹配网址URL的正则表达式：^[a-zA-z]+://(\w+(-\w+)*)(\.(\w+(-\w+)*))*(\?\S*)?$ 
            匹配帐号是否合法(字母开头，允许5-16字节，允许字母数字下划线)：^[a-zA-Z][a-zA-Z0-9_]{4,15}$ 
            匹配国内电话号码：(\d{3}-|\d{4}-)?(\d{8}|\d{7})? 
            匹配腾讯QQ号：^[1-9]*[1-9][0-9]*$ 
         * */
        private static Regex RegCHZN = new Regex("[\u4e00-\u9fa5]");

        #region 检测对象是否为空

        #region 重载1
        /// <summary>
        /// 检测对象是否为空，为空返回true
        /// </summary>
        /// <typeparam name="T">要验证的对象的类型</typeparam>
        /// <param name="data">要验证的对象</param>        
        public static bool IsNullOrEmpty<T>(T data)
        {
            //如果为null
            if (data == null)
            {
                return true;
            }

            //如果为""
            if (data.GetType() == typeof(String))
            {
                if (string.IsNullOrEmpty(data.ToString().Trim()))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

            //如果为DBNull
            if (data.GetType() == typeof(DBNull))
            {
                return true;
            }

            //不为空
            return false;
        }
        #endregion

        #region 重载2
        /// <summary>
        /// 检测对象是否为空，为空返回true
        /// </summary>
        /// <param name="data">要验证的对象</param>
        public static bool IsNullOrEmpty(object data)
        {
            return IsNullOrEmpty<object>(data);
        }
        #endregion

        #region 重载3
        /// <summary>
        /// 检测字符串是否为空，为空返回true
        /// </summary>
        /// <param name="text">要检测的字符串</param>
        public static bool IsNullOrEmpty(string text)
        {
            //检测是否为null
            if (text == null)
            {
                return true;
            }

            //检测字符串空值
            if (string.IsNullOrEmpty(text.ToString().Trim()))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #endregion

        #region 验证IP地址是否合法
        /// <summary>
        /// 验证IP地址是否合法
        /// </summary>
        /// <param name="ip">要验证的IP地址</param>        
        public static bool IsIP(string ip)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(ip))
            {
                return true;
            }

            //清除要验证字符串中的空格
            ip = ip.Trim();

            //模式字符串
            string pattern = @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$";

            //验证
            return IsMatch(ip, pattern);
        }
        #endregion

        #region  验证EMail是否合法
        /// <summary>
        /// 验证EMail是否合法
        /// </summary>
        /// <param name="email">要验证的Email</param>
        public static bool IsEmail(string email)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(email))
            {
                return true;
            }

            //清除要验证字符串中的空格
            email = email.Trim();

            //模式字符串
            string pattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";

            //验证
            return IsMatch(email, pattern);
        }
        #endregion

        #region 验证是否为整数
        /// <summary>
        /// 验证是否为整数
        /// </summary>
        /// <param name="number">要验证的整数</param>        
        public static bool IsInt(string number)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(number))
            {
                return true;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[1-9]+[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }
        #endregion

        #region 验证是否为数字
        /// <summary>
        /// 验证是否为数字
        /// </summary>
        /// <param name="number">要验证的数字</param>        
        public static bool IsNumber(string number)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(number))
            {
                return true;
            }

            //清除要验证字符串中的空格
            number = number.Trim();

            //模式字符串
            string pattern = @"^[1-9]+[0-9]*[.]?[0-9]*$";

            //验证
            return IsMatch(number, pattern);
        }
        #endregion

        #region 验证日期是否合法
        /// <summary>
        /// 验证日期是否合法,对不规则的作了简单处理
        /// </summary>
        /// <param name="date">日期</param>
        public static bool IsDate(ref string date)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(date))
            {
                return true;
            }

            //清除要验证字符串中的空格
            date = date.Trim();

            //替换\
            date = date.Replace(@"\", "-");
            //替换/
            date = date.Replace(@"/", "-");

            //如果查找到汉字"今",则认为是当前日期
            if (date.IndexOf("今") != -1)
            {
                date = DateTime.Now.ToString();
            }

            try
            {
                //用转换测试是否为规则的日期字符
                date = Convert.ToDateTime(date).ToString("d");
                return true;
            }
            catch
            {
                //如果日期字符串中存在非数字，则返回false
                if (!IsInt(date))
                {
                    return false;
                }

                #region 对纯数字进行解析
                //对8位纯数字进行解析
                if (date.Length == 8)
                {
                    //获取年月日
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);
                    string day = date.Substring(6, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12 || Convert.ToInt32(day) > 31)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month + "-" + day).ToString("d");
                    return true;
                }

                //对6位纯数字进行解析
                if (date.Length == 6)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 2);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }
                    if (Convert.ToInt32(month) > 12)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year + "-" + month).ToString("d");
                    return true;
                }

                //对5位纯数字进行解析
                if (date.Length == 5)
                {
                    //获取年月
                    string year = date.Substring(0, 4);
                    string month = date.Substring(4, 1);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = year + "-" + month;
                    return true;
                }

                //对4位纯数字进行解析
                if (date.Length == 4)
                {
                    //获取年
                    string year = date.Substring(0, 4);

                    //验证合法性
                    if (Convert.ToInt32(year) < 1900 || Convert.ToInt32(year) > 2100)
                    {
                        return false;
                    }

                    //拼接日期
                    date = Convert.ToDateTime(year).ToString("d");
                    return true;
                }
                #endregion

                return false;
            }
        }
        #endregion

        #region 验证身份证是否合法
        /// <summary>
        /// 验证身份证是否合法
        /// </summary>
        /// <param name="idCard">要验证的身份证</param>        
        public static bool IsIdCard(string idCard)
        {
            //如果为空，认为验证合格
            if (IsNullOrEmpty(idCard))
            {
                return true;
            }

            //清除要验证字符串中的空格
            idCard = idCard.Trim();

            //模式字符串
            StringBuilder pattern = new StringBuilder();
            pattern.Append(@"^(11|12|13|14|15|21|22|23|31|32|33|34|35|36|37|41|42|43|44|45|46|");
            pattern.Append(@"50|51|52|53|54|61|62|63|64|65|71|81|82|91)");
            pattern.Append(@"(\d{13}|\d{15}[\dx])$");

            //验证
            return IsMatch(idCard, pattern.ToString());
        }
        #endregion

        #region 检测客户的输入中是否有危险字符串
        /// <summary>
        /// 检测客户输入的字符串是否有效,并将原始字符串修改为有效字符串或空字符串。
        /// 当检测到客户的输入中有攻击性危险字符串,则返回false,有效返回true。
        /// </summary>
        /// <param name="input">要检测的字符串</param>
        public static bool IsValidInput(ref string input)
        {
            try
            {
                if (IsNullOrEmpty(input))
                {
                    //如果是空值,则跳出
                    return true;
                }
                else
                {
                    //替换单引号
                    input = input.Replace("'", "''").Trim();

                    //检测攻击性危险字符串
                    string testString = "and |or |exec |insert |select |delete |update |count |chr |mid |master |truncate |char |declare ";
                    string[] testArray = testString.Split('|');
                    foreach (string testStr in testArray)
                    {
                        if (input.ToLower().IndexOf(testStr) != -1)
                        {
                            //检测到攻击字符串,清空传入的值
                            input = string.Empty;
                            return false;
                        }
                    }

                    //未检测到攻击字符串
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        ///// <summary>
        ///// 检查是否含有非法字符
        ///// </summary>
        ///// <remarks>检查是否含有非法字符</remarks>
        ///// <param name="str">要检查的字符串</param>
        ///// <returns></returns>
        //public static bool IsValidInput(string str)
        //{
        //    bool result = false;
        //    if (string.IsNullOrEmpty(str))
        //        return result;
        //    string strBadChar, tempChar;
        //    string[] arrBadChar;
        //    strBadChar = "@@,+,',--,%,^,&,?,(,),<,>,[,],{,},/,\\,;,:,\",\"\"";
        //    arrBadChar = StringHelper.SplitString(strBadChar, ",");
        //    tempChar = str;
        //    for (int i = 0; i < arrBadChar.Length; i++)
        //    {
        //        if (tempChar.IndexOf(arrBadChar[i]) >= 0)
        //            result = true;
        //    }
        //    return result;
        //}
        #endregion

        #region 验证输入字符串是否与模式字符串匹配

        #region 重载1
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>        
        public static bool IsMatch(string input, string pattern)
        {
            return IsMatch(input, pattern, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 重载2
        /// <summary>
        /// 验证输入字符串是否与模式字符串匹配，匹配返回true
        /// </summary>
        /// <param name="input">输入的字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="options">筛选条件,比如是否忽略大小写</param>
        public static bool IsMatch(string input, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(input, pattern, options);
        }
        #endregion

        #endregion

        #region 获取匹配的值

        #region 重载1
        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="resultPattern">结果模式字符串,范例："$1"用来获取第一个( )内的值</param>
        /// <param name="options">筛选条件,比如是否忽略大小写</param>
        public static string GetMatchValue(string input, string pattern, string resultPattern, RegexOptions options)
        {
            //判断是否匹配
            if (Regex.IsMatch(input, pattern, options))
            {
                return Regex.Match(input, pattern, options).Result(resultPattern);
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #region 重载2
        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        /// <param name="resultPattern">结果模式字符串,范例："$1"用来获取第一个( )内的值</param>
        public static string GetMatchValue(string input, string pattern, string resultPattern)
        {
            return GetMatchValue(input, pattern, resultPattern, RegexOptions.IgnoreCase);
        }
        #endregion

        #region 重载3
        /// <summary>
        /// 获取匹配的值
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="pattern">模式字符串</param>
        public static string GetMatchValue(string input, string pattern)
        {
            if (Regex.IsMatch(input, pattern, RegexOptions.IgnoreCase))
            {
                return Regex.Match(input, pattern, RegexOptions.IgnoreCase).Value;
            }
            else
            {
                return string.Empty;
            }
        }
        #endregion

        #endregion

        #region 检测是否有中文字符

        /// <summary>
        /// 检测是否有中文字符
        /// </summary>
        /// <param name="inputData"></param>
        /// <returns></returns>
        public static bool IsHasCHZN(string inputData)
        {
            Match m = RegCHZN.Match(inputData);
            return m.Success;
        }

        #endregion

        #region 检测是否符合电话格式
        /// <summary>
        /// 检测是否符合电话格式
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsPhoneNumber(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, @"(^[0-9]{3,4}\-[0-9]{3,8}$)|(^[0-9]{3,8}$)|(^\([0-9]{3,4}\)[0-9]{3,8}$)");
        }
        #endregion

        #region 检测是否手机号码格式
        /// <summary>
        /// 检测是否手机号码格式
        /// </summary>
        /// <param name="phoneNumber"></param>
        /// <returns></returns>
        public static bool IsMobiletelePhone(string phoneNumber)
        {
            return Regex.IsMatch(phoneNumber, "^13[0-9]{1}[0-9]{8}$|^15[9]{1}[0-9]{8}$");
        }
        #endregion

        #region 检测是否符合url格式,前面必需含有http://
        /// <summary>
        /// 检测是否符合url格式,前面必需含有http://
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static bool IsURL(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return false;
            }
            return Regex.IsMatch(url, @"^http(s)?://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?$");
        }
        #endregion

        #region 检测是否符合时间格式
        /// <summary>
        /// 检测是否符合时间格式
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(string timeval)
        {
            return Regex.IsMatch(timeval, @"20\d{2}\-[0-1]{1,2}\-[0-3]?[0-9]?(\s*((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?))?");
        }

        #endregion

        #region 检测是否符合邮编格式
        /// <summary>
        /// 检测是否符合邮编格式
        /// </summary>
        /// <param name="postCode"></param>
        /// <returns></returns>
        public static bool IsPostCode(string postCode)
        {
            return Regex.IsMatch(postCode, @"^\d{6}$");
        }
        #endregion

        #region 验证是否为汉字,拼音数字
        /// <summary>
        ///  验证是否为汉字,拼音数字
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNTS(string input)
        {
            return Regex.IsMatch(input, "^[a-zA-Z0-9\\u4e00-\\u9fa5]+$");
        }
        #endregion
    }
    /// <summary>
    /// 确定对象
    /// </summary>
    public static class This
    {
        #region 基本正则判定
        /// <summary>
        /// 基本正则判定
        /// </summary>
        /// <param name="StrReg">正则表达式</param>
        /// <param name="TestStr">测试的内容</param>
        /// <returns></returns>
        public static Boolean IsMatch(String StrReg, String TestStr)
        {
            if (StrReg != null && TestStr != null)
            {
                Regex _Regex = new Regex(StrReg);
                return _Regex.IsMatch(TestStr);
            }
            return false;
        } 
        #endregion
        #region 是否为电子邮件
        /// <summary>
        /// 是否为电子邮件
        /// </summary>
        /// <param name="Reault"></param>
        /// <returns></returns>
        public static Boolean IsEmail(this object Result)
        {
            return IsMatch(@"^((?'name'.+?)\s*<)?(?'email'(?>[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+\x20*|""(?'user'(?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*""\x20*)*(?'angle'<))?(?'user'(?!\.)(?>\.?[a-zA-Z\d!#$%&'*+\-/=?^_`{|}~]+)+|""((?=[\x01-\x7f])[^""\\]|\\[\x01-\x7f])*"")@(?'domain'((?!-)[a-zA-Z\d\-]+(?<!-)\.)+[a-zA-Z]{2,}|\[(((?(?<!\[)\.)(25[0-5]|2[0-4]\d|[01]?\d?\d)){4}|[a-zA-Z\d\-]*[a-zA-Z\d]:((?=[\x01-\x7f])[^\\\[\]]|\\[\x01-\x7f])+)\])(?'angle')(?(name)>)$", Result.toString());
        } 
        #endregion
        #region 判断是否为空
        /// <summary>
        /// 判断是否为空
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static Boolean IsNullOrEmpty(this object Result)
        {
            if (Result == null)
            {
                return true;
            }
            if (String.IsNullOrWhiteSpace(Result.ToString()))
            {
                return true;
            }
            return false;
        } 
        #endregion
        #region 数据获取
        public static string QueryString(this string Result)
        {
            return System.Web.HttpContext.Current.Request.QueryString[Result];
        }
        public static string Form(this string Result)
        {
            return System.Web.HttpContext.Current.Request.Form[Result];
        }
        public static int Form(this string Result,int i)
        {
            return System.Web.HttpContext.Current.Request.Form[Result].toString(i);
        }
        #endregion
        #region 判断是否为数字
        /// <summary>
        /// 判断是否为数字
        /// </summary>
        /// <param name="TestStr">被测字符串</param>
        /// <returns></returns>
        public static Boolean IsNumberic(String TestStr)
        {
            if (!String.IsNullOrEmpty(TestStr))
            {
                return IsMatch(@"^[-]?\d+[.]?\d*$" , TestStr);
            }
            else
            {
                return false;
            }
        }
        #endregion
        #region 字符串转换
        /// <summary>
        /// 将数据转换为int数据
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static int toString(this object thisValue, int Result)
        {

            try
            {
                if (String.IsNullOrWhiteSpace(thisValue.toString()))
                {
                    return Result;
                }
                return Convert.ToInt32(thisValue);
            }
            catch
            {

                return Result;
            }
        }

        /// <summary>
        /// 将数据转换为int数据
        /// </summary>
        /// <param name="Result"></param>
        /// <returns></returns>
        public static long toString64(this object thisValue, long Result)
        {

            try
            {
                if (String.IsNullOrWhiteSpace(thisValue.toString()))
                {
                    return Result;
                }
                return Convert.ToInt64(thisValue);
            }
            catch
            {

                return Result;
            }
        }
        /// <summary>
        /// 将数据转换为string数据
        /// </summary>
        /// <param name="thisvalue"></param>
        /// <returns></returns>
        public static string toString(this object thisvalue)
        {
            try
            {

                return thisvalue.ToString();
            }
            catch
            {
                return "";
            }
        } 
        #endregion
        #region 字符串删除
        /// <summary>
        /// 删除A标签
        /// </summary>
        /// <param name="Str"></param>
        /// <returns></returns>
        public static string ReplaceHtml_A(this string Str)
        {
            if (!string.IsNullOrEmpty(Str))
            {
                Regex CutStyle = new Regex(@"<a([^>])*>(\w|\W)*?</a([^>])*>", RegexOptions.IgnoreCase);
                Str = CutStyle.Replace(Str, "");
            }
            return Str;
        }
        /// <summary>
        /// 删除html代码 保留 IMG P BR三个标签
        /// </summary>
        /// <param name="str">所需要删除HTML代码的字符串</param>
        /// <returns></returns>
        public static string ReplaceHtml_IPB(this string str)
        {
            if (str != "" && str != null)
            {
                //删除内含的 样式表代码
                Regex CutStyle = new Regex(@"<style([^>])*>(\w|\W)*?</style([^>])*>", RegexOptions.IgnoreCase);
                String TempStr = CutStyle.Replace(str, "");

                //<([^>]+)> 不过滤 img标签
                TempStr = TempStr.Replace("</p>", "[/p]");
                TempStr = TempStr.Replace("</P>", "[/p]");
                TempStr = TempStr.Replace("<p>", "[p]");
                TempStr = TempStr.Replace("<P>", "[p]");


                Regex BrHtml = new Regex("<br(.*?)>", RegexOptions.IgnoreCase);
                TempStr = BrHtml.Replace(TempStr, "[br/]");
                Regex SpanHtml1 = new Regex("<span", RegexOptions.IgnoreCase);
                TempStr = SpanHtml1.Replace(TempStr, "[span");
                Regex SpanHtml2 = new Regex("</span>", RegexOptions.IgnoreCase);
                TempStr = SpanHtml2.Replace(TempStr, "[/span]");
                Regex ImgHtml = new Regex("<img", RegexOptions.IgnoreCase);
                TempStr = ImgHtml.Replace(TempStr, "[img");
                Regex CutHtml = new Regex("<([^>]+)>", RegexOptions.IgnoreCase);
                TempStr = CutHtml.Replace(TempStr, "");
                //TempStr = TempStr.Replace ("/>" , ">");
                //Regex ImgHtml=new Regex("<img",RegexOptions.IgnoreCase);
                //格式化现有代码
                //TempStr = HttpUtility.HtmlEncode(TempStr);


                TempStr = TempStr.Replace("[img", "<img");
                TempStr = TempStr.Replace("[span", "<span");
                TempStr = TempStr.Replace("[p]", "<p>");
                TempStr = TempStr.Replace("[/p]", "</p>");
                TempStr = TempStr.Replace("[br/]", "<br/>");
                TempStr = TempStr.Replace("[/span]", "</span>");
                return TempStr;

            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 删除html代码 保留 IMG P BR三个标签
        /// </summary>
        /// <param name="str">所需要删除HTML代码的字符串</param>
        /// <returns></returns>
        public static string ReplaceHtml_PB(this string str)
        {
            if (str != "" && str != null)
            {
                //删除内含的 样式表代码
                Regex CutStyle = new Regex(@"<style([^>])*>(\w|\W)*?</style([^>])*>", RegexOptions.IgnoreCase);
                String TempStr = CutStyle.Replace(str, "");

                //<([^>]+)> 不过滤 img标签
                TempStr = TempStr.Replace("</p>", "[/p]");
                TempStr = TempStr.Replace("</P>", "[/p]");
                TempStr = TempStr.Replace("<p>", "[p]");
                TempStr = TempStr.Replace("<P>", "[p]");


                Regex BrHtml = new Regex("<br(.*?)>", RegexOptions.IgnoreCase);
                TempStr = BrHtml.Replace(TempStr, "[br/]");
                Regex SpanHtml1 = new Regex("<span", RegexOptions.IgnoreCase);
                TempStr = SpanHtml1.Replace(TempStr, "[span");
                Regex SpanHtml2 = new Regex("</span>", RegexOptions.IgnoreCase);
                TempStr = SpanHtml2.Replace(TempStr, "[/span]");

                Regex CutHtml = new Regex("<([^>]+)>", RegexOptions.IgnoreCase);
                TempStr = CutHtml.Replace(TempStr, "");
                Regex CutHtml2 = new Regex("\u003c([^>]+)\u003e", RegexOptions.IgnoreCase);
                TempStr = CutHtml2.Replace(TempStr, "");
                //TempStr = TempStr.Replace ("/>" , ">");
                //Regex ImgHtml=new Regex("<img",RegexOptions.IgnoreCase);
                //格式化现有代码
                //TempStr = HttpUtility.HtmlEncode(TempStr);


                
                TempStr = TempStr.Replace("[span", "<span");
                TempStr = TempStr.Replace("[p]", "<p>");
                TempStr = TempStr.Replace("[/p]", "</p>");
                TempStr = TempStr.Replace("[br/]", "<br/>");
                TempStr = TempStr.Replace("[/span]", "</span>");
                return TempStr;

            }
            else
            {
                return "";
            }
        }


        /// <summary>
        /// 删除html代码 保留 IMG
        /// </summary>
        /// <param name="str">所需要删除HTML代码的字符串</param>
        /// <returns></returns>
        public static string ReplaceHtml_I(this string str)
        {
            if (str != "" && str != null)
            {
                //删除内含的 样式表代码
                Regex CutStyle = new Regex(@"<style([^>])*>(\w|\W)*?</style([^>])*>", RegexOptions.IgnoreCase);
                String TempStr = CutStyle.Replace(str, "");

                //<([^>]+)> 不过滤 img标签
                //TempStr = TempStr.Replace("</p>", "[/p]");
                //TempStr = TempStr.Replace("</P>", "[/p]");
                //TempStr = TempStr.Replace("<p>", "[p]");
                //TempStr = TempStr.Replace("<P>", "[p]");


                Regex BrHtml = new Regex("<br(.*?)>", RegexOptions.IgnoreCase);
                TempStr = BrHtml.Replace(TempStr, "[br/]");
                //Regex SpanHtml1 = new Regex("<span", RegexOptions.IgnoreCase);
                //TempStr = SpanHtml1.Replace(TempStr, "[span");
                //Regex SpanHtml2 = new Regex("</span>", RegexOptions.IgnoreCase);
                //TempStr = SpanHtml2.Replace(TempStr, "[/span]");
                Regex ImgHtml = new Regex("<img", RegexOptions.IgnoreCase);
                TempStr = ImgHtml.Replace(TempStr, "[img");
                Regex CutHtml = new Regex("<([^>]+)>", RegexOptions.IgnoreCase);
                TempStr = CutHtml.Replace(TempStr, "");
                //TempStr = TempStr.Replace ("/>" , ">");
                //Regex ImgHtml=new Regex("<img",RegexOptions.IgnoreCase);
                //格式化现有代码
                //TempStr = HttpUtility.HtmlEncode(TempStr);


                TempStr = TempStr.Replace("[img", "<img");
                TempStr = TempStr.Replace("[span", "<span");
                TempStr = TempStr.Replace("[p]", "<p>");
                TempStr = TempStr.Replace("[/p]", "</p>");
                TempStr = TempStr.Replace("[br/]", "<br/>");
                TempStr = TempStr.Replace("[/span]", "</span>");
                return TempStr;

            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// 删除html代码
        /// </summary>
        /// <param name="str">所需要删除HTML代码的字符串</param>
        /// <returns></returns>
        public static string ReplaceHtml(this string str)
        {
            if (str != "" && str != null)
            {
                //删除内含的 样式表代码
                Regex CutStyle = new Regex(@"(?i)<style([^>])*>(\w|\W)*</style([^>])*>", RegexOptions.IgnoreCase);
                String TempStr = CutStyle.Replace(str, "");

                Regex CutHtml = new Regex("<([^>]+)>", RegexOptions.IgnoreCase);
                return CutHtml.Replace(TempStr, "");

            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 删除脚本
        /// </summary>
        /// <param name="str">接受处理的字符串</param>
        /// <returns></returns>
        public static string ReplaceScript(this string str)
        {
            string _temp = str;
            Regex CutScript = new Regex(@"(?i)<script([^>])*>(\w|\W)*</script([^>])*>", RegexOptions.IgnoreCase);
            _temp = CutScript.Replace(_temp, "");
            return _temp;
        }
        #endregion

    }
}
