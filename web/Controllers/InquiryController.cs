using Entity.ViewModel;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Common;
using Service.BaseBiz;
using System.IO;
using System.Data;

namespace web.Controllers
{
    public class InquiryController : BaseController
    {
        //
        // GET: /Inquiry/
        public int PAGESIZE = 20;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tracestate">跟踪状态</param>
        /// <param name="CustomerID"></param>
        /// <param name="status"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public ActionResult Index(string name, string tracestate, int CustomerID = 0, int status = -1, string begindate = "", string enddate = "", string sourceform = "", int pageSize=20, int p = 1)
        {
            List<InquiryEntity> mList = null;

            bool userManagers = false;
            string operatorid = getUserID(CurrentUser, ref userManagers);
            string sqlwhere = "";
            //管理者在客户询价查询库里，是不会搜索到释放库的咨询量
            if (userManagers)
            {
                sqlwhere = " and isnull(Status,'')!='释' ";
            }
            if (!String.IsNullOrEmpty(sourceform))
            {
                sqlwhere = " and SourceForm='" + sourceform + "'";
            }


            int count = InquiryService.GetInquiryCount(name, tracestate, status, begindate, enddate, operatorid, sqlwhere);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = pageSize;
            pager.SumCount = count;
            pager.URL = "/Inquiry";
            pager.DistinctCount = InquiryService.GetDistinctTelephone(name, tracestate, status, begindate, enddate, operatorid, sqlwhere);
            ViewBag.InquiryCode = BaseDataService.GetBaseDataByPCode("InquiryS00");//跟踪状态
            //if (!string.IsNullOrEmpty(name) || status > -1 || !string.IsNullOrEmpty(tracestate))
            //{
            mList = InquiryService.GetInquiryInfoByRule(name, tracestate, status, begindate, enddate, operatorid, CurrentUser.UserID.ToString(), sqlwhere, pager);
            //}
            //else
            //{
            //    mList = InquiryService.GetInquiryInfoPager(pager);
            //}
            ViewBag.SourceForm = sourceform;
            ViewBag.PageSize = pageSize;
            ViewBag.Name = name ?? "";
            ViewBag.Status = status;
            ViewBag.ModelCode = tracestate;
            ViewBag.CustomerID = CustomerID;
            ViewBag.Inquiry = mList;
            ViewBag.Pager = pager;
            ViewBag.BeginDate = begindate;
            ViewBag.EndDate = enddate;
            
            ViewBag.LoginUserID = CurrentUser.UserID;
            return View();
        }
        #region 手动添加咨询量
        /// <summary>
        /// 手动添加咨询量
        /// </summary>
        /// <returns></returns>
        public ActionResult Add()
        {
            ViewBag.UserInfo = CurrentUser;
            return View();
        }

        /// <summary>
        /// 手动添加咨询量--保存
        /// </summary>
        /// <param name="entity"></param>
        public void HandSave(InquiryEntity entity)
        {
            InquiryService.CreateSimple(entity, CurrentUser);
            Response.Redirect("/Inquiry/");
        }
        #endregion


        #region  移动到释放库
        /// <summary>
        /// 移动到释放库
        /// </summary>
        /// <param name="ppids"></param>
        /// <returns></returns>
        public JsonResult IntoHistoryInquiry(string ppids)
        {
            InquiryService.IntoHistoryInquiry(ppids);
            return new JsonResult
            {
                Data = "移动释放库成功！"
            };
        }
        #endregion

        public ActionResult Edit(string cid)
        {
            ViewBag.InquiryModel = BaseDataService.GetBaseDataByPCode("InquiryCode");

            List<UserEntity> userList = UserService.GetUserByRule("", 1, "", "4,7"); 

            if (!string.IsNullOrEmpty(cid))
            {
                ViewBag.Inquiry = InquiryService.GetInquiryEntityById(cid.ToLong(0), CurrentUser.UserID.ToString());
            }
            else
            {
                ViewBag.Inquiry = new InquiryEntity();
            }
            bool canEdit = StringHelper.checkRole(CurrentUser, 7) || StringHelper.checkRole(CurrentUser, 1);// 销售管理组 销售组
            if (canEdit)
            {
                int index = userList.FindIndex(p => p.UserID == CurrentUser.UserID);
                if (index > -1)
                {
                    userList.RemoveAt(index);
                }
            }
            ViewBag.CanEdit = canEdit;

            ViewBag.UserList = userList;
            return View();
        }

        public void Modify(InquiryEntity entity)
        {
            //if (entity != null)
            //{
            //    entity.OperatorID = CurrentUser.UserID.ToString();
            //}
            InquiryService.ModifyInquiry(entity);
            Response.Redirect("/Inquiry/");
        }

        public void Remove(string rid)
        {
            InquiryService.Remove(rid.ToInt(0));

            Response.Redirect("/Inquiry/");
        }

