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
using System.Text.RegularExpressions;
using System.Data;
namespace Service
{
    public class InquiryService
    {
        #region 咨询量逻辑

        public static string CreateInquiry(string Telephone, string productID, string sourceform, string contactName, string wxChartID)
        {

            StringBuilder log = new StringBuilder();

            CreateInuiryLog(productID, Telephone, sourceform);
            //LogHelper.WriteTextLog("咨询手机号", "Tel:" + Telephone, DateTime.Now);
            log.Append("咨询手机号:" + Telephone + "ProductID:" + productID + "SourceForm:" + sourceform);
            log.Append("\r\n");

            string SmsTempletText = BaseDataService.GetCodeValuesByRule("SmsTemplate").CodeValues;//短信模板
            CodeSEntity blackmobile = BaseDataService.GetCodeValuesByRule("BlackMobile");//手机号黑名单
            //当天同手机号同产品编号只能资讯2次
            List<InquiryEntity> listProTel = GetInquiryByRule(productID, StringHelper.ConvertBy123(Telephone), "", "and AddDate between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59' ", "", "");

            //手机号大于5次 同一天
            List<InquiryEntity> listTel = GetInquiryByRule("", StringHelper.ConvertBy123(Telephone), "", "and AddDate between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59' ", "", "");
            if (blackmobile != null && !string.IsNullOrEmpty(blackmobile.CodeValues) && blackmobile.CodeValues.Contains(Telephone))
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
                //LogHelper.WriteTextLog("咨询开始","资讯手机号开始" + Telephone, DateTime.Now);
                log.Append("咨询手机号:" + "资讯手机号开始:" + Telephone);
                log.Append("\r\n");
                string code = GetTimeRangleCode(Telephone, "");
                List<InquiryEntity> listInquiry = GetInquiryByRule("", "", "", " and (telphone='" + StringHelper.ConvertBy123(Telephone) + "' or telphone='" + Telephone + "') ", "", "");
                UserEntity entity = null;
                bool isNew = false;
                if(listInquiry!=null&&listInquiry.Count>0)
                {
                    log.Append("咨询手机号:" + "手机号已经存在，咨询条数：" + listInquiry.Count);
                    log.Append("\r\n");
                    //手机号已经存在咨询 直接从用户表中读取销售信息
                    entity = UserService.GetUserById(listInquiry[0].OperatorID.ToLong(0));
                    isNew = false;
                }
                else
                {
                    isNew = true;
                    log.Append("咨询手机号:" + "新咨询量");
                    log.Append("\r\n");
                    //新咨询量 走分配逻辑
                    entity = GetSalesNameNew(code);
                }

                InquiryInfo info = AddInquiry(Telephone, productID, sourceform, contactName, wxChartID, entity, isNew);
                log.Append("咨询手机号:" + JsonHelper.ToJson(info));
                log.Append("\r\n");
                sendSMS(SmsTempletText, productID, info.smsMess, entity, log);
            }
            return "";
        }

