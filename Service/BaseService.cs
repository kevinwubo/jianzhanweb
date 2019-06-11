using Infrastructure.Cache;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class BaseService
    {
        public static string URL = "http://116.62.124.214/";
        protected static CacheRuntime Cache
        {
            get
            {
                CacheRuntime cache = new CacheRuntime();
                cache.ExpiredTimespan = 60;//设置60分钟过期
                return cache;
            }
        }

        /// <summary>
        /// 拼接图片完整路径
        /// </summary>
        protected static string FileUrl 
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["FileUrl"].ToString();
            }
        }
    }
}