        #region 手机站点资讯页面
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="tracestate">跟踪状态</param>
        /// <param name="CustomerID"></param>
        /// <param name="status"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public ActionResult MobileIndex(string name, string tracestate, int CustomerID = 0, int status = -1, string begindate = "", string enddate = "", int p = 1)
        {
            List<InquiryEntity> mList = null;
            bool userManagers = false;
            string operatorid = getUserID(CurrentUser, ref userManagers);

            string sqlwhere = "";
            //管理者在客户询价查询库里，是不会搜索到释放库的咨询量
            if (userManagers)
            {
                sqlwhere = " and isnull(Status,'')!='释' ";
            }

            int count = InquiryService.GetInquiryCount(name, tracestate, status, begindate, enddate, operatorid, sqlwhere);

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/Inquiry/MobileIndex";
            pager.DistinctCount = InquiryService.GetDistinctTelephone(name, tracestate, status, begindate, enddate, operatorid, sqlwhere);
            ViewBag.InquiryCode = BaseDataService.GetBaseDataByPCode("InquiryS00");//跟踪状态            
            mList = InquiryService.GetInquiryInfoByRule(name, tracestate, status, begindate, enddate, operatorid, CurrentUser.UserID.ToString(), sqlwhere, pager);
            ViewBag.Name = name ?? "";
            ViewBag.Status = status;
            ViewBag.ModelCode = tracestate;
            ViewBag.CustomerID = CustomerID;
            ViewBag.Inquiry = mList;
            ViewBag.Pager = pager;
            ViewBag.BeginDate = begindate;
            ViewBag.EndDate = enddate;
            return View();
        }

        /// <summary>
        /// 获取当前用户ID
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private string getUserID(UserEntity user, ref bool userManagers)
        {
            string operatorid = "";
            if (user != null && user.Roles.Count > 0)
            {
                int roleID = user.Roles[0].RoleID;
                int i = (int)RoleEnum.Admin;
                int sm = (int)RoleEnum.SalesManager;

                if (roleID == i)//管理员查询所有
                {
                    userManagers = true;
                    return operatorid;
                }
                else if (roleID == sm)//销售主管可以查看当前城市下所有的销售
                {
                    List<UserEntity> listUser = UserService.GetUserAll().FindAll(p => p.CityName.Equals(user.CityName));
                    if (listUser != null && listUser.Count > 0)
                    {
                        foreach (UserEntity item in listUser)
                        {
                            operatorid += item.UserID + ",";
                        }
                    }
                    userManagers = true;
                    return !string.IsNullOrEmpty(operatorid) ? operatorid.Substring(0, operatorid.Length - 1) : "";
                }
                else
                {
                    userManagers = false;
                    operatorid = user.UserID.ToString();
                }

            }
            return operatorid;
        }

        /// <summary>
        ///  手机站点编辑页面
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>

        public ActionResult MobileEdit(string cid)
        {
            ViewBag.InquiryModel = BaseDataService.GetBaseDataByPCode("InquiryCode");

            List<UserEntity> userList = UserService.GetUserByRule("", 1, "", "4,7");

            if (!string.IsNullOrEmpty(cid))
            {
                ViewBag.Inquiry = InquiryService.GetInquiryEntityById(cid.ToLong(0), CurrentUser.UserID.ToString());
            }
            else
            {
                ViewBag.Inquiry = new InquiryEntity();
            }
            bool canEdit = StringHelper.checkRole(CurrentUser, 7) || StringHelper.checkRole(CurrentUser, 1);// 销售管理组 销售组
            if (canEdit)
            {
                int index = userList.FindIndex(p => p.UserID == CurrentUser.UserID);
                if (index > -1)
                {
                    userList.RemoveAt(index);
                }
            }
            ViewBag.UserList = userList;
            ViewBag.CanEdit = canEdit;
            return View();
        }
        public void MobileModify(InquiryEntity entity)
        {
            InquiryService.ModifyInquiry(entity);
            Response.Redirect("/Inquiry/MobileIndex");
        }
        #endregion



        #region 销售转移监控列表
        public ActionResult InquiryMonitor(int p = 1)
        {
            List<InquiryMonitorEntity> mList = null;


            int count = InquiryMonitorService.GetInquiryCount();

            PagerInfo pager = new PagerInfo();
            pager.PageIndex = p;
            pager.PageSize = PAGESIZE;
            pager.SumCount = count;
            pager.URL = "/InquiryMonitor";


            mList = InquiryMonitorService.GetInquiryInfoByRule(pager);
            ViewBag.InquiryMonitor = mList;
            ViewBag.Pager = pager;
            return View();
        }
        #endregion


        #region 咨询量数据导入
        public JsonResult InquiryImportData()
        {
            List<InquiryImportDataEntity> list = new List<InquiryImportDataEntity>();
            DataSet ds = new DataSet();
            if (Request.Files.Count == 0)
            {
                throw new Exception("请选择导入文件！");
            }
            int count=0;
            // 保存文件到UploadFiles文件夹
            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i];
                var fileName = file.FileName;
                var filePath = Server.MapPath(string.Format("~/{0}", "UploadFiles"));
                string path = Path.Combine(filePath, fileName);
                file.SaveAs(path);
                ds = ExcelHelper.ImportExcelXLSXtoDt(path);

                if (ds != null && ds.Tables[0] != null)
                {
                    count = InquiryService.InquiryImportData(ds.Tables[0]);
                }

            }
            return new JsonResult
            {
                Data = "成功导入" + count + "条数据！"
            };
        }
        #endregion
    }
}
