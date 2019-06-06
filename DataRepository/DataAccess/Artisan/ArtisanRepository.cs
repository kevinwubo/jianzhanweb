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



        public List<ArtisanInfo> GetAllArtisan()
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.GetAllArtisan, "Text"));
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }

        public List<ArtisanInfo> GetArtisansByRule(string artisanType,int count, string IsCooperation,string sqlwhere)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            string sqlText = count > 0 ? ArtisanSatement.GetTopCountArtisanByRule : ArtisanSatement.GetAllArtisanByRule;
            if (!string.IsNullOrEmpty(artisanType))
            {
                sqlText += " AND artisanType =@artisanType";
            }

            if (!string.IsNullOrEmpty(IsCooperation))
            {
                sqlText += " AND IsCooperation =@IsCooperation";
            }

            if (!string.IsNullOrEmpty(sqlwhere))
            {
                sqlText += sqlwhere;
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text")); ;
            if (count > 0)
            {
                command = new DataCommand(ConnectionString, GetDbCommand(string.Format(sqlText, " TOP " + count), "Text"));
            }

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

        public ArtisanInfo GetArtisanByKey(string artisanID)
        {
            ArtisanInfo result = new ArtisanInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.GetArtisanByArtisanID, "Text"));
            command.AddInputParameter("@artisanID", DbType.String, artisanID);
            result = command.ExecuteEntity<ArtisanInfo>();
            return result;
        }

        public List<ArtisanInfo> GetArtisanByKeys(string keys)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            string sqlText = ArtisanSatement.GetArtisanByKeys;
            sqlText = sqlText.Replace("#ids#", keys);
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }

       

        public int RemoveArtisan(long mid)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.Remove, "Text"));
            command.AddInputParameter("@ArtisanID", DbType.Int64, mid);
            int result=command.ExecuteNonQuery();
            return result;
        }


        #region 分页方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="artisantype">艺人类型</param>
        /// <returns></returns>
        public List<ArtisanInfo> GetAllArtisanInfoByRule(string artisantype, string artisanname, PagerInfo pager)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();


            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(artisantype))
            {
                builder.Append(" AND ArtisanType=@ArtisanType ");
            }
            if (!string.IsNullOrEmpty(artisanname))
            {
                builder.Append(" AND artisanName=@artisanName ");
            }
           
            string sql = ArtisanSatement.GetAllArtisanInfoPagerHeader + builder.ToString() + ArtisanSatement.GetAllArtisanInfoPagerFooter;

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));

            if (!string.IsNullOrEmpty(artisantype))
            {
                command.AddInputParameter("@ArtisanType", DbType.String, artisantype);
            }
            if (!string.IsNullOrEmpty(artisanname))
            {
                command.AddInputParameter("@artisanName", DbType.String, artisanname);
            }
            
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);

            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }


        /// <summary>
        ///         
        /// </summary>
        /// <param name="artisantype">艺人类型</param>
        /// <returns></returns>
        public int GetArtisanCount(string artisantype, string artisanname)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ArtisanSatement.GetArtisanCount);
            if (!string.IsNullOrEmpty(artisantype))
            {
                builder.Append(" AND ArtisanType=@ArtisanType ");
            }
            if (!string.IsNullOrEmpty(artisanname))
            {
                builder.Append(" AND artisanName=@artisanName ");
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));

            if (!string.IsNullOrEmpty(artisantype))
            {
                command.AddInputParameter("@ArtisanType", DbType.String, artisantype);
            }
            if (!string.IsNullOrEmpty(artisanname))
            {
                command.AddInputParameter("@artisanName", DbType.String, artisanname);
            }

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
