using Infrastructure.EntityFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataModel
{
    public class InquiryInfo
    {
        /// <summary>
        /// 询价ID 自动增长
        /// </summary>
        [DataMapping("PPId", DbType.Int32)]
        public int PPId { get; set; }

        /// <summary>
        /// 关联产品表 产品ID
        /// </summary>        
        [DataMapping("ProductID", DbType.String)]
        public string ProductID { get; set; }

        //（询价内容+评论内容+手机号+qq号+留言时间+处理时间+处理状态+省份城市+姓名+性别+添加跟踪内容、跟踪状态(有意向、意向很大、意向大、无意向)、下次回访时间）
        /// <summary>
        /// 手机号
        /// </summary>
        [DataMapping("telphone", DbType.String)]
        public string telphone { get; set; }
        /// <summary>
        /// QQ号
        /// </summary>
        [DataMapping("WebChartID", DbType.String)]
        public string WebChartID { get; set; }

        /// <summary>
        /// 询价内容  留言内容
        /// </summary>
        [DataMapping("InquiryContent", DbType.String)]
        public string InquiryContent { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>
        [DataMapping("CommentContent", DbType.String)]
        public string CommentContent { get; set; }

        /// <summary>
        /// 处理状态  1:已处理 0:未处理
        /// </summary>
        [DataMapping("ProcessingState", DbType.String)]
        public string ProcessingState { get; set; }
        /// <summary>
        /// 处理时间
        /// </summary>
        [DataMapping("ProcessingTime", DbType.DateTime)]
        public DateTime ProcessingTime { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [DataMapping("Provence", DbType.String)]
        public string Provence { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [DataMapping("City", DbType.String)]
        public string City { get; set; }
        /// <summary>
        /// 跟踪内容
        /// </summary>
        [DataMapping("TraceContent", DbType.String)]
        public string TraceContent { get; set; }
        /// <summary>
        /// 跟踪状态(有意向、意向很大、意向大、无意向)
        /// </summary>
        [DataMapping("TraceState", DbType.String)]
        public string TraceState { get; set; }
        /// <summary>
        /// 下次回访时间
        /// </summary>
        [DataMapping("NextVisitTime", DbType.DateTime)]
        public DateTime NextVisitTime { get; set; }

        /// <summary>
        /// 客户姓名
        /// </summary>
        [DataMapping("CustomerName", DbType.String)]
        public string CustomerName { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        [DataMapping("Sex", DbType.String)]
        public string Sex { get; set; }

        /// <summary>
        /// 新
        /// </summary>
        [DataMapping("status", DbType.String)]
        public string status { get; set; }

        /// <summary>
        /// 数据来源 MB:手机 PC:电脑
        /// </summary>
        [DataMapping("SourceForm", DbType.String)]
        public string SourceForm { get; set; }

        /// <summary>
        /// 操作员ID
        /// </summary>
        [DataMapping("OperatorID", DbType.String)]
        public string OperatorID { get; set; }
        /// <summary>
        /// 历史操作员ID
        /// </summary>

        [DataMapping("HistoryOperatorID", DbType.String)]
        public string HistoryOperatorID { get; set; }
        /// <summary>
        /// 销售电话
        /// </summary>
        [DataMapping("SaleTelephone", DbType.String)]
        public string SaleTelephone { get; set; }


        /// <summary>
        /// 数据状态
        /// </summary>
        [DataMapping("datastatus", DbType.String)]
        public String datastatus { get; set; }

        /// <summary>
        /// 添加时间 留言时间
        /// </summary>
        [DataMapping("AddDate", DbType.DateTime)]
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 添加时间 修改时间
        /// </summary>
        [DataMapping("ChangeDate", DbType.DateTime)]
        public DateTime ChangeDate { get; set; }


        public string smsMess { get; set; }
       
    }

    public class DefineInquiryInfo
    {
        /// <summary>
        /// 销售姓名
        /// </summary>
        [DataMapping("SaleName", DbType.String)]
        public string SaleName { get; set; }
        /// <summary>
        /// 当天配置咨询总数量
        /// </summary>
        [DataMapping("salesCount", DbType.Int32)]
        public int salesCount { get; set; }
        /// <summary>
        /// 当天咨询量
        /// </summary>
        [DataMapping("countCurrentDay", DbType.Int32)]
        public int countCurrentDay { get; set; }
    }
}
