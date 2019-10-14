using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class ExtendsController : BaseController
    {
        //
        // GET: /Extends/

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult CodesIndex(string codes = "", string isshow = "0")
        {

            ViewBag.CodesList = BaseDataService.GetCodeListByRule(codes, isshow);
            ViewBag.Codes = codes;
            return View();
        }
    }
}
