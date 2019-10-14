using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.Data
{
    /// <summary>
    /// 数据统计相关
    /// </summary>
    public class StatsRepository : DataAccessBase
    {
        #region 推广数据功能
        /// <summary>
        ///  推广数据
        /// </summary>
        /// <returns></returns>
        public DataSet GetProInquiryAdver(string datetime)
        {
            StringBuilder strSql = new StringBuilder();
            if (string.IsNullOrEmpty(datetime))
            {
                strSql.Append("select CONVERT(varchar(100),  a.AddDate, 23) as date");
                strSql.Append(",SUM(case when SourceForm='MB' and ISNULL(a.status,'')='新' then 1 else 0 end) as MobileCount ");
                strSql.Append(",SUM(case when SourceForm='AD' and ISNULL(a.status,'')='新' then 1 else 0 end) as ADCount ");
                strSql.Append(" , SUM(case when SourceForm='PC' and ISNULL(a.status,'')='新' then 1 else 0 end) as  PCCount  from dbo.dt_proInquiry a, dt_manager b  ");
                strSql.Append(" where  convert(nvarchar(7) ,a.AddDate,23)= convert(nvarchar(7) ,getdate(),23)and a.OperatorID=b.id  and datastatus=0");
                strSql.Append("group by CONVERT(varchar(100),  a.AddDate, 23)  order by date desc");
            }
            else
            {
                strSql.Append("select CONVERT(varchar(100),  a.AddDate, 23) as date");
                strSql.Append(",SUM(case when SourceForm='MB' and ISNULL(a.status,'')='新' then 1 else 0 end) as MobileCount ");
                strSql.Append(",SUM(case when SourceForm='AD' and ISNULL(a.status,'')='新' then 1 else 0 end) as ADCount ");
                strSql.Append(" , SUM(case when SourceForm='PC' and ISNULL(a.status,'')='新' then 1 else 0 end) as  PCCount  from dbo.dt_proInquiry a, dt_manager b  ");
                strSql.Append(" where  convert(nvarchar(7) ,a.AddDate,23)='" + datetime + "'  and a.OperatorID=b.id  and datastatus=0");
                strSql.Append("group by CONVERT(varchar(100),  a.AddDate, 23)  order by date desc");
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(strSql.ToString(), "Text"));
            DataSet ds = command.ExecuteDataSet();

            return ds;
        }



        /// <summary>
        /// 推广数据最新
        /// </summary>
        /// <returns></returns>
        public DataSet GetProInquiryAdver_Total(string datetime)
        {
            StringBuilder strSql = new StringBuilder();
            if (string.IsNullOrEmpty(datetime))
            {
                strSql.Append("select convert(nvarchar(7) ,a.AddDate,23) as date");
                strSql.Append(",SUM(case when SourceForm='MB' and ISNULL(a.status,'')='新' then 1 else 0 end) as MobileCount ");
                strSql.Append(",SUM(case when SourceForm='AD' and ISNULL(a.status,'')='新' then 1 else 0 end) as ADCount ");
                strSql.Append(", SUM(case when SourceForm='PC' and ISNULL(a.status,'')='新' then 1 else 0 end) as  PCCount from dbo.dt_proInquiry a, dt_manager b  ");
                strSql.Append("where convert(nvarchar(7) ,a.AddDate,23)= convert(nvarchar(7) ,getdate(),23) and a.OperatorID=b.id  and datastatus=0");
                strSql.Append("group by convert(nvarchar(7) ,a.AddDate,23)");
            }
            else
            {
                strSql.Append("select convert(nvarchar(7) ,a.AddDate,23) as date");
                strSql.Append(",SUM(case when SourceForm='MB' and ISNULL(a.status,'')='新' then 1 else 0 end) as MobileCount ");
                strSql.Append(",SUM(case when SourceForm='AD' and ISNULL(a.status,'')='新' then 1 else 0 end) as ADCount ");
                strSql.Append(", SUM(case when SourceForm='PC' and ISNULL(a.status,'')='新' then 1 else 0 end) as  PCCount from dbo.dt_proInquiry a, dt_manager b  ");
                strSql.Append("where convert(nvarchar(7) ,a.AddDate,23)= '" + datetime + "' and a.OperatorID=b.id  and datastatus=0");
                strSql.Append("group by convert(nvarchar(7) ,a.AddDate,23)");
            }
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(strSql.ToString(), "Text"));
            DataSet ds = command.ExecuteDataSet();
            return ds;
        }
        #endregion


        #region 咨询量统计
        /// <summary>
        /// 查询销售咨询量
        /// </summary>
        /// <returns></returns>
        public DataSet GetProInquiry(string datetime, string cityname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select CONVERT(varchar(100),  a.AddDate, 23) as date,b.real_name ,b.CityName");
            strSql.Append("    ,SUM(case when status='Hand' then 1 else 0 end) as HandCount ");
            strSql.Append(", SUM(case when status like '%新%' then 1 else 0 end) as  SystemCount from dbo.dt_proInquiry a, dt_manager b  ");
            strSql.Append("where  CONVERT(varchar(100),  a.AddDate, 23)='" + datetime + "'  and a.OperatorID=b.id and b.CityName='" + cityname + "' and datastatus=0 ");//and  a.status like '%新%'
            strSql.Append("group by CONVERT(varchar(100),  a.AddDate, 23),real_name,b.CityName  order by SystemCount asc");
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(strSql.ToString(), "Text"));
            DataSet ds = command.ExecuteDataSet();

            return ds;
        }

        public DataSet GetDateTime(string datetime)
        {
            StringBuilder strSql = new StringBuilder();
            if (!string.IsNullOrEmpty(datetime))
            {
                strSql.Append("select CONVERT(varchar(100),  AddDate, 23) date from dt_proInquiry where  convert(nvarchar(7) ,AddDate,23)= '" + datetime + "' group by  CONVERT(varchar(100),  AddDate, 23) order by date asc");
            }
            else
            {
                strSql.Append("select CONVERT(varchar(100),  AddDate, 23) date from dt_proInquiry where  convert(nvarchar(7) ,AddDate,23)= convert(nvarchar(7) ,getdate(),23) group by  CONVERT(varchar(100),  AddDate, 23) order by date asc");
            }
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(strSql.ToString(), "Text"));
            DataSet ds = command.ExecuteDataSet();

            return ds;
        }

        /// <summary>
        /// 获取当月销售咨询量
        /// </summary>
        /// <returns></returns>
        public DataSet GetStatisticsOfMonth(string month, string cityname)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append(" select convert(nvarchar(7) ,a.AddDate,23) as date,b.real_name,b.cityname ");
            strSql.Append(" ,SUM(case when status='Hand' then 1 else 0 end) as HandCount ");
            strSql.Append(" , SUM(case when status like '%新%' then 1 else 0 end) as  SystemCount from dbo.dt_proInquiry a, dt_manager b  ");
            strSql.Append(" where convert(nvarchar(7) ,a.AddDate,23)= '" + month + "' and b.cityname='" + cityname + "'  ");//and a.status like '%新%'
            strSql.Append(" and a.OperatorID=b.id group by convert(nvarchar(7) ,a.AddDate,23),real_name,b.cityname order by SystemCount asc");
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(strSql.ToString(), "Text"));
            DataSet ds = command.ExecuteDataSet();

            return ds;
        }
        #endregion
    }
}
