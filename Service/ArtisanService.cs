using Common;
using DataRepository.DataAccess.Artisan;
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
    public class ArtisanService : BaseService
    {
        public static ArtisanEntity GetArtisanByArtisanID(int ArtisanID)
        {
            ArtisanRepository mr = new ArtisanRepository();
            ArtisanInfo info = mr.GetArtisanByArtisanID(ArtisanID);

            return TranslateArtisanEntity(info);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artisanType">艺人类型</param>
        /// <param name="IsCooperation">是否合作</param>
        /// <returns></returns>
        public static List<ArtisanEntity> GetArtisansByRule(string artisanType, string IsCooperation)
        {
            List<ArtisanEntity> lstNews = new List<ArtisanEntity>();
            ArtisanRepository mr = new ArtisanRepository();
            List<ArtisanInfo> lstInfo = mr.GetArtisansByRule(artisanType, IsCooperation);
            if (lstInfo != null && lstInfo.Count > 0)
            {
                foreach (ArtisanInfo info in lstInfo)
                {
                    lstNews.Add(TranslateArtisanEntity(info));
                }
            }
            return lstNews;
        }

        public static ArtisanEntity TranslateArtisanEntity(ArtisanInfo info)
        {
            ArtisanEntity entity = new ArtisanEntity();
            entity.artisanID = info.artisanID;
            entity.artisanName = info.artisanName;
            entity.artisanName2 = info.artisanName2;
            entity.sex = info.sex;
            entity.IDNumber = info.IDNumber;
            entity.birthday = info.birthday;
            entity.workPlace = info.workPlace;
            entity.reviewDate = info.reviewDate;
            entity.artisanType = info.artisanType;
            entity.artisanTitle = info.artisanTitle;
            entity.masterWorker = info.masterWorker;
            entity.artisanSpecial = info.artisanSpecial;
            entity.introduction = info.introduction;
            entity.IDHead = info.IDHead;
            entity.DetailedIntroduction = info.DetailedIntroduction;
            entity.VideoUrl = info.VideoUrl;
            entity.IsCooperation = info.IsCooperation;
            entity.IsRecommend = info.IsRecommend;
            entity.IsPushMall = info.IsPushMall;
            return entity;
        }


        public static ArtisanInfo TranslateArtisanInfo(ArtisanEntity entity)
        {
            ArtisanInfo info = new ArtisanInfo();
            info.artisanID = entity.artisanID;
            info.artisanName = entity.artisanName;
            info.artisanName2 = entity.artisanName2;
            info.sex = entity.sex;
            info.IDNumber = entity.IDNumber;
            info.birthday = entity.birthday;
            info.workPlace = entity.workPlace;
            info.reviewDate = entity.reviewDate;
            info.artisanType = entity.artisanType;
            info.artisanTitle = entity.artisanTitle;
            info.masterWorker = entity.masterWorker;
            info.artisanSpecial = entity.artisanSpecial;
            info.introduction = entity.introduction;
            info.IDHead = entity.IDHead;
            info.DetailedIntroduction = entity.DetailedIntroduction;
            info.VideoUrl = entity.VideoUrl;
            info.IsCooperation = entity.IsCooperation;
            info.IsRecommend = entity.IsRecommend;
            info.IsPushMall = entity.IsPushMall;
            return info;
        }

        public static void Remove(int artisanID)
        {
            ArtisanRepository mr = new ArtisanRepository();
            mr.Remove(artisanID);
        }

        public static void ModifyArtisan(ArtisanEntity entity)
        {
            ArtisanRepository mr = new ArtisanRepository();
            ArtisanInfo info= TranslateArtisanInfo(entity);
            if (entity.artisanID > 0)
            {
                mr.ModifyArtisan(info);
            }
            else
            {                
                mr.CreateNew(info);
            }
        }


        #region 分页相关
        public static int GetArtisanCount(string type)
        {
            return new ArtisanRepository().GetArtisanCount(type);
        }

        public static List<ArtisanEntity> GetArtisanInfoPager(PagerInfo pager)
        {
            List<ArtisanEntity> all = new List<ArtisanEntity>();
            ArtisanRepository mr = new ArtisanRepository();
            List<ArtisanInfo> miList = mr.GetAllArtisanInfoPager(pager);
            foreach (ArtisanInfo mInfo in miList)
            {
                ArtisanEntity carEntity = TranslateArtisanEntity(mInfo);
                all.Add(carEntity);
            }
            return all;
        }

        public static List<ArtisanEntity> GetAllArtisanInfoByRule(string type, PagerInfo pager)
        {
            List<ArtisanEntity> all = new List<ArtisanEntity>();
            ArtisanRepository mr = new ArtisanRepository();
            List<ArtisanInfo> miList = mr.GetAllArtisanInfoByRule(type, pager);

            if (!miList.IsEmpty())
            {
                foreach (ArtisanInfo mInfo in miList)
                {
                    ArtisanEntity storeEntity = TranslateArtisanEntity(mInfo);
                    all.Add(storeEntity);
                }
            }

            return all;
        }
        #endregion

    }
}
