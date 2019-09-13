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
    public class ArtisanController : Controller
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

            ViewBag.Inquiry = mList;
            ViewBag.Pager = pager;
            return View();
        }


        public ActionResult Edit(string pid)
        {
            List<BaseDataEntity> list= BaseDataService.GetBaseDataAll();
            ViewBag.Type2 = list.Where(t => t.PCode == "InquiryCode" && t.Status == 1).ToList();
            ViewBag.Type3 = list.Where(t => t.PCode == "InquiryCode" && t.Status == 1).ToList();
            ViewBag.Type4 = list.Where(t => t.PCode == "InquiryCode" && t.Status == 1).ToList();
            ViewBag.Type5 = list.Where(t => t.PCode == "InquiryCode" && t.Status == 1).ToList();
            ViewBag.Type6 = list.Where(t => t.PCode == "InquiryCode" && t.Status == 1).ToList();
            ViewBag.Type7 = list.Where(t => t.PCode == "InquiryCode" && t.Status == 1).ToList();
            if (!string.IsNullOrEmpty(pid))
            {
                ViewBag.Artisan = ArtisanService.GetArtisanByKey(pid);
            }
            else
            {
                ViewBag.Artisan = new ArtisanEntity();
            }

            return View();
        }

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
