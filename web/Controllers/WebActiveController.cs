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
    public class WebActiveController : Controller
    {
        //
        // GET: /WebActive/

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 商城首页
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_shop(string type2, string type3, string type4, string type7, string author, string artisanType, string keyword = "", int p = 1)
        {
            List<ProductEntity> mList = null;

            if (string.IsNullOrEmpty(type2) && string.IsNullOrEmpty(type3) && string.IsNullOrEmpty(type4) && string.IsNullOrEmpty(type7) && string.IsNullOrEmpty(author) && string.IsNullOrEmpty(artisanType) && string.IsNullOrEmpty(keyword))
            {
                artisanType = "业界大师";
            }

            if (!string.IsNullOrEmpty(author))
            {
                author = "'" + author + "'";
            }

            if (!string.IsNullOrEmpty(artisanType))
            {
                List<ArtisanEntity> listArt = ArtisanService.GetArtisansByRule(artisanType);
                if (listArt != null && listArt.Count > 0)
                {
                    foreach (ArtisanEntity entity in listArt)
                    {
                        author += "'" + entity.artisanName + "',";
                    }
                }
            }

            int count = ProductService.GetProductCount(type2, type3, type4, type7, string.IsNullOrEmpty(author) ? "" : author.TrimEnd(','), " and InventoryCount>0 ", keyword);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 12;
            pager.SumCount = count;
            pager.URL = "/WebHome/mn_shop";



            //if (!string.IsNullOrEmpty(type2) || !string.IsNullOrEmpty(type3) || !string.IsNullOrEmpty(type4) || !string.IsNullOrEmpty(type7) || !string.IsNullOrEmpty(keyword) || !string.IsNullOrEmpty(author))
            //{

            mList = ProductService.GetAllProductInfoByRule(type2, type3, type4, type7, string.IsNullOrEmpty(author) ? "" : author.TrimEnd(','), " and InventoryCount>0 ", keyword, " mn_shop", "", pager);
            //}
            //else
            //{
            //    mList = ProductService.GetProductInfoPager("ORDER BY Adddate Desc", pager);
            //}

            ViewBag.YJDSJson = JsonHelper.ToJson(ArtisanService.getSimpleArtisanList("业界大师"));//业界大师
            ViewBag.LPCCRJson = JsonHelper.ToJson(ArtisanService.getSimpleArtisanList("老牌传承人"));//老牌传承人
            ViewBag.MJGYSJson = JsonHelper.ToJson(ArtisanService.getSimpleArtisanList("名家工艺师"));//名家工艺师
            ViewBag.QXJson = JsonHelper.ToJson(BaseDataService.GetBaseDataAll().Where(t => t.PCode == "QX000" && t.Status == 1).ToList());//器型
            ViewBag.YSJson = JsonHelper.ToJson(BaseDataService.GetBaseDataAll().Where(t => t.PCode == "YS000" && t.Status == 1).ToList());//釉色
            ViewBag.ArtisanType = artisanType;
            ViewBag.Product = mList;
            ViewBag.Pager = pager;
            string keywords = Check(!string.IsNullOrEmpty(artisanType) ? "" : author) + Check(keyword) + Check(type2) + Check(type3) + Check(type4) + Check(type7) + Check(artisanType);
            ViewBag.Keyword = string.IsNullOrEmpty(keywords) ? keywords : keywords.TrimEnd(',');
            return View();
        }

        private string Check(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("'", "") + ",";
            }
            return str;
        }

        /// <summary>
        /// 商城首页详情页面
        /// </summary>
        /// <returns></returns> 
        public ActionResult mn_shopdetail(string productid)
        {
            ViewBag.ProductInfo = ProductService.GetProductByProductID(productid);
            return View();
        }

    }
}
