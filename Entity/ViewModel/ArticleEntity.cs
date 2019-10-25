using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entity.ViewModel
{
    public class ArticleEntity
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string articleTitle { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        public string zhaiyao { get; set; }
        /// <summary>
        /// 文章图片
        /// </summary>
        public string img_url { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string content { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime AddDate { get; set; }

        /// <summary>
        /// 别名 测试1,测试2  格式
        /// </summary>
        public string Call_Index { get; set; }

        /// <summary>
        /// 文章分类
        /// </summary>
        public string articleType { get; set; }
        
    }


    public partial class article
    {
        //无参构造函数
        public article() { }

        private int _id;
        private int _channel_id = 0;
        private int _category_id = 0;
        private string _call_index = "";
        private string _title = "";
        private string _link_url = "";
        private string _img_url = "";
        private string _zhaiyao = "";
        private string _content;
        private int _sort_id = 99;
        private string _groupids_view = "";
        private string _user_name;
        private DateTime _add_time = DateTime.Now;

        /// <summary>
        /// 自增ID
        /// </summary>
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 频道ID
        /// </summary>
        public int channel_id
        {
            set { _channel_id = value; }
            get { return _channel_id; }
        }
        /// <summary>
        /// 类别ID
        /// </summary>
        public int category_id
        {
            set { _category_id = value; }
            get { return _category_id; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string title
        {
            set { _title = value; }
            get { return _title; }
        }
        /// <summary>
        /// 图片地址
        /// </summary>
        public string img_url
        {
            set { _img_url = value; }
            get { return _img_url; }
        }
       
        /// <summary>
        /// 内容摘要
        /// </summary>
        public string zhaiyao
        {
            set { _zhaiyao = value; }
            get { return _zhaiyao; }
        }
        /// <summary>
        /// 详细内容
        /// </summary>
        public string content
        {
            set { _content = value; }
            get { return _content; }
        }
       
        /// <summary>
        /// 用户名
        /// </summary>
        public string user_name
        {
            set { _user_name = value; }
            get { return _user_name; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime add_time
        {
            set { _add_time = value; }
            get { return _add_time; }
        }
        
    }
}