        /// <summary>
        /// 短信模板
        /// </summary>
        /// <param name="SmsTempletText"></param>
        /// <param name="info"></param>
        /// <returns></returns>
        private static bool sendSMS(string SmsTempletText, string productID, string smsMess, UserEntity entity, StringBuilder log)
        {
            string SmsMess = string.Format(SmsTempletText, "不详", "不详", "不详", DateTime.Now.ToString());

            try
            {
                if (!string.IsNullOrEmpty(productID))
                {
                    ProductEntity proEntity = ProductService.GetProductByProductID(productID);
                    SmsMess = string.Format(SmsTempletText, proEntity.Author, proEntity.ProductName, proEntity.ProductID, DateTime.Now.ToString()) + smsMess;
                    if (entity != null)
                    {
                        SmsMess += "跟踪销售：" + entity.NickName;
                    }
                    SmsMess = SmsMess.Replace("-", "-");

                    SMSHelper.SeedSMS(entity.Telephone, SmsMess);
                    //LogHelper.WriteTextLog("sendSMS", "--工作手机号 销售：" + entity.Telephone + "-询价-短信内容：" + SmsMess, DateTime.Now);
                    log.Append("短信发送："+"--工作手机号 销售：" + entity.Telephone + "-询价-短信内容：" + SmsMess);
                    log.Append("\r\n");
                    if (!string.IsNullOrEmpty(entity.PrivateTelephone))
                    {
                        SMSHelper.SeedSMS(entity.PrivateTelephone, SmsMess);
                        //LogHelper.WriteTextLog("sendSMS", "--私人手机号 销售：" + entity.PrivateTelephone + "-询价-短信内容：" + SmsMess, DateTime.Now);
                        log.Append("短信发送：" + "--私人手机号 销售：" + entity.PrivateTelephone + "-询价-短信内容：" + SmsMess);
                        log.Append("\r\n");
                    }
                    ////发送给老板
                    //SMSHelper.SeedSMS("13916116545", SmsMess);
                    //LogHelper.WriteTextLog("sendSMS", "--手机号 BOSS：" + entity.Telephone + "-询价-短信内容：" + SmsMess, DateTime.Now);
                    //发送城市对应的主管销售人员
                    if (entity.CityName.Equals("厦门"))
                    {
                        SMSHelper.SeedSMS("17359271665", SmsMess);
                        //LogHelper.WriteTextLog("sendSMS", "--手机号 厦门：17359271665-询价-短信内容：" + SmsMess, DateTime.Now);
                        log.Append("短信发送：" + "--手机号 厦门：17359271665-询价-短信内容：" + SmsMess);
                        log.Append("\r\n");
                    }
                    else if (entity.CityName.Equals("武夷山"))
                    {
                        SMSHelper.SeedSMS("13163806316", SmsMess);
                        log.Append("短信发送：" + "--手机号 武夷山：13163806316-询价-短信内容：" + SmsMess);
                        log.Append("\r\n");
                        //LogHelper.WriteTextLog("sendSMS", "--手机号 武夷山：13163806316-询价-短信内容：" + SmsMess, DateTime.Now);
                    }
                }
                LogHelper.WriteTextLog("咨询日志记录", log.ToString(), DateTime.Now);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("sendSMS", ex.ToString(), DateTime.Now);
            }
            return true;
        }        

