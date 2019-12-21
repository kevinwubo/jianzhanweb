using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModel
{
    public class InquiryMonitorEntity
    {
        /// <summary>
        /// 询价ID 自动增长
        /// </summary>
        public int ID { get; set; }

        /// <summary>
        /// 咨询量表ID 
        /// </summary>
        public int PPId { get; set; }


        /// <summary>
        /// 产品ID
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 原始销售ID
        /// </summary>
        public string OriginOperatorID { get; set; }

        /// <summary>
        /// 原始销售姓名
        /// </summary>
        public string OriginSalesName { get; set; }

        /// <summary>
        /// 转移后销售ID
        /// </summary>
        public string NewOperatorID { get; set; }

        /// <summary>
        /// 转移后销售姓名
        /// </summary>
        public string NewSalesName { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 添加时间 
        /// </summary>
        public DateTime CreateDate { get; set; }
    }
}
