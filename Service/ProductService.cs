using DataRepository.DataAccess.Product;
using DataRepository.DataModel;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class ProductService
    {

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

    }
}
