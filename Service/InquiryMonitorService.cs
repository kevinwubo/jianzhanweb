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
    public class InquiryMonitorService
    {
        /// <summary>
        /// 添加 更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool Modify(InquiryMonitorEntity entity)
        {
            long result = 0;
            if (entity != null)
            {
                InquiryMonitorRepository mr = new InquiryMonitorRepository();
                InquiryMonitorInfo info = TranslateInquiryEntity(entity);
                if (info.ID > 0)
                {
                    //result = mr.Modify(info);
                }
                else
                {
                    result = mr.CreateNew(info);
                }
            }
            return result > 0;
        }

        private static InquiryMonitorEntity TranslateInquiryMonitorEntity(InquiryMonitorInfo info)
        {
            InquiryMonitorEntity entity = new InquiryMonitorEntity();
            entity.ID = info.ID;
            entity.PPId = info.PPId;
            entity.ProductID = info.ProductID;
            entity.OriginOperatorID = info.OriginOperatorID;
            entity.OriginSalesName = info.OriginSalesName;
            entity.NewOperatorID = info.NewOperatorID;
            entity.NewSalesName = info.NewSalesName;
            entity.Remark = info.Remark;
            entity.CreateDate = info.CreateDate;
            return entity;
        }

        private static InquiryMonitorInfo TranslateInquiryEntity(InquiryMonitorEntity entity)
        {
            InquiryMonitorInfo info = new InquiryMonitorInfo();
            info.ID = entity.ID;
            info.PPId = entity.PPId;
            info.ProductID = entity.ProductID;
            info.OriginOperatorID = entity.OriginOperatorID;
            info.OriginSalesName = entity.OriginSalesName;
            info.NewOperatorID = entity.NewOperatorID;
            info.NewSalesName = entity.NewSalesName;
            info.Remark = entity.Remark;
            info.CreateDate = entity.CreateDate;
            return info;
        }


        #region 分页相关
        public static int GetInquiryCount()
        {
            return new InquiryMonitorRepository().GetInquiryMonitorCount();
        }

        public static List<InquiryMonitorEntity> GetInquiryInfoPager(PagerInfo pager)
        {
            List<InquiryMonitorEntity> all = new List<InquiryMonitorEntity>();
            InquiryMonitorRepository mr = new InquiryMonitorRepository();
            List<InquiryMonitorInfo> miList = mr.GetAllInquiryMonitorInfoPager(pager);
            foreach (InquiryMonitorInfo mInfo in miList)
            {
                InquiryMonitorEntity carEntity = TranslateInquiryMonitorEntity(mInfo);
                all.Add(carEntity);
            }
            return all;
        }



        public static List<InquiryMonitorEntity> GetInquiryInfoByRule(PagerInfo pager)
        {
            List<InquiryMonitorEntity> all = new List<InquiryMonitorEntity>();
            InquiryMonitorRepository mr = new InquiryMonitorRepository();
            List<InquiryMonitorInfo> miList = mr.GetAllInquiryMonitorInfoByRule(pager);

            if (!miList.IsEmpty())
            {
                foreach (InquiryMonitorInfo mInfo in miList)
                {
                    InquiryMonitorEntity storeEntity = TranslateInquiryMonitorEntity(mInfo);
                    all.Add(storeEntity);
                }
            }

            return all;
        }
        #endregion
    }
}
