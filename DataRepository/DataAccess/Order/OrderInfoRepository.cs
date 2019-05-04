using Common;
using DataRepository.DataModel;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.Order
{
    public class OrderInfoRepository : DataAccessBase
    {
        public List<OrderInfo> GetAllOrder()
        {
            List<OrderInfo> result = new List<OrderInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.GetAllOrderInfo, "Text"));
            result = command.ExecuteEntityList<OrderInfo>();
            return result;
        }

        public List<OrderInfo> GetOrderByKeys(string keys)
        {
            List<OrderInfo> result = new List<OrderInfo>();
            if (!string.IsNullOrEmpty(keys))
            {
                string sqlText = OrderInfoStatement.GetOrderInfoByKeys;
                sqlText = sqlText.Replace("#ids#", keys);
                DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
                result = command.ExecuteEntityList<OrderInfo>();
            }

            return result;
        }

        public List<OrderInfo> GetOrderByRule(string name, int status)
        {
            List<OrderInfo> result = new List<OrderInfo>();
            string sqlText = OrderInfoStatement.GetAllOrderInfoByRule;
            if (!string.IsNullOrEmpty(name))
            {
                sqlText += " AND OrderName LIKE '%'+@key+'%'";
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

            result = command.ExecuteEntityList<OrderInfo>();
            return result;
        }

        public OrderInfo GetOrderByKey(long gid)
        {
            OrderInfo result = new OrderInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.GetOrderInfoByKey, "Text"));
            command.AddInputParameter("@ID", DbType.String, gid);
            result = command.ExecuteEntity<OrderInfo>();
            return result;
        }

        public long CreateNew(OrderInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.CreateNewOrderInfo, "Text"));
            command.AddInputParameter("@OrderNo", DbType.String, info.OrderNo);
            command.AddInputParameter("@PayWay", DbType.String, info.PayWay);
            command.AddInputParameter("@CollectedAmount", DbType.Decimal, info.CollectedAmount);
            command.AddInputParameter("@CollectedDate", DbType.DateTime, info.CollectedDate);
            command.AddInputParameter("@TransportNumber", DbType.String, info.TransportNumber);
            command.AddInputParameter("@DeliveryDate", DbType.DateTime, info.DeliveryDate);
            command.AddInputParameter("@Province", DbType.String, info.Province);
            command.AddInputParameter("@City", DbType.String, info.City);
            command.AddInputParameter("@Address", DbType.String, info.Address);
            command.AddInputParameter("@ReceiverName", DbType.String, info.ReceiverName);
            command.AddInputParameter("@Telephone", DbType.String, info.Telephone);
            command.AddInputParameter("@Remark", DbType.String, info.Remark);
            command.AddInputParameter("@Status", DbType.Int32, info.Status);
            command.AddInputParameter("@AddDate", DbType.String, info.Adddate);
            command.AddInputParameter("@ChangeDate", DbType.String, info.ChangeDate);
            command.AddInputParameter("@OperatorID", DbType.String, info.OperatorID);
            return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }

        public int ModifyOrder(OrderInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.ModifyOrderInfo, "Text"));
            command.AddInputParameter("@OrderNo", DbType.String, info.OrderNo);
            command.AddInputParameter("@PayWay", DbType.String, info.PayWay);
            command.AddInputParameter("@CollectedAmount", DbType.Decimal, info.CollectedAmount);
            command.AddInputParameter("@CollectedDate", DbType.DateTime, info.CollectedDate);
            command.AddInputParameter("@TransportNumber", DbType.String, info.TransportNumber);
            command.AddInputParameter("@DeliveryDate", DbType.DateTime, info.DeliveryDate);
            command.AddInputParameter("@Province", DbType.String, info.Province);
            command.AddInputParameter("@City", DbType.String, info.City);
            command.AddInputParameter("@Address", DbType.String, info.Address);
            command.AddInputParameter("@ReceiverName", DbType.String, info.ReceiverName);
            command.AddInputParameter("@Telephone", DbType.String, info.Telephone);
            command.AddInputParameter("@Remark", DbType.String, info.Remark);
            command.AddInputParameter("@Status", DbType.Int32, info.Status);
            command.AddInputParameter("@ChangeDate", DbType.DateTime, info.ChangeDate);
            command.AddInputParameter("@OperatorID", DbType.String, info.OperatorID);
            command.AddInputParameter("@ID", DbType.Int32, info.ID);
            return command.ExecuteNonQuery();
        }

        public int Remove(long ID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.Remove, "Text"));
            command.AddInputParameter("@ID", DbType.Int64, ID);
            int result = command.ExecuteNonQuery();
            return result;
        }


        public List<OrderInfo> GetAllOrderInfoByRule(string title, int status)
        {
            List<OrderInfo> result = new List<OrderInfo>();
            string sqlText = OrderInfoStatement.GetAllOrderInfoByRule;
            if (!string.IsNullOrEmpty(title))
            {
                sqlText += " AND Title like '%" + title + "'%";
            }
            if (status != -1)
            {
                sqlText += " AND Status = '" + status + "'";
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            result = command.ExecuteEntityList<OrderInfo>();
            return result;
        }

        #region 订单明细相关
        public long CreateNewOrderDetail(OrderDetailInfo info)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.CreateNewOrderDetailInfo, "Text"));
            command.AddInputParameter("@OrderID", DbType.String, info.OrderID);
            command.AddInputParameter("@Author", DbType.String, info.Author);
            command.AddInputParameter("@ProductID", DbType.String, info.ProductID);
            command.AddInputParameter("@ProductName", DbType.String, info.ProductName);
            command.AddInputParameter("@Quantity", DbType.String, info.Quantity);
            command.AddInputParameter("@CostPrice", DbType.Decimal, info.CostPrice);
            command.AddInputParameter("@SalePrice", DbType.Decimal, info.SalePrice);
            command.AddInputParameter("@CollectedSalePrice", DbType.Decimal, info.CollectedSalePrice);
            command.AddInputParameter("@Rate", DbType.Decimal, info.Rate);
            command.AddInputParameter("@AddDate", DbType.DateTime, info.Adddate);
            return command.ExecuteNonQuery();
            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }


        public List<OrderDetailInfo> GetAllOrderDetailInfoByRule(int orderid)
        {
            List<OrderDetailInfo> result = new List<OrderDetailInfo>();
            string sqlText = OrderInfoStatement.GetAllOrderDetailInfoByRule;
            if (orderid > 0)
            {
                sqlText += " AND OrderID =@OrderID";
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (orderid > 0)
            {
                command.AddInputParameter("@OrderID", DbType.Int32, orderid);
            }
            result = command.ExecuteEntityList<OrderDetailInfo>();
            return result;
        }

        public int RemoveOrderDetail(long ID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.RemoveOrderDetailInfo, "Text"));
            command.AddInputParameter("@DetailID", DbType.Int64, ID);
            int result = command.ExecuteNonQuery();
            return result;
        }

        public int RemoveOrderDetailByOrderID(int ID)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.RemoveOrderDetailInfoByOrderID, "Text"));
            command.AddInputParameter("@OrderID", DbType.Int32, ID);
            int result = command.ExecuteNonQuery();
            return result;
        }
        #endregion

        #region 分页方法
        public List<OrderInfo> GetAllOrderInfoByRule(string keywords, string receivername, int status, PagerInfo pager)
        {
            List<OrderInfo> result = new List<OrderInfo>();


            StringBuilder builder = new StringBuilder();

            if (!string.IsNullOrEmpty(keywords))
            {
                builder.Append(" AND (ProductID LIKE '%'+@keywords+'%' or telphone LIKE '%'+@keywords+'%' )");
            }
            if (!string.IsNullOrEmpty(receivername))
            {
                builder.Append(" AND ReceiverName=@ReceiverName ");
            }

            string sql = OrderInfoStatement.GetAllOrderInfoPagerHeader + builder.ToString() + OrderInfoStatement.GetAllOrderInfoPagerFooter;

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sql, "Text"));

            //if (!string.IsNullOrEmpty(customerName))
            //{
            //    command.AddInputParameter("@CustomerName", DbType.Int64, customerName);
            //}
            if (!string.IsNullOrEmpty(receivername))
            {
                command.AddInputParameter("@ReceiverName", DbType.String, receivername);
            }
            //if (dealStatus > -1)
            //{
            //    command.AddInputParameter("@DealStatus", DbType.Int32, dealStatus);
            //}
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);

            result = command.ExecuteEntityList<OrderInfo>();
            return result;
        }


        public int GetOrderCount(string name, string receivername, int status)
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(OrderInfoStatement.GetOrderInfoCount);
            if (!string.IsNullOrEmpty(name))
            {
                //builder.Append(" AND CustomerName LIKE '%'+@CustomerName+'%' ");
            }
            if (!string.IsNullOrEmpty(receivername))
            {
                builder.Append(" AND ReceiverName=@ReceiverName ");
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(builder.ToString(), "Text"));

            if (!string.IsNullOrEmpty(receivername))
            {
                command.AddInputParameter("@ReceiverName", DbType.String, receivername);
            }


            var o = command.ExecuteScalar<object>();
            return Convert.ToInt32(o);
        }

        public List<OrderInfo> GetAllOrderInfoPager(PagerInfo pager)
        {
            List<OrderInfo> result = new List<OrderInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(OrderInfoStatement.GetAllOrderInfoPager, "Text"));
            command.AddInputParameter("@PageIndex", DbType.Int32, pager.PageIndex);
            command.AddInputParameter("@PageSize", DbType.Int32, pager.PageSize);
            command.AddInputParameter("@recordCount", DbType.Int32, pager.SumCount);
            result = command.ExecuteEntityList<OrderInfo>();
            return result;
        }
        #endregion
    }
}
