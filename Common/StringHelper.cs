using Entity.ViewModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Common
{
    public static class StringHelper
    {

        public static long ToLong(this string str, long defaultVal)
        {
            long result = 0;

            try
            {
                long.TryParse(str, out result);
                if (result == default(long))
                {
                    result = defaultVal;
                }
            }
            catch
            {
                result = defaultVal;
            }

            return result;
        }

        public static int ToInt(this string str, int defaultVal)
        {
            int result = 0;

            try
            {
                int.TryParse(str, out result);
                if (result == default(int))
                {
                    result = defaultVal;
                }
            }
            catch
            {
                result = defaultVal;
            }

            return result;
        }

        /// <summary>
        ///  数字替换成字母 1A 2B 3C 4D 5E 6F 7G 8H 9I 0M
        ///  随机字母 
        /// </summary>
        /// <returns></returns>
        public static string ConvertBy123(string str)
        {
            try
            {
                str = str.Replace("1", "A").Replace("2", "B").Replace("3", "C").Replace("4", "D").Replace("5", "E").Replace("6", "F").Replace("7", "G").Replace("8", "H").Replace("9", "I").Replace("0", "M");
            }
            catch (Exception ex)
            {
                return str;
            }
            return str;
        }

        /// <summary>
        /// 字母替换成数字 A1 B2 C3 D4 E5 F6 G7 H8 I9 M0
        /// </summary>
        /// <returns></returns>
        public static string ConvertByABC(string str)
        {
            try
            {
                str = str.Replace("A", "1").Replace("B", "2").Replace("C", "3").Replace("D", "4").Replace("E", "5").Replace("F", "6").Replace("G", "7").Replace("H", "8").Replace("I", "9").Replace("M", "0");
            }
            catch (Exception ex)
            {
                return str;
            }
            return str;
        }

        public static string NoHTML(string Htmlstring)
        {
            //删除脚本   
            Htmlstring = Regex.Replace(Htmlstring, @"<script[^>]*?>.*?</script>", "", RegexOptions.IgnoreCase);
            //删除HTML   
            Htmlstring = Regex.Replace(Htmlstring, @"<(.[^>]*)>", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"([\r\n])[\s]+", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"-->", "", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"<!--.*", "", RegexOptions.IgnoreCase);

            Htmlstring = Regex.Replace(Htmlstring, @"&(quot|#34);", "\"", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(amp|#38);", "&", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(lt|#60);", "<", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(gt|#62);", ">", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(nbsp|#160);", "   ", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(iexcl|#161);", "\xa1", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(cent|#162);", "\xa2", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(pound|#163);", "\xa3", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&(copy|#169);", "\xa9", RegexOptions.IgnoreCase);
            Htmlstring = Regex.Replace(Htmlstring, @"&#(\d+);", "", RegexOptions.IgnoreCase);

            Htmlstring.Replace("<", "");
            Htmlstring.Replace(">", "");
            Htmlstring.Replace("\r\n", "");
            Htmlstring = HttpContext.Current.Server.HtmlEncode(Htmlstring).Trim();

            return Htmlstring.Length > 50 ? Htmlstring.Substring(0, 49) : Htmlstring;
        }

        /// <summary>
        /// 获取省份城市信息
        /// </summary>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public static ProCityEntity getCity(string telephone)
        {
            ProCityEntity info = new ProCityEntity();
            try
            {
                string url = "http://apis.juhe.cn/mobile/get?phone=" + telephone + "&key=f6b3c53f05453d39221ac36b31bf170e";
                //请求数据
                HttpWebRequest res = (HttpWebRequest)WebRequest.Create(url);
                //方法名
                res.Method = "GET";
                //获取响应数据
                HttpWebResponse resp = (HttpWebResponse)res.GetResponse();
                //读取数据流
                StreamReader sr = new StreamReader(resp.GetResponseStream(), Encoding.UTF8);
                //编译成字符串
                string resphtml = sr.ReadToEnd();

                TelephoneJson result = JsonConvert.DeserializeObject<TelephoneJson>(resphtml);
                if (result != null)
                {
                    result re = result.result;
                    if (re != null)
                    {
                        info.city = !string.IsNullOrEmpty(re.city) ? re.city : "";
                        info.province = !string.IsNullOrEmpty(re.province) ? re.province : "";
                    }
                }
            }
            catch (Exception ex)
            {
                info = null;
            }
            return info;
        }
    }
}
