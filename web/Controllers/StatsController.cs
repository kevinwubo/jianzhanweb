using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class StatsController : BaseController
    {
        private StatsService service = new StatsService();
        //
        // GET: /Stats/

        public ActionResult SeoStats(string datetime = "")
        {
            ViewBag.StatsList = service.GetInquiryAdver(datetime);
            ViewBag.Time = datetime;
            return View();
        }


        public ActionResult InquiryStats(string datetime = "")
        {
            ViewBag.StatsList = service.GetProInquiry(datetime);
            ViewBag.Time = datetime;
            return View();
        }
        

    }
}
