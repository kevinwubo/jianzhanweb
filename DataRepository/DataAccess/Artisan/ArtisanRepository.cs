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

        public List<ArtisanInfo> GetArtisansByRule(string artisanType, int count, string IsCooperation, string sqlwhere)
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

        public long CreateNew(ArtisanInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.CreateNew, "Text"));
            command.AddInputParameter("@artisanName", DbType.String, info.artisanName);
            command.AddInputParameter("@artisanName2", DbType.String, info.artisanName2);
            command.AddInputParameter("@sex", DbType.String, info.sex);
            command.AddInputParameter("@IDNumber", DbType.String, info.IDNumber);
            command.AddInputParameter("@birthday", DbType.String, info.birthday);
            command.AddInputParameter("@workPlace", DbType.String, info.workPlace);
            command.AddInputParameter("@reviewDate", DbType.String, info.reviewDate);
            command.AddInputParameter("@artisanType", DbType.String, info.artisanType);
            command.AddInputParameter("@artisanTitle", DbType.String, info.artisanTitle);
            command.AddInputParameter("@masterWorker", DbType.String, info.masterWorker);

            command.AddInputParameter("@artisanSpecial", DbType.String, info.artisanSpecial);
            command.AddInputParameter("@introduction", DbType.String, info.introduction);
            command.AddInputParameter("@IDHead", DbType.String, info.IDHead);
            command.AddInputParameter("@DetailedIntroduction", DbType.String, info.DetailedIntroduction);
            command.AddInputParameter("@VideoUrl", DbType.String, info.VideoUrl);
            command.AddInputParameter("@IsCooperation", DbType.String, info.IsCooperation);
            command.AddInputParameter("@IsRecommend", DbType.String, info.IsRecommend);
            command.AddInputParameter("@IsPushMall", DbType.Decimal, info.IsPushMall);
            command.AddInputParameter("@Sort", DbType.Int32, info.Sort);
            command.AddInputParameter("@AddDate", DbType.DateTime, info.Adddate);
            command.AddInputParameter("@update_time", DbType.DateTime, DateTime.Now);

            return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }

        public long Modify(ArtisanInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.Modify, "Text"));
            command.AddInputParameter("@artisanName", DbType.String, info.artisanName);
            command.AddInputParameter("@artisanName2", DbType.String, info.artisanName2);
            command.AddInputParameter("@sex", DbType.String, info.sex);
            command.AddInputParameter("@IDNumber", DbType.String, info.IDNumber);
            command.AddInputParameter("@birthday", DbType.String, info.birthday);
            command.AddInputParameter("@workPlace", DbType.String, info.workPlace);
            command.AddInputParameter("@reviewDate", DbType.String, info.reviewDate);
            command.AddInputParameter("@artisanType", DbType.String, info.artisanType);
            command.AddInputParameter("@artisanTitle", DbType.String, info.artisanTitle);
            command.AddInputParameter("@masterWorker", DbType.String, info.masterWorker);

            command.AddInputParameter("@artisanSpecial", DbType.String, info.artisanSpecial);
            command.AddInputParameter("@introduction", DbType.String, info.introduction);
            command.AddInputParameter("@IDHead", DbType.String, info.IDHead);
            command.AddInputParameter("@DetailedIntroduction", DbType.String, info.DetailedIntroduction);
            command.AddInputParameter("@VideoUrl", DbType.String, info.VideoUrl);
            command.AddInputParameter("@IsCooperation", DbType.String, info.IsCooperation);
            command.AddInputParameter("@IsRecommend", DbType.String, info.IsRecommend);
            command.AddInputParameter("@IsPushMall", DbType.Decimal, info.IsPushMall);
            command.AddInputParameter("@Sort", DbType.Int32, info.Sort);            
            command.AddInputParameter("@update_time", DbType.DateTime, DateTime.Now);

            command.AddInputParameter("@artisanID", DbType.Int32, info.artisanID);            
            return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }


        public long ModifySort(ArtisanInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.ModifySort, "Text"));
            command.AddInputParameter("@Sort", DbType.Int32, info.Sort);
            command.AddInputParameter("@update_time", DbType.DateTime, DateTime.Now);
            command.AddInputParameter("@artisanID", DbType.Int32, info.artisanID);
            return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }


        public int Remove(int artisanID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.Remove, "Text"));
            command.AddInputParameter("@artisanID", DbType.Int32, artisanID);
            int result = command.ExecuteNonQuery();
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
