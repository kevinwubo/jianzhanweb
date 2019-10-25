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
using System.Text.RegularExpressions;

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
            entity.id = info.id;
            entity.img_url = info.img_url;
            if (string.IsNullOrEmpty(info.img_url))
            {
                Regex r = new Regex(@"<img[\s\S]*?>", RegexOptions.IgnoreCase);
                // 定义正则表达式用来匹配 img 标签            
                MatchCollection collImages = r.Matches(info.content);
                List<string> lstImages = new List<string>();
                foreach (Match item in collImages)
                {
                    lstImages.Add(GetHtmlImageUrlList(item.Value));
                }

                if (lstImages != null && lstImages.Count > 0)
                {
                    entity.img_url = lstImages[0].Replace("http://121.42.156.253", FileUrl);
                }
                else
                {
                    int i = info.id & 5;
                    entity.img_url = "/Images/" + i + ".jpg";
                }
            }
            else
            {
                entity.img_url = info.img_url.Replace("http://121.42.156.253", FileUrl);
            }
            entity.zhaiyao = info.zhaiyao;
            entity.content = info.content;
            return entity;
        }

        /// <summary> 
        /// 取得HTML中所有图片的 URL。 
        /// </summary> 
        /// <param name="sHtmlText">HTML代码</param> 
        /// <returns>图片的URL列表</returns> 
        public static string GetHtmlImageUrlList(string sHtmlText)
        {
            // 定义正则表达式用来匹配 img 标签 
            Regex regImg = new Regex(@"<img\b[^<>]*?\bsrc[\s\t\r\n]*=[\s\t\r\n]*[""']?[\s\t\r\n]*(?<imgUrl>[^\s\t\r\n""'<>]*)[^<>]*?/?[\s\t\r\n]*>", RegexOptions.IgnoreCase);

            // 搜索匹配的字符串 
            MatchCollection matches = regImg.Matches(sHtmlText);
            int i = 0;
            //string[] sUrlList = new string[matches.Count];
            string str = string.Empty;
            // 取得匹配项列表 
            foreach (Match match in matches)
                str = match.Groups["imgUrl"].Value;
            return str;
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

        public static int ModifyContent(ArticleEntity entity)
        {
            ArticleRepository mr = new ArticleRepository();
            ArticleInfo info=new ArticleInfo();
            info.id = entity.id;
            info.content = entity.content;
            info.update_time = DateTime.Now;
            return  mr.ModifyContent(info);
        }

        public static List<ArticleEntity> GetArticleByRule(int category_id, int count)
        {
            List<ArticleEntity> all = new List<ArticleEntity>();
            ArticleRepository mr = new ArticleRepository();
            List<ArticleInfo> miList = Cache.Get<List<ArticleInfo>>("GetArticleByRule" + category_id + count);
            if (miList.IsEmpty())
            {
                miList = mr.GetArticleByRule(category_id, count);
                Cache.Add("GetArticleByRule" + category_id + count, miList);
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
            List<ArticleInfo> miList = Cache.Get<List<ArticleInfo>>("GetAllArticleInfoByRule" + category_id + pager.PageIndex + pager.PageCount);

            if (miList.IsEmpty())
            {
                miList = mr.GetAllArticleInfoByRule(category_id, pager);
                Cache.Add("GetAllArticleInfoByRule" + category_id + pager.PageIndex + pager.PageCount, miList);
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
