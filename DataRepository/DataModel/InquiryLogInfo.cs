using Infrastructure.EntityFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataModel
{
    public class InquiryLogInfo
    {
        //,ProductID,,,,
        /// <summary>
        /// LogID
        /// </summary>
        [DataMapping("LogID", DbType.Int32)]
        public int LogID { get; set; }

        /// <summary>
        /// 关联产品表 产品ID
        /// </summary>        
        [DataMapping("ProductID", DbType.String)]
        public string ProductID { get; set; }

        /// <summary>
        /// 未加密手机号
        /// </summary>        
        [DataMapping("Telephone", DbType.String)]
        public string Telephone { get; set; }

        /// <summary>
        /// 加密手机号
        /// </summary>        
        [DataMapping("JMTelephone", DbType.String)]
        public string JMTelephone { get; set; }

        /// <summary>
        /// 来源 
        /// </summary>        
        [DataMapping("SourceForm", DbType.String)]
        public string SourceForm { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>        
        [DataMapping("CreateDate", DbType.DateTime)]
        public DateTime CreateDate { get; set; }
    }
}
