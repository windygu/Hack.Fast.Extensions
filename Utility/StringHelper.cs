using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
namespace Hack.Extensions.Utility
{
    public static class StringHelper
    {

        /// <summary>
        ///过滤单引号(')
        /// </summary>
        /// <param name="str">要过滤的字符串</param>
        /// <returns></returns>
       public static string FilterString(object strInput)
       {         
           string strRtn = "";
           try
           {
               if (strInput == null) strRtn = "";
               else
               {
                   strRtn = strInput.ToString();
                   ////删除脚本
                   strRtn = Regex.Replace(strRtn, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                   //删除HTML
                   //strRtn = Regex.Replace(strRtn, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                   //strRtn = Regex.Replace(strRtn, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                   //回车\n\r和商品属性冲突
                   //strRtn = Regex.Replace(strRtn, @"([])[\s]+", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"-->", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"<!--.*", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"javript", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"alert", "", RegexOptions.IgnoreCase);
                   //strRtn = Regex.Replace(strRtn, @"<", "", RegexOptions.IgnoreCase);
                   //strRtn = Regex.Replace(strRtn, @">", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"1=", "\"", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"'='", "\"", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                   //strRtn = Regex.Replace(strRtn, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "'", "", RegexOptions.IgnoreCase);
                   //删除与数据库相关的词
                   //和SinSelect.aspx冲突
                   //strRtn = Regex.Replace(strRtn, "select", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "insert", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "delete from", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "count''", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "drop table", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "truncate", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "asc", "", RegexOptions.IgnoreCase);
                   //这里的mid和自定义栏目冲突
                   //strRtn = Regex.Replace(strRtn, "mid", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "char", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "exec master", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                   //这里的and 和 商品品牌相冲突
               }
           }
           catch
           {
               strRtn = "";
           }
           return strRtn;


         
       }
       /// <summary>
       /// 处理德国客户网站  价格小数点变成逗号    执行插入更新的时候产生问题
       /// </summary>
       /// <param name="strInput"></param>
       /// <returns></returns>
       public static string FilterDouhao(object strInput)
       {
           return strInput.ToString().Replace(",", ".");
       }
       /// <summary>
       ///过滤单引号(')
       /// </summary>
       /// <param name="str">要过滤的字符串</param>
       /// <returns></returns>
       public static string FilterStringUrl(object strInput)
       {
           string strRtn = "";
           try
           {
               if (strInput == null) strRtn = "";
               else
               {
                   strRtn = strInput.ToString();
                   strRtn = strRtn.Replace("%2F", "/");
                   strRtn = strRtn.Replace("<object", "");
                   strRtn = strRtn.Replace("</script>", "<script>");
                   ////删除脚本
                   strRtn = Regex.Replace(strRtn, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
                   //删除HTML
                   strRtn = Regex.Replace(strRtn, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
                   //strRtn = Regex.Replace(strRtn, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
                   //回车\n\r和商品属性冲突
                   //strRtn = Regex.Replace(strRtn, @"([])[\s]+", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"-->", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"<!--.*", "", RegexOptions.IgnoreCase);

                   strRtn = Regex.Replace(strRtn, @"<", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @">", "", RegexOptions.IgnoreCase);

                   strRtn = Regex.Replace(strRtn, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
                   //strRtn = Regex.Replace(strRtn, @"&(nbsp|#160);", " ", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, @"&#(\d+);", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "xp_cmdshell", "", RegexOptions.IgnoreCase);

                   //删除与数据库相关的词
                   //和SinSelect.aspx冲突
                   //strRtn = Regex.Replace(strRtn, "select", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "insert", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "delete from", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "count''", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "drop table", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "truncate", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "", "", RegexOptions.IgnoreCase);
                   //这里的mid和自定义栏目冲突
                   //strRtn = Regex.Replace(strRtn, "mid", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "char", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "xp_cmdshell", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "exec master", "", RegexOptions.IgnoreCase);
                   strRtn = Regex.Replace(strRtn, "net localgroup administrators", "", RegexOptions.IgnoreCase);
                   //这里的and 和 商品品牌相冲突
                   //strRtn = Regex.Replace(strRtn, "and", "", RegexOptions.IgnoreCase);
                   //strRtn = strRtn.Replace("'", "");
                   strRtn = strRtn.Replace("/", "%2f");
                   strRtn = strRtn.Replace("'", "");
                   strRtn = strRtn.Replace("[", "");
                   strRtn = strRtn.Replace("]", "");
                   strRtn = strRtn.Replace("#", "");
                   strRtn = strRtn.Replace("$", "");
                   strRtn = strRtn.Replace("^", "");
                   strRtn = strRtn.Replace("*", "");
                   strRtn = strRtn.Replace(@",./';[]\!@#$%^&*()", "");
                   //strRtn = strRtn.Replace("\"", "");
               }
           }
           catch
           {
               strRtn = "";
           }
           return strRtn;
       }
       /// <summary>
       /// 截取前台字符数  
       /// </summary>
       /// <param name="opstring"></param>
       /// <param name="leng"></param>
       /// <returns></returns>
       public static  string  OperteString(string opstring,int leng)
      {
          if (opstring.Length >= leng)
          {
              return opstring.Substring(0, leng);
          }
          return opstring;
      }

        public static int FilterInt(object strInput)
        {
            string strRtn = "";
            try
            {
                if (strInput == null) strRtn = "";
                else
                {
                    strRtn = strInput.ToString();
                    strRtn = strRtn.Replace("'", "''");
                }
            }
            catch
            {
                strRtn = "";
            }
            return Convert.ToInt32(strRtn);
        }

        public static string Left(string str, int length)
        {
            if (length >= str.Length)
                return str;
            return str.Substring(0, length);
        }

        public static string Right(string original, int length)
        {
            if (original == null)
                throw new ArgumentNullException("original", "Right cannot be evaluated on a null string.");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length", length, "Length must not be negative.");

            if (original.Length == 0 || length == 0)
                return String.Empty;

            if (length >= original.Length)
                return original;

            return original.Substring(original.Length - length);
        }

        /// <summary>
        /// 判断对象是否为Int32类型的数字
        /// </summary>
        /// <param name="Expression"></param>
        /// <returns></returns>
        public static bool CheckNumeric(object Expression)
        {
            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*[.]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return true;
                    }
                }
            }
            return false;

        }

       /// <summary>
        /// 判断对象是否为Double类型的数字
       /// </summary>
       /// <param name="Expression"></param>
       /// <returns></returns>
        public static bool CheckDouble(object Expression)
        {
            if (Expression != null)
            {
                return Regex.IsMatch(Expression.ToString(), @"^([0-9])[0-9]*(\.\w*)?$");
            }
            return false;
        }

        /// <summary>
        /// string型转换为float型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static float StringToFloat(object strValue, float defValue)
        {
            if ((strValue == null) || (strValue.ToString().Length > 10))
            {
                return defValue;
            }

            float intValue = defValue;
            if (strValue != null)
            {
                bool IsFloat = Regex.IsMatch(strValue.ToString(), @"^([-]|[0-9])[0-9]*(\.\w*)?$");
                if (IsFloat)
                {
                    intValue = Convert.ToSingle(strValue);
                }
            }
            return intValue;
        }

       /// <summary>
       /// 将为""和null的字符串转为Empty
       /// </summary>
       /// <param name="str"></param>
       /// <returns></returns>
        public static string FormatToEmpty(string str)
        {
            if (str == "")
            {
                str = string.Empty;
            }
            else if (str == null)
            {
                str = string.Empty;
            }
            else if (str == "&nbsp;")
            {
                str = string.Empty;
            }
            return str;
        }
        /// <summary>
        /// 判断给定的字符串数组(strNumber)中的数据是不是都为数值型
        /// </summary>
        /// <param name="strNumber">要确认的字符串数组</param>
        /// <returns>是则返加true 不是则返回 false</returns>
        public static bool CheckNumericArray(string[] strNumber)
        {
            if (strNumber == null)
            {
                return false;
            }
            if (strNumber.Length < 1)
            {
                return false;
            }
            foreach (string id in strNumber)
            {
                if (!CheckNumeric(id))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 将对象转换为Int32类型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的int类型结果</returns>
        public static int StringToInt(object Expression, int defValue)
        {
            if (Expression != null)
            {
                string str = Expression.ToString();
                if (str.Length > 0 && str.Length <= 11 && Regex.IsMatch(str, @"^[-]?[0-9]*$"))
                {
                    if ((str.Length < 10) || (str.Length == 10 && str[0] == '1') || (str.Length == 11 && str[0] == '-' && str[1] == '1'))
                    {
                        return Convert.ToInt32(str);
                    }
                }
            }
            return defValue;
        }

        /// <summary>
        /// string型转换为bool型
        /// </summary>
        /// <param name="strValue">要转换的字符串</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>转换后的bool类型结果</returns>
        public static bool StringToBool(object Expression, bool defValue)
        {
            if (Expression != null)
            {
                if (string.Compare(Expression.ToString(), "true", true) == 0)
                {
                    return true;
                }
                else if (string.Compare(Expression.ToString(), "false", true) == 0)
                {
                    return false;
                }
            }
            return defValue;
        }


    }
}
