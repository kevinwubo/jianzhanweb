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
    public class OrderController : BaseController
    {
        //
        // GET: /Order/
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
        public ActionResult Index(string name, string receivername, int status = -1, int p = 1)
        {
            List<OrderEntity> mList = null;

            int count = OrderService.GetOrderCount(name, receivername, status);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/Order";

            if (!string.IsNullOrEmpty(name) || status > -1 || !string.IsNullOrEmpty(receivername))
            {
                mList = OrderService.GetOrderInfoByRule(name, receivername, status, pager);
            }
            else
            {
                mList = OrderService.GetOrderInfoPager(pager);
            }       

            ViewBag.Name = name ?? "";
            ViewBag.Status = status;
            ViewBag.Order = mList;
            ViewBag.Pager = pager;

            return View();
        }


        public ActionResult Edit(string cid)
        {
            ViewBag.Province = BaseDataService.GetAllProvince();
            ViewBag.OrderModel = BaseDataService.GetBaseDataAll().Where(t => t.PCode == "OrderCode" && t.Status == 1).ToList();
            if (!string.IsNullOrEmpty(cid))
            {
                ViewBag.Order = OrderService.GetOrderEntityById(cid.ToLong(0));
            }
            else
            {
               OrderEntity entity= new OrderEntity();
               entity.DeliveryDate = DateTime.Now;
               entity.CollectedDate = DateTime.Now;
               ViewBag.Order = entity;

            }

            OrderButtonEntity entityRole = new OrderButtonEntity();

            if (CurrentUser != null)
            {
                if (CurrentUser.Roles != null && CurrentUser.Roles.Count > 0)
                {
                    entityRole = OrderService.ConvertButton(CurrentUser.Roles);
                }
            }
            ViewBag.OrderButton = entityRole;

            return View();
        }

        public void Modify(OrderEntity entity)
        {
            if (entity != null)
            {
                entity.OperatorID = CurrentUser.UserID.ToString();
            }
            OrderService.ModifyOrder(entity);
            Response.Redirect("/Order/");
        }


        public void RemoveOrderDetail(string cid)
        {
            OrderService.RemoveOrderDetail(cid.ToInt(0));            
        }


    }
}
