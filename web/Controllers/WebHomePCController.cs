using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class WebHomePCController : Controller
    {
        //
        // GET: /WebHomePC/

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult home()
        {
            return View();
        }
        /// <summary>
        /// 商城首页
        /// </summary>
        /// <returns></returns>
        public ActionResult shop()
        {
            return View();
        }

        /// <summary>
        /// 产品详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult shopdetail()
        {
            return View();
        }

        /// <summary>
        /// 艺人首页
        /// </summary>
        /// <returns></returns>
        public ActionResult famous()
        {
            return View();
        }

        /// <summary>
        /// 艺人详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult famousdetail()
        {
            return View();
        }

        /// <summary>
        /// 建盏学院
        /// </summary>
        /// <returns></returns>
        public ActionResult college()
        {
            return View();
        }

        /// <summary>
        /// 建盏学院列表
        /// </summary>
        /// <returns></returns>
        public ActionResult collegelist()
        {
            return View();
        }

        /// <summary>
        /// 建盏学院详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult collegedetail()
        {
            return View();
        }

        /// <summary>
        /// 收藏首页
        /// </summary>
        /// <returns></returns>
        public ActionResult souchang()
        {
            return View();
        }


        /// <summary>
        /// 经典首页
        /// </summary>
        /// <returns></returns>
        public ActionResult jingdian()
        {
            return View();
        }

        /// <summary>
        /// 经典首页详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult jingdiandetail()
        {
            return View();
        }
    }
}
