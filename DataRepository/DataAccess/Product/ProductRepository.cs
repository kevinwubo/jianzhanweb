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

        public ProductInfo GetProductByKey(long gid)
        {
            ProductInfo result = new ProductInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.GetProductByKey, "Text"));
            command.AddInputParameter("@ProductID", DbType.String, gid);
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


            return command.ExecuteNonQuery();
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

        public int Remove(long ProductID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ProductSatement.Remove, "Text"));
            command.AddInputParameter("@ProductID", DbType.Int64, ProductID);
            int result = command.ExecuteNonQuery();
            return result;
        }


        public List<ProductInfo> GetAllProductByRule(string author, int count, string orderdesc)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            string sqlText = count > 0 ? String.Format(ProductSatement.GetAllProductTopCountByRule, " TOP " + count) : ProductSatement.GetAllProductByRule;
            if (!string.IsNullOrEmpty(author))
            {
                sqlText += " AND Author = '" + author + "' OR (Type2 like '%" + author + "%' or Type3 like '%" + author + "%') ";
            }
            if (!string.IsNullOrEmpty(orderdesc))
            {
                sqlText += orderdesc;
            }
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }

        public List<ProductInfo> GetProductsByRule(string name, int status)
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
            int result = command.ExecuteNonQuery();
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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="type2">工艺釉色</param>
        /// <param name="type3">器型</param>
        /// <param name="type4">口径尺寸</param>
        /// <param name="type7">价格区间</param>
        /// <param name="author">作者</param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<ProductInfo> GetAllProductInfoByRule(string type2, string type3, string type4, string type7, string author, string sqlwhere, string KEYWORD, string pagename, PagerInfo pager)
        {
            List<ProductInfo> result = new List<ProductInfo>();


            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(type2))
            {
                builder.Append(" AND Type2=@Type2 ");
            }
            if (!string.IsNullOrEmpty(type3))
            {
                builder.Append(" AND Type3=@Type3 ");
            }
            if (!string.IsNullOrEmpty(type4))
            {
                builder.Append(" AND Type4=@Type4 ");
            }
            if (!string.IsNullOrEmpty(type7))
            {
                builder.Append(" AND Type7=@Type7 ");
            }
            if (!string.IsNullOrEmpty(author))
            {
                //builder.Append(" AND Author in (@Author) ");
                builder.Append(" AND Author in(" + author + ")");
            }
            if (!string.IsNullOrEmpty(sqlwhere))
            {
                builder.Append(sqlwhere);
            }

            if (!string.IsNullOrEmpty(KEYWORD))
            {
                builder.Append("and ProductID='" + KEYWORD + "' or ProductName like '%" + KEYWORD + "%' or Author like '%" + KEYWORD + "%' or Type2 like '" + KEYWORD + "' or Type3 like '" + KEYWORD + "' or Type4 like '" + KEYWORD + "' or Type5 like '" + KEYWORD + "'");
            }

            string sql = ProductSatement.GetAllProductInfoPagerHeader + builder.ToString() + ProductSatement.GetAllProductInfoPagerFooter;
            if (pagename.Equals("mn_souchang"))
            {
                sql = ProductSatement.GetAllProductInfoSouChangPagerHeader + builder.ToString() + ProductSatement.GetAllProductInfoSouChangPagerFooter;
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));

            if (!string.IsNullOrEmpty(type2))
            {
                command.AddInputParameter("@Type2", DbType.String, type2);
            }
            if (!string.IsNullOrEmpty(type3))
            {
                command.AddInputParameter("@Type3", DbType.String, type3);
            }
            if (!string.IsNullOrEmpty(type4))
            {
                command.AddInputParameter("@Type4", DbType.String, type4);
            }
            if (!string.IsNullOrEmpty(type7))
            {
                command.AddInputParameter("@Type7", DbType.String, type7);
            }
            //if (!string.IsNullOrEmpty(author))
            //{
            //    command.AddInputParameter("@Author", DbType.String, author);
            //}            
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }


        public int GetProductCount(string type2, string type3, string type4, string type7, string author, string sqlwhere, string KEYWORD)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ProductSatement.GetProductCount);
            if (!string.IsNullOrEmpty(type2))
            {
                builder.Append(" AND Type2=@Type2 ");
            }
            if (!string.IsNullOrEmpty(type3))
            {
                builder.Append(" AND Type3=@Type3 ");
            }
            if (!string.IsNullOrEmpty(type4))
            {
                builder.Append(" AND Type4=@Type4 ");
            }
            if (!string.IsNullOrEmpty(type7))
            {
                builder.Append(" AND Type7=@Type7 ");
            }
            if (!string.IsNullOrEmpty(author))
            {
                //builder.Append(" AND Author=@Author ");
                builder.Append(" AND Author in(" + author + ")");
            }

            if (!string.IsNullOrEmpty(sqlwhere))
            {
                builder.Append(sqlwhere);
            }

            if (!string.IsNullOrEmpty(KEYWORD))
            {
                builder.Append("and ProductID='" + KEYWORD + "' or ProductName like '%" + KEYWORD + "%' or Author like '%" + KEYWORD + "%' or Type2 like '" + KEYWORD + "' or Type3 like '" + KEYWORD + "' or Type4 like '" + KEYWORD + "' or Type5 like '" + KEYWORD + "'");
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));

            if (!string.IsNullOrEmpty(type2))
            {
                command.AddInputParameter("@Type2", DbType.String, type2);
            }
            if (!string.IsNullOrEmpty(type3))
            {
                command.AddInputParameter("@Type3", DbType.String, type3);
            }
            if (!string.IsNullOrEmpty(type4))
            {
                command.AddInputParameter("@Type4", DbType.String, type4);
            }
            if (!string.IsNullOrEmpty(type7))
            {
                command.AddInputParameter("@Type7", DbType.String, type7);
            }
            //if (!string.IsNullOrEmpty(author))
            //{
            //    command.AddInputParameter("@Author", DbType.String, author);
            //}   


            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }

        public List<ProductInfo> GetAllProductInfoPager(string pagename, PagerInfo pager)
        {
            List<ProductInfo> result = new List<ProductInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(pagename.Equals("mn_souchang") ? ProductSatement.GetAllProductInfoSouChangPager : ProductSatement.GetAllProductInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<ProductInfo>();
            return result;
        }
        #endregion
    }
}
