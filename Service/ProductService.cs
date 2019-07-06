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

namespace Service
{
    public class ProductService
    {
        public static ProductEntity GetProductByKey(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            ProductEntity entity = new ProductEntity();
            ProductRepository mr = new ProductRepository();
            return TranslateProductEntity(mr.GetProductByKey(id.ToInt(0)));
        }

        public static void ModifyProduct(ProductEntity entity)
        {
            ProductRepository mr = new ProductRepository();
            ProductInfo info=new ProductInfo();
            info = TranslateProductInfo(entity);
            mr.ModifyProduct(info);
        }

        public static void Remove(string id)
        {
            ProductRepository mr = new ProductRepository();
            mr.Remove(id.ToInt(0));
        }


        public static ProductInfo TranslateProductInfo(ProductEntity entity)
        {
            ProductInfo info = new ProductInfo();
            if (info != null)
            {
                info.ID = entity.ID;
                info.ProductID = entity.ProductID;
                info.ProductName = entity.ProductName;
                info.SubTitle = entity.SubTitle;
                info.Type1 = entity.Type1;
                info.Type2 = entity.Type2;
                info.Type3 = entity.Type3;
                info.Type4 = entity.Type4;
                info.Type5 = entity.Type5;
                info.Type6 = entity.Type6;
                info.Type7 = entity.Type7;
                info.Images = entity.Images;
                info.summary = entity.summary;
                info.ProductDetail = entity.ProductDetail;
                info.ProImageDetail = entity.ProImageDetail;
                info.IsPushMall = entity.IsPushMall;
                info.Material = entity.Material;
                info.Volume = entity.Volume;
                info.CostPrice = entity.CostPrice;
                info.MarketPrice = entity.MarketPrice;
                info.LowPrice = entity.LowPrice;
                info.ArtisanID = entity.ArtisanID;
                info.VideoUrl = entity.VideoUrl;
                info.VideoDetail = entity.VideoDetail;
                info.PlatePosition = entity.PlatePosition;
                info.InventoryCount = entity.InventoryCount;
                info.Author = entity.Author;
                info.Adddate = entity.Adddate;
                info.UpdateDate = entity.UpdateDate;
            }
            return info;
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
                entity.Images = info.Images;
                entity.summary = info.summary;
                entity.ProductDetail = info.ProductDetail;
                entity.ProImageDetail = info.ProImageDetail;
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

                entity.UpdateDate = info.UpdateDate;
            }
            return entity;
        }

        #region 分页相关
        public static int GetProductCount(string type)
        {
            return new ProductRepository().GetProductCount(type);
        }

        public static List<ProductEntity> GetProductInfoPager(PagerInfo pager)
        {
            List<ProductEntity> all = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> miList = mr.GetAllProductInfoPager(pager);
            foreach (ProductInfo mInfo in miList)
            {
                ProductEntity carEntity = TranslateProductEntity(mInfo);
                all.Add(carEntity);
            }
            return all;
        }

        public static List<ProductEntity> GetAllProductInfoByRule(string type, PagerInfo pager)
        {
            List<ProductEntity> all = new List<ProductEntity>();
            ProductRepository mr = new ProductRepository();
            List<ProductInfo> miList = mr.GetAllProductInfoByRule(type, pager);

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
