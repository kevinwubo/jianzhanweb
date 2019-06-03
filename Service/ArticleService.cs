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
    public class ArticleService : BaseService
    {
        public static ArticleEntity GetArticleByID(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            ArticleEntity entity = new ArticleEntity();
            ArticleRepository mr = new ArticleRepository();
            return TranslateArticleEntity(mr.GetArticleByID(id));
        }

        private static ArticleEntity TranslateArticleEntity(ArticleInfo info)
        {
            ArticleEntity entity = new ArticleEntity();
            entity.AddDate = info.add_time;
            entity.articleTitle = info.title;
            entity.articleType = getArticleType(info.category_id);
            entity.id = info.id.ToString();
            entity.img_url = info.img_url;
            entity.zhaiyao = info.zhaiyao;
            entity.content = info.content;
            return entity;
        }

        private static string getArticleType(int id)
        {
            string type = "";
            if (id == 3)
                type = "拍卖行情";
            else if (id == 33)
                type = "建盏天下";
            else if (id == 5)
                type = "建盏工艺";
            else if (id == 6)
                type = "百科知识";
            else if (id == 7)
                type = "选盏技巧";
            else if (id == 8)
                type = "文化历史";
            else if (id == 1)
                type = "新闻资讯";

            return type;
        }

        #region 分页相关
        public static int GetArticleCount(int category_id)
        {
            return new ArticleRepository().GetArticleCount(category_id);
        }

        public static List<ArticleEntity> GetArticleInfoPager(PagerInfo pager)
        {
            List<ArticleEntity> all = new List<ArticleEntity>();
            ArticleRepository mr = new ArticleRepository();
            List<ArticleInfo> miList = Cache.Get<List<ArticleInfo>>("GetArticleInfoPager");
            if (miList.IsEmpty())
            {
                miList = mr.GetAllArticleInfoPager(pager);
                Cache.Add("GetArticleInfoPager", miList);
            }

            if (!miList.IsEmpty())
            {
                foreach (ArticleInfo mInfo in miList)
                {
                    ArticleEntity carEntity = TranslateArticleEntity(mInfo);
                    all.Add(carEntity);
                }
            }
            return all;
        }

        public static List<ArticleEntity> GetAllArticleInfoByRule(int category_id, PagerInfo pager)
        {
            List<ArticleEntity> all = new List<ArticleEntity>();
            ArticleRepository mr = new ArticleRepository();
            List<ArticleInfo> miList = Cache.Get<List<ArticleInfo>>("GetAllArticleInfoByRule" + category_id);

            if (miList.IsEmpty())
            {
                miList = mr.GetAllArticleInfoByRule(category_id, pager);
                Cache.Add("GetAllArticleInfoByRule" + category_id, miList);
            }
            if (!miList.IsEmpty())
            {
                foreach (ArticleInfo mInfo in miList)
                {
                    ArticleEntity storeEntity = TranslateArticleEntity(mInfo);
                    all.Add(storeEntity);
                }
            }
            return all;
        }
        #endregion
    }
}
