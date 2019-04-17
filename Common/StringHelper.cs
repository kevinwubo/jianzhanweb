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
    }
}
