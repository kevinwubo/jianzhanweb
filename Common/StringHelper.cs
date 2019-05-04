using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
