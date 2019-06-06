using Common;
using DataRepository.DataAccess.News;
using DataRepository.DataModel;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Helper;
using Service.BaseBiz;
using DataRepository.DataAccess.BaseData;
namespace Service
{
    public class InquiryService
    {
        #region 咨询量逻辑

        public static string CreateInquiry(string telephone, string productID, string sourceform)
        {
            string SmsTempletText = BaseDataService.GetCodeValuesByRule("SmsTemplate").CodeValues;//短信模板
            CodeSEntity blackmobile = BaseDataService.GetCodeValuesByRule("BlackMobile");//手机号黑名单
            //当天同手机号同产品编号只能资讯2次
            List<InquiryEntity> listProTel = GetInquiryByRule(productID, StringHelper.ConvertBy123(telephone), "", "and AddDate between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59' ", 0);

            //手机号大于5次 同一天
            List<InquiryEntity> listTel = GetInquiryByRule(productID, "", "", "and AddDate between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59' ", 0);
            if (blackmobile != null && !string.IsNullOrEmpty(blackmobile.CodeValues) && blackmobile.CodeValues.Contains(telephone))
            {

            }
            else if (listProTel != null && listProTel.Count > 2)
            {

            }
            else if (listTel != null && listTel.Count > 5)
            {

            }
            else
            {
                string code= GetTimeRangleCode(telephone);
                string realSaleName = GetSalesName(code);
                InquiryInfo info = AddInquiry(telephone, productID, sourceform, realSaleName);
                sendSMS(SmsTempletText, realSaleName, info);
            }
            return "";
        }

        /// <summary>
        /// 短信模板
        /// </summary>
        /// <param name="SmsTempletText"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static bool sendSMS(string SmsTempletText, string realSaleName, InquiryInfo info)
        {
            string SmsMess = string.Format(SmsTempletText, "不详", "不详", "不详", DateTime.Now.ToString());

            if (info != null)
            {
                ProductEntity proEntity = ProductService.GetProductByProductID(info.ProductID);
                SmsMess = string.Format(SmsTempletText, proEntity.Author, proEntity.ProductName, proEntity.ProductID, DateTime.Now.ToString()) + info.smsMess;

                string saleName = "";
                ManagerInfo mInfo = new ManagerInfo();
                if (!string.IsNullOrEmpty(info.OperatorID))
                {
                    ManagerRepository mr = new ManagerRepository();
                    mInfo = mr.GetManagerByID(info.OperatorID.ToInt(0));
                    saleName = (mInfo != null ? mInfo.real_name : realSaleName);
                    SmsMess += "跟踪销售：" + saleName;
                }
                SmsMess = SmsMess.Replace("-", "-" + saleName);

                //SMSHelper.SeedSMS(mInfo.telephone, SmsMess);               
                ////发送给老板
                //SMSHelper.SeedSMS("13916116545", SmsMess);

                ////发送城市对应的主管销售人员
                //if (mInfo.CityName.Equals("厦门"))
                //{
                //    SMSHelper.SeedSMS("17359271665", SmsMess);
                //}
                //else if (mInfo.CityName.Equals("武夷山"))
                //{
                //    SMSHelper.SeedSMS("13163806316", SmsMess);
                //}
            }
            return true;
        }

        /// <summary>
        /// 添加咨询量到数据表
        /// </summary>
        /// <param name="telephone"></param>
        /// <param name="productID"></param>
        /// <param name="sourceform"></param>
        /// <param name="realSaleName"></param>
        /// <returns></returns>
        private static InquiryInfo AddInquiry(string telephone, string productID, string sourceform, string realSaleName)
        {
            InquiryRepository ir = new InquiryRepository();
            ManagerInfo saleInfo = new ManagerInfo();
            ManagerRepository mmr = new ManagerRepository();
            List<ManagerInfo> mlist = mmr.GetBaseDataBySalesName(realSaleName);
            if (mlist != null && mlist.Count > 0)
            {
                saleInfo = mlist[0];
            }

            InquiryInfo info = new InquiryInfo();
            info.ProductID = productID;
            info.SourceForm = sourceform;
            info.ProcessingState = "0";
            info.telphone = telephone;
            //info.Provence=
            //info.City=
            //if (bll.GetCount(Tel) == 0)
            //{
            //    info.status = "新";
            //}
            //if (InquiryType.Equals("特"))
            //    info.status += "特";

            List<InquiryEntity> listInquiry = GetInquiryByRule("", "", "", " and (telphone='" + telephone + "' or telphone='" + StringHelper.ConvertBy123(telephone) + "') ", 0);
            if (listInquiry != null && listInquiry.Count > 0)
            {
                InquiryEntity entity = listInquiry[0];
                info.HistoryOperatorID = entity.OperatorID;
                info.OperatorID = entity.OperatorID;
                info.SaleTelephone = entity.SaleTelephone;
                info.smsMess = "老客户咨询！";
            }
            else
            {
                info.HistoryOperatorID = saleInfo.id.ToString();
                info.OperatorID = saleInfo.id.ToString();
                info.SaleTelephone = saleInfo.telephone;
            }
            ir.CreateNew(info);
            return info;
        }


