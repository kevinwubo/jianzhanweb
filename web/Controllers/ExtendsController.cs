using Entity.ViewModel;
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

        public ActionResult ModifyCodes(int id, string codevalues, string type = "")
        {
            CodeSEntity entity = new CodeSEntity();
            entity.ID = id;
            entity.CodeValues = codevalues;
            bool result = BaseDataService.ModifyCodes(entity);
            //return new JsonResult
            //{
            //    Data = result
            //};
            Response.Redirect("/Extends/CodesIndex?type=" + type);
            return View();
        }

        public ActionResult CodesIndex(string codes = "", string isshow = "0", string type = "")
        {
            if (!string.IsNullOrEmpty(type))
            {
                if (type.Equals("WYS"))//武夷山
                {
                    codes = "WuYiShanSales";
                }
                if (type.Equals("XM"))//厦门
                {
                    codes = "XiaMenSales";
                }
            }

            ViewBag.CodesList = BaseDataService.GetCodeListByRule(codes, isshow);
            ViewBag.Codes = codes;
            ViewBag.Type = type;
            return View();
        }
    }
}
