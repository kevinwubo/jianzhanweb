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
    public class InquiryMonitorRepository : DataAccessBase
    {

        public long CreateNew(InquiryMonitorInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryMonitorStatement.CreateNewInquiry, "Text"));
            command.AddInputParameter("@PPId", DbType.String, info.PPId);
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@OriginOperatorID", DbType.String, info.OriginOperatorID);
            command.AddInputParameter("@OriginSalesName", DbType.String, info.OriginSalesName);
            command.AddInputParameter("@NewOperatorID", DbType.String, info.NewOperatorID);
            command.AddInputParameter("@NewSalesName", DbType.String, info.NewSalesName);
            command.AddInputParameter("@Remark", DbType.String, info.Remark);
            command.AddInputParameter("@CreateDate", DbType.DateTime, info.CreateDate);
            return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }


        #region 分页方法
        public List<InquiryMonitorInfo> GetAllInquiryMonitorInfoByRule(PagerInfo pager)
        {
            List<InquiryMonitorInfo> result = new List<InquiryMonitorInfo>();


            StringBuilder builder = new StringBuilder();

            string sql = InquiryMonitorStatement.GetAllInquiryMonitorInfoPagerHeader + builder.ToString() + InquiryMonitorStatement.GetAllInquiryMonitorInfoPagerFooter;

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));

            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);

            result = command.ExecuteEntityList<InquiryMonitorInfo>();
            return result;
        }


        public int GetInquiryMonitorCount()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(InquiryMonitorStatement.GetInquiryMonitorCount);
           
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));


            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }

        public List<InquiryMonitorInfo> GetAllInquiryMonitorInfoPager(PagerInfo pager)
        {
            List<InquiryMonitorInfo> result = new List<InquiryMonitorInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(InquiryMonitorStatement.GetAllInquiryMonitorInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<InquiryMonitorInfo>();
            return result;
        }
        #endregion
    }
}
