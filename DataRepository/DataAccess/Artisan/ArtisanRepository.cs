/* ==============================================================================
 * Copyright (C) CtripCorpBiz OR Author. All rights reserved.
 * 
 * 类名称：ArtisanRepository
 * 类描述：
 * 创建人：Ethen Shen
 * 创建时间：4/28/2018 9:56:46 AM
 * 修改人：
 * 修改时间：
 * 修改备注：
 * 代码请保留相关关键处的注释
 * ==============================================================================*/

using Common;
using DataRepository.DataAccess.Artisan;
using DataRepository.DataAccess.Artisan;
using DataRepository.DataModel;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.Artisan
{
    public class ArtisanRepository : DataAccessBase
    {
        public ArtisanInfo GetArtisanByArtisanID(int ArtisanID)
        {
            ArtisanInfo result = new ArtisanInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.GetArtisanByArtisanID, "Text"));
            result = command.ExecuteEntity<ArtisanInfo>();
            return result;
        }
        public List<ArtisanInfo> GetAllArtisan()
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.GetAllArtisan, "Text"));
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }

        public  List<ArtisanInfo> GetArtisansByRule(string artisanType, string IsCooperation)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            string sqlText=ArtisanSatement.GetAllArtisanByRule;
            if (!string.IsNullOrEmpty(artisanType))
            {
                sqlText += " AND artisanType =@artisanType";
            }

            if (!string.IsNullOrEmpty(IsCooperation))
            {
                sqlText += " AND IsCooperation =@IsCooperation";
            }


            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(artisanType))
            {
                command.AddInputParameter("@artisanType", DbType.String, artisanType);
            }
            if (!string.IsNullOrEmpty(IsCooperation))
            {
                command.AddInputParameter("@IsCooperation", DbType.String, IsCooperation);
            }
           
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }

        public  ArtisanInfo GetArtisanByKey(string artisanID)
        {
            ArtisanInfo result = new ArtisanInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.GetArtisanByArtisanID, "Text"));
            command.AddInputParameter("@artisanID", DbType.String, artisanID);
            result = command.ExecuteEntity<ArtisanInfo>();
            return result;
        }

        public  List<ArtisanInfo> GetArtisanByKeys(string keys)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            string sqlText = ArtisanSatement.GetArtisanByKeys;
            sqlText = sqlText.Replace("#ids#", keys);
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }

        public  int Remove(int artisanID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.Remove, "Text"));
            command.AddInputParameter("@ArtisanID", DbType.Int32, artisanID);
            int result = command.ExecuteNonQuery();
            return result;
        }

        public  long CreateNew(ArtisanInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.CreateArtisan, "Text"));

            //return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }

        public  int ModifyArtisan(ArtisanInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.ModifyArtisan, "Text"));
            command.AddInputParameter("@artisanID", DbType.Int32, info.artisanID);
            return command.ExecuteNonQuery();
        }


        #region 分页方法
        public  List<ArtisanInfo> GetAllArtisanInfoByRule(string type, PagerInfo pager)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();


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

            string sql = ArtisanSatement.GetAllArtisanInfoPagerHeader + builder.ToString() + ArtisanSatement.GetAllArtisanInfoPagerFooter;

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

            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }


        public int GetArtisanCount(string type)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ArtisanSatement.GetArtisanCount);
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

        public List<ArtisanInfo> GetAllArtisanInfoPager(PagerInfo pager)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.GetAllArtisanInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }
        #endregion
    }
}
