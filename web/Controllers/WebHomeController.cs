﻿using Common;
using Entity.ViewModel;
using Infrastructure.Cache;
using Service;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class WebHomeController : BaseWebontroller
    {
        public int PAGESIZE = 20;

        public ActionResult Auto()
        {
            return View();
        }

        public void AutoHtml(string type, string paramper)
        {
            try
            {
                string urlPar = "";
                string urlPar2 = "";
                string pageUrl = "";
                if (!string.IsNullOrEmpty(paramper))
                {
                    urlPar = "?pagetype=" + paramper;
                    urlPar2 = "&pagetype=" + paramper;
                    pageUrl = "/wxpage/";
                }

                if ("clearCache".Equals(type))//清空缓存
                {
                    CacheRuntime cache = new CacheRuntime();
                    cache.ClearAll();//
                }
                if ("index".Equals(type)) //首页
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "webhome/mn_index" + urlPar, pageUrl + "m_index.html");
                }
                else if ("shop".Equals(type))//商城
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "webhome/mn_shop" + urlPar, pageUrl + "m_product_list.html");
                }
                else if ("famous".Equals(type))//学堂
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "webhome/mn_famous" + urlPar, pageUrl + "m_artisan_list.html");
                }
                else if ("gyys".Equals(type))//工艺釉色
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "webhome/mn_jingdian" + urlPar, pageUrl + "m_gyys.html");
                }
                else if ("college".Equals(type))//学院
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "webhome/mn_college" + urlPar, pageUrl + "m_school.html");
                }
                else if ("souchang".Equals(type))//收藏
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "webhome/mn_souchang" + urlPar, pageUrl + "m_collection.html");
                }
                else if ("allProduct".Equals(type))//所有产品
                {
                    List<ProductEntity> list = ProductService.GetAllProduct();
                    if (list != null && list.Count > 0)
                    {
                        foreach (ProductEntity entity in list)
                        {
                            Common.HtmlHelper.CreateStaticPage(Url + "WebHome/mn_shopdetail?productid=" + entity.ProductID + urlPar2, pageUrl + "m_" + entity.ProductID + ".html");
                        }
                    }
                }
                else if ("allfamous".Equals(type))//所有艺人
                {
                    List<ArtisanEntity> list = ArtisanService.GetAllArtisan();
                    if (list != null && list.Count > 0)
                    {
                        foreach (ArtisanEntity entity in list)
                        {
                            Common.HtmlHelper.CreateStaticPage(Url + "webhome/mn_famousdetail?artisanid=" + entity.artisanID + urlPar2, pageUrl + "m_art" + entity.artisanID + ".html");
                        }
                    }
                }
                else if ("allnews".Equals(type))//所有新闻
                {
                    List<ArticleEntity> list = ArticleService.GetArticleByRule(-1, -1);
                    if (list != null && list.Count > 0)
                    {
                        foreach (ArticleEntity entity in list)
                        {
                            Common.HtmlHelper.CreateStaticPage(Url + "webhome/mn_collegedetail?id=" + entity.id + urlPar2, pageUrl + "m_new" + entity.id + ".html");
                        }
                    }
                }


                if ("pc_index".Equals(type)) //首页
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "WebHomePC/home", "index.html");
                }
                else if ("pc_shop".Equals(type))//商城
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "WebHomePC/shop", "product_list.html");
                }
                else if ("pc_famous".Equals(type))//学堂
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "WebHomePC/famous", "artisan_list.html");
                }
                else if ("pc_college".Equals(type))//学院
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "WebHomePC/college", "article_list2.html");
                }
                else if ("pc_gyys".Equals(type))//工艺釉色
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "WebHomePC/jingdian", "gyys.html");
                }
                else if ("pc_souchang".Equals(type))//收藏
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "WebHomePC/souchang", "collection.html");
                }
                else if ("pc_allProduct".Equals(type))//所有产品
                {
                    List<ProductEntity> list = ProductService.GetAllProduct();
                    if (list != null && list.Count > 0)
                    {
                        foreach (ProductEntity entity in list)
                        {
                            Common.HtmlHelper.CreateStaticPage(Url + "WebHomePC/shopdetail?productid=" + entity.ProductID, entity.ProductID + ".html");
                        }
                    }
                }
                else if ("pc_allfamous".Equals(type))//所有艺人
                {
                    List<ArtisanEntity> list = ArtisanService.GetAllArtisan();
                    if (list != null && list.Count > 0)
                    {
                        foreach (ArtisanEntity entity in list)
                        {
                            Common.HtmlHelper.CreateStaticPage(Url + "WebHomePC/famousdetail?artisanid=" + entity.artisanID, "art" + entity.artisanID + ".html");
                        }
                    }
                }
                else if ("adextend".Equals(type))//推广页面
                {
                    Common.HtmlHelper.CreateStaticPage(Url + "Adextend/Index", "Bd01/B1_index.html");
                }



            }
            catch (Exception ex)
            {
                LogHelper.WriteErrorLog("auto", ex.ToString(), DateTime.Now);
            }

            Response.Redirect("Auto");
        }
        //
        // GET: /首页/

        public ActionResult mn_index()
        {
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

            List<ProductEntity> listNew = ProductService.GetProductsBySqlWhere(9, 2, "  and InventoryCount>0 ");//新品好货

            #region 人气推荐
            List<ProductEntity> listHot = new List<ProductEntity>();//人气推荐
            CodeSEntity entityrq = BaseDataService.GetCodeValuesByRule("MB_RQTJ");
            if (entityrq != null)
            {
                if (!string.IsNullOrEmpty(entityrq.CodeValues))
                {
                    listHot = ProductService.GetProductsBySqlWhere(9, "and ProductID in(" + entityrq.CodeValues + ")  and InventoryCount>0 ", " InventoryCount desc,Adddate desc");
                }
            }
            #endregion

            
            //List<ArtisanEntity> listqxys = ArtisanService.GetArtisansByRule("", 8, "", "and artisanType in('器型','釉色')");

            #region 大师推荐
            List<ProductEntity> listDSTJ = new List<ProductEntity>();//大师推荐
            List<BaseDataEntity> listBase = BaseDataService.GetBaseDataByType("DSZP001");
            if (listBase != null && listBase.Count>0)
            {
                string  DSTJ=listBase[0].ValueInfo;
                string[] authors = DSTJ.Split(',');
                foreach(string author  in authors)
                {
                    List<ProductEntity> listProduct = ProductService.GetAllProductByRule(author, 1, " Order By InventoryCount desc,Adddate desc");
                    if (listProduct != null && listProduct.Count > 0)
                    {
                        listDSTJ.Add(listProduct[0]);
                    }
                }
                
            }
            #endregion
            

            List<ArtisanEntity> listArt = ArtisanService.GetArtisansByRule("名家工艺师", 4, "1");
            //ViewBag.listQXYS=listqxys;
            ViewBag.listMJMT = listArt;//名家名堂
            //ViewBag.listYX = listYX;
            ViewBag.listNew = listNew;
            ViewBag.listHot = listHot;
            ViewBag.listDSTJ = listDSTJ;

           
            return View();
        }

        /// <summary>
        /// 商城首页
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_shop(string type2, string type3, string type4, string type7, string author, string artisanType, string keyword = "", int p = 1)
        {
            List<ProductEntity> mList = null;

            if (string.IsNullOrEmpty(type2) && string.IsNullOrEmpty(type3) && string.IsNullOrEmpty(type4) && string.IsNullOrEmpty(type7) && string.IsNullOrEmpty(author) && string.IsNullOrEmpty(artisanType) && string.IsNullOrEmpty(keyword))
            {
                artisanType = "业界大师";
            }

            if (!string.IsNullOrEmpty(author))
            {
                author = "'" + author + "'";
            }

            if (!string.IsNullOrEmpty(artisanType))
            {
                List<ArtisanEntity> listArt = ArtisanService.GetArtisansByRule(artisanType);
                if (listArt != null && listArt.Count > 0)
                {
                    foreach (ArtisanEntity entity in listArt)
                    {
                        author += "'" + entity.artisanName + "',";
                    }
                }
            }

            int count = ProductService.GetProductCount(type2, type3, type4, type7, string.IsNullOrEmpty(author) ? "" : author.TrimEnd(','), " and InventoryCount>0 ", keyword);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 12;
            pager.SumCount = count;
            pager.URL = "/WebHome/mn_shop";

            

            //if (!string.IsNullOrEmpty(type2) || !string.IsNullOrEmpty(type3) || !string.IsNullOrEmpty(type4) || !string.IsNullOrEmpty(type7) || !string.IsNullOrEmpty(keyword) || !string.IsNullOrEmpty(author))
            //{

                mList = ProductService.GetAllProductInfoByRule(type2, type3, type4, type7, string.IsNullOrEmpty(author) ? "" : author.TrimEnd(','), " and InventoryCount>0 ", keyword, " mn_shop", "", pager);
            //}
            //else
            //{
            //    mList = ProductService.GetProductInfoPager("ORDER BY Adddate Desc", pager);
            //}

            ViewBag.YJDSJson = JsonHelper.ToJson(ArtisanService.getSimpleArtisanList("业界大师"));//业界大师
            ViewBag.LPCCRJson = JsonHelper.ToJson(ArtisanService.getSimpleArtisanList("老牌传承人"));//老牌传承人
            ViewBag.MJGYSJson = JsonHelper.ToJson(ArtisanService.getSimpleArtisanList("名家工艺师"));//名家工艺师
            ViewBag.QXJson = JsonHelper.ToJson(BaseDataService.GetBaseDataByPCode("QX000"));//器型
            ViewBag.YSJson = JsonHelper.ToJson(BaseDataService.GetBaseDataByPCode("YS000"));//釉色
            ViewBag.ArtisanType = artisanType;
            ViewBag.Product = mList;
            ViewBag.Pager = pager;
            string keywords = Check(!string.IsNullOrEmpty(artisanType) ? "" : author) + Check(keyword) + Check(type2) + Check(type3) + Check(type4) + Check(type7) + Check(artisanType);
            ViewBag.Keyword = string.IsNullOrEmpty(keywords) ? keywords : keywords.TrimEnd(',');
            return View();
        }

        private string Check(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                str = str.Replace("'", "") + ",";
            }
            return str;
        }

        /// <summary>
        /// 商城首页详情页面
        /// </summary>
        /// <returns></returns> 
        public ActionResult mn_shopdetail(string productid)
        {
            ViewBag.ProductInfo = ProductService.GetProductByProductID(productid);
            return View();
        }

        /// <summary>
        /// 建盏学院
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_college(int category_id = 0, int p = 1)
        {

            List<ArticleEntity> mList = null;
            int count = ArticleService.GetArticleCount(category_id);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 12;
            pager.SumCount = count;
            pager.URL = "/WebHome/mn_college";

            if (category_id>0)
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
        /// 建盏学院详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_collegedetail(string id)
        {
            ViewBag.ArticleModel = ArticleService.GetArticleByID(id);
            return View();
        }

        /// <summary>
        /// 名家名堂
        /// </summary>
        /// <param name="artisantype"></param>
        /// <returns></returns>
        public ActionResult mn_famous(string artisantype, string artisanname, int p = 1)
        {
            List<ArtisanEntity> mList = null;
            int count = ArtisanService.GetArtisanCount(artisantype, artisanname);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 8;
            pager.SumCount = count;
            pager.URL = "/WebHome/mn_famous";

            if (!string.IsNullOrEmpty(artisantype) || !string.IsNullOrEmpty(artisanname))
            {
                mList = ArtisanService.GetAllArtisanInfoByRule(artisantype, artisanname, pager);
            }
            else
            {
                mList = ArtisanService.GetArtisanInfoPager(pager);
            }
            ViewBag.Pager = pager;
            ViewBag.ArtisanList = mList;
            return View();
        }

        /// <summary>
        /// 名家名堂详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_famousdetail(string artisanid)
        {
            ViewBag.ArtisanModel = ArtisanService.GetArtisanByKey(artisanid);
            return View();
        }


        /// <summary>
        /// 收藏
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_souchang(int p = 1)
        {
            List<ProductEntity> mList = null;
            string sqlwhere = " and InventoryCount>0 and (MarketPrice>6000 and type6 not in('全品整器','残缺瑕疵','标本残片'))";//or type6 in('全品整器','残缺瑕疵','标本残片')
            int count = ProductService.GetProductCount("", "", "", "", "", sqlwhere, "");

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 15;
            pager.SumCount = count;
            pager.URL = "/WebHome/mn_souchang";

            //if (1==1)
            //{
                mList = ProductService.GetAllProductInfoByRule("", "", "", "", "", sqlwhere, "", "mn_souchang", " ORDER BY NEWID()", pager);
            //}
            //else
            //{
            //    mList = ProductService.GetProductInfoPager(" ORDER BY MarketPrice Desc ", pager);
            //}


            ViewBag.souchang = mList;
            ViewBag.Pager = pager;
            return View();
        }

        /// <summary>
        /// 经典釉色
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_jingdian()
        {
            List<ArtisanEntity> listqxys = ArtisanService.GetArtisansByRule("", 0, "", "and artisanType in('器型','釉色')");

            ViewBag.listQXYS = listqxys;
            return View();
        }

        /// <summary>
        /// 经典釉色详情页面
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_jingdiandetail(string artisanid)
        {
            ViewBag.ArtisanModel = ArtisanService.GetArtisanByKey(artisanid);
            return View();
        }

        /// <summary>
        /// 关于我们
        /// </summary>
        /// <returns></returns>
        public ActionResult mn_about(string type)
        {
            ViewBag.Type = type;
            return View();
        }

        /// <summary>
        /// 立即资讯
        /// </summary>
        /// <returns></returns>
        public JsonResult CreateInquiry(string telephone, string productID, string sourceform, string contactName = "", string wxChartID = "")
        {
            string result = InquiryService.CreateInquiry(telephone, productID, sourceform, contactName, wxChartID);
            return new JsonResult
            {
                Data = "成功！您的询价对我们很重要，建盏顾问将很快回复！"
            };

        }

        public JsonResult getWxCodeByDays()
        {
            string wxCodes = getWXCode();
            return new JsonResult
            {
                Data = wxCodes
            };
        }
    }
}
