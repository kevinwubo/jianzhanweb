using Infrastructure.EntityFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataModel
{
    public class OrderInfo
    {

        
        /// <summary>
        /// 订单表ID 主键
        /// </summary>
        [DataMapping("ID", DbType.Int32)]
        public int ID { get; set; }
        
        /// <summary>
        /// 订单编号
        /// </summary>
        [DataMapping("OrderNo", DbType.String)]
        public string OrderNo { get; set; }
        

        /// <summary>
        /// 支付方式
        /// </summary>
        [DataMapping("PayWay", DbType.String)]
        public string PayWay { get; set; }

        /// <summary>
        /// 实际收款
        /// </summary>
        [DataMapping("CollectedAmount", DbType.Decimal)]
        public decimal CollectedAmount { get; set; }


        /// <summary>
        /// 收款时间
        /// </summary>
        [DataMapping("CollectedDate", DbType.DateTime)]
        public DateTime CollectedDate { get; set; }


        /// <summary>
        /// 运单号
        /// </summary>
        [DataMapping("TransportNumber", DbType.String)]
        public string TransportNumber { get; set; }


        /// <summary>
        /// 发货时间
        /// </summary>
        [DataMapping("DeliveryDate", DbType.DateTime)]
        public DateTime DeliveryDate { get; set; }


        /// <summary>
        /// 省份
        /// </summary>
        [DataMapping("Province", DbType.String)]
        public string Province { get; set; }


        /// <summary>
        /// 城市
        /// </summary>
        [DataMapping("City", DbType.String)]
        public string City { get; set; }


        /// <summary>
        /// 详细地址
        /// </summary>
        [DataMapping("Address", DbType.String)]
        public string Address { get; set; }


        /// <summary>
        /// 收件人
        /// </summary>
        [DataMapping("ReceiverName", DbType.String)]
        public string ReceiverName { get; set; }

        [DataMapping("Telephone", DbType.String)]
        /// <summary>
        /// 手机号
        /// </summary>
        public string Telephone { get; set; }

        [DataMapping("Remark", DbType.String)]
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }


        /// <summary>
        /// 操作人
        /// </summary>
        [DataMapping("OperatorID", DbType.String)]
        public string OperatorID { get; set; }

        /// <summary>
        /// 数据状态 1：使用中 0：已删除
        /// </summary>
       [DataMapping("Status", DbType.String)]
        public int Status { get; set; }



        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMapping("Adddate", DbType.DateTime)]
        public DateTime Adddate { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMapping("ChangeDate", DbType.DateTime)]
        public DateTime ChangeDate { get; set; }
    }

    public class OrderDetailInfo
    {
        /// <summary>
        /// 订单明细ID
        /// </summary>
        [DataMapping("DetailID", DbType.Int32)]
        public int DetailID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>
        [DataMapping("OrderID", DbType.Int32)]
        public int OrderID { get; set; }

        /// <summary>
        /// 师傅/作者
        /// </summary>
        [DataMapping("Author", DbType.String)]

        public string Author { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        [DataMapping("ProductID", DbType.String)]
        public string ProductID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMapping("ProductName", DbType.String)]
        public string ProductName { get; set; }


        /// <summary>
        /// 产品数量
        /// </summary>
        [DataMapping("Quantity", DbType.Int32)]
        public int Quantity { get; set; }


        /// <summary>
        /// 成本价
        /// </summary>
        [DataMapping("CostPrice", DbType.Decimal)]
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        [DataMapping("SalePrice", DbType.Decimal)]
        public decimal SalePrice { get; set; }

        /// <summary>
        /// 实际卖家
        /// </summary>
        [DataMapping("CollectedSalePrice", DbType.Decimal)]
        public decimal CollectedSalePrice { get; set; }



        /// <summary>
        /// 毛利率
        /// </summary>
        [DataMapping("Rate", DbType.Decimal)]
        public decimal Rate { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMapping("Adddate", DbType.DateTime)]
        public DateTime Adddate { get; set; }
    }
}
