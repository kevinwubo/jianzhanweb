using Entity.ViewModel;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Service.BaseBiz;

namespace web.Controllers
{
    public class NewsController : BaseController
    {
        int PAGESIZE = 20;
        //
        // GET: /News/

        public ActionResult Index(int category_id = -1, int p = 1)
        {
            List<ArticleEntity> nList = null;
            int count = ArticleService.GetArticleCount(category_id);
            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/Index";

            nList = ArticleService.GetAllArticleInfoByRule(category_id, pager);

            List<BaseDataEntity>  newTypeList= BaseDataService.GetBaseDataByPCode("NewsS00");

            ViewBag.category_id = category_id;
            ViewBag.Pager = pager;
            ViewBag.News = nList;
            ViewBag.newTypeList = newTypeList;
            return View();
        }

        [HttpGet]
        public ActionResult Edit(string ID)
        {
            List<BaseDataEntity> newTypeList = BaseDataService.GetBaseDataByPCode("NewsS00");
            if (!string.IsNullOrEmpty(ID))
            {
                ViewBag.News = ArticleService.GetArticleByID(ID);
            }
            else
            {
                ViewBag.News = new ArticleEntity();
            }
            ViewBag.newTypeList = newTypeList;
            return View();
        }

        

        [HttpPost]
        [ValidateInput(false)]
        public void Modify()
        {
            int id = (Request["ID"] ?? "").ToInt(0);
            string Title = Request["Title"] ?? "";
            int status = (Request["Status"] ?? "").ToInt(0);
            int ChannelID = (Request["ChannelID"] ?? "").ToInt(0);
            string content = Request["Content"] ?? "";
            content = Request["Content"] ?? "";
            string zhaiyao = Request["zhaiyao"] ?? "";
            string AttachmentIDs = Request["AttachmentIDs"] ?? "";
            NewsEntity entity = new NewsEntity();
            entity.ID = id;
            entity.Title = Title;
            entity.ChannelID = ChannelID;
            entity.zhaiyao = zhaiyao;
            entity.AttachmentIDs = AttachmentIDs;
            entity.Operator = CurrentUser == null ? 0 : CurrentUser.UserID;
            entity.Content = content;
            entity.Status = status;
            NewsService.ModifiyNews(entity);
            Response.Redirect("/News/");
        }

        public void Remove(string rid)
        {
            NewsService.RemoveNews(rid.ToInt(0));

            Response.Redirect("/Role/");
        }


    }
}
