using DataRepository.DataAccess.Artisan;
using DataRepository.DataModel;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ArtisanService
    {

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

    }
}
