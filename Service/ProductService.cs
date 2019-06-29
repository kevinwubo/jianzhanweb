using Common;
using DataRepository.DataAccess.Product;
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
    public class ProductService : BaseService
    {

        public static List<ProductEntity> GetAllProduct()
        {
            List<ProductEntity> all = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> miList = Cache.Get<List<ProductInfo>>("GetAllProduct");
            if (miList.IsEmpty())
            {
                miList = mr.GetAllProduct();
                Cache.Add("GetAllProduct", miList);
            }

            if (!miList.IsEmpty())
            {
                foreach (ProductInfo mInfo in miList)
                {
                    ProductEntity carEntity = TranslateProductEntity(mInfo);
                    all.Add(carEntity);
                }
            }
            return all;
        }

        public static ProductEntity GetProductByProductID(string ProductID)
        {
            if (string.IsNullOrEmpty(ProductID))
            {
                return null;
            }
            ProductEntity entity = new ProductEntity();
            ProductRepository mr = new ProductRepository();
            return TranslateProductEntity(mr.GetProductByProductID(ProductID));
        }


        public static List<ProductEntity> GetAllProductByRule(string author,int count,string orderdesc)
        {
            List<ProductEntity> all = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> miList = Cache.Get<List<ProductInfo>>("GetProductByAuthor" + author + count + orderdesc);
            if (miList.IsEmpty())
            {
                miList = mr.GetAllProductByRule(author, count,orderdesc);
                Cache.Add("GetProductByAuthor" + author + count + orderdesc, miList);
            }

            if (!miList.IsEmpty())
            {
                foreach (ProductInfo mInfo in miList)
                {
                    ProductEntity carEntity = TranslateProductEntity(mInfo);
                    all.Add(carEntity);
                }
            }
            return all;
        }

        public static List<ProductEntity> GetProductsBySqlWhere(int count, int pCount, string sqlwhere)
        {
            List<ProductEntity> lstNews = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> lstInfo = mr.GetProductsBySqlWhere(count, pCount, sqlwhere);
            if (lstInfo != null && lstInfo.Count > 0)
            {
                foreach (ProductInfo info in lstInfo)
                {
                    lstNews.Add(TranslateProductEntity(info));
                }
            }
            return lstNews;
        }

        public static List<ProductEntity> GetProductsBySqlWhere(int count, string sqlwhere, string orderDesc)
        {
            List<ProductEntity> list = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> lstInfo = mr.GetProductsBySqlWhere(count, sqlwhere, orderDesc);
            if (lstInfo != null && lstInfo.Count > 0)
            {
                foreach (ProductInfo info in lstInfo)
                {
                    list.Add(TranslateProductEntity(info));
                }
            }
            return list;
        }

        public static List<ProductEntity> GetNews(string title, int status)
        {
            List<ProductEntity> lstNews = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> lstInfo = mr.GetProductsByRule(title, status);
            if (lstInfo != null && lstInfo.Count > 0)
            {
                foreach (ProductInfo info in lstInfo)
                {
                    lstNews.Add(TranslateProductEntity(info));
                }
            }
            return lstNews;
        }

        public static ProductEntity TranslateProductEntity(ProductInfo info)
        {
            ProductEntity entity = new ProductEntity();
            if (info != null)
            {
                entity.ID = info.ID;
                entity.ProductID = info.ProductID;
                entity.ProductName = info.ProductName;
                entity.SubTitle = info.SubTitle;
                entity.Type1 = info.Type1;
                entity.Type2 = info.Type2;
                entity.Type3 = info.Type3;
                entity.Type4 = info.Type4;
                entity.Type5 = info.Type5;
                entity.Type6 = info.Type6;
                entity.Type7 = info.Type7;
                entity.Images = URL + info.Images;
                entity.summary = string.IsNullOrEmpty(info.summary) ? "" : info.summary.Replace("http://121.42.156.253", URL);
                entity.ProductDetail = info.ProductDetail;
                entity.ProImageDetail = string.IsNullOrEmpty(info.ProImageDetail) ? "" : info.ProImageDetail.Replace("http://121.42.156.253", URL);
                entity.IsPushMall = info.IsPushMall;
                entity.Material = info.Material;
                entity.Volume = info.Volume;
                entity.CostPrice = info.CostPrice;
                entity.MarketPrice = info.MarketPrice;
                entity.LowPrice = info.LowPrice;
                entity.ArtisanID = info.ArtisanID;
                entity.VideoUrl = info.VideoUrl;
                entity.VideoDetail = info.VideoDetail;
                entity.PlatePosition = info.PlatePosition;
                entity.InventoryCount = info.InventoryCount;
                entity.Author = info.Author;
                entity.Adddate = info.Adddate;
                Regex r = new Regex(@"<img[\s\S]*?>", RegexOptions.IgnoreCase);

                // 定义正则表达式用来匹配 img 标签     
                List<string> lstImages = new List<string>();
                if (!string.IsNullOrEmpty(info.summary))
                {
                    MatchCollection collImages = r.Matches(entity.summary);                    
                    List<string> lstImagesUrl = new List<string>();
                    foreach (Match item in collImages)
                    {
                        lstImages.Add(item.Value);
                    }
                    //}
                }
                entity.lstImages = lstImages;
                entity.UpdateDate = info.UpdateDate;
                entity.Introduction = info.Introduction;
            }
            return entity;
        }

        #region 分页相关
        public static int GetProductCount(string type2, string type3, string type4, string type7, string author, string sqlwhere,string Keyword)
        {
            return new ProductRepository().GetProductCount(type2, type3, type4, type7, author, sqlwhere, Keyword);
        }

        public static List<ProductEntity> GetProductInfoPager(string pagename, PagerInfo pager)
        {
            List<ProductEntity> all = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> miList = Cache.Get<List<ProductInfo>>("GetAllProductInfoPager");
            if (miList.IsEmpty())
            {
                miList = mr.GetAllProductInfoPager(pagename, pager);
                Cache.Add("GetAllProductInfoPager", miList);
            }

            if (!miList.IsEmpty())
            {
                foreach (ProductInfo mInfo in miList)
                {
                    ProductEntity carEntity = TranslateProductEntity(mInfo);
                    all.Add(carEntity);
                }
            }
            return all;
        }

        public static List<ProductEntity> GetAllProductInfoByRule(string type2, string type3, string type4, string type7,string author,string sqlwhere,string keyword,string pagename, PagerInfo pager)
        {
            List<ProductEntity> all = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> miList = Cache.Get<List<ProductInfo>>("GetAllProductInfoByRule" + type2 + type3 + type4 + type7 + author + sqlwhere + keyword + pagename);

            if (miList.IsEmpty())
            {
                miList = mr.GetAllProductInfoByRule(type2, type3, type4, type7, author, sqlwhere, keyword, pagename, pager);
                Cache.Add("GetAllProductInfoByRule" + type2 + type3 + type4 + type7 + author + sqlwhere + keyword + pagename, miList);
            }
            if (!miList.IsEmpty())
            {
                foreach (ProductInfo mInfo in miList)
                {
                    ProductEntity storeEntity = TranslateProductEntity(mInfo);
                    all.Add(storeEntity);
                }
            }
            return all;
        }
        #endregion

    }
}
