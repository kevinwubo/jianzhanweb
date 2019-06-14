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
        public static List<ArtisanEntity> GetAllArtisan()
        {
            List<ArtisanEntity> all = new List<ArtisanEntity>();
            ArtisanRepository mr = new ArtisanRepository();
            List<ArtisanInfo> miList = Cache.Get<List<ArtisanInfo>>("GetAllArtisan");
            if (miList.IsEmpty())
            {
                miList = mr.GetAllArtisan();
                Cache.Add("GetAllArtisan", miList);
            }

            if (!miList.IsEmpty())
            {
                foreach (ArtisanInfo mInfo in miList)
                {
                    ArtisanEntity carEntity = TranslateArtisanEntity(mInfo);
                    all.Add(carEntity);
                }
            }
            return all;
        }
        public static ArtisanEntity GetArtisanByKey(string artisanID)
        {
            ArtisanEntity entity = new ArtisanEntity();
            ArtisanInfo info = Cache.Get<ArtisanInfo>("GetArtisanByKey" + artisanID);
            ArtisanRepository mr = new ArtisanRepository();
            if (info == null)
            {
                info = mr.GetArtisanByKey(artisanID);
                Cache.Add("GetArtisanByKey" + artisanID, info);
            }

            if (info != null)
            {
                entity = TranslateArtisanEntity(info, true, 0);
            }
            return entity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artisanType">艺人类型</param>
        /// <param name="IsCooperation">是否合作</param>
        /// <returns></returns>
        public static List<ArtisanEntity> GetArtisansByRule(string artisanType, int count = 0, string IsCooperation = "", string sqlwhere="")
        {
            List<ArtisanEntity> lstNews = new List<ArtisanEntity>();
            ArtisanRepository mr = new ArtisanRepository();
            List<ArtisanInfo> lstInfo = mr.GetArtisansByRule(artisanType, count, IsCooperation, sqlwhere);
            if (lstInfo != null && lstInfo.Count > 0)
            {
                foreach (ArtisanInfo info in lstInfo)
                {
                    lstNews.Add(TranslateArtisanEntity(info));
                }
            }
            return lstNews;
        }


        public static List<SimpleArtisanEntity> getSimpleArtisanList(string artisanType, int count = 0, string IsCooperation = "1")
        {
            List<SimpleArtisanEntity> listR = new List<SimpleArtisanEntity>();
            List<ArtisanEntity> list= GetArtisansByRule(artisanType,count,IsCooperation);
            if (list != null && list.Count > 0)
            {
                foreach (ArtisanEntity entity in list)
                {
                    SimpleArtisanEntity model = new SimpleArtisanEntity();
                    model.artisanID = entity.artisanID;
                    model.artisanName = entity.artisanName;
                    listR.Add(model);
                }
            }
            return listR;
        }

        public static ArtisanEntity TranslateArtisanEntity(ArtisanInfo info, bool IsReader = false, int count = 3)
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
            entity.IDHead = "http://116.62.124.214/" + info.IDHead;
            entity.DetailedIntroduction = info.DetailedIntroduction;
            entity.VideoUrl = info.VideoUrl;
            entity.IsCooperation = info.IsCooperation;
            entity.IsRecommend = info.IsRecommend;
            entity.IsPushMall = info.IsPushMall;
            entity.adddate = info.Adddate;
            if (IsReader)
            {
                entity.listProduct = ProductService.GetAllProductByRule(info.artisanName, count, " order by AddDate desc ");
            }
            return entity;
        }

        #region 分页相关
        public static int GetArtisanCount(string artisantype, string artisanname)
        {
            return new ArtisanRepository().GetArtisanCount(artisantype, artisanname);
        }

        public static List<ArtisanEntity> GetArtisanInfoPager(PagerInfo pager)
        {
            List<ArtisanEntity> all = new List<ArtisanEntity>();
            ArtisanRepository mr = new ArtisanRepository();
            List<ArtisanInfo> miList = Cache.Get<List<ArtisanInfo>>("GetArtisanInfoPager");
            if (miList.IsEmpty())
            {
                miList = mr.GetAllArtisanInfoPager(pager);
                Cache.Add("GetAllArtisanInfoPager", miList);
            }

            if (!miList.IsEmpty())
            {
                foreach (ArtisanInfo mInfo in miList)
                {
                    ArtisanEntity carEntity = TranslateArtisanEntity(mInfo, true);
                    all.Add(carEntity);
                }
            }
            return all;
        }

        public static List<ArtisanEntity> GetAllArtisanInfoByRule(string artisantype, string artisanname, PagerInfo pager)
        {
            List<ArtisanEntity> all = new List<ArtisanEntity>();
            ArtisanRepository mr = new ArtisanRepository();
            List<ArtisanInfo> miList = Cache.Get<List<ArtisanInfo>>("GetAllArtisanInfoByRule" + artisantype + artisanname);

            if (miList.IsEmpty())
            {
                miList = mr.GetAllArtisanInfoByRule(artisantype, artisanname, pager);
                Cache.Add("GetAllArtisanInfoByRule" + artisantype + artisanname, miList);
            }
            if (!miList.IsEmpty())
            {
                foreach (ArtisanInfo mInfo in miList)
                {
                    ArtisanEntity storeEntity = TranslateArtisanEntity(mInfo, true);
                    all.Add(storeEntity);
                }
            }
            return all;
        }
        #endregion

    }
}
