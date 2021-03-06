﻿using Common;
using DataRepository.DataAccess.New;
using DataRepository.DataModel;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.News
{
    public class ArticleRepository : DataAccessBase
    {
        public ArticleInfo GetArticleByID(string id)
        {
            ArticleInfo result = new ArticleInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArticleSatement.GetArticleByID, "Text"));
            command.AddInputParameter("@id", DbType.String, id);
            result = command.ExecuteEntity<ArticleInfo>();
            return result;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category_id"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<ArticleInfo> GetArticleByRule(int category_id, int count)
        {
            List<ArticleInfo> result = new List<ArticleInfo>();


            StringBuilder builder = new StringBuilder();

            if (category_id > 0)
            {
                builder.Append(" AND category_id=@category_id ");
            }
            string sql = string.Format(ArticleSatement.GetArticleByRule, "");
            if (count > 0)
            {
                sql = string.Format(ArticleSatement.GetArticleByRule, " TOP " + count);
            }
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql + builder.ToString() + " Order By add_time desc ", "Text"));

            if (category_id > 0)
            {
                command.AddInputParameter("@category_id", DbType.Int32, category_id);
            }

            result = command.ExecuteEntityList<ArticleInfo>();
            return result;
        }


        public int ModifyContent(ArticleInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArticleSatement.ModifyContent, "Text"));
            command.AddInputParameter("@content", DbType.String, info.content);
            command.AddInputParameter("@update_time", DbType.String, info.update_time);
            command.AddInputParameter("@id", DbType.String, info.id);
            return command.ExecuteNonQuery();
        }

        public int ModifyImageUrlByID(ArticleInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArticleSatement.ModifyImageUrlByID, "Text"));
            command.AddInputParameter("@img_url", DbType.String, info.img_url);
            command.AddInputParameter("@update_time", DbType.String, info.update_time);
            command.AddInputParameter("@id", DbType.String, info.id);
            return command.ExecuteNonQuery();
        }
        

        #region 分页方法
        /// <summary>
        /// 
        /// </summary>
        /// <param name="category_id"></param>
        /// <param name="pager"></param>
        /// <returns></returns>
        public List<ArticleInfo> GetAllArticleInfoByRule(int category_id, PagerInfo pager)
        {
            List<ArticleInfo> result = new List<ArticleInfo>();


            StringBuilder builder = new StringBuilder();

            if (category_id>0)
            {
                builder.Append(" AND category_id=@category_id ");
            }
            
            string sql = ArticleSatement.GetAllArticleInfoPagerHeader + builder.ToString() + ArticleSatement.GetAllArticleInfoPagerFooter;

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));


            if (category_id > 0)
            {
                command.AddInputParameter("@category_id", DbType.Int32, category_id);
            }
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);

            result = command.ExecuteEntityList<ArticleInfo>();
            return result;
        }


        public int GetArticleCount(int category_id)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(ArticleSatement.GetArticleCount);
            if (category_id > 0)
            {
                builder.Append(" AND category_id=@category_id ");
            }
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));

            if (category_id > 0)
            {
                command.AddInputParameter("@category_id", DbType.Int32, category_id);
            }
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }

        public List<ArticleInfo> GetAllArticleInfoPager(PagerInfo pager)
        {
            List<ArticleInfo> result = new List<ArticleInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArticleSatement.GetAllArticleInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<ArticleInfo>();
            return result;
        }
        #endregion
    }
}
