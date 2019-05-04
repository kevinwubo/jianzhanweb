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
    public class ProductInfo
    {
        [DataMapping("ID", DbType.Int32)]
        /// <summary>
        /// 自动增长ID
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 产品ID
        /// </summary>
        [DataMapping("ProductID", DbType.String)]
        public string ProductID { get; set; }
        /// <summary>
        /// 产品名称
        /// </summary>
        [DataMapping("ProductName", DbType.String)]
        public string ProductName { get; set; }
        /// <summary>
        /// 副标题
        /// </summary>
        [DataMapping("SubTitle", DbType.String)]
        public string SubTitle { get; set; }
        /// <summary>
        /// 名家品牌1.大师/传承人2.名家/工艺师3.无落款
        /// </summary>
        [DataMapping("Type1", DbType.String)]
        public string Type1 { get; set; }
        /// <summary>
        /// 二、工艺釉色1.  兔毫1)	褐兔毫2)	银(蓝)兔毫3)	金(彩)兔毫2.  油滴/鹧鸪斑1)	褐油滴)	银(蓝)油滴3)	金(彩)油滴3.  金缕鹧鸪斑（兔毫和油滴兼有）4.  曜变1)	日本类曜变2)	国内类曜变5.	乌金釉.	杂色釉/异毫1)	柿红)	茶叶末3)	铁锈斑4)	珍珠斑5)	西瓜花6)	芝麻釉7)	灰皮8)	龟背纹7.	龙鳞纹/苍蝇翅
        /// </summary>
        [DataMapping("Type2", DbType.String)]
        public string Type2 { get; set; }
        /// <summary>
        /// 器型1.  束口盏（经典器型）1)	标准束口2)	香炉格3)	改良束口2.  撇口盏1)	深撇口2)	浅撇口/喇叭3.  敛口盏 4.  敞口盏/斗笠.  灯盏1)	单层灯盏2)	双层灯盏4.	钵型盏5.	创新器型1)	禅定杯2)	公道杯
        /// </summary>
        [DataMapping("Type3", DbType.String)]
        public string Type3 { get; set; }
        /// <summary>
        /// 口径尺寸 1.大（口径15cm以上）2.中（口径11-15cm）3.小（口径11cm以下）
        /// </summary>
        [DataMapping("Type4", DbType.String)]
        public string Type4 { get; set; }
        /// <summary>
        /// 用途功能1.新手入门2.私享主人杯3.客人杯4.套装/送礼5.收藏投资6.展示/装饰
        /// </summary>
        [DataMapping("Type5", DbType.String)]
        public string Type5 { get; set; }
        /// <summary>
        /// 传世老盏1)全品/整器2)残缺瑕疵3)标本残片4)有底款5)无底款
        /// </summary>
        [DataMapping("Type6", DbType.String)]
        public string Type6 { get; set; }
        /// <summary>
        /// 价格区间1.1000以下 2.1001~3000 3.3001~5000 4.5001~10000 5.10001~50000 6.50000以上
        /// </summary>
        [DataMapping("Type7", DbType.String)]
        public string Type7 { get; set; }
        /// <summary>
        /// 产品图片
        /// </summary>
        [DataMapping("Images", DbType.String)]
        public string Images { get; set; }
        /// <summary>
        /// 摘要
        /// </summary>
        [DataMapping("summary", DbType.String)]
        public string summary { get; set; }
        /// <summary>
        /// 产品对应图片描述
        /// </summary>
        [DataMapping("ProductDetail", DbType.String)]
        public string ProductDetail { get; set; }

        /// <summary>
        /// 产品图片详细
        /// </summary>
        [DataMapping("ProImageDetail", DbType.String)]
        public string ProImageDetail { get; set; }

        /// <summary>
        /// 是否推荐到商城  0 否 1 是
        /// </summary>
        [DataMapping("IsPushMall", DbType.String)]
        public string IsPushMall { get; set; }
        /// <summary>
        /// 材质
        /// </summary>
        [DataMapping("Material", DbType.String)]
        public string Material { get; set; }
        /// <summary>
        /// 口径尺寸
        /// </summary>
        [DataMapping("Volume", DbType.String)]
        public string Volume { get; set; }
        /// <summary>
        /// 成本价
        /// </summary>

        [DataMapping("CostPrice", DbType.Decimal)]
        public string CostPrice { get; set; }
        /// <summary>
        /// 市场价
        /// </summary>
        [DataMapping("MarketPrice", DbType.Decimal)]
        public string MarketPrice { get; set; }
        /// <summary>
        /// 最低价
        /// </summary>
        [DataMapping("LowPrice", DbType.Decimal)]
        public string LowPrice { get; set; }
        /// <summary>
        /// 关联艺人ID
        /// </summary>
        [DataMapping("ArtisanID", DbType.Int32)]
        public string ArtisanID { get; set; }
        /// <summary>
        /// 视频地址
        /// </summary>
        [DataMapping("VideoUrl", DbType.String)]
        public string VideoUrl { get; set; }
        /// <summary>
        /// 商品详情小视频地址
        /// </summary>
        [DataMapping("VideoDetail", DbType.String)]
        public string VideoDetail { get; set; }

        /// <summary>
        /// 推荐板块位置
        /// </summary>
        [DataMapping("PlatePosition", DbType.String)]
        public string PlatePosition { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        [DataMapping("InventoryCount", DbType.Int32)]
        public int InventoryCount { get; set; }

        /// <summary>
        /// 产品作者
        /// </summary>
        [DataMapping("Author", DbType.String)]
        public string Author { get; set; }


        /// <summary>
        /// 添加时间
        /// </summary>
        [DataMapping("Adddate", DbType.DateTime)]
        public DateTime Adddate { get; set; }

        [DataMapping("UpdateDate", DbType.DateTime)]
        public DateTime UpdateDate { get; set; }
    }
}
