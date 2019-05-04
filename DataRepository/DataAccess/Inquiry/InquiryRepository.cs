using Common;
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
    public class InquiryRepository : DataAccessBase
    {
        public List<InquiryInfo> GetAllInquiry()
        {
            List<InquiryInfo> result = new List<InquiryInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryStatement.GetAllInquiry, "Text"));
            result = command.ExecuteEntityList<InquiryInfo>();
            return result;
        }

        public List<InquiryInfo> GetInquiryByKeys(string keys)
        {
            List<InquiryInfo> result = new List<InquiryInfo>();
            if (!string.IsNullOrEmpty(keys))
            {
                string sqlText = InquiryStatement.GetInquiryByKeys;
                sqlText = sqlText.Replace("#ids#", keys);
                DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
                result = command.ExecuteEntityList<InquiryInfo>();
            }

            return result;
        }

        public List<InquiryInfo> GetInquiryByRule(string name, int status)
        {
            List<InquiryInfo> result = new List<InquiryInfo>();
            string sqlText = InquiryStatement.GetAllInquiryByRule;
            if (!string.IsNullOrEmpty(name))
            {
                sqlText += " AND InquiryName LIKE '%'+@key+'%'";
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

            result = command.ExecuteEntityList<InquiryInfo>();
            return result;
        }

        public InquiryInfo GetInquiryByKey(long gid)
        {
            InquiryInfo result = new InquiryInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryStatement.GetInquiryByKey, "Text"));
            command.AddInputParameter("@InquiryID", DbType.String, gid);
            result = command.ExecuteEntity<InquiryInfo>();
            return result;
        }

        public long CreateNew(InquiryInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryStatement.CreateNewInquiry, "Text"));
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@telphone", DbType.String, info.telphone);
            command.AddInputParameter("@WebChartID", DbType.String, info.WebChartID);
            command.AddInputParameter("@InquiryContent", DbType.String, info.InquiryContent);
            command.AddInputParameter("@CommentContent", DbType.String, info.CommentContent);
            command.AddInputParameter("@ProcessingState", DbType.String, info.ProcessingState);
            command.AddInputParameter("@ProcessingTime", DbType.String, info.ProcessingTime);
            command.AddInputParameter("@Provence", DbType.String, info.Provence);
            command.AddInputParameter("@City", DbType.String, info.City);
            command.AddInputParameter("@TraceContent", DbType.String, info.TraceContent);
            command.AddInputParameter("@TraceState", DbType.String, info.TraceState);
            command.AddInputParameter("@NextVisitTime", DbType.String, info.NextVisitTime);
            command.AddInputParameter("@CustomerName", DbType.String, info.CustomerName);
            command.AddInputParameter("@sex", DbType.String, info.Sex);

            command.AddInputParameter("@status", DbType.String, info.status);
            command.AddInputParameter("@SourceForm", DbType.String, info.SourceForm);
            command.AddInputParameter("@AddDate", DbType.String, info.AddDate);
            command.AddInputParameter("@OperatorID", DbType.String, info.OperatorID);
            //command.AddInputParameter("@datastatus", DbType.String, info.datastatus);
            return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }

        public int ModifyInquiry(InquiryInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryStatement.ModifyInquiry, "Text"));
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@telphone", DbType.String, info.telphone);
            command.AddInputParameter("@WebChartID", DbType.String, info.WebChartID);
            command.AddInputParameter("@InquiryContent", DbType.String, info.InquiryContent);
            command.AddInputParameter("@CommentContent", DbType.String, info.CommentContent);
            command.AddInputParameter("@ProcessingState", DbType.String, info.ProcessingState);
            command.AddInputParameter("@ProcessingTime", DbType.String, info.ProcessingTime);
            command.AddInputParameter("@Provence", DbType.String, info.Provence);
            command.AddInputParameter("@City", DbType.String, info.City);
            command.AddInputParameter("@TraceContent", DbType.String, info.TraceContent);
            command.AddInputParameter("@TraceState", DbType.String, info.TraceState);
            command.AddInputParameter("@NextVisitTime", DbType.String, info.NextVisitTime);
            command.AddInputParameter("@CustomerName", DbType.String, info.CustomerName);
            command.AddInputParameter("@sex", DbType.String, info.Sex);

            command.AddInputParameter("@status", DbType.String, info.status);
            command.AddInputParameter("@SourceForm", DbType.String, info.SourceForm);            
            command.AddInputParameter("@OperatorID", DbType.String, info.OperatorID);
            command.AddInputParameter("@ChangeDate", DbType.String, info.ChangeDate);
            command.AddInputParameter("@PPId", DbType.String, info.PPId);
            return command.ExecuteNonQuery();
        }

        public int Remove(long InquiryID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryStatement.Remove, "Text"));
            command.AddInputParameter("@InquiryID", DbType.Int64, InquiryID);
            int result = command.ExecuteNonQuery();
            return result;
        }


        public List<InquiryInfo> GetAllInventoryByRule(string title, int status)
        {
            List<InquiryInfo> result = new List<InquiryInfo>();
            string sqlText = InquiryStatement.GetAllInquiryByRule;
            if (!string.IsNullOrEmpty(title))
            {
                sqlText += " AND Title like '%" + title + "'%";
            }
            if (status != -1)
            {
                sqlText += " AND Status = '" + status + "'";
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            result = command.ExecuteEntityList<InquiryInfo>();
            return result;
        }


        #region 分页方法
        public List<InquiryInfo> GetAllInquiryInfoByRule(string keywords, string tracestate, int dealStatus, PagerInfo pager)
        {
            List<InquiryInfo> result = new List<InquiryInfo>();


            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(keywords))
            {
                builder.Append(" AND (ProductID LIKE '%'+@keywords+'%' or telphone LIKE '%'+@keywords+'%' )");
            }
            if (!string.IsNullOrEmpty(tracestate))
            {
                builder.Append(" AND TraceState=@TraceState ");
            }

            string sql = InquiryStatement.GetAllInquiryInfoPagerHeader + builder.ToString() + InquiryStatement.GetAllInquiryInfoPagerFooter;

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));

            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    command.AddInputParameter("@CustomerName", DbType.Int64, customerName);
            //}
            if (!string.IsNullOrEmpty(tracestate))
            {
                command.AddInputParameter("@TraceState", DbType.String, tracestate);
            }
            //if (dealStatus > -1)
            //{
            //    command.AddInputParameter("@DealStatus", DbType.Int32, dealStatus);
            //}
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);

            result = command.ExecuteEntityList<InquiryInfo>();
            return result;
        }


        public int GetInquiryCount(string name, string tracestate, int dealStatus)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(InquiryStatement.GetInquiryCount);
            if (!string.IsNullOrEmpty(name))
            {
                //builder.Append(" AND CustomerName LIKE '%'+@CustomerName+'%' ");
            }
            if (!string.IsNullOrEmpty(tracestate))
            {
                builder.Append(" AND TraceState=@TraceState ");
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));

            if (!string.IsNullOrEmpty(tracestate))
            {
                command.AddInputParameter("@TraceState", DbType.String, tracestate);
            }


            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }

        public List<InquiryInfo> GetAllInquiryInfoPager(PagerInfo pager)
        {
            List<InquiryInfo> result = new List<InquiryInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryStatement.GetAllInquiryInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<InquiryInfo>();
            return result;
        }
        #endregion
    }
}
