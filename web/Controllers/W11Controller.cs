﻿using Common;
using Entity.ViewModel;
using Service;
using Service.BaseBiz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace web.Controllers
{
    public class W11Controller : Controller
    {
        //
        // GET: /W11/

        /// <summary>
        /// 手机号添加
        /// </summary>
        /// <param name="tel"></param>
        /// <returns></returns>
        public ActionResult Tel(string date1,string date2)
        {
            if (!string.IsNullOrEmpty(date1) || !string.IsNullOrEmpty(date2))
            {
                //BaseDataService.InsertTelephone(tel);

                //List<InquiryEntity> list = InquiryService.GetInquiryByRule("", "", "", " and (telphone='" + StringHelper.ConvertBy123(date1) + "' or telphone='" + date1 + "') ", "", "");
                List<InquiryEntity> list = InquiryService.GetInquiryByRule("", "", "", " AND isnull(status,'')='' AND telphone!='' AND AddDate Between '" + date1 + " 00:00:01' and '" + date2 + " 23:59:59'", "", "");
                List<InquiryEntity> list111 = list.FindAll(p => p.telphone.Equals("ACFMEGGBBGF"));
                if (list != null && list.Count > 0)
                {
                    var groupInfo = list.GroupBy(m => m.telphone).ToList();
                    foreach (var item in groupInfo)
                    {
                        List<InquiryEntity> listNew = InquiryService.GetInquiryByRule("", "", "", " and (telphone='" + StringHelper.ConvertBy123(item.Key) + "' or telphone='" + item.Key + "') ", "", "");
                        if (item.Key.Equals("13605772276"))
                        {

                            string trl = "";
                        }
                        if (listNew != null && listNew.Count > 0)
                        {
                            List<InquiryEntity> listXin = listNew.FindAll(p => p.status.Equals("新"));

                            if (listXin.Count == 0)
                            {
                                List<InquiryEntity> listOrderBy = listNew.OrderBy(p => p.PPId).ToList();
                                InquiryEntity inquiryEntity = new InquiryEntity();
                                inquiryEntity.PPId = listOrderBy[0].PPId;
                                inquiryEntity.status = "新";
                                inquiryEntity.CommentContent = "Update_New";
                                InquiryService.ModifyInquiryStatus(inquiryEntity);
                            }
                        }

                    }
                }

            }
            else
            {
                List<InquiryEntity> listNew = InquiryService.GetInquiryByRule("", "", "", " and (telphone='" + StringHelper.ConvertBy123(date1) + "' or telphone='" + date1 + "') ", "", "");
            }
            return View();
        }

        public ActionResult login(string msg)
        {
            return View();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public JsonResult CheckTelephone(string telephone)
        {
            bool result = false;

            List<string> list= BaseDataService.GetTelephone();
            if (list != null && list.Count > 0)
            {
                result = list.Contains(telephone);
            }

            if (result)
            {
                Response.Cookies["Telephone"].Value = telephone;
                Response.Cookies["Telephone"].Expires = DateTime.Now.AddDays(1);
            }

            return new JsonResult
            {
                Data = result
            };
        }

        /// <summary>
        /// 商城首页
        /// </summary>
        /// <returns></returns>
        public ActionResult paimai( string type2, string type3, string type4, string type7, string author, string artisanType, string keyword = "", int p = 1)
        {
            string telephone = Request.Cookies["Telephone"] != null ? Request.Cookies["Telephone"].Value : "";
            if (string.IsNullOrEmpty(telephone))
            {
                Response.Redirect("/W11/login");
            }
            if (!string.IsNullOrEmpty(telephone))
            {
                List<string> list = BaseDataService.GetTelephone();
                bool result = false;
                if (list != null && list.Count > 0)
                {
                    result = list.Contains(telephone);
                }
                if (result==false)
                {
                    Response.Redirect("/W11/login");
                }                
            }

            List<ProductEntity> mList = null;

            if (string.IsNullOrEmpty(type2) && string.IsNullOrEmpty(type3) && string.IsNullOrEmpty(type4) && string.IsNullOrEmpty(type7) && string.IsNullOrEmpty(author) && string.IsNullOrEmpty(artisanType) && string.IsNullOrEmpty(keyword))
            {
                //artisanType = "业界大师";
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

            int count = ProductService.GetProductCount(type2, type3, type4, type7, string.IsNullOrEmpty(author) ? "" : author.TrimEnd(','), " and InventoryCount=-2 ", keyword);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = 12;
            pager.SumCount = count;
            pager.URL = "/W11/paimai";



            //if (!string.IsNullOrEmpty(type2) || !string.IsNullOrEmpty(type3) || !string.IsNullOrEmpty(type4) || !string.IsNullOrEmpty(type7) || !string.IsNullOrEmpty(keyword) || !string.IsNullOrEmpty(author))
            //{

            mList = ProductService.GetAllProductInfoByRule(type2, type3, type4, type7, string.IsNullOrEmpty(author) ? "" : author.TrimEnd(','), " and InventoryCount=-2 ", keyword, " mn_shop", "", pager);
            //}
            //else
            //{
            //    mList = ProductService.GetProductInfoPager("ORDER BY Adddate Desc", pager);
            //}

            //ViewBag.telephone = telephone;
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
        public ActionResult paimaidetail(string productid)
        {
            ViewBag.ProductInfo = ProductService.GetProductByProductID(productid);
            return View();
        }

    }
}
