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
    public class InquiryHistoryRepository : DataAccessBase
    {
        public List<InquiryHistoryInfo> GetAllInquiry(int count)
        {
            List<InquiryHistoryInfo> result = new List<InquiryHistoryInfo>();

            string sql = InquiryHistoryStatement.GetAllInquiry;
            if (count > 0)
            {
                sql = string.Format(InquiryHistoryStatement.GetAllInquiryTopCount, count);
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));
            result = command.ExecuteEntityList<InquiryHistoryInfo>();
            return result;
        }

        public List<DefineInquiryInfo> GetLastSaleNameByOperatorID(string OperatorID)
        {
            List<DefineInquiryInfo> result = new List<DefineInquiryInfo>();
            string sqlText = string.Format(InquiryHistoryStatement.GetLastSaleNameByOperatorID, OperatorID);
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            DataSet ds = command.ExecuteDataSet();
            result = ListByDataSet(ds);
            return result;
        }



        public int IntoHistoryInquiry(string keys)
        {
            string sqlText = InquiryHistoryStatement.IntoHistoryInquiry;
            sqlText = sqlText.Replace("#ppids#", keys);
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }
        public List<InquiryHistoryInfo> GetInquiryByKeys(string keys)
        {
            List<InquiryHistoryInfo> result = new List<InquiryHistoryInfo>();
            if (!string.IsNullOrEmpty(keys))
            {
                string sqlText = InquiryHistoryStatement.GetInquiryByKeys;
                sqlText = sqlText.Replace("#ids#", keys);
                DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
                result = command.ExecuteEntityList<InquiryHistoryInfo>();
            }

            return result;
        }

        public List<InquiryHistoryInfo> GetInquiryByRule(string productid, string telephone, string name, string sqlwhere, string status, string OperatorID)
        {
            List<InquiryHistoryInfo> result = new List<InquiryHistoryInfo>();
            string sqlText = InquiryHistoryStatement.GetAllInquiryByRule;
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
            result = command.ExecuteEntityList<InquiryHistoryInfo>();
            return result;
        }

        public InquiryHistoryInfo GetInquiryByKey(long gid)
        {
            InquiryHistoryInfo result = new InquiryHistoryInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryHistoryStatement.GetInquiryByKey, "Text"));
            command.AddInputParameter("@PPID", DbType.String, gid);
            result = command.ExecuteEntity<InquiryHistoryInfo>();
            return result;
        }

        public long CreateSimpleInquiry(InquiryHistoryInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryHistoryStatement.CreateSimpleInquiry, "Text"));
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@telphone", DbType.String, info.telphone);
            command.AddInputParameter("@WebChartID", DbType.String, info.WebChartID);
            command.AddInputParameter("@Provence", DbType.String, info.Provence);
            command.AddInputParameter("@City", DbType.String, info.City);
            command.AddInputParameter("@InquiryContent", DbType.String, info.InquiryContent);
            command.AddInputParameter("@CustomerName", DbType.String, info.CustomerName);
            command.AddInputParameter("@OperatorID", DbType.String, info.OperatorID);
            command.AddInputParameter("@HistoryOperatorID", DbType.String, info.HistoryOperatorID);
            command.AddInputParameter("@status", DbType.String, info.status);
            command.AddInputParameter("@ProcessingState", DbType.String, info.ProcessingState);
            command.AddInputParameter("@SourceForm", DbType.String, info.SourceForm);
            command.AddInputParameter("@TraceState", DbType.String, info.TraceState);
            command.AddInputParameter("@IpAddress", DbType.String, info.IpAddress);
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }


        public long CreateNew(InquiryHistoryInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryHistoryStatement.CreateNewInquiry, "Text"));
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
            command.AddInputParameter("@HistoryOperatorID", DbType.String, info.HistoryOperatorID);
            command.AddInputParameter("@datastatus", DbType.String, info.datastatus);
            return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }

        public int ModifyInquiry(InquiryHistoryInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryHistoryStatement.ModifyInquiry, "Text"));
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@telphone", DbType.String, info.telphone);
            command.AddInputParameter("@WebChartID", DbType.String, info.WebChartID);
            command.AddInputParameter("@InquiryContent", DbType.String, info.InquiryContent);
            command.AddInputParameter("@CommentContent", DbType.String, info.CommentContent);
            command.AddInputParameter("@ProcessingState", DbType.String, info.ProcessingState);
            command.AddInputParameter("@ProcessingTime", DbType.DateTime, DateTime.Now);
            command.AddInputParameter("@Provence", DbType.String, info.Provence);
            command.AddInputParameter("@City", DbType.String, info.City);
            command.AddInputParameter("@TraceContent", DbType.String, info.TraceContent);
            command.AddInputParameter("@TraceState", DbType.String, info.TraceState);
            command.AddInputParameter("@NextVisitTime", DbType.DateTime, info.NextVisitTime);
            command.AddInputParameter("@CustomerName", DbType.String, info.CustomerName);
            command.AddInputParameter("@sex", DbType.String, info.Sex);

            command.AddInputParameter("@status", DbType.String, info.status);
            command.AddInputParameter("@SourceForm", DbType.String, info.SourceForm);            
            command.AddInputParameter("@OperatorID", DbType.String, info.OperatorID);
            //command.AddInputParameter("@ChangeDate", DbType.String, info.ChangeDate);
            command.AddInputParameter("@PPId", DbType.String, info.PPId);
            return command.ExecuteNonQuery();
        }

        public int UpdateOperatorIDByPPId(InquiryHistoryInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryHistoryStatement.UpdateOperatorIDByPPId, "Text"));          
            command.AddInputParameter("@OperatorID", DbType.String, info.OperatorID);            
            command.AddInputParameter("@PPId", DbType.String, info.PPId);
            return command.ExecuteNonQuery();
        }
        

        public int Remove(long InquiryID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryHistoryStatement.Remove, "Text"));
            command.AddInputParameter("@InquiryID", DbType.Int64, InquiryID);
            int result = command.ExecuteNonQuery();
            return result;
        }
        #region 自定义方法

        public List<DefineInquiryInfo> GetLastSaleName(string salenames)
        {
            List<DefineInquiryInfo> result = new List<DefineInquiryInfo>();
            string sqlText = InquiryHistoryStatement.GetLastSaleName;     
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(salenames))
            {
                command.AddInputParameter("@real_name", DbType.String, salenames);
            }
            DataSet  ds= command.ExecuteDataSet();
            result = ListByDataSet(ds);
            return result;
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="ds"></param>
        /// <returns></returns>
        private List<DefineInquiryInfo> ListByDataSet(DataSet ds)
        {
            List<DefineInquiryInfo> result = new List<DefineInquiryInfo>();
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        DefineInquiryInfo info = new DefineInquiryInfo();
                        info.countCurrentDay = dr["countCurrentDay"].ToString().ToInt(0);
                        info.SaleName = dr["real_name"].ToString();
                        info.salesCount = dr["salesCount"].ToString().ToInt(0);
                        info.OperatorID = dr["OperatorID"].ToString();
                        result.Add(info);
                    }
                }
            }
            return result;
        }

        public List<DefineInquiryInfo> GetLastSaleNameByCodes(string salenames)
        {
            List<DefineInquiryInfo> result = new List<DefineInquiryInfo>();
            string sqlText = string.Format(InquiryHistoryStatement.GetLastSaleNameByCodes, salenames);
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            DataSet ds = command.ExecuteDataSet();
            result = ListByDataSet(ds);
            return result;
        }

        #endregion

        #region 分页方法
        public List<InquiryHistoryInfo> GetAllInquiryInfoByRule(string keywords, string tracestate, int dealStatus, string begindate, string enddate, string operatorid, PagerInfo pager)
        {
            List<InquiryHistoryInfo> result = new List<InquiryHistoryInfo>();


            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(keywords))
            {
                builder.Append(" AND (ProductID LIKE '%" + keywords + "%' or telphone LIKE '%" + keywords + "%' or ProductID in(select ProductID from dt_product where Author like '%" + keywords + "%'))");
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
            string sql = InquiryHistoryStatement.GetAllInquiryInfoPagerHeader + builder.ToString() + InquiryHistoryStatement.GetAllInquiryInfoPagerFooter;

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));

            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    command.AddInputParameter("@CustomerName", DbType.Int64, customerName);
            //}
            if (!string.IsNullOrEmpty(tracestate))
            {
                command.AddInputParameter("@TraceState", DbType.String, tracestate);
            }
            if (!string.IsNullOrEmpty(begindate) && !string.IsNullOrEmpty(enddate))
            {
                command.AddInputParameter("@begindate", DbType.String, begindate+" 00:00:01");
                command.AddInputParameter("@enddate", DbType.String, enddate+" 23:59:59");
            }
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);

            result = command.ExecuteEntityList<InquiryHistoryInfo>();
            return result;
        }


        public int GetInquiryCount(string keywords, string tracestate, int dealStatus,string begindate, string enddate,string operatorid)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(InquiryHistoryStatement.GetInquiryCount);
            if (!string.IsNullOrEmpty(keywords))
            {
                builder.Append(" AND (ProductID LIKE '%" + keywords + "%' or telphone LIKE '%" + keywords + "%' or ProductID in(select ProductID from dt_product where Author like '%" + keywords + "%'))");
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
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));

            if (!string.IsNullOrEmpty(tracestate))
            {
                command.AddInputParameter("@TraceState", DbType.String, tracestate);
            }
            if (!string.IsNullOrEmpty(begindate) && !string.IsNullOrEmpty(enddate))
            {
                command.AddInputParameter("@begindate", DbType.String, begindate+" 00:00:01");
                command.AddInputParameter("@enddate", DbType.String, enddate+" 23:59:59");
            }

            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }

        public List<InquiryHistoryInfo> GetAllInquiryInfoPager(PagerInfo pager)
        {
            List<InquiryHistoryInfo> result = new List<InquiryHistoryInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryHistoryStatement.GetAllInquiryInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<InquiryHistoryInfo>();
            return result;
        }
        #endregion
    }
}
