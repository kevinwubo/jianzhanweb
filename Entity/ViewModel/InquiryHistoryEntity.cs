using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModel
{
    public class InquiryHistoryEntity
    {
        /// <summary>
        /// 询价ID 自动增长
        /// </summary>
        public int PPId { get; set; }

        /// <summary>
        /// 关联产品表 产品ID
        /// </summary>
        public string ProductID { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        public string CustomerName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public string Sex { get; set; }

        //（询价内容+评论内容+手机号+qq号+留言时间+处理时间+处理状态+省份城市+姓名+性别+添加跟踪内容、跟踪状态(有意向、意向很大、意向大、无意向)、下次回访时间）
        /// <summary>
        /// 手机号
        /// </summary>
        public string telphone { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>
        public string WebChartID { get; set; }

        /// <summary>
        /// 询价内容  留言内容
        /// </summary>
        public string InquiryContent { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        public string CommentContent { get; set; }

        /// <summary>
        /// 处理状态  1:已处理 0:未处理
        /// </summary>
        public string ProcessingState { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        public DateTime ProcessingTime { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        public string Provence { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        public string City { get; set; }
        /// <summary>
        /// 跟踪内容
        /// </summary>
        public string TraceContent { get; set; }
        /// <summary>
        /// 跟踪状态(有意向、意向很大、意向大、无意向)
        /// </summary>
        public string TraceState { get; set; }
        /// <summary>
        /// 下次回访时间
        /// </summary>
        public DateTime NextVisitTime { get; set; }

        /// <summary>
        /// 添加时间 留言时间
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        public string OperatorID { get; set; }

        /// <summary>
        /// 历史操作员ID
        /// </summary>
        public string HistoryOperatorID { get; set; }

        /// <summary>
        /// 销售电话
        /// </summary>
        public string SaleTelephone { get; set; }

        /// <summary>
        /// 新
        /// </summary>
        public string status { get; set; }

        /// <summary>
        /// 数据来源 MB:手机 PC:电脑
        /// </summary>
        public string SourceForm { get; set; }

        /// <summary>
        /// 关联产品信息
        /// </summary>
        public ProductEntity product { get; set; }

        /// <summary>
        ///  关联销售
        /// </summary>
        public UserEntity user { get; set; }

        /// <summary>
        /// 转移次数
        /// </summary>
        public int transferCount { get; set; }
    }

}
