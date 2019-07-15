using Common;
using Entity.ViewModel;
using Service;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class WebHomePCController : Controller
    {
        //
        // GET: /WebHomePC/

        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult home()
        {
            #region 新品好货
            List<ArtisanEntity> list = ArtisanService.GetArtisansByRule("名家工艺师", 0, "1");
            string sqlwhere = "";
            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ArtisanEntity item in list)
                {
                    sb.Append("'" + item.artisanName + "',");
                }
                sqlwhere = " and Author in(" + sb.ToString().TrimEnd(',') + ")";
            }

            List<ProductEntity> listNew = ProductService.GetProductsBySqlWhere(4, 2, "");//新品好货
            #endregion

            #region 名家名堂  显示艺人最新上架 每个人显示一个
            List<ArtisanEntity> listAll = ArtisanService.GetAllArtisan();
            sqlwhere = "";
            if (list != null && list.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                foreach (ArtisanEntity item in list)
                {
                    sb.Append("'" + item.artisanName + "',");
                }
                sqlwhere = " and Author in(" + sb.ToString().TrimEnd(',') + ")";
            }

            List<ProductEntity> listArtisanProduct = ProductService.GetProductsBySqlWhere(6, 1, sqlwhere);//名家名堂 最新上架
            #endregion

            #region 人气推荐
            List<ProductEntity> listHot = new List<ProductEntity>();//人气推荐
            CodeSEntity entityrq = BaseDataService.GetCodeValuesByRule("MB_RQTJ");
            if (entityrq != null)
            {
                if (!string.IsNullOrEmpty(entityrq.CodeValues))
                {
                    listHot = ProductService.GetProductsBySqlWhere(4, "and ProductID in(" + entityrq.CodeValues + ")", " InventoryCount desc,Adddate desc");
                }
            }
            #endregion

            #region 大师推荐
            List<ProductEntity> listDSTJ = new List<ProductEntity>();//大师推荐
            List<BaseDataEntity> listBase = BaseDataService.GetBaseDataByType("DSZP001");
            if (listBase != null && listBase.Count > 0)
            {
                string DSTJ = listBase[0].ValueInfo;
                string[] authors = DSTJ.Split(',');
                foreach (string author in authors)
                {
                    List<ProductEntity> listProduct = ProductService.GetAllProductByRule(author, 1, " Order By InventoryCount desc,Adddate desc");
                    if (listProduct != null && listProduct.Count > 0)
                    {
                        if (listDSTJ.Count<4)//只显示4个
                        {
                            listDSTJ.Add(listProduct[0]);
                        }                        
                    }
                }

            }
            #endregion

            ViewBag.listArt = ArticleService.GetArticleByRule(-1, 7);

            List<ArtisanEntity> listArt = ArtisanService.GetArtisansByRule("名家工艺师", 4, "1");
            ViewBag.listMJMT = listArt;//名家名堂
            ViewBag.listNew = listNew;
            ViewBag.listHot = listHot;
            ViewBag.listDSTJ = listDSTJ;
            ViewBag.listArtisanProduct = listArtisanProduct;
            return View();
        }
        /// <summary>
        /// 商城首页Type2釉色  type3器型 type4口径 type5功能 type7 价格
        /// </summary>
        /// <returns></returns>
        public ActionResult shop(string type2, string type3, string type4, string type5, string type7, string author, string artisanType, string keyword = "", string tag = "", int p = 1)
        {


            List<ProductEntity> mList = null;

            //if (string.IsNullOrEmpty(type2) && string.IsNullOrEmpty(type3) && string.IsNullOrEmpty(type4) && string.IsNullOrEmpty(type7) && string.IsNullOrEmpty(author) && string.IsNullOrEmpty(artisanType) && string.IsNullOrEmpty(keyword))
            //{
            //    artisanType = "业界大师";
            //}
            string OrderBy = " ORDER BY Adddate Desc ";

            if (!string.IsNullOrEmpty(author))
            {
                if (author.IndexOf("业界大师") > -1)
                    artisanType = "业界大师";
                else if (author.IndexOf("老牌传承人") > -1)
                    artisanType = "老牌传承人";
                else if (author.IndexOf("名家工艺师") > -1)
                    artisanType = "名家工艺师";
                else if (author.IndexOf("知名品牌") > -1)
                    artisanType = "知名品牌";
                else
                    author = "'" + author + "'";
            }

            //排序规则
            if (!string.IsNullOrEmpty(tag))
            {
                if (tag.Equals("默认"))//默认
                    OrderBy = " ORDER BY InventoryCount Desc ";
                else if (tag.Equals("新品"))//新品
                    OrderBy = " ORDER BY Adddate Desc ";
                else if (tag.Equals("销量"))//销量
                    OrderBy = " ORDER BY InquiryCount Desc ";
                else if (tag.Equals("人气"))//人气
                    OrderBy = " ORDER BY InventoryCount Desc ";
            }

            string artisanName = "";
            if (!string.IsNullOrEmpty(artisanType))
            {
                List<ArtisanEntity> listArt = ArtisanService.GetArtisansByRule(artisanType);
                if (listArt != null && listArt.Count > 0)
                {
                    foreach (ArtisanEntity entity in listArt)
                    {
                        artisanName += "'" + entity.artisanName + "',";
                    }
                }
                author = artisanName;
            }

            int count = ProductService.GetProductCount(type2, type3, type4, type7, string.IsNullOrEmpty(author) ? "" : author.TrimEnd(','), "", keyword);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 12;
            pager.SumCount = count;
            pager.URL = "/WebHomePC/shop";

            
            

            if (!string.IsNullOrEmpty(type2) || !string.IsNullOrEmpty(type3) || !string.IsNullOrEmpty(type4) || !string.IsNullOrEmpty(type7) || !string.IsNullOrEmpty(keyword) || !string.IsNullOrEmpty(author))
            {
                mList = ProductService.GetAllProductInfoByRule(type2, type3, type4, type7, string.IsNullOrEmpty(author) ? "" : author.TrimEnd(','), "", keyword, " mn_shop", OrderBy, pager);
            }
            else
            {
                mList = ProductService.GetProductInfoPager("ORDER BY Adddate Desc ", pager);
            }
            ViewBag.listTJ = ProductService.GetProductsBySqlWhere(4, 1, "");//新品好货

            ViewBag.YJDSList = ArtisanService.getSimpleArtisanList("业界大师");//业界大师
            ViewBag.LPCCRList = ArtisanService.getSimpleArtisanList("老牌传承人");//老牌传承人
            ViewBag.MJGYSList = ArtisanService.getSimpleArtisanList("名家工艺师");//名家工艺师
            ViewBag.ZMPPList = ArtisanService.getSimpleArtisanList("知名品牌");//知名品牌
            List<BaseDataEntity> listAll = BaseDataService.GetBaseDataAll();
            ViewBag.QXList = listAll.Where(t => t.PCode == "QX000" && t.Status == 1).ToList();//器型
            ViewBag.YSList = listAll.Where(t => t.PCode == "YS000" && t.Status == 1).ToList();//釉色
            ViewBag.KJList = listAll.Where(t => t.PCode == "KJCodes" && t.Status == 1).ToList();//口径
            ViewBag.JGList = listAll.Where(t => t.PCode == "JGCodes" && t.Status == 1).ToList();//价格
            ViewBag.GNList = listAll.Where(t => t.PCode == "GNCodes" && t.Status == 1).ToList();//价格
            ViewBag.ArtisanType = artisanType;
            ViewBag.ProductList = mList;
            ViewBag.Pager = pager;

            ViewBag.type2 = type2;
            ViewBag.type3 = type3;
            ViewBag.type4 = type4;
            ViewBag.type5 = type5;
            ViewBag.type7 = type7;
            ViewBag.author = string.IsNullOrEmpty(artisanType) ? author : artisanType;
            return View();
        }

        /// <summary>
        /// 产品详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult shopdetail(string productid)
        {
            ProductEntity entity=  ProductService.GetProductByProductID(productid);
            ViewBag.ProductInfo = entity;
            ViewBag.ListByAuthor = ProductService.GetAllProductByRule(entity.Author, 5, "  order by AddDate desc  ");
            return View();
        }

        /// <summary>
        /// 艺人首页
        /// </summary>
        /// <returns></returns>
        public ActionResult famous(string artisantype, string artisanname, int p = 1)
        {
            List<ArtisanEntity> mList = null;
            int count = ArtisanService.GetArtisanCount(artisantype, artisanname);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 8;
            pager.SumCount = count;
            pager.URL = "/WebHomePC/famous";

            if (!string.IsNullOrEmpty(artisantype) || !string.IsNullOrEmpty(artisanname))
            {
                mList = ArtisanService.GetAllArtisanInfoByRule(artisantype, artisanname, pager, 4);
            }
            else
            {
                mList = ArtisanService.GetArtisanInfoPager(pager, 4);
            }
            ViewBag.Pager = pager;
            ViewBag.ArtisanList = mList;
            return View();
        }

        /// <summary>
        /// 艺人详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult famousdetail(string artisanid)
        {
            ViewBag.ArtisanModel = ArtisanService.GetArtisanByKey(artisanid);

            #region 人气推荐
            List<ProductEntity> listHot = new List<ProductEntity>();//人气推荐
            CodeSEntity entityrq = BaseDataService.GetCodeValuesByRule("MB_RQTJ");
            if (entityrq != null)
            {
                if (!string.IsNullOrEmpty(entityrq.CodeValues))
                {
                    listHot = ProductService.GetProductsBySqlWhere(8, "and ProductID in(" + entityrq.CodeValues + ")", " InventoryCount desc,Adddate desc");
                }
            }
            #endregion

            ViewBag.listHot = listHot;
            ViewBag.listTJ = ProductService.GetProductsBySqlWhere(4, 1, "");//新品好货
            return View();
        }

        /// <summary>
        /// 建盏学院 1新闻资讯/3拍卖行情/33建盏天下/5建盏工艺/6百科知识/7选盏技巧/8文化历史
        /// </summary>
        /// <returns></returns>
        public ActionResult college()
        {
            ViewBag.ListA = ArticleService.GetArticleByRule(1, 6);
            ViewBag.ListB = ArticleService.GetArticleByRule(5, 6);
            ViewBag.ListC = ArticleService.GetArticleByRule(6, 6);
            ViewBag.ListD = ArticleService.GetArticleByRule(7, 6);
            ViewBag.listTJ = ProductService.GetProductsBySqlWhere(4, 1, "");//推荐
            return View();
        }

        /// <summary>
        /// 建盏学院列表
        /// </summary>
        /// <returns></returns>
        public ActionResult collegelist(int category_id = 0, int p = 1)
        {

            List<ArticleEntity> mList = null;
            int count = ArticleService.GetArticleCount(category_id);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 12;
            pager.SumCount = count;
            pager.URL = "/WebHomePC/collegelist";

            if (category_id > 0)
            {
                mList = ArticleService.GetAllArticleInfoByRule(category_id, pager);
            }
            else
            {
                mList = ArticleService.GetArticleInfoPager(pager);
            }
            ViewBag.Category_ID = category_id;
            ViewBag.ArticleList = mList;
            ViewBag.Pager = pager;
            return View();
        }

        /// <summary>
        /// 建盏学院详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult collegedetail(string id)
        {
            ViewBag.ArticleModel = ArticleService.GetArticleByID(id);

            #region 人气推荐
            List<ProductEntity> listHot = new List<ProductEntity>();//人气推荐
            CodeSEntity entityrq = BaseDataService.GetCodeValuesByRule("MB_RQTJ");
            if (entityrq != null)
            {
                if (!string.IsNullOrEmpty(entityrq.CodeValues))
                {
                    listHot = ProductService.GetProductsBySqlWhere(8, "and ProductID in(" + entityrq.CodeValues + ")", " InventoryCount desc,Adddate desc");
                }
            }
            #endregion

            ViewBag.listHot = listHot;
            ViewBag.listTJ = ProductService.GetProductsBySqlWhere(4, 1, "");//推荐
            return View();
        }

        /// <summary>
        /// 收藏首页
        /// </summary>
        /// <returns></returns>
        public ActionResult souchang(int p = 1)
        {
            List<ProductEntity> mList = null;
            string sqlwhere = " and (MarketPrice>30000 or type6 in('全品整器','残缺瑕疵','标本残片'))";
            int count = ProductService.GetProductCount("", "", "", "", "", sqlwhere, "");

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 12;
            pager.SumCount = count;
            pager.URL = "/WebHomePC/souchang";

            if (1 == 1)
            {
                mList = ProductService.GetAllProductInfoByRule("", "", "", "", "", sqlwhere, "", "mn_souchang", "", pager);
            }
            else
            {
                mList = ProductService.GetProductInfoPager(" ORDER BY MarketPrice Desc  ", pager);
            }


            ViewBag.souchang = mList;
            ViewBag.Pager = pager;
            return View();
        }


        /// <summary>
        /// 经典首页
        /// </summary>
        /// <returns></returns>
        public ActionResult jingdian()
        {
            List<ArtisanEntity> listqxys = ArtisanService.GetArtisansByRule("", 4, "", "and artisanType in('器型','釉色')");
            List<ArtisanEntity> listqxysall = ArtisanService.GetArtisansByRule("", 0, "", "and artisanType in('器型','釉色')");


            #region 人气推荐
            List<ProductEntity> listHot = new List<ProductEntity>();//人气推荐
            CodeSEntity entityrq = BaseDataService.GetCodeValuesByRule("MB_RQTJ");
            if (entityrq != null)
            {
                if (!string.IsNullOrEmpty(entityrq.CodeValues))
                {
                    listHot = ProductService.GetProductsBySqlWhere(8, "and ProductID in(" + entityrq.CodeValues + ")", " InventoryCount desc,Adddate desc");
                }
            }
            #endregion
            ViewBag.listTJ = ProductService.GetProductsBySqlWhere(4, 1, "");//推荐
            ViewBag.listHot = listHot;

            ViewBag.listQXYS = listqxys;
            ViewBag.listQXYSALL = listqxysall;
            return View();
        }

        /// <summary>
        /// 经典首页详情页
        /// </summary>
        /// <returns></returns>
        public ActionResult jingdiandetail(string artisanid)
        {
            #region 人气推荐
            List<ProductEntity> listHot = new List<ProductEntity>();//人气推荐
            CodeSEntity entityrq = BaseDataService.GetCodeValuesByRule("MB_RQTJ");
            if (entityrq != null)
            {
                if (!string.IsNullOrEmpty(entityrq.CodeValues))
                {
                    listHot = ProductService.GetProductsBySqlWhere(8, "and ProductID in(" + entityrq.CodeValues + ")", " InventoryCount desc,Adddate desc");
                }
            }
            #endregion
            ViewBag.listTJ = ProductService.GetProductsBySqlWhere(4, 1, "");//推荐
            ViewBag.listHot = listHot;
            ViewBag.ArtisanModel = ArtisanService.GetArtisanByKey(artisanid);
            return View();
        }
    }
}
