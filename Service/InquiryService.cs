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
namespace Service
{
    public class InquiryService
    {
        #region 咨询量逻辑

        public static string CreateInquiry(string Telephone, string productID, string sourceform, string contactName, string wxChartID)
        {

            CreateInuiryLog(productID, Telephone, sourceform);

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
                LogHelper.WriteTextLog("咨询开始","资讯手机号开始" + Telephone, DateTime.Now);
                string code = GetTimeRangleCode(Telephone, "");
                List<InquiryEntity> listInquiry = GetInquiryByRule("", "", "", " and (telphone='" + StringHelper.ConvertBy123(Telephone) + "' or telphone='" + Telephone + "') ", "", "");
                UserEntity entity = null;
                bool isNew = false;
                if(listInquiry!=null&&listInquiry.Count>0)
                {
                    //手机号已经存在咨询 直接从用户表中读取销售信息
                    entity = UserService.GetUserById(listInquiry[0].OperatorID.ToLong(0));
                    isNew = false;
                }
                else
                {
                    isNew = true;
                    //新咨询量 走分配逻辑
                    entity = GetSalesNameNew(code);
                }

                InquiryInfo info = AddInquiry(Telephone, productID, sourceform, contactName, wxChartID, entity, isNew);
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
        private static bool sendSMS(string SmsTempletText, string productID, string smsMess, UserEntity entity)
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
                    LogHelper.WriteTextLog("sendSMS", "--工作手机号 销售：" + entity.Telephone + "-询价-短信内容：" + SmsMess, DateTime.Now);

                    if (!string.IsNullOrEmpty(entity.PrivateTelephone))
                    {
                        SMSHelper.SeedSMS(entity.PrivateTelephone, SmsMess);
                        LogHelper.WriteTextLog("sendSMS", "--私人手机号 销售：" + entity.Telephone + "-询价-短信内容：" + SmsMess, DateTime.Now);
                    }
                    ////发送给老板
                    //SMSHelper.SeedSMS("13916116545", SmsMess);
                    //LogHelper.WriteTextLog("sendSMS", "--手机号 BOSS：" + entity.Telephone + "-询价-短信内容：" + SmsMess, DateTime.Now);
                    //发送城市对应的主管销售人员
                    if (entity.CityName.Equals("厦门"))
                    {
                        SMSHelper.SeedSMS("17359271665", SmsMess);
                        LogHelper.WriteTextLog("sendSMS", "--手机号 厦门：" + entity.Telephone + "-询价-短信内容：" + SmsMess, DateTime.Now);
                    }
                    else if (entity.CityName.Equals("武夷山"))
                    {
                        SMSHelper.SeedSMS("13163806316", SmsMess);
                        LogHelper.WriteTextLog("sendSMS", "--手机号 武夷山：" + entity.Telephone + "-询价-短信内容：" + SmsMess, DateTime.Now);
                    }
                }
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
                LogHelper.WriteTextLog("所有销售队列", JsonHelper.ToJson(allList), DateTime.Now);
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
            list = GetInquiryByRule("", "", "", " AND datediff(mi,AddDate,GETDATE())>15 AND status='新' and ProcessingState='0' ", "", "");
            if (list != null && list.Count > 0)
            {
                LogHelper.WriteAutoSystemLog("重新分配", JsonHelper.ToJson(list), DateTime.Now);
                string SmsTempletText = BaseDataService.GetCodeValuesByRule("SmsTemplate").CodeValues;//短信模板
                foreach (InquiryEntity item in list)
                {
                    string code = GetTimeRangleCode(item.telphone, item.City);
                    UserEntity entity = GetSalesNameNew(code);
                    sendSMS(SmsTempletText, item.ProductID, "超时转移", entity);

                    //更新OperatorID
                    InquiryRepository ir = new InquiryRepository();
                    InquiryInfo infoU = new InquiryInfo();
                    infoU.OperatorID = entity.UserID.ToString();
                    infoU.PPId = item.PPId;
                    infoU.ChangeDate = DateTime.Now;
                    ir.UpdateOperatorIDByPPId(infoU);

                    #region 记录转移日志
                    InquiryMonitorEntity monitorEntity = new InquiryMonitorEntity();
                    monitorEntity.PPId = item.PPId;
                    monitorEntity.ProductID = item.ProductID;
                    monitorEntity.OriginOperatorID = item.OperatorID;
                    monitorEntity.OriginSalesName = item.user.NickName;
                    monitorEntity.NewOperatorID = entity.UserID.ToString();
                    monitorEntity.NewSalesName = entity.NickName;
                    monitorEntity.Remark = "超时自动转移";
                    monitorEntity.CreateDate = DateTime.Now;
                    InquiryMonitorService.Modify(monitorEntity);
                    #endregion                    
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
            entity.TraceState = info.TraceState;
            entity.NextVisitTime = info.NextVisitTime;
            entity.AddDate = info.AddDate;
            entity.OperatorID = info.OperatorID;
            entity.SaleTelephone = info.SaleTelephone;
            entity.status = info.status;
            entity.SourceForm = info.SourceForm;
            entity.user = UserService.GetUserById(info.OperatorID.ToLong(0));
            entity.product = ProductService.GetProductByProductID(info.ProductID);
            entity.colorStyle = StringHelper.getColorStyle(info.TraceState);
            entity.showTelephone = getTelephone(loginUserID, entity.telphone, entity.OperatorID);
            return entity;
        }

        private static string getTelephone(string loginUserID,string telephone,string operatorid)
        {
            if (!string.IsNullOrEmpty(loginUserID))
            {
                if (!loginUserID.Equals(operatorid))
                {
                    return Regex.Replace(telephone, "(\\d{3})\\d{4}(\\d{4})", "$1****$2");
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
                    InquiryInfo.ChangeDate = DateTime.Now;
                    InquiryInfo.AddDate = DateTime.Now;
                    result = mr.CreateNew(InquiryInfo);
                }


                //List<InquiryInfo> miList = mr.GetAllInquiry();//刷新缓存
                //Cache.Add("InquiryALL", miList);
            }
            return result > 0;
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
            return new InquiryRepository().GetInquiryCount(keywords, tracestate, -1, begindate, enddate, operatorid, sqlwhere);
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
    }
}
