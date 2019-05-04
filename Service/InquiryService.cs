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
namespace Service
{
    public class InquiryService
    {
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

        public static List<InquiryEntity> GetInquiryByRule(string name, int status)
        {
            List<InquiryEntity> all = new List<InquiryEntity>();
            InquiryRepository mr = new InquiryRepository();
            List<InquiryInfo> miList = mr.GetInquiryByRule(name, status);

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
