using Common;
using Service;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();

            //定义定时器  
            System.Timers.Timer myTimer = new System.Timers.Timer(300000);//五分钟
            myTimer.Elapsed += new ElapsedEventHandler(myTimer_Elapsed);
            myTimer.Enabled = true;
            myTimer.AutoReset = true;

            //释放库分配
            System.Timers.Timer myTimerHistory = new System.Timers.Timer(1000);
            myTimerHistory.Elapsed += new ElapsedEventHandler(myTimer_ElapsedHistory);
            myTimerHistory.Enabled = true;
            myTimerHistory.AutoReset = true;
        }


        void myTimer_ElapsedHistory(object source, ElapsedEventArgs e)
        {

            try
            {
                DateTime dtNow = DateTime.Now;
                int hour = dtNow.Hour;
                int minute = dtNow.Minute;
                int second = dtNow.Second;

                //每天早上9点10分10秒开始执行 历史数据分配
                if (hour == 9 && minute == 10 && second == 10)
                {
                    LogHelper.WriteAutoSystemLog("释放库分配", "执行自动分配--开始", DateTime.Now);
                    InquiryHistoryService.HandHistoryInquiry();
                    LogHelper.WriteAutoSystemLog("释放库分配", "执行自动分配--结束", DateTime.Now);
                }
            }
            catch (Exception ee)
            {
                LogHelper.WriteErrorLog("Application Error", ee.ToString(), DateTime.Now);
            }
        }

        void myTimer_Elapsed(object source, ElapsedEventArgs e)
        {

            try
            {
                LogHelper.WriteAutoSystemLog("Application Start !", "", DateTime.Now); 
                YourTask();
            }
            catch (Exception ee)
            {
                LogHelper.WriteErrorLog("Application Error", ee.ToString(), DateTime.Now);
            }
        }
        void YourTask()
        {
            //在这里写你需要执行的任务  
            LogHelper.WriteAutoSystemLog("重新分配", "执行自动分配--开始", DateTime.Now);
            InquiryService.AutoAllocation();//自动分配
            LogHelper.WriteAutoSystemLog("重新分配", "执行自动分配--结束", DateTime.Now);   
        }

        protected void Application_End(object sender, EventArgs e)
        {

            LogHelper.WriteAutoSystemLog("Application End!!", "", DateTime.Now);
            //下面的代码是关键，可解决IIS应用程序池自动回收的问题  
            Thread.Sleep(1000);
            //这里设置你的web地址，可以随便指向你的任意一个aspx页面甚至不存在的页面，目的是要激发Application_Start  
            string url = "";
            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
            Stream receiveStream = myHttpWebResponse.GetResponseStream();//得到回写的字节流  
        }       　     　
    }
}