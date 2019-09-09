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
    public class InquiryController : BaseController
    {
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
        public ActionResult Index(string name, string tracestate, int CustomerID = 0, int status = -1, int p = 1)
        {
            List<InquiryEntity> mList = null;

            int count = InquiryService.GetInquiryCount(name, tracestate, status);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/Inquiry";

            ViewBag.InquiryCode = BaseDataService.GetBaseDataAll().Where(t => t.PCode == "InquiryS00" && t.Status == 1).ToList();//跟踪状态
            if (!string.IsNullOrEmpty(name) || status > -1 || !string.IsNullOrEmpty(tracestate))
            {
                mList = InquiryService.GetInquiryInfoByRule(name, tracestate, status, pager);
            }
            else
            {
                mList = InquiryService.GetInquiryInfoPager(pager);
            }

            ViewBag.Name = name ?? "";
            ViewBag.Status = status;
            ViewBag.ModelCode = tracestate;
            ViewBag.CustomerID = CustomerID;
            ViewBag.Inquiry = mList;
            ViewBag.Pager = pager;
            return View();
        }
        #region 手动添加咨询量
        /// <summary>
        /// 手动添加咨询量
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            ViewBag.UserInfo = CurrentUser;
            return View();
        }

        /// <summary>
        /// 手动添加咨询量--保存
        /// </summary>
        /// <param name="entity"></param>
        public void HandSave(InquiryEntity entity)
        {
            InquiryService.CreateSimple(entity, CurrentUser);
            Response.Redirect("/Inquiry/");
        }
        #endregion

       

        public ActionResult Edit(string cid)
        {
            ViewBag.InquiryModel = BaseDataService.GetBaseDataAll().Where(t => t.PCode == "InquiryCode" && t.Status == 1).ToList();

            List<UserEntity> userList = UserService.GetUserAll();

            if (!string.IsNullOrEmpty(cid))
            {
                ViewBag.Inquiry = InquiryService.GetInquiryEntityById(cid.ToLong(0));
            }
            else
            {
                ViewBag.Inquiry = new InquiryEntity();
            }
            ViewBag.UserList = userList;
            return View();
        }

        public void Modify(InquiryEntity entity)
        {
            //if (entity != null)
            //{
            //    entity.OperatorID = CurrentUser.UserID.ToString();
            //}
            InquiryService.ModifyInquiry(entity);
            Response.Redirect("/Inquiry/");
        }

        public void Remove(string rid)
        {
            InquiryService.Remove(rid.ToInt(0));

            Response.Redirect("/Inquiry/");
        }


    }
}