        public static UserEntity GetSalesNameNew(string code)
        {
            ManagerRepository mmr = new ManagerRepository();
            UserEntity entity = new UserEntity();
            try
            {
                List<UserEntity> allList = UserService.GetUserAll(false);
                //LogHelper.WriteTextLog("所有销售队列", JsonHelper.ToJson(allList), DateTime.Now);
                //当前销售队列
                string codes = BaseDataService.GetCodeValuesByRule(code).CodeValues;
                //当前销售队列
                List<UserEntity> listCurrent = getCurrentSalesListQuence(codes, allList);
                //执行排除队列
                List<UserEntity> listTask = getOutSalesList(listCurrent);
                //获取最终销售
                entity = GetManagerByOperatorID(listTask);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("GetSalesNameNew", ex.ToString(), DateTime.Now);    
            }

            return entity;
        }
        /// <summary>
        /// 当前队列所有信息
        /// </summary>
        /// <returns></returns>
        private static List<UserEntity> getCurrentSalesListQuence(string codes, List<UserEntity> allList)
        {
            List<UserEntity> list = new List<UserEntity>();
            if (!string.IsNullOrEmpty(codes) && allList.Count > 0)
            {
                string[] codeList = codes.Split(',');
                foreach (string name in codeList)
                {
                    UserEntity entity = allList.Find(p => p.NickName.Equals(name) && (p.SalesCount > p.currentSalesCount || p.currentSalesCount == 0));
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
                        UserEntity entity = allList.Find(p => p.NickName.Equals(name));
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
        /// 排除队列
        /// </summary>
        /// <param name="currentList"></param>
        /// <returns></returns>
        private static List<UserEntity> getOutSalesList(List<UserEntity> currentList)
        {
            List<UserEntity> oldList = new List<UserEntity>();
            if (currentList != null && currentList.Count > 0)
            {
                foreach (UserEntity entity in currentList)
                {
                    oldList.Add(entity);
                }
            }

            List<UserEntity> removeList = new List<UserEntity>();
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
                            if (sale.Equals(currentList[i].NickName))
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
                    if (code.Equals(currentList[i].Telephone))
                    {
                        currentList.Remove(currentList[i]);
                    }
                }
            }

            return currentList != null && currentList.Count > 0 ? currentList : oldList;
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
        public static string GetTimeRangleCode(string Telephone, string city)
        {
            string code = "";
            try
            {
                DateTime dtNow = Convert.ToDateTime(DateTime.Now.ToShortTimeString());

                //ASalesQueue 凌晨2点到12点分分配队列
                //BSalesQueue 12点~13点30分分配队列
                //CSalesQueue 13点30分~18点30分分配队列
                //DSalesQueue 18点30分~21点30分分配队列
                //ESalesQueue 21点30分~凌晨2点分配队列
                //班次分配
                //ASalesQueue 1）凌晨班：凌晨1点016到早上5点30
                //BSalesQueue 2）早班：5点31~9点30，这
                //CSalesQueue 3）上午班：9点31~12点
                //DSalesQueue 4）午睡班：12点01~14点
                //ESalesQueue 5）下午班：14点01~18点
                //FSalesQueue 6）晚班：18:01到21点45
                //GSalesQueue 7）夜班：21点46到凌晨1点
                string datetime = DateTime.Now.AddDays(-1).ToShortDateString();
                //string sqlTime = "";
                if (dtNow.CompareTo(Convert.ToDateTime("01:16")) > 0 && dtNow.CompareTo(Convert.ToDateTime("05:30")) < 0)
                {
                    //sqlTime = " and AddDate between '" + datetime + " 01:16' and '" + datetime + " 05:30'";
                    code = "ASalesQueue";
                }
                else if (dtNow.CompareTo(Convert.ToDateTime("05:31")) > 0 && dtNow.CompareTo(Convert.ToDateTime("09:30")) < 0)
                {
                    //sqlTime = " and AddDate between '" + datetime + " 05:31' and '" + datetime + " 09:30'";
                    code = "BSalesQueue";
                }
                else if (dtNow.CompareTo(Convert.ToDateTime("09:31")) > 0 && dtNow.CompareTo(Convert.ToDateTime("12:00")) < 0)
                {
                    //sqlTime = " and AddDate between '" + datetime + " 09:31' and '" + datetime + " 12:00'";
                    code = "CSalesQueue";
                }
                else if (dtNow.CompareTo(Convert.ToDateTime("12:01")) > 0 && dtNow.CompareTo(Convert.ToDateTime("14:00")) < 0)
                {
                    //sqlTime = " and AddDate between '" + datetime + " 12:01' and '" + datetime + " 14:00'";
                    code = "DSalesQueue";
                }
                else if (dtNow.CompareTo(Convert.ToDateTime("14:01")) > 0 && dtNow.CompareTo(Convert.ToDateTime("18:00")) < 0)
                {
                    //sqlTime = " and AddDate between '" + datetime + " 14:01' and '" + datetime + " 18:00'";
                    code = "ESalesQueue";
                }
                else if (dtNow.CompareTo(Convert.ToDateTime("18:01")) > 0 && dtNow.CompareTo(Convert.ToDateTime("21:45")) < 0)
                {
                    //sqlTime = " and AddDate between '" + datetime + " 18:01' and '" + datetime + " 21:45'";
                    code = "FSalesQueue";
                }
                else if (dtNow.CompareTo(Convert.ToDateTime("21:46")) > 0 && dtNow.CompareTo(Convert.ToDateTime(DateTime.Now.AddDays(1).ToShortDateString() + " 01:15")) < 0)
                {
                    //sqlTime = " and AddDate between '" + datetime + " 21:46' and '" + datetime + " 01:15'";
                    code = "GSalesQueue";
                }

                if (string.IsNullOrEmpty(code))
                {
                    code = "GSalesQueue";
                }
                LogHelper.WriteTextLog("GetTimeRangleCode", "CODE:" + code, DateTime.Now);
                #region 城市信息优先级最高
                ProCityEntity info = StringHelper.getProCityInfo();//StringHelper.getCity(Telephone);
                String CityName = "";
                if (info != null || !string.IsNullOrEmpty(city))
                {
                    if (!string.IsNullOrEmpty(info.city) || !string.IsNullOrEmpty(city))
                    {
                        CityName = !string.IsNullOrEmpty(city) ? city : info.city;

                        if (CityName.Contains("北京") || CityName.Contains("廊坊"))//|| CityName.Contains("天津")
                        {
                            code = "BeiJingSalesQueue";
                        }
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("GetTimeRangleCode", ex.ToString(), DateTime.Now);
            }
            return code;
        }

        /// <summary>
        /// 获取当前用户ID 手机号去重数据
        /// </summary>
        /// <param name="operatorid"></param>
        /// <param name="traceState"></param>
        /// <returns></returns>
        public static int GetDistinctTelephone(string keywords, string tracestate, int status, string begindate, string enddate, string operatorid, string sqlwhere)
        {
            InquiryRepository mr = new InquiryRepository();
            try
            {
                DataSet ds = mr.GetDistinctTelephone(keywords, tracestate, status, begindate, enddate, operatorid, sqlwhere);
                if (ds != null && ds.Tables[0] != null && ds.Tables.Count > 0)
                {
                    return Convert.ToInt32(ds.Tables[0].Rows.Count);
                }
            }
            catch (Exception ex)
            {

            }
            return 0;
        }

        public static List<DefineInquiryInfo> GetLastSaleNameBySaleName(string salenames)
        {
            InquiryRepository mr = new InquiryRepository();
            return mr.GetLastSaleName(salenames);
        }


        /// <summary>
        /// 资讯日志流水添加
        /// </summary>
        /// <param name="productID"></param>
        /// <param name="telephone"></param>
        /// <param name="sourceform"></param>
        public static void CreateInuiryLog(string productID,string telephone,string sourceform)
        {
            try
            {
                InquiryRepository mr = new InquiryRepository();
                InquiryLogInfo info=new InquiryLogInfo();
                info.ProductID = productID;
                info.Telephone = telephone;
                info.JMTelephone = StringHelper.ConvertBy123(telephone);
                info.SourceForm = sourceform;
                info.CreateDate = DateTime.Now;
                mr.CreateInuiryLog(info);
            }
            catch (Exception ex)
            {
                
            }
        }


        public static int IntoHistoryInquiry(string ppids)
        {
            InquiryRepository mr = new InquiryRepository();
            return mr.IntoHistoryInquiry(ppids);
        }


        public static List<DefineInquiryInfo> GetLastSaleNameByCodes(string salenames)
        {
            InquiryRepository mr = new InquiryRepository();
            return mr.GetLastSaleNameByCodes(salenames);
        }

        /// <summary>
        /// 添加咨询量到数据表
        /// </summary>
        /// <param name="Telephone"></param>
        /// <param name="productID"></param>
        /// <param name="sourceform"></param>
        /// <param name="realSaleName"></param>
        /// <returns></returns>
        private static InquiryInfo AddInquiry(string Telephone, string productID, string sourceform, string contactName, string wxChartID, UserEntity entity,bool isNew)
        {
            InquiryRepository ir = new InquiryRepository();
            InquiryInfo info = new InquiryInfo();
            try
            {
                info.ProductID = productID;
                info.SourceForm = sourceform;
                info.ProcessingState = "0";
                info.telphone = StringHelper.ConvertBy123(Telephone);
                ProCityEntity pcentity = StringHelper.getProCityInfo();
                if (pcentity != null)
                {
                    info.IpAddress = pcentity.IpAddress;
                    info.Provence = pcentity.province;
                    info.City = pcentity.city;
                }
                if (isNew)
                {
                    info.status = "新";
                }
                else
                {
                    info.smsMess = "老客户咨询！";
                    info.status = "";
                }
                info.HistoryOperatorID = entity.UserID.ToString();
                info.OperatorID = entity.UserID.ToString();
                info.SaleTelephone = entity.Telephone;
                info.CustomerName = contactName;
                info.WebChartID = wxChartID;
                info.AddDate = DateTime.Now;
                ir.CreateSimpleInquiry(info);
            }
            catch (Exception ex)
            {
                 LogHelper.WriteErrorLog("AddInquiry", ex.ToString(), DateTime.Now);   
            }
            return info;
        }

        #region 最新获取



        /// <summary>
        /// 获取最终分配销售
        /// </summary>
        /// <param name="listTask"></param>
        /// <returns></returns>
        private static UserEntity GetManagerByOperatorID(List<UserEntity> listTask)
        {
            UserEntity entity = new UserEntity();
            //获取当前队列中最新资讯销售
            string OperatorIDs = "";
            foreach (UserEntity item in listTask)
            {
                OperatorIDs += item.UserID + ",";
            }
            List<DefineInquiryInfo> listLastName = GetLastSaleNameByOperatorID(OperatorIDs.TrimEnd(','));
            string lastOperatorID = listLastName[0].OperatorID;

            int currentindex = listTask.FindIndex(p => p.UserID == lastOperatorID.ToInt(0));

            int index = currentindex + 1;

            if (index >= listTask.Count)
            {
                entity = listTask[0];
            }
            else
            {
                entity = listTask[index];
            }

            return entity;
        }

        #endregion

        #region 超过15分钟未处理，重新分配
        public static void AutoAllocation()
        {
            List<InquiryEntity> list = new List<InquiryEntity>();
            InquiryRepository ir = new InquiryRepository();
            StringBuilder log=new StringBuilder();
            list = GetInquiryByRule("", "", "", " AND datediff(mi,AddDate,GETDATE())>15 AND status='新' and ProcessingState='0' ", "", "");
            if (list != null && list.Count > 0)
            {
                //LogHelper.WriteAutoSystemLog("重新分配", "待分配数量：" + list.Count, DateTime.Now);
                log.Append("超时转移："+ "待分配数量：" + list.Count);
                log.Append("\r\n");
                string SmsTempletText = BaseDataService.GetCodeValuesByRule("SmsTemplate").CodeValues;//短信模板
                foreach (InquiryEntity item in list)
                {
                    try
                    {
                        log.Append("超时转移："+ "ProductID：" + item.ProductID + "Adddate:" + item.AddDate + "Telphone:" + item.telphone);
                        log.Append("\r\n");
                        
                        //LogHelper.WriteAutoSystemLog("重新分配", "ProductID：" + item.ProductID + "Adddate:" + item.AddDate + "Telphone:" + item.telphone, DateTime.Now);
                        string code = GetTimeRangleCode(item.telphone, item.City);
                        UserEntity entity = GetSalesNameNew(code);


                        //该手机下的资讯量 转移处理
                        List<InquiryEntity> inquiryList = GetInquiryByRule("", StringHelper.ConvertBy123(item.telphone), "", "", "", "");
                        if (inquiryList != null && inquiryList.Count > 0)
                        {
                            foreach (InquiryEntity ientity in inquiryList)
                            {
                                //更新OperatorID
                                InquiryInfo infoUp = new InquiryInfo();
                                infoUp.OperatorID = entity.UserID.ToString();
                                infoUp.PPId = ientity.PPId;
                                infoUp.ChangeDate = DateTime.Now;
                                ir.UpdateOperatorIDByPPId(infoUp);


                                #region 记录转移日志
                                InquiryMonitorEntity monitorEntity = new InquiryMonitorEntity();
                                monitorEntity.PPId = ientity.PPId;
                                monitorEntity.ProductID = ientity.ProductID;
                                monitorEntity.OriginOperatorID = ientity.OperatorID;
                                monitorEntity.OriginSalesName = ientity.user.NickName;
                                monitorEntity.NewOperatorID = entity.UserID.ToString();
                                monitorEntity.NewSalesName = entity.NickName;
                                monitorEntity.Remark = "超时自动转移";
                                monitorEntity.CreateDate = DateTime.Now;
                                InquiryMonitorService.Modify(monitorEntity);
                                #endregion    
                            }
                        }

                        sendSMS(SmsTempletText, item.ProductID, "超时转移", entity, log);

                        //InquiryInfo infoU = new InquiryInfo();
                        //infoU.OperatorID = entity.UserID.ToString();
                        //infoU.PPId = item.PPId;
                        //infoU.ChangeDate = DateTime.Now;
                        //ir.UpdateOperatorIDByPPId(infoU);

                                     
                    }
                    catch (Exception ex)
                    {
                        LogHelper.WriteErrorLog("AutoAllocation", ex.ToString(), DateTime.Now);
                    }
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

        public static InquiryEntity GetInquiryEntityById(long cid,string userid)
        {
            InquiryEntity result = new InquiryEntity();
            InquiryRepository mr = new InquiryRepository();
            InquiryInfo info = mr.GetInquiryByKey(cid);
            result = TranslateInquiryEntity(info,userid);
            return result;
        }

        private static InquiryEntity TranslateInquiryEntity(InquiryInfo info, string loginUserID = "")
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
            entity.TraceState = !string.IsNullOrEmpty(info.TraceState) ? info.TraceState : "未处理";
            entity.NextVisitTime = info.NextVisitTime;
            entity.AddDate = info.AddDate;
            entity.OperatorID = info.OperatorID;
            entity.SaleTelephone = info.SaleTelephone;
            entity.status = info.status;
            entity.SourceForm = info.SourceForm;
            entity.user = UserService.GetUserById(info.OperatorID.ToLong(0));
            entity.product = ProductService.GetProductByProductID(info.ProductID);
            entity.colorStyle = info.ProcessingState.Equals("0") ? "background:#ff7308;" : StringHelper.getColorStyle(info.TraceState);
            entity.mobileColorStyle = info.ProcessingState.Equals("0") ? "background:#ff7308;" : StringHelper.getMobileColorStyle(info.TraceState);
            entity.showTelephone = getTelephone(loginUserID, entity.telphone, entity.OperatorID);
            return entity;
        }

        private static string getTelephone(string loginUserID,string telephone,string operatorid)
        {
            if (!string.IsNullOrEmpty(loginUserID))
            {
                if (!loginUserID.Equals(operatorid))
                {
                    return Regex.Replace(telephone, "(\\d{2})\\d{4}(\\d{4})", "$1*****$2");
                }
                
            }
            return telephone;
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

        /// <summary>
        /// 保存更新咨询量
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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
                    InquiryInfo.telphone = StringHelper.ConvertBy123(InquiryInfo.telphone);
                    InquiryInfo.ProcessingTime = DateTime.Now;
                    InquiryInfo.ChangeDate = DateTime.Now;
                    InquiryInfo.AddDate = DateTime.Now;
                    result = mr.CreateSimpleInquiry(InquiryInfo);
                }


                //List<InquiryInfo> miList = mr.GetAllInquiry();//刷新缓存
                //Cache.Add("InquiryALL", miList);
            }
            return result > 0;
        }


        public static void ModifyInquiryStatus(InquiryEntity entity)
        {
            InquiryRepository mr = new InquiryRepository();
            InquiryInfo info=new InquiryInfo();
            info.PPId = entity.PPId;
            info.status = entity.status;
            info.CommentContent = entity.CommentContent;
            mr.ModifyInquiryStatus(info);
        }
        /// <summary>
        /// 手动添加咨询量
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        public static void CreateSimple(InquiryEntity entity, UserEntity user)
        {
            InquiryRepository ir = new InquiryRepository();
            InquiryInfo info = new InquiryInfo();
            info.ProductID = entity.ProductID;
            info.SourceForm = "HD";
            info.ProcessingState = "1";//已处理
            info.telphone = StringHelper.ConvertBy123(entity.telphone);

            info.Provence = "";
            info.City = "";
            info.TraceState = entity.TraceState;
            info.status = "Hand";
            info.HistoryOperatorID = user.UserID.ToString();
            info.OperatorID = user.UserID.ToString();
            info.SaleTelephone = user.Telephone;
            info.CustomerName = entity.CustomerName;
            info.AddDate = DateTime.Now;
            ir.CreateSimpleInquiry(info);
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

        public static List<InquiryEntity> GetInquiryByRule(string productid, string Telephone, string name, string sqlwhere, string status,string operatorid)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetInquiryByRule(productid, Telephone, name, sqlwhere, status, operatorid);

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
        public static int GetInquiryCount(string keywords, string tracestate, int status, string begindate, string enddate, string operatorid,string sqlwhere="")
        {
            return new InquiryRepository().GetInquiryCount(keywords, tracestate, status, begindate, enddate, operatorid, sqlwhere);
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

        public static List<InquiryEntity> GetInquiryInfoByRule(string keywords, string tracestate, int status, string begindate, string enddate, string operatorid,string userID,string sqlwhere, PagerInfo pager)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetAllInquiryInfoByRule(keywords, tracestate, status, begindate, enddate, operatorid, sqlwhere, pager);

            if (!miList.IsEmpty())
            {
                foreach (InquiryInfo mInfo in miList)
                {
                    InquiryEntity storeEntity = TranslateInquiryEntity(mInfo, userID);
                    all.Add(storeEntity);
                }
            }

            return all;
        }
        #endregion

        #region 咨询量导入
        public static int InquiryImportData(DataTable dt)
        {
            int count = 0;
            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    InquiryImportDataEntity entity = new InquiryImportDataEntity();
                    if (!string.IsNullOrEmpty(dr["日期"].ToString()))
                    {
                        entity.date = Convert.ToDateTime(dr["日期"].ToString()).ToShortDateString();
                        entity.time = Convert.ToDateTime(dr["时间"].ToString()).ToShortTimeString();
                        entity.telephone = dr["手机号"].ToString();
                        entity.productID = dr["作品编号"].ToString();

                        List<InquiryEntity> inquiryList = GetInquiryByRule("", "", "", "  and (telphone='" + StringHelper.ConvertBy123(entity.telephone) + "' or telphone='" + entity.telephone + "')  ", "", "");
                        if (inquiryList == null || inquiryList.Count == 0)
                        {
                            AddInquiry(entity.telephone, entity.productID, entity.date + " " + entity.time);
                            count = count + 1;
                        }
                    }
                }
            }
            return count;
        }

        private static InquiryInfo AddInquiry(string Telephone, string productID, string adddate)
        {
            InquiryRepository ir = new InquiryRepository();
            InquiryInfo info = new InquiryInfo();
            try
            {
                info.ProductID = productID;
                info.SourceForm = "MB";
                info.ProcessingState = "0";
                info.InquiryContent = "91import";
                info.telphone = StringHelper.ConvertBy123(Telephone);
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
        #endregion
    }
}
