using Entity.ViewModel;
using Infrastructure.Cache;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Infrastructure.Helper;
using Common;
using System.Configuration;

namespace web.Controllers
{
    public class BaseWebontroller : Controller
    {
        public static string Url = ConfigurationManager.ConnectionStrings["HttpUrl"].ToString();
        public CacheRuntime Cache { 
             get { 
                 CacheRuntime cache=new CacheRuntime(); 
                 cache.ExpiredTimespan=30;//设置30分钟过期
                 return cache;
             } 
        }

        public string UKey {
            get {
                string ukey = "";
                if (!string.IsNullOrEmpty(Request["ukey"] ?? ""))
                {
                    ukey = Request["ukey"];//如果表单内有值就从表单内获取
                }
                else
                {
                    ukey = Request.Cookies["ukey"]!=null?Request.Cookies["ukey"].Value:"";//表单内没有就从cookie中获取
                }

                return ukey;
            }
        }

       

        public JsonResult GetCity(int pid)
        {
            List<City> listCity = BaseDataService.GetAllCity();
            if (!listCity.IsEmpty())
            {
                listCity = listCity.Where(t => t.ProvinceID == pid).ToList();
            }

            return Json(listCity);
        }



        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {

            
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.UKey = UKey;
            ViewBag.WXCodes = getWXCode();
            //获取菜单信息
            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
 
        }

        public static string getWXCode()
        {
            string WXCode = "13163806316";
            DateTime dt = DateTime.Now;
            int week = Convert.ToInt32(dt.DayOfWeek);

            string codes = BaseDataService.GetCodeValuesByRule("WXChartList").CodeValues;
            if (!string.IsNullOrEmpty(codes))
            {
                string[] codeStr = codes.Split(',');
                week = week == 0 ? 7 : week;
                if (codeStr.Length < week)
                {
                    WXCode = codeStr[0];
                }
                else
                {
                    WXCode = codeStr[week - 1];
                }
            }
            return WXCode;
        }

    }
}
