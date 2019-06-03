using Infrastructure.EntityFactory;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataModel
{
     /// <summary>
    /// article:实体类
    /// </summary>
    [Serializable]
    public partial class ArticleInfo
    {
        //无参构造函数
        public ArticleInfo() { }

        private int _id;
        private int _channel_id = 0;
        private int _category_id = 0;
        private string _call_index = "";
        private string _title = "";
        private string _link_url = "";
        private string _img_url = "";
        private string _seo_title = "";
        private string _seo_keywords = "";
        private string _seo_description = "";
        private string _zhaiyao = "";
        private string _content;
        private int _sort_id = 99;
        private int _click = 0;
        private int _status = 0;
        private string _groupids_view = "";
        private int _vote_id = 0;
        private int _is_top = 0;
        private int _is_red = 0;
        private int _is_hot = 0;
        private int _is_slide = 0;
        private int _is_sys = 0;
        private int _is_msg = 0;
        private string _user_name;
        private DateTime _add_time = DateTime.Now;
        private DateTime? _update_time;

        /// <summary>
        /// 自增ID
        /// </summary>
        [DataMapping("id", DbType.Int32)]
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 频道ID
        /// </summary>
        [DataMapping("channel_id", DbType.Int32)]
        public int channel_id
        {
            set { _channel_id = value; }
            get { return _channel_id; }
        }
        /// <summary>
        /// 类别ID
        /// </summary>
        [DataMapping("category_id", DbType.Int32)]
        public int category_id
        {
            set { _category_id = value; }
            get { return _category_id; }
        }
        /// <summary>
        /// 调用别名
        /// </summary>
        [DataMapping("call_index", DbType.String)]
        public string call_index
        {
            set { _call_index = value; }
            get { return _call_index; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        [DataMapping("title", DbType.String)]
        public string title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 外部链接
        /// </summary>
        [DataMapping("link_url", DbType.String)]
        public string link_url
        {
            set { _link_url = value; }
            get { return _link_url; }
        }
        /// <summary>
        /// 图片地址
        /// </summary>
        [DataMapping("img_url", DbType.String)]
        public string img_url
        {
            set { _img_url = value; }
            get { return _img_url; }
        }
        /// <summary>
        /// SEO标题
        /// </summary>
        [DataMapping("seo_title", DbType.String)]
        public string seo_title
        {
            set { _seo_title = value; }
            get { return _seo_title; }
        }
        /// <summary>
        /// SEO关健字
        /// </summary>
        [DataMapping("seo_keywords", DbType.String)]
        public string seo_keywords
        {
            set { _seo_keywords = value; }
            get { return _seo_keywords; }
        }
        /// <summary>
        /// SEO描述
        /// </summary>
        [DataMapping("seo_description", DbType.String)]
        public string seo_description
        {
            set { _seo_description = value; }
            get { return _seo_description; }
        }
        /// <summary>
        /// 内容摘要
        /// </summary>
        [DataMapping("zhaiyao", DbType.String)]
        public string zhaiyao
        {
            set { _zhaiyao = value; }
            get { return _zhaiyao; }
        }
        /// <summary>
        /// 详细内容
        /// </summary>
        [DataMapping("content", DbType.String)]
        public string content
        {
            set { _content = value; }
            get { return _content; }
        }
        /// <summary>
        /// 排序
        /// </summary>
        [DataMapping("sort_id", DbType.Int32)]
        public int sort_id
        {
            set { _sort_id = value; }
            get { return _sort_id; }
        }
        /// <summary>
        /// 浏览次数
        /// </summary>
        [DataMapping("click", DbType.Int32)]
        public int click
        {
            set { _click = value; }
            get { return _click; }
        }
        /// <summary>
        /// 状态0正常1未审核2锁定
        /// </summary>
        [DataMapping("status", DbType.Int32)]
        public int status
        {
            set { _status = value; }
            get { return _status; }
        }
        /// <summary>
        /// 阅读权限
        /// </summary>
        [DataMapping("groupids_view", DbType.String)]
        public string groupids_view
        {
            set { _groupids_view = value; }
            get { return _groupids_view; }
        }
        /// <summary>
        /// 关联投票ID
        /// </summary>
        [DataMapping("vote_id", DbType.Int32)]
        public int vote_id
        {
            set { _vote_id = value; }
            get { return _vote_id; }
        }
        /// <summary>
        /// 是否置顶
        /// </summary>
       [DataMapping("is_top", DbType.Int32)]
        public int is_top
        {
            set { _is_top = value; }
            get { return _is_top; }
        }
        /// <summary>
        /// 是否推荐
        /// </summary>
        [DataMapping("is_red", DbType.Int32)]
        public int is_red
        {
            set { _is_red = value; }
            get { return _is_red; }
        }
        /// <summary>
        /// 是否热门
        /// </summary>
    [DataMapping("is_hot", DbType.Int32)]
        public int is_hot
        {
            set { _is_hot = value; }
            get { return _is_hot; }
        }
        /// <summary>
        /// 是否幻灯片
        /// </summary>
        [DataMapping("is_slide", DbType.Int32)]
        public int is_slide
        {
            set { _is_slide = value; }
            get { return _is_slide; }
        }
        /// <summary>
        /// 是否管理员发布0不是1是
        /// </summary>
        [DataMapping("is_sys", DbType.Int32)]
        public int is_sys
        {
            set { _is_sys = value; }
            get { return _is_sys; }
        }
        /// <summary>
        /// 是否允许评论
        /// </summary>
        [DataMapping("is_msg", DbType.Int32)]
        public int is_msg
        {
            set { _is_msg = value; }
            get { return _is_msg; }
        }
        /// <summary>
        /// 用户名
        /// </summary>
        [DataMapping("user_name", DbType.String)]
        public string user_name
        {
            set { _user_name = value; }
            get { return _user_name; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMapping("add_time", DbType.DateTime)]
        public DateTime add_time
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMapping("update_time", DbType.DateTime)]
        public DateTime? update_time
        {
            set { _update_time = value; }
            get { return _update_time; }
        }        
    }
}
