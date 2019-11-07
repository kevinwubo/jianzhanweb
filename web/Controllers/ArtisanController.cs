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
    public class ArtisanController : BaseController
    {
        //
        // GET: /Artisan/

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
        public ActionResult Index(string artisantype, string artisanname, int p = 1)
        {
            List<ArtisanEntity> mList = null;

            int count = ArtisanService.GetArtisanCount(artisantype,  artisanname);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/Artisan";

            if (!string.IsNullOrEmpty(artisantype) || !string.IsNullOrEmpty(artisanname))
            {
                mList = ArtisanService.GetAllArtisanInfoByRule(artisantype, artisanname, pager);
            }
            else
            {
                mList = ArtisanService.GetArtisanInfoPager(pager);
            }

            ViewBag.listAType = BaseDataService.GetBaseDataByPCode("ArtisanCodes");
            ViewBag.artisanName = artisanname;
            ViewBag.artisanType = artisantype;
            ViewBag.ArtisanList = mList;
            ViewBag.Pager = pager;
            return View();
        }


        /// <summary>
        /// 更新艺人顺序
        /// </summary>
        /// <param name="ID"></param>
        /// <param name="inventoryCount"></param>
        /// <returns></returns>
        public JsonResult ModifySortByID(int id, int sort)
        {
            ArtisanService.ModifySort(id, sort);
            return new JsonResult
            {
                Data = "顺序更新成功！"
            };
        }

        public ActionResult Edit(string aid)
        {
            ViewBag.listAType = BaseDataService.GetBaseDataByPCode("ArtisanCodes");

            if (!string.IsNullOrEmpty(aid))
            {
                ViewBag.Artisan = ArtisanService.GetArtisanByKey(aid);
            }
            else
            {
                ViewBag.Artisan = new ArtisanEntity();
            }

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public void Modify(ArtisanEntity entity)
        {
            if (entity != null)
            {
                //entity.OperatorID = CurrentUser.UserID.ToString();
            }
            ArtisanService.Modify(entity);
            Response.Redirect("/Artisan/");
        }

        public void Remove(string aid)
        {
            ArtisanService.Remove(aid.ToInt(0));
            Response.Redirect("/Artisan/");
        }

    }
}
