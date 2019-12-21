using Common;
using DataRepository.DataAccess.BaseData;
using DataRepository.DataAccess.News;
using DataRepository.DataModel;
using Entity.ViewModel;
using Infrastructure.Cache;
using Service;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class TempController : Controller
    {
        public int PAGESIZE = 20;

        public ActionResult Index()
        {
            return View();
        }
        public void Execute()
        {
            List<InquiryEntity> inquiryList = InquiryService.GetInquiryByRule("", "", "", " and  AddDate BETWEEN '2019-12-15 00:00:01' AND '2019-12-16 00:00:01' ", "", "");
            if (inquiryList != null && inquiryList.Count > 0)
            {
                foreach (InquiryEntity entity in inquiryList)
                {
                    List<InquiryEntity> list = InquiryService.GetInquiryByRule("", entity.telphone, "", " and  AddDate <'2019-12-15 00:00:01'", "", "");
                    foreach (InquiryEntity entityIn in list)
                    {

                        InquiryRepository ir = new InquiryRepository();
                        InquiryInfo infoU = new InquiryInfo();
                        infoU.OperatorID = entityIn.user.UserID.ToString();
                        infoU.PPId = entityIn.PPId;
                        infoU.ChangeDate = DateTime.Now;
                        ir.UpdateOperatorIDByPPId(infoU);
                    }
                }
            }
        }



        //public void Execute()
        //{
        //    try
        //    {
        //        TestRepository test = new TestRepository();
        //        DataSet ds = test.GetDataSet();
        //        DataTable dt = ds.Tables[0];
        //        if (dt.Rows.Count > 0)
        //        {
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                List<InquiryEntity> inquiryList = InquiryService.GetInquiryByRule("", StringHelper.ConvertBy123(dr["D3"].ToString()), "", "", "", "");
        //                //if (inquiryList == null || inquiryList.Count == 0)
        //                //{
        //                string dt1 = Convert.ToDateTime(dr["D1"].ToString()).ToShortDateString() + " " + Convert.ToDateTime(dr["D2"].ToString()).ToShortTimeString();
        //                    AddInquiry(dr["D3"].ToString(), dr["D4"].ToString(), dt1);
        //                //}
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.WriteErrorLog("auto", ex.ToString(), DateTime.Now);
        //    }

        //    Response.Redirect("Auto");
        //}

        private static InquiryInfo AddInquiry(string tel, string productid, string adddate)
        {
            InquiryRepository ir = new InquiryRepository();
            InquiryInfo info = new InquiryInfo();
            try
            {
                info.ProductID = productid;
                info.SourceForm = "99";
                info.ProcessingState = "0";
                info.telphone = StringHelper.ConvertBy123(tel);


                info.Provence = "";
                info.City = "";
                info.InquiryContent = "99资讯量";
                info.status = "新";
                info.HistoryOperatorID = "2";
                info.OperatorID = "2";
                info.SaleTelephone = "";
                info.CustomerName = "";
                info.WebChartID = "";
                info.AddDate = Convert.ToDateTime(adddate);
                ir.CreateSimpleInquiry(info);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("AddInquiry", ex.ToString(), DateTime.Now);
            }
            return info;
        }
    }
}
