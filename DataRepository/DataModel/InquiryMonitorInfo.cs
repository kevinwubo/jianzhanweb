using Infrastructure.EntityFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataModel
{
    public class InquiryMonitorInfo
    {
        /// <summary>
        /// 询价ID 自动增长
        /// </summary>
        [DataMapping("ID", DbType.Int32)]
        public int ID { get; set; }

        /// <summary>
        /// 咨询量表ID 
        /// </summary>
        [DataMapping("PPId", DbType.Int32)]
        public int PPId { get; set; }


        /// <summary>
        /// 产品ID
        /// </summary>
        [DataMapping("ProductID", DbType.String)]
        public string ProductID { get; set; }

        /// <summary>
        /// 原始销售ID
        /// </summary>
        [DataMapping("OriginOperatorID", DbType.String)]
        public string OriginOperatorID { get; set; }

        /// <summary>
        /// 原始销售姓名
        /// </summary>
        [DataMapping("OriginSalesName", DbType.String)]
        public string OriginSalesName { get; set; }

        /// <summary>
        /// 转移后销售ID
        /// </summary>
        [DataMapping("NewOperatorID", DbType.String)]
        public string NewOperatorID { get; set; }

        /// <summary>
        /// 转移后销售姓名
        /// </summary>
        [DataMapping("NewSalesName", DbType.String)]
        public string NewSalesName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMapping("Remark", DbType.String)]
        public string Remark { get; set; }

        /// <summary>
        /// 添加时间 
        /// </summary>
        [DataMapping("CreateDate", DbType.DateTime)]
        public DateTime CreateDate { get; set; }
    }
}
