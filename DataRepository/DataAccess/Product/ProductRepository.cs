/* ==============================================================================
 * Copyright (C) CtripCorpBiz OR Author. All rights reserved.
 * 
 * 类名称：ProductRepository
 * 类描述：
 * 创建人：Ethen Shen
 * 创建时间：4/28/2018 9:56:46 AM
 * 修改人：
 * 修改时间：
 * 修改备注：
 * 代码请保留相关关键处的注释
 * ==============================================================================*/

using Common;
using DataRepository.DataAccess.Product;
using DataRepository.DataModel;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.Product
{
    public class ProductRepository : DataAccessBase
    {


        public List<ProductInfo> GetAllProduct()
        {
            List<ProductInfo> result = new List<ProductInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.GetAllProduct, "Text"));
            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }

        public List<ProductInfo> GetProductByKeys(string keys)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            if (!string.IsNullOrEmpty(keys))
            {
                string sqlText = ProductSatement.GetProductByKeys;
                sqlText = sqlText.Replace("#ids#", keys);
                DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
                result = command.ExecuteEntityList<ProductInfo>();
            }

            return result;
        }

        public List<ProductInfo> GetProductByRule(string name, int status)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            string sqlText = ProductSatement.GetAllProductByRule;
            if (!string.IsNullOrEmpty(name))
            {
                sqlText += " AND ProductName LIKE '%'+@key+'%'";
            }
            if (status > -1)
            {
                sqlText += " AND Status=@Status";
            }


            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(name))
            {
                command.AddInputParameter("@key", DbType.String, name);
            }
            if (status > -1)
            {
                command.AddInputParameter("@Status", DbType.Int32, status);
            }

            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }

        public ProductInfo GetProductByKey(int id)
        {
            ProductInfo result = new ProductInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.GetProductByKey, "Text"));
            command.AddInputParameter("@id", DbType.Int32, id);
            result = command.ExecuteEntity<ProductInfo>();
            return result;
        }

        public long CreateNew(ProductInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.CreateNewProduct, "Text"));
            
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@ProductName", DbType.String, info.ProductName);
            command.AddInputParameter("@SubTitle", DbType.String, info.SubTitle);
            command.AddInputParameter("@Type1", DbType.String, info.Type1);
            command.AddInputParameter("@Type2", DbType.String, info.Type2);
            command.AddInputParameter("@Type3", DbType.String, info.Type3);
            command.AddInputParameter("@Type4", DbType.String, info.Type4);
            command.AddInputParameter("@Type5", DbType.String, info.Type5);
            command.AddInputParameter("@Type6", DbType.String, info.Type6);
            command.AddInputParameter("@Type7", DbType.String, info.Type7);

            command.AddInputParameter("@Images", DbType.String, info.Images);
            command.AddInputParameter("@summary", DbType.String, info.summary);
            command.AddInputParameter("@ProductDetail", DbType.String, info.ProductDetail);
            command.AddInputParameter("@InventoryCount", DbType.Int32, info.InventoryCount);
            command.AddInputParameter("@Material", DbType.String, info.Material);
            command.AddInputParameter("@Volume", DbType.String, info.Volume);
            command.AddInputParameter("@CostPrice", DbType.Decimal, info.CostPrice);
            command.AddInputParameter("@MarketPrice", DbType.Decimal, info.MarketPrice);
            command.AddInputParameter("@LowPrice", DbType.String, info.LowPrice);

            command.AddInputParameter("@ArtisanID", DbType.Int32, info.ArtisanID);
            command.AddInputParameter("@VideoUrl", DbType.String, info.VideoUrl);
            command.AddInputParameter("@VideoDetail", DbType.String, info.VideoDetail);
            command.AddInputParameter("@PlatePosition", DbType.String, info.PlatePosition);
            command.AddInputParameter("@Author", DbType.String, info.Author);
            command.AddInputParameter("@ProImageDetail", DbType.String, info.ProImageDetail);
            command.AddInputParameter("@IsPushMall", DbType.String, info.IsPushMall);

            command.AddInputParameter("@AddDate", DbType.DateTime, info.Adddate);
            command.AddInputParameter("@UpdateDate", DbType.DateTime, info.UpdateDate); 

            
            //return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }

        public int ModifyProduct(ProductInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.ModifyProduct, "Text"));
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@ProductName", DbType.String, info.ProductName);
            command.AddInputParameter("@SubTitle", DbType.String, info.SubTitle);
            command.AddInputParameter("@Type1", DbType.String, info.Type1);
            command.AddInputParameter("@Type2", DbType.String, info.Type2);
            command.AddInputParameter("@Type3", DbType.String, info.Type3);
            command.AddInputParameter("@Type4", DbType.String, info.Type4);
            command.AddInputParameter("@Type5", DbType.String, info.Type5);
            command.AddInputParameter("@Type6", DbType.String, info.Type6);
            command.AddInputParameter("@Type7", DbType.String, info.Type7);

            command.AddInputParameter("@Images", DbType.String, info.Images);
            command.AddInputParameter("@summary", DbType.String, info.summary);
            command.AddInputParameter("@ProductDetail", DbType.String, info.ProductDetail);
            command.AddInputParameter("@InventoryCount", DbType.Int32, info.InventoryCount);
            command.AddInputParameter("@Material", DbType.String, info.Material);
            command.AddInputParameter("@Volume", DbType.String, info.Volume);
            command.AddInputParameter("@CostPrice", DbType.Decimal, info.CostPrice);
            command.AddInputParameter("@MarketPrice", DbType.Decimal, info.MarketPrice);
            command.AddInputParameter("@LowPrice", DbType.String, info.LowPrice);

            command.AddInputParameter("@ArtisanID", DbType.Int32, info.ArtisanID);
            command.AddInputParameter("@VideoUrl", DbType.String, info.VideoUrl);
            command.AddInputParameter("@VideoDetail", DbType.String, info.VideoDetail);
            command.AddInputParameter("@PlatePosition", DbType.String, info.PlatePosition);
            command.AddInputParameter("@Author", DbType.String, info.Author);
            command.AddInputParameter("@ProImageDetail", DbType.String, info.ProImageDetail);
            command.AddInputParameter("@IsPushMall", DbType.String, info.IsPushMall);
            command.AddInputParameter("@UpdateDate", DbType.DateTime, info.UpdateDate);
            command.AddInputParameter("@ID", DbType.DateTime, info.ID);
            return command.ExecuteNonQuery();
        }

        public int Remove(int id)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.Remove, "Text"));
            command.AddInputParameter("@id", DbType.Int32, id);
            int result = command.ExecuteNonQuery();
            return result;
        }


        public List<ProductInfo> GetAllInventoryByRule(string title, int status)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            string sqlText = ProductSatement.GetAllProductByRule;
            if (!string.IsNullOrEmpty(title))
            {
                sqlText += " AND Title like '%" + title + "'%";
            }
            if (status != -1)
            {
                sqlText += " AND Status = '" + status + "'";
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }

        public List<ProductInfo> GetProductsByRule(string name,int status)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            string sqlText=ProductSatement.GetAllProductByRule;
            if(!string.IsNullOrEmpty(name))
            {
                sqlText+=" AND ProductName LIKE '%'+@key+'%'";
            }
            if(status>-1)
            {
                sqlText+=" AND Status=@Status";
            }


            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(name))
            {
                command.AddInputParameter("@key", DbType.String, name);
            }
            if (status > -1)
            {
                command.AddInputParameter("@Status", DbType.Int32, status);
            }
           
            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }

        public ProductInfo GetProductByProductID(string ProductID)
        {
            ProductInfo result = new ProductInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.GetProductByProductID, "Text"));
            command.AddInputParameter("@ProductID", DbType.String, ProductID);
            result = command.ExecuteEntity<ProductInfo>();
            return result;
        }       

        public int RemoveProduct(long mid)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.Remove, "Text"));
            command.AddInputParameter("@ProductID", DbType.Int64, mid);
            int result=command.ExecuteNonQuery();
            return result;
        }


        #region 自定义SQL脚本
        /// <summary>
        /// 每个产品查询两个
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public List<ProductInfo> GetProductsBySqlWhere(int count, int pCount, string sqlwhere)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            string sqlText = string.Format(ProductSatement.GetProductBySqlWhere, count, pCount, sqlwhere);

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));

            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="count">查询数量</param>
        /// <param name="sqlwhere">查询条件</param>
        /// <param name="orderDesc">排序条件</param>
        /// <returns></returns>
        public List<ProductInfo> GetProductsBySqlWhere(int count, string sqlwhere, string orderDesc)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            string topCount = count > 0 ? " top " + count : "";
            string sqlText = string.Format(ProductSatement.GetProductBySqlWhere2, topCount, sqlwhere, orderDesc);

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));

            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }
        #endregion
        


        #region 分页方法
        public List<ProductInfo> GetAllProductInfoByRule(string type, PagerInfo pager)
        {
            List<ProductInfo> result = new List<ProductInfo>();


            StringBuilder builder = new StringBuilder();

            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    builder.Append(" AND CustomerName LIKE '%'+@CustomerName+'%' ");
            //}
            //if (!string.IsNullOrEmpty(title))
            //{
            //    builder.Append(" AND AdviseTitle LIKE '%'+@AdviseTitle+'%' ");
            //}
            //if (dealStatus > -1)
            //{
            //    builder.Append(" AND DealStatus=@DealStatus ");
            //}

            string sql = ProductSatement.GetAllProductInfoPagerHeader + builder.ToString() + ProductSatement.GetAllProductInfoPagerFooter;

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));

            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    command.AddInputParameter("@CustomerName", DbType.Int64, customerName);
            //}
            //if (!string.IsNullOrEmpty(title))
            //{
            //    command.AddInputParameter("@AdviseTitle", DbType.String, title);
            //}
            //if (dealStatus > -1)
            //{
            //    command.AddInputParameter("@DealStatus", DbType.Int32, dealStatus);
            //}
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);

            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }


        public int GetProductCount(string type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ProductSatement.GetProductCount);
            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    builder.Append(" AND CustomerName LIKE '%'+@CustomerName+'%' ");
            //}
            //if (!string.IsNullOrEmpty(title))
            //{
            //    builder.Append(" AND AdviseTitle LIKE '%'+@AdviseTitle+'%' ");
            //}
            //if (dealStatus > -1)
            //{
            //    builder.Append(" AND DealStatus=@DealStatus ");
            //}

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));

            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    command.AddInputParameter("@CustomerName", DbType.Int64, customerName);
            //}
            //if (!string.IsNullOrEmpty(title))
            //{
            //    command.AddInputParameter("@AdviseTitle", DbType.String, title);
            //}
            //if (dealStatus > -1)
            //{
            //    command.AddInputParameter("@DealStatus", DbType.Int32, dealStatus);
            //}


            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }

        public List<ProductInfo> GetAllProductInfoPager(PagerInfo pager)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.GetAllProductInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }
        #endregion
    }
}