        /// <summary>
        /// 获取分配销售姓名
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static string GetSalesName(string code)
        {
            ManagerRepository mmr = new ManagerRepository();
            //当前销售队列
            string codes = BaseDataService.GetCodeValuesByRule(code).CodeValues;
            string realnames = "";
            string codeNames = "";
            if (!string.IsNullOrEmpty(codes))
            {
                string[] codeList = codes.Split(',');
                foreach (string str in codeList)
                {
                    List<DefineInquiryInfo> listDefine = GetLastSaleNameBySaleName(str);
                    if (listDefine != null && listDefine.Count > 0)
                    {
                        DefineInquiryInfo info = listDefine[0];
                        if (info.salesCount > info.countCurrentDay || info.countCurrentDay == 0)
                        {
                            codeNames += "'" + str + "'" + ",";
                        }
                    }
                }

                //如果销售都分配满了 按照队列去自动分配
                if (string.IsNullOrEmpty(codeNames))
                {
                    foreach (string str in codeList)
                    {
                        codeNames += "'" + str + "'" + ",";
                    }
                }
            }

            List<DefineInquiryInfo> listLstDefine=GetLastSaleNameByCodes(codeNames.TrimEnd(','));

            string lastSaleName = listLstDefine != null && listLstDefine.Count > 0 ? listLstDefine[0].SaleName : ""; // 最近资讯销售姓名
            
            if (!string.IsNullOrEmpty(codes))
            {
                #region 老逻辑
                string[] salesList = codeNames.Split(',');
                List<string> lstSales = new List<string>();
                string OutSalesCodes = GetOutSalesName();
                foreach (string sale in salesList)
                {
                    //厦门武夷山排除销售
                    if (!OutSalesCodes.Contains(sale.Replace("'", "")))
                    {
                        ManagerInfo saleInfo = new ManagerInfo();
                        List<ManagerInfo> mlist = mmr.GetBaseDataBySalesName(sale.Replace("'", ""));
                        if (mlist != null && mlist.Count > 0)
                        {
                            saleInfo = mlist[0];
                        }

                        #region 当天在微信队列中的排除销售咨询队列
                        DateTime dt = DateTime.Now;
                        int week = Convert.ToInt32(dt.DayOfWeek);

                        string wxCodes = BaseDataService.GetCodeValuesByRule("WXChartList").CodeValues;
                        string WXCode = "";
                        try
                        {
                            if (!string.IsNullOrEmpty(wxCodes))
                            {
                                week = week == 0 ? 7 : week;
                                string[] codeStr = wxCodes.Split(',');
                                if (codeStr.Length < week)
                                {
                                    WXCode = codeStr[0];
                                }
                                else
                                {
                                    int cou = week - 1;
                                    WXCode = codeStr[cou];
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            WXCode = wxCodes.Split(',')[0];
                        }
                        #endregion
                        if (saleInfo != null)
                        {
                            if (!WXCode.Equals(saleInfo.telephone))
                            {
                                lstSales.Add(sale.Replace("'", ""));
                            }
                        }
                    }
                }
                try
                {
                    if (lstSales != null && lstSales.Count > 0)
                    {
                        if (lstSales.Contains(lastSaleName))
                        {
                            if (lstSales.IndexOf(lastSaleName) + 1 < lstSales.Count)
                            {
                                realnames = lstSales[lstSales.IndexOf(lastSaleName) + 1];
                            }
                            else
                            {
                                realnames = lstSales[0];
                            }
                        }
                        else
                        {
                            realnames = lstSales[0];
                        }
                    }
                    else
                    {
                        realnames = salesList[0];
                    }
                }
                catch (Exception ex)
                {
                    realnames = salesList[0];
                }
                #endregion
            }
            return realnames;
        }

        /// <summary>
        /// 获取排除队列数据
        /// </summary>
        /// <returns></returns>
        public static string GetOutSalesName()
        {
            string XMsales = BaseDataService.GetCodeValuesByRule("XiaMenSales").CodeValues;
            string WYSsales = BaseDataService.GetCodeValuesByRule("WuYiShanSales").CodeValues;

            return XMsales.TrimEnd(',') + "," + WYSsales;
        }

        /// <summary>
        /// 确定时间范围
        /// </summary>
        /// <returns></returns>
        public static string GetTimeRangleCode(string telephone)
        {
            string code = "";
            DateTime dtNow = Convert.ToDateTime(DateTime.Now.ToShortTimeString());

            //ASalesQueue 凌晨2点到12点分分配队列
            //BSalesQueue 12点~13点30分分配队列
            //CSalesQueue 13点30分~18点30分分配队列
            //DSalesQueue 18点30分~21点30分分配队列
            //ESalesQueue 21点30分~凌晨2点分配队列
            string datetime = DateTime.Now.AddDays(-1).ToShortDateString();
            string sqlTime = "";
            if (dtNow.CompareTo(Convert.ToDateTime("02:00")) > 0 && dtNow.CompareTo(Convert.ToDateTime("12:00")) < 0)
            {
                sqlTime = " and AddDate between '" + datetime + " 02:00' and '" + datetime + " 12:00'";
                code = "ASalesQueue";
            }
            else if (dtNow.CompareTo(Convert.ToDateTime("12:01")) > 0 && dtNow.CompareTo(Convert.ToDateTime("14:00")) < 0)
            {
                sqlTime = " and AddDate between '" + datetime + " 12:01' and '" + datetime + " 14:01'";
                code = "BSalesQueue";
            }
            else if (dtNow.CompareTo(Convert.ToDateTime("14:01")) > 0 && dtNow.CompareTo(Convert.ToDateTime("18:30")) < 0)
            {
                sqlTime = " and AddDate between '" + datetime + " 14:01' and '" + datetime + " 18:30'";
                code = "CSalesQueue";
            }
            else if (dtNow.CompareTo(Convert.ToDateTime("18:31")) > 0 && dtNow.CompareTo(Convert.ToDateTime("21:30")) < 0)
            {
                sqlTime = " and AddDate between '" + datetime + " 18:31' and '" + datetime + " 21:30'";
                code = "DSalesQueue";
            }
            else if (dtNow.CompareTo(Convert.ToDateTime("21:31")) > 0 && dtNow.CompareTo(Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " 01:59")) < 0)
            {
                sqlTime = " and AddDate between '" + datetime + " 21:31' and '" + datetime + " 01:59'";
                code = "ESalesQueue";
            }

            #region 城市信息优先级最高
            //ProCityInfo info = getCity(telephone);
            //string city = "";
            //string province = "";
            //if (info != null)
            //{
            //    city = info.city;
            //    province = info.province;
            //    if (!string.IsNullOrEmpty(info.city))
            //    {
            //        if (info.city.Equals("北京") || info.city.Equals("天津") || info.city.Equals("廊坊"))
            //        {
            //            code = "BeiJingSalesQueue";
            //        }
            //    }
            //}
            #endregion
            return code;
        }


        public static List<DefineInquiryInfo> GetLastSaleNameBySaleName(string salenames)
        {
            InquiryRepository mr = new InquiryRepository();
            return mr.GetLastSaleName(salenames);
        }


        public static List<DefineInquiryInfo> GetLastSaleNameByCodes(string salenames)
        {
            InquiryRepository mr = new InquiryRepository();
            return mr.GetLastSaleNameByCodes(salenames);
        }

        #endregion

        public static InquiryEntity GetInquiryEntityById(long cid)
        {
            InquiryEntity result = new InquiryEntity();
            InquiryRepository mr = new InquiryRepository();
            InquiryInfo info = mr.GetInquiryByKey(cid);
            result = TranslateInquiryEntity(info);
            return result;
        }

        private static InquiryEntity TranslateInquiryEntity(InquiryInfo info)
        {
            InquiryEntity entity = new InquiryEntity();
            entity.PPId = info.PPId;
            entity.ProductID = info.ProductID;
            entity.CustomerName = info.CustomerName;
            entity.Sex = info.Sex;
            entity.telphone = StringHelper.ConvertByABC(info.telphone);
            entity.WebChartID = info.WebChartID;
            entity.InquiryContent = info.InquiryContent;
            entity.CommentContent = info.CommentContent;
            entity.ProcessingState = info.ProcessingState;
            entity.ProcessingTime = info.ProcessingTime;
            entity.Provence = info.Provence;
            entity.City = info.City;
            entity.TraceContent = info.TraceContent;
            entity.TraceState = info.TraceState;
            entity.NextVisitTime = info.NextVisitTime;
            entity.AddDate = info.AddDate;
            entity.OperatorID = info.OperatorID;
            entity.SaleTelephone = info.SaleTelephone;
            entity.status = info.status;
            entity.SourceForm = info.SourceForm;

            entity.product = ProductService.GetProductByProductID(info.ProductID);
            return entity;
        }

        private static InquiryInfo TranslateInquiryInfo(InquiryEntity entity)
        {
            InquiryInfo info = new InquiryInfo();
            if (info != null)
            {
                info.PPId = entity.PPId;
                info.ProductID = entity.ProductID;
                info.CustomerName = entity.CustomerName;
                info.Sex = entity.Sex;
                info.telphone = entity.telphone;
                info.WebChartID = entity.WebChartID;
                info.InquiryContent = entity.InquiryContent;
                info.CommentContent = entity.CommentContent;
                info.ProcessingState = entity.ProcessingState;
                info.ProcessingTime = entity.ProcessingTime;
                info.Provence = entity.Provence;
                info.City = entity.City;
                info.TraceContent = entity.TraceContent;
                info.TraceState = entity.TraceState;
                info.NextVisitTime = entity.NextVisitTime;
                info.AddDate = entity.AddDate;
                info.OperatorID = entity.OperatorID;
                info.SaleTelephone = entity.SaleTelephone;
                info.status = entity.status;
                info.SourceForm = entity.SourceForm;
            }

            return info;
        }

        public static bool ModifyInquiry(InquiryEntity entity)
        {
            long result = 0;
            if (entity != null)
            {
                InquiryRepository mr = new InquiryRepository();

                InquiryInfo InquiryInfo = TranslateInquiryInfo(entity);

                

                if (entity.PPId > 0)
                {
                    InquiryInfo.PPId = entity.PPId;
                    InquiryInfo.ChangeDate = DateTime.Now;
                    result = mr.ModifyInquiry(InquiryInfo);
                }
                else
                {
                    InquiryInfo.ChangeDate = DateTime.Now;
                    InquiryInfo.AddDate = DateTime.Now;
                    result = mr.CreateNew(InquiryInfo);
                }


                //List<InquiryInfo> miList = mr.GetAllInquiry();//刷新缓存
                //Cache.Add("InquiryALL", miList);
            }
            return result > 0;
        }

        public static InquiryEntity GetInquiryById(long gid)
        {
            InquiryEntity result = new InquiryEntity();
            InquiryRepository mr = new InquiryRepository();
            InquiryInfo info = mr.GetInquiryByKey(gid);
            result = TranslateInquiryEntity(info);
            return result;
        }

        public static List<InquiryEntity> GetInquiryAll()
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetAllInquiry();//Cache.Get<List<InquiryInfo>>("InquiryALL");
            //if (miList.IsEmpty())
            //{
            //    miList = mr.GetAllInquiry();
            //    Cache.Add("InquiryALL", miList);
            //}
            if (!miList.IsEmpty())
            {
                foreach (InquiryInfo mInfo in miList)
                {
                    InquiryEntity InquiryEntity = TranslateInquiryEntity(mInfo);
                    all.Add(InquiryEntity);
                }
            }

            return all;

        }

