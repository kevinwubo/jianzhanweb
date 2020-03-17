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
    public class InquiryLogHistoryRepository : DataAccessBase
    {
        public List<InquiryHistoryLogInfo> GetAllInquiry()
        {
            List<InquiryHistoryLogInfo> result = new List<InquiryHistoryLogInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryLogStatement.GetAllInquiry, "Text"));
            result = command.ExecuteEntityList<InquiryHistoryLogInfo>();
            return result;
        }

        
        public List<InquiryHistoryLogInfo> GetInquiryByKeys(string keys)
        {
            List<InquiryHistoryLogInfo> result = new List<InquiryHistoryLogInfo>();
            if (!string.IsNullOrEmpty(keys))
            {
                string sqlText = InquiryLogStatement.GetInquiryByKeys;
                sqlText = sqlText.Replace("#ids#", keys);
                DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
                result = command.ExecuteEntityList<InquiryHistoryLogInfo>();
            }

            return result;
        }

        public List<InquiryHistoryLogInfo> GetInquiryByRule(string productid, string telephone, string name, string sqlwhere, string status, string OperatorID)
        {
            List<InquiryHistoryLogInfo> result = new List<InquiryHistoryLogInfo>();
            string sqlText = InquiryLogStatement.GetAllInquiryByRule;
            if (!string.IsNullOrEmpty(name))
            {
                sqlText += " AND InquiryName LIKE '%'+@key+'%'";
            }
            if (!string.IsNullOrEmpty(status))
            {
                sqlText += " AND status=@status ";
            }
            if (!string.IsNullOrEmpty(productid))
            {
                sqlText += " AND ProductID=@ProductID ";
            }
            if (!string.IsNullOrEmpty(telephone))
            {
                sqlText += " AND telphone=@telphone ";
            }
            if (!string.IsNullOrEmpty(OperatorID))
            {
                sqlText += " AND OperatorID=@OperatorID ";
            }
            if (!string.IsNullOrEmpty(sqlwhere) )
            {
                sqlText += sqlwhere;
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(name))
            {
                command.AddInputParameter("@key", DbType.String, name);
            }
            if (!string.IsNullOrEmpty(status))
            {
                command.AddInputParameter("@Status", DbType.String, status);
            }
            if (!string.IsNullOrEmpty(productid))
            {
                command.AddInputParameter("@ProductID", DbType.String, productid);
            }
            if (!string.IsNullOrEmpty(telephone))
            {
                command.AddInputParameter("@telphone", DbType.String, telephone);
            }
            if (!string.IsNullOrEmpty(OperatorID))
            {
                command.AddInputParameter("@OperatorID", DbType.String, OperatorID);
            }
            result = command.ExecuteEntityList<InquiryHistoryLogInfo>();
            return result;
        }

        public InquiryHistoryLogInfo GetInquiryByKey(long gid)
        {
            InquiryHistoryLogInfo result = new InquiryHistoryLogInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryLogStatement.GetInquiryByKey, "Text"));
            command.AddInputParameter("@PPID", DbType.String, gid);
            result = command.ExecuteEntity<InquiryHistoryLogInfo>();
            return result;
        }


        

        public long CreateNew(InquiryHistoryLogInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryLogStatement.CreateNewInquiry, "Text"));
            command.AddInputParameter("@ProductID", DbType.String, !string.IsNullOrEmpty(info.ProductID) ? info.ProductID : "");
            command.AddInputParameter("@telphone", DbType.String, info.telphone);
            command.AddInputParameter("@WebChartID", DbType.String, info.WebChartID);
            command.AddInputParameter("@ProcessingState", DbType.String, info.ProcessingState);
            command.AddInputParameter("@Provence", DbType.String, info.Provence);
            command.AddInputParameter("@City", DbType.String, info.City);
            command.AddInputParameter("@TraceState", DbType.String, info.TraceState);
            command.AddInputParameter("@CustomerName", DbType.String, info.CustomerName);

            command.AddInputParameter("@status", DbType.String, info.status);
            command.AddInputParameter("@SourceForm", DbType.String, info.SourceForm);
            command.AddInputParameter("@AddDate", DbType.DateTime, info.AddDate);
            command.AddInputParameter("@OperatorID", DbType.String, info.OperatorID);
            command.AddInputParameter("@HistoryOperatorID", DbType.String, info.HistoryOperatorID);
            command.AddInputParameter("@datastatus", DbType.String, info.datastatus);
            command.AddInputParameter("@ChangeDate", DbType.DateTime, info.ChangeDate);
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }

        
        public int Remove(long InquiryID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryLogStatement.Remove, "Text"));
            command.AddInputParameter("@InquiryID", DbType.Int64, InquiryID);
            int result = command.ExecuteNonQuery();
            return result;
        }

        #region 分页方法
        public List<InquiryHistoryLogInfo> GetAllInquiryInfoByRule(string keywords, string tracestate, int status, string begindate, string enddate, string operatorid, string sqlwhere, PagerInfo pager)
        {
            List<InquiryHistoryLogInfo> result = new List<InquiryHistoryLogInfo>();


            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(keywords))
            {
                builder.Append(" AND (ProductID LIKE '%" + keywords + "%' or telphone LIKE '%" + keywords + "%'  or telphone LIKE '%" +StringHelper.ConvertBy123(keywords) + "%' or ProductID in(select ProductID from dt_product where Author like '%" + keywords + "%'))");
            }
            if (!string.IsNullOrEmpty(tracestate))
            {
                builder.Append(" AND TraceState=@TraceState ");
            }
            if (!string.IsNullOrEmpty(begindate) && !string.IsNullOrEmpty(enddate))
            {
                builder.Append(" AND AddDate BETWEEN @begindate AND @enddate ");
            }
            if (!string.IsNullOrEmpty(operatorid))//根据销售编号查询
            {
                builder.Append(" AND OperatorID IN(" + operatorid + ") ");
            }
            if (status > -1)
            {
                builder.Append(" AND ProcessingState=@ProcessingState ");//ProcessingState 0未跟踪  1已跟踪
            }
            if (!string.IsNullOrEmpty(sqlwhere))
            {
                builder.Append(sqlwhere);
            }

            string sql = InquiryLogStatement.GetAllInquiryInfoPagerHeader + builder.ToString() + InquiryLogStatement.GetAllInquiryInfoPagerFooter;

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));

            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    command.AddInputParameter("@CustomerName", DbType.Int64, customerName);
            //}
            if (!string.IsNullOrEmpty(tracestate))
            {
                command.AddInputParameter("@TraceState", DbType.String, tracestate);
            }
            if (status > -1)
            {
                command.AddInputParameter("@ProcessingState", DbType.String, status);//ProcessingState 0未跟踪  1已跟踪
            }
            if (!string.IsNullOrEmpty(begindate) && !string.IsNullOrEmpty(enddate))
            {
                command.AddInputParameter("@begindate", DbType.String, begindate+" 00:00:01");
                command.AddInputParameter("@enddate", DbType.String, enddate+" 23:59:59");
            }
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);

            result = command.ExecuteEntityList<InquiryHistoryLogInfo>();
            return result;
        }


        public int GetInquiryCount(string keywords, string tracestate, int status, string begindate, string enddate, string operatorid, string sqlwhere)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(InquiryLogStatement.GetInquiryCount);
            if (!string.IsNullOrEmpty(keywords))
            {
                builder.Append(" AND (ProductID LIKE '%" + keywords + "%' or telphone LIKE '%" + keywords + "%' or telphone LIKE '%" + StringHelper.ConvertBy123(keywords) + "%' or ProductID in(select ProductID from dt_product where Author like '%" + keywords + "%'))");
            }
            if (!string.IsNullOrEmpty(tracestate))
            {
                builder.Append(" AND TraceState=@TraceState ");
            }
            if (!string.IsNullOrEmpty(begindate) && !string.IsNullOrEmpty(enddate))
            {
                builder.Append(" AND AddDate BETWEEN @begindate AND @enddate ");
            }
            if (!string.IsNullOrEmpty(operatorid))//根据销售编号查询
            {
                builder.Append(" AND OperatorID IN(" + operatorid + ") ");
            }
            if (status > -1)
            {
                builder.Append(" AND ProcessingState=@ProcessingState ");//ProcessingState 0未跟踪  1已跟踪
            }
            if (!string.IsNullOrEmpty(sqlwhere))
            {
                builder.Append(sqlwhere);
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));

            if (!string.IsNullOrEmpty(tracestate))
            {
                command.AddInputParameter("@TraceState", DbType.String, tracestate);
            }
            if (status > -1)
            {
                command.AddInputParameter("@ProcessingState", DbType.String, status);//ProcessingState 0未跟踪  1已跟踪
            }
            if (!string.IsNullOrEmpty(begindate) && !string.IsNullOrEmpty(enddate))
            {
                command.AddInputParameter("@begindate", DbType.String, begindate+" 00:00:01");
                command.AddInputParameter("@enddate", DbType.String, enddate+" 23:59:59");
            }

            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }

        public List<InquiryHistoryLogInfo> GetAllInquiryInfoPager(PagerInfo pager)
        {
            List<InquiryHistoryLogInfo> result = new List<InquiryHistoryLogInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryLogStatement.GetAllInquiryInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<InquiryHistoryLogInfo>();
            return result;
        }
        #endregion
    }
}
