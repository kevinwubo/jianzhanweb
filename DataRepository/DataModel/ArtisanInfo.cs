using Infrastructure.EntityFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataModel
{
    [Serializable]
    public class ArtisanInfo
    {
        /// <summary>
        /// artisanID
        /// </summary>
        [DataMapping("artisanID", DbType.Int32)]
        public int artisanID { get; set; }
        /// <summary>
        /// 艺人姓名
        /// </summary>
        [DataMapping("artisanName", DbType.String)]
        public string artisanName { get; set; }
        /// <summary>
        /// 艺人姓名2
        /// </summary>
        [DataMapping("artisanName2", DbType.String)]
        public string artisanName2 { get; set; }
        /// <summary>
        /// 艺人性别
        /// </summary>
        [DataMapping("sex", DbType.String)]
        public string sex { get; set; }
        /// <summary>
        /// 艺人身份证号
        /// </summary>
        [DataMapping("IDNumber", DbType.String)]
        public string IDNumber { get; set; }
        /// <summary>
        /// 出生日期
        /// </summary>
        [DataMapping("birthday", DbType.DateTime)]
        public string birthday { get; set; }
        /// <summary>
        /// 工作单位
        /// </summary>
        [DataMapping("workPlace", DbType.String)]
        public string workPlace { get; set; }
        /// <summary>
        /// 评审通过日期
        /// </summary>
        [DataMapping("reviewDate", DbType.DateTime)]
        public string reviewDate { get; set; }
        /// <summary>
        /// 工艺师类别
        /// </summary>
        [DataMapping("artisanType", DbType.String)]
        public string artisanType { get; set; }
        /// <summary>
        /// 荣誉称号
        /// </summary>
        [DataMapping("artisanTitle", DbType.String)]
        public string artisanTitle { get; set; }
        /// <summary>
        /// 师从
        /// </summary>
        [DataMapping("masterWorker", DbType.String)]
        public string masterWorker { get; set; }
        /// <summary>
        /// 特点
        /// </summary>
        [DataMapping("artisanSpecial", DbType.String)]
        public string artisanSpecial { get; set; }
        /// <summary>
        /// 简介
        /// </summary>
        [DataMapping("introduction", DbType.String)]
        public string introduction { get; set; }
        /// <summary>
        /// 身份证头像
        /// </summary>
        [DataMapping("IDHead", DbType.String)]
        public string IDHead { get; set; }
        /// <summary>
        /// 详细介绍
        /// </summary>
        [DataMapping("DetailedIntroduction", DbType.String)]
        public string DetailedIntroduction { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        [DataMapping("VideoUrl", DbType.String)]
        public string VideoUrl { get; set; }
        /// <summary>
        /// 是否合作
        /// </summary>
        [DataMapping("IsCooperation", DbType.String)]
        public string IsCooperation { get; set; }
        /// <summary>
        /// 是否推荐到首页
        /// </summary>
        [DataMapping("IsRecommend", DbType.String)]
        public string IsRecommend { get; set; }

        /// <summary>
        /// 是否推荐到商城
        /// </summary>
        [DataMapping("IsPushMall", DbType.String)]
        public string IsPushMall { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [DataMapping("Sort", DbType.Int32)]
        public int Sort { get; set; }

        
        [DataMapping("Adddate", DbType.DateTime)]
        public DateTime Adddate { get; set; }
    }
}
