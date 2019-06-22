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
            List<InquiryEntity> listProTel = GetInquiryByRule(productID, StringHelper.ConvertBy123(telephone), "", "and AddDate between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59' ", "", "");

            //手机号大于5次 同一天
            List<InquiryEntity> listTel = GetInquiryByRule(productID, "", "", "and AddDate between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59' ", "", "");
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
                string code = GetTimeRangleCode(telephone);
                ManagerEntity entity = GetSalesNameNew(code);
                InquiryInfo info = AddInquiry(telephone, productID, sourceform, entity);
                sendSMS(SmsTempletText, productID, info.smsMess, entity);
            }
            return "";
        }

        /// <summary>
        /// 短信模板
        /// </summary>
        /// <param name="SmsTempletText"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static bool sendSMS(string SmsTempletText, string productID, string smsMess, ManagerEntity entity)
        {
            string SmsMess = string.Format(SmsTempletText, "不详", "不详", "不详", DateTime.Now.ToString());

            if (!string.IsNullOrEmpty(productID))
            {
                ProductEntity proEntity = ProductService.GetProductByProductID(productID);
                SmsMess = string.Format(SmsTempletText, proEntity.Author, proEntity.ProductName, proEntity.ProductID, DateTime.Now.ToString()) + smsMess;
                if (entity != null)
                {
                    SmsMess += "跟踪销售：" + entity.real_name;
                }
                SmsMess = SmsMess.Replace("-", "-");

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

        public static ManagerEntity GetSalesNameNew(string code)
        {
            ManagerRepository mmr = new ManagerRepository();
            List<ManagerEntity> allList = ManagerService.GetManagerAll();
            //当前销售队列
            string codes = BaseDataService.GetCodeValuesByRule(code).CodeValues;
            //当前销售队列
            List<ManagerEntity> listCurrent = getCurrentSalesListQuence(codes, allList);
            //执行排除队列
            List<ManagerEntity> listTask = getOutSalesList(listCurrent);
            //获取最终销售
            ManagerEntity entity = GetManagerByOperatorID(listTask, codes, allList);

            return entity;
        }
        /// <summary>
        /// 当前队列所有信息
        /// </summary>
        /// <returns></returns>
        private static List<ManagerEntity> getCurrentSalesListQuence(string codes, List<ManagerEntity> allList)
        {
            List<ManagerEntity> list = new List<ManagerEntity>();
            if (!string.IsNullOrEmpty(codes) && allList.Count > 0)
            {
                string[] codeList = codes.Split(',');
                foreach (string name in codeList)
                {
                    ManagerEntity entity = allList.Find(p => p.real_name.Equals(name) && (p.salesCount < p.currentSalesCount || p.currentSalesCount == 0));
                    if (entity != null)
                    {
                        list.Add(entity);
                    }
                }

                //如果销售都分配满了 按照队列去自动分配
                if (list == null || list.Count == 0)
                {
                    foreach (string name in codeList)
                    {
                        ManagerEntity entity = allList.Find(p => p.real_name.Equals(name));
                        if (entity != null)
                        {
                            list.Add(entity);
                        }
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 当前队列所有信息
        /// </summary>
        /// <returns></returns>
        private static List<ManagerEntity> getCurrentSalesList(string codes, List<ManagerEntity> allList)
        {
            List<ManagerEntity> list = new List<ManagerEntity>();
            if (!string.IsNullOrEmpty(codes) && allList.Count > 0)
            {
                string[] codeList = codes.Split(',');
                foreach (string name in codeList)
                {
                    ManagerEntity entity = allList.Find(p => p.real_name.Equals(name));
                    if (entity != null)
                    {
                        list.Add(entity);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 排除队列
        /// </summary>
        /// <param name="currentList"></param>
        /// <returns></returns>
        private static List<ManagerEntity> getOutSalesList(List<ManagerEntity> currentList)
        {
            List<ManagerEntity> removeList = new List<ManagerEntity>();
            string OutSalesCodes = GetOutSalesName();//排除队列
            string wxCodes = BaseDataService.GetCodeValuesByRule("WXChartList").CodeValues;//当天在微信队列中的排除销售咨询队列
            if (!string.IsNullOrEmpty(OutSalesCodes))
            {
                //排除队列
                foreach (string sale in OutSalesCodes.Split(','))
                {
                    if (!string.IsNullOrEmpty(sale))
                    {
                        for (int i = 0; i < currentList.Count; i++)
                        {
                            if (sale.Equals(currentList[i].real_name))
                            {
                                currentList.Remove(currentList[i]);
                            }
                        }
                    }
                }
            }
            //当天在微信队列中的排除销售咨询队列
            string code = GetWXCode();
            if (!string.IsNullOrEmpty(code))
            {
                for (int i = 0; i < currentList.Count; i++)
                {
                    if (code.Equals(currentList[i].telephone))
                    {
                        currentList.Remove(currentList[i]);
                    }
                }
            }

            return currentList;
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
            ProCityEntity info = StringHelper.getCity(telephone);
            string city = "";
            string province = "";
            if (info != null)
            {
                city = info.city;
                province = info.province;
                if (!string.IsNullOrEmpty(info.city))
                {
                    if (info.city.Equals("北京") || info.city.Equals("天津") || info.city.Equals("廊坊"))
                    {
                        code = "BeiJingSalesQueue";
                    }
                }
            }
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

        /// <summary>
        /// 短信模板
        /// </summary>
        /// <param name="SmsTempletText"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static bool sendSMS(string SmsTempletText, ManagerEntity entity, InquiryInfo info)
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
                    saleName = (mInfo != null ? mInfo.real_name : entity.real_name);
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
        private static InquiryInfo AddInquiry(string telephone, string productID, string sourceform, ManagerEntity mEntity)
        {
            InquiryRepository ir = new InquiryRepository();
            InquiryInfo info = new InquiryInfo();
            info.ProductID = productID;
            info.SourceForm = sourceform;
            info.ProcessingState = "0";
            info.telphone = telephone;
            ProCityEntity pcentity = StringHelper.getCity(telephone);
            if (pcentity != null)
            {
                info.Provence = pcentity.province;
                info.City = pcentity.city;
            }

            List<InquiryEntity> listInquiry = GetInquiryByRule("", "", "", " and (telphone='" + telephone + "' or telphone='" + StringHelper.ConvertBy123(telephone) + "') ", "", "");
            if (listInquiry != null && listInquiry.Count > 0)
            {
                InquiryEntity entity = listInquiry[0];
                info.HistoryOperatorID = entity.OperatorID;
                info.OperatorID = entity.OperatorID;
                info.SaleTelephone = entity.SaleTelephone;
                info.smsMess = "老客户咨询！";
                info.status = "";
            }
            else
            {
                info.status = "新";
                info.HistoryOperatorID = mEntity.id.ToString();
                info.OperatorID = mEntity.id.ToString();
                info.SaleTelephone = mEntity.telephone;
            }
            ir.CreateSimpleInquiry(info);
            return info;
        }

        #region 最新获取



        /// <summary>
        /// 获取最终分配销售
        /// </summary>
        /// <param name="listTask"></param>
        /// <returns></returns>
        private static ManagerEntity GetManagerByOperatorID(List<ManagerEntity> listTask, string codes,List<ManagerEntity> allList)
        {
            ManagerEntity entity = new ManagerEntity();
            //获取当前队列中最新资讯销售
            string OperatorIDs = "";
            
            //
            List<ManagerEntity> listManager = listTask;

            if (listTask == null || listTask.Count == 0)
            {
                listManager = getCurrentSalesList(codes, allList);
            }

            foreach (ManagerEntity item in listManager)
            {
                OperatorIDs += item.id + ",";
            }
            if (!string.IsNullOrEmpty(OperatorIDs))
            {
                List<DefineInquiryInfo> listLastName = GetLastSaleNameByOperatorID(OperatorIDs.TrimEnd(','));
                string lastOperatorID = listLastName[0].OperatorID;

                int currentindex = listManager.FindIndex(p => p.id == lastOperatorID.ToInt(0));

                int index = currentindex + 1;

                if (index >= listManager.Count)
                {
                    entity = listManager[0];
                }
                else
                {
                    entity = listManager[index];
                }
            }

            return entity;
        }

        #endregion

        #region 超过15分钟未处理，重新分配
        public static void AutoAllocation()
        {
            List<InquiryEntity> list = new List<InquiryEntity>();
            list = GetInquiryByRule("", "", "", " AND datediff(mi,AddDate,GETDATE())>15 AND status='新' and ProcessingState='0' ", "", "");
            if (list != null && list.Count > 0)
            {
                LogHelper.WriteAutoSystemLog("重新分配", JsonHelper.ToJson(list), DateTime.Now);
                string SmsTempletText = BaseDataService.GetCodeValuesByRule("SmsTemplate").CodeValues;//短信模板
                foreach (InquiryEntity item in list)
                {
                    string code = GetTimeRangleCode(item.telphone);
                    ManagerEntity entity = GetSalesNameNew(code);
                    sendSMS(SmsTempletText, item.ProductID, "超时转移", entity);

                    //更新OperatorID
                    InquiryRepository ir = new InquiryRepository();
                    InquiryInfo infoU = new InquiryInfo();
                    infoU.OperatorID = entity.id.ToString();
                    infoU.PPId = item.PPId;
                    infoU.ChangeDate = DateTime.Now;
                    ir.UpdateOperatorIDByPPId(infoU);
                }
            }
        }
        #endregion


        

        /// <summary>
        /// 队列中最新资讯
        /// </summary>
        /// <param name="OperatorID"></param>
        /// <returns></returns>
        public static List<DefineInquiryInfo> GetLastSaleNameByOperatorID(string OperatorID)
        {
            InquiryRepository mr = new InquiryRepository();
            return mr.GetLastSaleNameByOperatorID(OperatorID);
        }        

        

        /// <summary>
        /// 获取当天的微信号
        /// </summary>
        /// <returns></returns>
        private static string GetWXCode()
        {
            DateTime dt = DateTime.Now;
            int week = Convert.ToInt32(dt.DayOfWeek);
            string wxCodes = BaseDataService.GetCodeValuesByRule("WXChartList").CodeValues;
            string code = "";
            try
            {
                if (!string.IsNullOrEmpty(wxCodes))
                {
                    week = week == 0 ? 7 : week;
                    string[] codeStr = wxCodes.Split(',');
                    if (codeStr.Length < week)
                    {
                        code = codeStr[0];
                    }
                    else
                    {
                        int cou = week - 1;
                        code = codeStr[cou];
                    }
                }
            }
            catch (Exception ex)
            {
                code = wxCodes.Split(',')[0];
            }
            return code;
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

        private static InquiryEntity TranslateInquiryEntity(InquiryInfo info, List<ManagerEntity> listManager = null)
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
            if (listManager != null && listManager.Count > 0)
            {
                ManagerEntity mEntity = listManager.Find(p => p.id.ToString().Equals(entity.OperatorID));
                if (mEntity != null)
                {
                    entity.manager = mEntity;
                }
            }
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

        public static List<InquiryEntity> GetInquiryByRule(string productid, string telephone, string name, string sqlwhere, string status,string operatorid)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetInquiryByRule(productid, telephone, name, sqlwhere, status, operatorid);

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
            List<ManagerEntity> listManager = ManagerService.GetManagerAll();
            foreach (InquiryInfo mInfo in miList)
            {
                InquiryEntity carEntity = TranslateInquiryEntity(mInfo, listManager);
                all.Add(carEntity);
            }
            return all;
        }

        public static List<InquiryEntity> GetInquiryInfoByRule(string name, string tracestate, int status, PagerInfo pager)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetAllInquiryInfoByRule(name, tracestate, status, pager);
            List<ManagerEntity> listManager = ManagerService.GetManagerAll();
            if (!miList.IsEmpty())
            {
                foreach (InquiryInfo mInfo in miList)
                {
                    InquiryEntity storeEntity = TranslateInquiryEntity(mInfo, listManager);
                    all.Add(storeEntity);
                }
            }

            return all;
        }
        #endregion
    }
}
