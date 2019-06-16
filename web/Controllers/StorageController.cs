using Common;
using Entity.ViewModel;
using Service;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GuoChe.Controllers
{
    public class StorageController : BaseController
    {
        //
        // GET: /Inquiry/
        public int PAGESIZE = 20;
        public ActionResult Index(string name = "",int status = -1, int p = 1)
        {
            
            List<InquiryEntity> mList = null;

            int count = InquiryService.GetInquiryCount("", "", status);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/Inquiry";


            if (!string.IsNullOrEmpty(name) || status > -1)
            {
                mList = InquiryService.GetInquiryInfoByRule(name, "", status, pager);
            }
            else
            {
                mList = InquiryService.GetInquiryInfoPager(pager);
            }
            ViewBag.Name = name ?? "";
            ViewBag.Status = status;
            //ViewBag.ModelCode = mcode;
            ViewBag.Inquiry = mList;
            ViewBag.Pager = pager;
            return View();
        }


        public ActionResult Edit(string cid)
        {
            ViewBag.Province = BaseDataService.GetAllProvince();
            if (!string.IsNullOrEmpty(cid))
            {
                //ViewBag.Inquiry = InquiryService.GetInquiryEntityById(cid.ToLong(0));
            }
            else
            {
                ViewBag.Inquiry = new InquiryEntity();
            }

            return View();
        }

        public void Modify(InquiryEntity Inquiry)
        {
            if (Inquiry != null)
            {
                Inquiry.OperatorID = CurrentUser.UserID.ToString();
            }
            //InquiryService.ModifyInquiry(Inquiry);
            Response.Redirect("/Inquiry/");
        }

        public void Remove(string InquiryID)
        {
            //InquiryService.RemoveInquiry(InquiryID);
            Response.Redirect("/Inquiry/");
        }

    }
}
