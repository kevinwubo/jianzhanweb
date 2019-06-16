using Common;
using Entity.ViewModel;
using Infrastructure.Cache;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuoChe.Controllers
{
    public class HomeController:Controller
    {
        public CacheRuntime Cache = new CacheRuntime();

        public ActionResult Index()
        {
            DateTime dt= DateTime.Parse("2019-01-26").AddDays(180);

            ViewBag.UKey = Guid.NewGuid().ToString();

            return View();
        }

        public ContentResult Login(string name, string pwd, string ukey)
        {
            Log.WriteTextLog("home" + name + "pwd:" + pwd, DateTime.Now);
            string result = "";
            UserEntity user = UserService.GetLoginUser(name, EncryptHelper.MD5Encrypt(pwd));
            Log.WriteTextLog(JsonHelper.ToJson(user), DateTime.Now);
            if (user != null && user.UserID > 0)
            {
                Log.WriteTextLog(user.UserID.ToString(), DateTime.Now);
                HttpCookie cookie = WebHelper.CreateSingleValueCookie("ukey", ukey, 0);//把缓存key放入cookie
                Response.Cookies.Add(cookie);
                HttpCookie cookieu = WebHelper.CreateSingleValueCookie("ckey", EncryptHelper.Encryption(user.UserID.ToString()), 0);//把缓存key放入cookie
                Response.Cookies.Add(cookieu);
                Cache.Add<UserEntity>(ukey, user);//用户信息放入缓存
                result = "T";
            }
            else
            {
                result = "F";
            }
            Log.WriteTextLog("home1", DateTime.Now);
            return Content(result);
        }


        public void LogOut()
        {
            string ukey=Request.Cookies["ukey"]!=null?Request.Cookies["ukey"].Value:"";
            Cache.Remove(ukey);//删掉缓存
            Response.Cookies["ukey"].Expires = DateTime.Now.AddDays(-1);
            Response.Cookies["ckey"].Expires = DateTime.Now.AddDays(-1);
            Response.Redirect("/", true);
        }



    }
}
