using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModel
{
    [Serializable]
    public class OrderEntity
    {

        /// <summary>
        /// 订单表ID 主键
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayWay { get; set; }
        /// <summary>
        /// 实际收款
        /// </summary>
        public decimal CollectedAmount { get; set; }
        /// <summary>
        /// 收款时间
        /// </summary>
        public DateTime CollectedDate { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string TransportNumber { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public DateTime DeliveryDate { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string Province { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string ReceiverName { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string Telephone { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// 操作人
        /// </summary>
        public string OperatorID { get; set; }
        /// <summary>
        /// 数据状态 1：使用中 0：已删除
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Adddate { get; set; }

        /// <summary>
        /// 订单明细信息
        /// </summary>
        public List<OrderDetailEntity> orderDetailList { get; set; }

        public string OrderDetailJson { get; set; }

        public City cityinfo { get; set; }

        public Province provinceinfo { get; set; }

        public string provinceandCity { get; set; }
    }


    public class OrderButtonEntity
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNoCss { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public string PayWayCss { get; set; }
        /// <summary>
        /// 实际收款
        /// </summary>
        public string CollectedAmountCss { get; set; }
        /// <summary>
        /// 收款时间
        /// </summary>
        public string CollectedDateCss { get; set; }
        /// <summary>
        /// 运单号
        /// </summary>
        public string TransportNumberCss { get; set; }
        /// <summary>
        /// 发货时间
        /// </summary>
        public string DeliveryDateCss { get; set; }
        /// <summary>
        /// 省份
        /// </summary>
        public string ProvinceCss { get; set; }
        /// <summary>
        /// 城市
        /// </summary>
        public string CityCss { get; set; }
        /// <summary>
        /// 详细地址
        /// </summary>
        public string AddressCss { get; set; }
        /// <summary>
        /// 收件人
        /// </summary>
        public string ReceiverNameCss { get; set; }
        /// <summary>
        /// 手机号
        /// </summary>
        public string TelephoneCss { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string RemarkCss { get; set; }

        public bool isOperator { get; set; }
    }


    [Serializable]
    public class OrderJsonEntity
    {
        public List<OrderDetailJsonEntity> orderDetail { get; set; }
    }

    public class OrderDetailJsonEntity
    {

        /// <summary>
        /// 师傅/作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// 产品数量
        /// </summary>
        public string Quantity { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public string SalePrice { get; set; }

        /// <summary>
        /// 实际卖家
        /// </summary>
        public string CollectedSalePrice { get; set; }

    }


    public class OrderDetailEntity
    {
        /// <summary>
        /// 产品明细ID
        /// </summary>
        public int DetailID { get; set; }

        /// <summary>
        /// 订单ID
        /// </summary>

        public int OrderID { get; set; }
        /// <summary>
        /// 师傅/作者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 产品编号
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ProductName { get; set; }


        /// <summary>
        /// 产品数量
        /// </summary>
        public int Quantity { get; set; }

        /// <summary>
        /// 成本价
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal SalePrice { get; set; }

        /// <summary>
        /// 实际卖家
        /// </summary>
        public decimal CollectedSalePrice { get; set; }

        /// <summary>
        /// 毛利率
        /// </summary>
        public decimal Rate { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime Adddate { get; set; }

    }
}
