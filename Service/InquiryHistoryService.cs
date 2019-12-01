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
    public class InquiryHistoryService
    {

        #region 定时分配释放库数据

        /// <summary>
        /// 定时分配释放库数据
        /// </summary>
        public static void HandHistoryInquiry()
        {
            CodeSEntity entityrq = BaseDataService.GetCodeValuesByRule("HistoryInquiryCodes");
            CodeSEntity entityCount = BaseDataService.GetCodeValuesByRule("HistoryInquiryCount");
            string count = entityCount != null ? entityCount.CodeValues : "10";
            if (entityrq != null)
            {
                string[] list = entityrq.CodeValues.Split(',');
                foreach (string name in list)
                {
                    List<UserEntity> userList = UserService.GetUserByRule("", -1, name);
                    UserEntity userInfo = userList != null && userList.Count > 0 ? userList[0] : null;
                    if (userInfo != null)
                    {
                        List<InquiryHistoryEntity> listAll = GetInquiryAll(count.ToInt(0));
                        if (listAll != null && listAll.Count > 0)
                        {
                            foreach (InquiryHistoryEntity entity in listAll)
                            {
                                entity.OperatorID = userInfo.UserID.ToString();
                                //添加到正式库
                                AddInquiry(entity);
                                //释放库删除
                                Remove(entity.PPId);
                            }
                        }
                    }
                }
            }
        }


        private static InquiryInfo AddInquiry(InquiryHistoryEntity entity)
        {
            InquiryRepository ir = new InquiryRepository();
            InquiryInfo info = new InquiryInfo();
            try
            {
                info.ProductID = entity.ProductID;
                info.SourceForm = entity.SourceForm;
                info.ProcessingState = "0";
                info.telphone = StringHelper.ConvertBy123(entity.telphone);


                info.Provence = entity.Provence;
                info.City = entity.City;
                info.InquiryContent = "释放库转移重新分配";
                info.status = "";
                info.HistoryOperatorID = !string.IsNullOrEmpty(entity.HistoryOperatorID) ? entity.HistoryOperatorID : entity.OperatorID;
                info.OperatorID = entity.OperatorID;
                info.SaleTelephone = entity.SaleTelephone;
                info.CustomerName = entity.CustomerName;
                info.WebChartID = entity.WebChartID;
                ir.CreateSimpleInquiry(info);
            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("AddInquiry", ex.ToString(), DateTime.Now);
            }
            return info;
        }
        #endregion


        public static InquiryHistoryEntity GetInquiryEntityById(long cid)
        {
            InquiryHistoryEntity result = new InquiryHistoryEntity();
            InquiryHistoryRepository mr = new InquiryHistoryRepository();
            InquiryHistoryInfo info = mr.GetInquiryByKey(cid);
            result = TranslateInquiryEntity(info);
            return result;
        }

        private static InquiryHistoryEntity TranslateInquiryEntity(InquiryHistoryInfo info)
        {
            InquiryHistoryEntity entity = new InquiryHistoryEntity();
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
            return entity;
        }

        private static InquiryHistoryInfo TranslateInquiryInfo(InquiryHistoryEntity entity)
        {
            InquiryHistoryInfo info = new InquiryHistoryInfo();
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
        public static bool ModifyInquiry(InquiryHistoryEntity entity)
        {
            long result = 0;
            if (entity != null)
            {
                InquiryHistoryRepository mr = new InquiryHistoryRepository();

                InquiryHistoryInfo InquiryHistoryInfo = TranslateInquiryInfo(entity);

                

                if (entity.PPId > 0)
                {
                    InquiryHistoryInfo.PPId = entity.PPId;
                    InquiryHistoryInfo.ChangeDate = DateTime.Now;
                    result = mr.ModifyInquiry(InquiryHistoryInfo);
                }
                else
                {
                    InquiryHistoryInfo.ChangeDate = DateTime.Now;
                    InquiryHistoryInfo.AddDate = DateTime.Now;
                    result = mr.CreateNew(InquiryHistoryInfo);
                }


                //List<InquiryHistoryInfo> miList = mr.GetAllInquiry();//刷新缓存
                //Cache.Add("InquiryALL", miList);
            }
            return result > 0;
        }

        /// <summary>
        /// 手动添加咨询量
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="user"></param>
        public static void CreateSimple(InquiryHistoryEntity entity, UserEntity user)
        {
            InquiryHistoryRepository ir = new InquiryHistoryRepository();
            InquiryHistoryInfo info = new InquiryHistoryInfo();
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
            ir.CreateSimpleInquiry(info);
        }

        public static InquiryHistoryEntity GetInquiryById(long gid)
        {
            InquiryHistoryEntity result = new InquiryHistoryEntity();
            InquiryHistoryRepository mr = new InquiryHistoryRepository();
            InquiryHistoryInfo info = mr.GetInquiryByKey(gid);
            result = TranslateInquiryEntity(info);
            return result;
        }

        public static List<InquiryHistoryEntity> GetInquiryAll(int count)
        {
            List<InquiryHistoryEntity> all = new List<InquiryHistoryEntity>();
            InquiryHistoryRepository mr = new InquiryHistoryRepository();
            List<InquiryHistoryInfo> miList = mr.GetAllInquiry(count);//Cache.Get<List<InquiryHistoryInfo>>("InquiryALL");
            //if (miList.IsEmpty())
            //{
            //    miList = mr.GetAllInquiry();
            //    Cache.Add("InquiryALL", miList);
            //}
            if (!miList.IsEmpty())
            {
                foreach (InquiryHistoryInfo mInfo in miList)
                {
                    InquiryHistoryEntity InquiryHistoryEntity = TranslateInquiryEntity(mInfo);
                    all.Add(InquiryHistoryEntity);
                }
            }

            return all;

        }

        public static List<InquiryHistoryEntity> GetInquiryByRule(string productid, string Telephone, string name, string sqlwhere, string status,string operatorid)
        {
            List<InquiryHistoryEntity> all = new List<InquiryHistoryEntity>();
            InquiryHistoryRepository mr = new InquiryHistoryRepository();
            List<InquiryHistoryInfo> miList = mr.GetInquiryByRule(productid, Telephone, name, sqlwhere, status, operatorid);

            if (!miList.IsEmpty())
            {
                foreach (InquiryHistoryInfo mInfo in miList)
                {
                    InquiryHistoryEntity InquiryHistoryEntity = TranslateInquiryEntity(mInfo);
                    all.Add(InquiryHistoryEntity);
                }
            }

            return all;
        }

        public static List<InquiryHistoryEntity> GetInquiryByKeys(string ids)
        {
            List<InquiryHistoryEntity> all = new List<InquiryHistoryEntity>();
            InquiryHistoryRepository mr = new InquiryHistoryRepository();
            List<InquiryHistoryInfo> miList = mr.GetInquiryByKeys(ids);

            if (!miList.IsEmpty())
            {
                foreach (InquiryHistoryInfo mInfo in miList)
                {
                    InquiryHistoryEntity entity = TranslateInquiryEntity(mInfo);
                    all.Add(entity);
                }
            }

            return all;
        }

        public static int Remove(long gid)
        {
            InquiryHistoryRepository mr = new InquiryHistoryRepository();
            int i = mr.Remove(gid);
            //List<InquiryHistoryInfo> miList = mr.GetAllInquiry();//刷新缓存
            //Cache.Add("InquiryALL", miList);
            return i;
        }

        #region 分页相关
        public static int GetInquiryCount(string keywords, string tracestate, int status, string begindate, string enddate, string operatorid)
        {
            return new InquiryHistoryRepository().GetInquiryCount(keywords, tracestate, -1, begindate, enddate, operatorid);
        }

        public static List<InquiryHistoryEntity> GetInquiryInfoPager(PagerInfo pager)
        {
            List<InquiryHistoryEntity> all = new List<InquiryHistoryEntity>();
            InquiryHistoryRepository mr = new InquiryHistoryRepository();
            List<InquiryHistoryInfo> miList = mr.GetAllInquiryInfoPager(pager);
            foreach (InquiryHistoryInfo mInfo in miList)
            {
                InquiryHistoryEntity carEntity = TranslateInquiryEntity(mInfo);
                all.Add(carEntity);
            }
            return all;
        }

        public static List<InquiryHistoryEntity> GetInquiryInfoByRule(string keywords, string tracestate, int status, string begindate, string enddate, string operatorid, PagerInfo pager)
        {
            List<InquiryHistoryEntity> all = new List<InquiryHistoryEntity>();
            InquiryHistoryRepository mr = new InquiryHistoryRepository();
            List<InquiryHistoryInfo> miList = mr.GetAllInquiryInfoByRule(keywords, tracestate, status,begindate,enddate, operatorid, pager);

            if (!miList.IsEmpty())
            {
                foreach (InquiryHistoryInfo mInfo in miList)
                {
                    InquiryHistoryEntity storeEntity = TranslateInquiryEntity(mInfo);
                    all.Add(storeEntity);
                }
            }

            return all;
        }
        #endregion
    }
}
