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

namespace GuoChe.Controllers
{
    public class BaseController : Controller
    {
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

        protected UserEntity CurrentUser {
            get {
                UserEntity user = Cache.Get<UserEntity>(UKey);

                if (user == null)
                {
                    string ckey=Request.Cookies["ckey"]!=null?Request.Cookies["ckey"].Value:"";
                    if (string.IsNullOrEmpty(ckey))
                    {
                        Response.Redirect("/", true);
                    }
                    else
                    {
                        ckey = EncryptHelper.Decrypt(ckey);
                        UserEntity cuser = UserService.GetUserById(ckey.ToLong(0));
                        if (cuser == null)
                        {
                            Response.Redirect("/", true);
                        }
                        else
                        {
                            user = cuser;
                            Cache.Add<UserEntity>(UKey, cuser);//用户信息放入缓存
                        }
                    }
                    
                }

                return user;
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
            string action = RouteData.Route.GetRouteData(this.HttpContext).Values["controller"].ToString();
            Response.Cookies["userName"].Value = action;
            Response.Cookies["userName"].Expires = DateTime.Now.AddDays(1);
            if (CurrentUser != null)
            {
                List<MenuEntity> listNew = new List<MenuEntity>();
                List<MenuEntity> list = CurrentUser.Menus;
                if (list != null && list.Count > 0)
                {
                    foreach (MenuEntity entity in list)
                    {
                        if (entity.URL.Contains(action))
                        {                            
                            entity.IsShow = true;
                        }
                        else
                        {                            
                            entity.IsShow = false;
                        }
                        listNew.Add(entity);
                    }
                }

                ViewBag.CommonMenus = listNew;
                ViewBag.CurrentSelectMenus = action;
            }
            //获取菜单信息
            base.OnActionExecuting(filterContext);
        }

        protected override void OnException(ExceptionContext filterContext)
        {
 
        }


    }
}