        public static List<InquiryEntity> GetInquiryByRule(string productid, string telephone, string name, string sqlwhere, int status)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetInquiryByRule(productid, telephone, name, sqlwhere, status);

            if (!miList.IsEmpty())
            {
                foreach (InquiryInfo mInfo in miList)
                {
                    InquiryEntity InquiryEntity = TranslateInquiryEntity(mInfo);
                    all.Add(InquiryEntity);
                }
            }

            return all;

        }

        public static List<InquiryEntity> GetInquiryByKeys(string ids)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetInquiryByKeys(ids);

            if (!miList.IsEmpty())
            {
                foreach (InquiryInfo mInfo in miList)
                {
                    InquiryEntity entity = TranslateInquiryEntity(mInfo);
                    all.Add(entity);
                }
            }

            return all;
        }

        public static int Remove(long gid)
        {
            InquiryRepository mr = new InquiryRepository();
            int i = mr.Remove(gid);
            //List<InquiryInfo> miList = mr.GetAllInquiry();//刷新缓存
            //Cache.Add("InquiryALL", miList);
            return i;
        }

        #region 分页相关
        public static int GetInquiryCount(string name, string tracestate, int status)
        {
            return new InquiryRepository().GetInquiryCount(name, tracestate, -1);
        }

        public static List<InquiryEntity> GetInquiryInfoPager(PagerInfo pager)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetAllInquiryInfoPager(pager);
            foreach (InquiryInfo mInfo in miList)
            {
                InquiryEntity carEntity = TranslateInquiryEntity(mInfo);
                all.Add(carEntity);
            }
            return all;
        }

        public static List<InquiryEntity> GetInquiryInfoByRule(string name, string tracestate, int status, PagerInfo pager)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetAllInquiryInfoByRule(name, tracestate, status, pager);

            if (!miList.IsEmpty())
            {
                foreach (InquiryInfo mInfo in miList)
                {
                    InquiryEntity storeEntity = TranslateInquiryEntity(mInfo);
                    all.Add(storeEntity);
                }
            }

            return all;
        }
        #endregion
    }
}
