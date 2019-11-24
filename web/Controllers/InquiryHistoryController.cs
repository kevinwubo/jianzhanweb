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
    public class InquiryHistoryController : BaseController
    {
        //
        // GET: /InquiryHistory/
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
        public ActionResult Index(string name, string tracestate, int CustomerID = 0, int status = -1, string begindate = "", string enddate = "", int p = 1)
        {
            List<InquiryHistoryEntity> mList = null;

            string operatorid = getUserID(CurrentUser);

            int count = InquiryHistoryService.GetInquiryCount(name, tracestate, status, begindate, enddate, operatorid);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/InquiryHistory";

            ViewBag.InquiryCode = BaseDataService.GetBaseDataByPCode("InquiryS00");//跟踪状态
            //if (!string.IsNullOrEmpty(name) || status > -1 || !string.IsNullOrEmpty(tracestate))
            //{
            mList = InquiryHistoryService.GetInquiryInfoByRule(name, tracestate, status, begindate, enddate, operatorid, pager);
            //}
            //else
            //{
            //    mList = InquiryHistoryService.GetInquiryInfoPager(pager);
            //}

            ViewBag.Name = name ?? "";
            ViewBag.Status = status;
            ViewBag.ModelCode = tracestate;
            ViewBag.CustomerID = CustomerID;
            ViewBag.InquiryHistory = mList;
            ViewBag.Pager = pager;
            ViewBag.BeginDate = begindate;
            ViewBag.EndDate = enddate;
            return View();
        }



        public ActionResult Edit(string cid)
        {
            ViewBag.InquiryModel = BaseDataService.GetBaseDataByPCode("InquiryCode");

            List<UserEntity> userList = UserService.GetUserAll();

            if (!string.IsNullOrEmpty(cid))
            {
                ViewBag.InquiryHistory = InquiryHistoryService.GetInquiryEntityById(cid.ToLong(0));
            }
            else
            {
                ViewBag.InquiryHistory = new InquiryHistoryEntity();
            }
            ViewBag.UserList = userList;
            return View();
        }

        public void Modify(InquiryHistoryEntity entity)
        {
            //if (entity != null)
            //{
            //    entity.OperatorID = CurrentUser.UserID.ToString();
            //}
            InquiryHistoryService.ModifyInquiry(entity);
            Response.Redirect("/InquiryHistory/");
        }

        public void Remove(string rid)
        {
            InquiryHistoryService.Remove(rid.ToInt(0));

            Response.Redirect("/InquiryHistory/");
        }
       

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string getUserID(UserEntity user)
        {
            string operatorid = "";
            if (user != null && user.Roles.Count > 0)
            {
                int roleID = user.Roles[0].RoleID;
                int i = (int)RoleEnum.Admin;
                int sm = (int)RoleEnum.SalesManager;

                if (roleID == i)//管理员查询所有
                {
                    return operatorid;
                }
                else if (roleID == sm)//销售主管可以查看当前城市下所有的销售
                {
                    List<UserEntity> listUser = UserService.GetUserAll().FindAll(p => p.CityName.Equals(user.CityName));
                    if (listUser != null && listUser.Count > 0)
                    {
                        foreach (UserEntity item in listUser)
                        {
                            operatorid += item.UserID + ",";
                        }
                    }
                    return !string.IsNullOrEmpty(operatorid) ? operatorid.Substring(0, operatorid.Length - 1) : "";
                }
                else
                {
                    operatorid = user.UserID.ToString();
                }

            }
            return operatorid;
        }
    }
}
