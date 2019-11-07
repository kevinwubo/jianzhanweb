using Common;
using Entity.ViewModel;
using Service;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class ProductController : BaseController
    {
        //
        // GET: /Product/

        //
        // GET: /Inquiry/
        public int PAGESIZE = 20;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tracestate">跟踪状态</param>
        /// <param name="CustomerID"></param>
        /// <param name="status"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public ActionResult Index(string type2, string type3, string type4, string type7, string author, string sqlwhere, string Keyword, int p = 1)
        {
            List<ProductEntity> mList = null;

            int count = ProductService.GetProductCount(type2, type3, type4, type7, author, sqlwhere, Keyword);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/Product";

            //if (!string.IsNullOrEmpty(type2) )
            //{
                mList = ProductService.GetAllProductInfoByRule(type2, type3, type4, type7, author, sqlwhere, Keyword, "", "", pager);
            //}
            //else
            //{
            //    mList = ProductService.GetProductInfoPager("", pager);
            //}
            List<BaseDataEntity> list = BaseDataService.GetBaseDataAll();
            ViewBag.TypeList2 = list.Where(t => t.PCode == "YS000" && t.Status == 1).ToList();
            ViewBag.TypeList3 = list.Where(t => t.PCode == "QX000" && t.Status == 1).ToList();
            ViewBag.TypeList4 = list.Where(t => t.PCode == "KJCodes" && t.Status == 1).ToList();
            ViewBag.TypeList7 = list.Where(t => t.PCode == "JGCodes" && t.Status == 1).ToList();

            ViewBag.type2 = type2;
            ViewBag.type3 = type3;
            ViewBag.type4 = type4;
            ViewBag.type7 = type7;
            ViewBag.author = author;

            ViewBag.Product = mList;
            ViewBag.Pager = pager;
            return View();
        }


        public ActionResult Edit(string pid)
        {
            List<BaseDataEntity> list= BaseDataService.GetBaseDataAll();
            ViewBag.Type2 = list.Where(t => t.PCode == "YS000" && t.Status == 1).ToList();
            ViewBag.Type3 = list.Where(t => t.PCode == "QX000" && t.Status == 1).ToList();
            ViewBag.Type4 = list.Where(t => t.PCode == "KJCodes" && t.Status == 1).ToList();
            ViewBag.Type5 = list.Where(t => t.PCode == "GNCodes" && t.Status == 1).ToList();
            ViewBag.Type6 = list.Where(t => t.PCode == "LZCodes" && t.Status == 1).ToList();
            ViewBag.Type7 = list.Where(t => t.PCode == "JGCodes" && t.Status == 1).ToList();
            if (!string.IsNullOrEmpty(pid))
            {
                ViewBag.Product = ProductService.GetProductByKey(pid.ToLong(0));
            }
            else
            {
                ViewBag.Product = new ProductEntity();
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public void Modify(ProductEntity entity)
        {
            if (entity != null)
            {
                //entity.OperatorID = CurrentUser.UserID.ToString();
            }
            ProductService.ModifyProduct(entity);
            Response.Redirect("/Product/");
        }

        /// <summary>
        /// 更新库存数量
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="inventoryCount"></param>
        /// <returns></returns>
        public JsonResult ModifyInventoryCountByID(int id, int inventoryCount)
        {
            ProductService.ModifyInventoryCountByID(id, inventoryCount);
            return new JsonResult
            {
                Data = "库存更新成功！"
            };
        }

        public void Remove(string pid)
        {
            ProductService.Remove(pid.ToLong(0));
            Response.Redirect("/Product/");
        }

    }
}
