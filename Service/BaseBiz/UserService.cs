using DataRepository.DataModel;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Helper;
using DataRepository.DataAccess.User;
using Common;


namespace Service.BaseBiz
{
    public class UserService : BaseService
    {
        

        private static UserInfo TranslateUserInfo(UserEntity userEntity)
        {
            UserInfo userInfo = new UserInfo();
            if (userEntity != null)
            {
                int[] groupids = userEntity.Groups.Select(t => t.GroupID).ToArray();
                int[] roleids = userEntity.Roles.Select(t => t.RoleID).ToArray();
                StringBuilder groupbuilder = new StringBuilder();
                if (groupids != null && groupids.Length > 0)
                {
                    for (int j = 0; j < groupids.Length; j++)
                    {
                        groupbuilder.Append(groupids[j]);
                        groupbuilder.Append(",");
                    }
                }
                StringBuilder rolebuilder = new StringBuilder();
                if (roleids != null && roleids.Length > 0)
                {
                    for (int j = 0; j < roleids.Length; j++)
                    {
                        rolebuilder.Append(roleids[j]);
                        rolebuilder.Append(",");
                    }
                }
                userInfo.UserID = userEntity.UserID;
                userInfo.UserName = userEntity.UserName;
                userInfo.NickName = userEntity.NickName;
                userInfo.Status = userEntity.Status;
                userInfo.Telephone = userEntity.Telephone;
                userInfo.PrivateTelephone = userEntity.PrivateTelephone;
                userInfo.SalesCount = userEntity.SalesCount;
                userInfo.CityName = userEntity.CityName;
                userInfo.GroupIDs = groupbuilder.ToString().TrimEnd(',');
                userInfo.RoleIDs = rolebuilder.ToString().TrimEnd(',');

            }


            return userInfo;
        }

        private static UserEntity TranslateUserEntity(UserInfo userInfo, bool isRead)
        {
            UserEntity userEntity = new UserEntity();
            if (userInfo != null)
            {
                userEntity.UserID = userInfo.UserID;
                userEntity.UserName = userInfo.UserName;
                userEntity.NickName = userInfo.NickName;                
                userEntity.Status = userInfo.Status;
                userEntity.Telephone = userInfo.Telephone;
                userEntity.PrivateTelephone = userInfo.PrivateTelephone;
                userEntity.SalesCount = userInfo.SalesCount;
                userEntity.CityName = userInfo.CityName;
                if (isRead)
                {
                    List<MenuEntity> allMenus = new List<MenuEntity>();
                    MenuCompare compare = new MenuCompare();
                    if (!string.IsNullOrEmpty(userInfo.RoleIDs))
                    {
                        userEntity.Roles = RoleService.GetRoleByKeys(userInfo.RoleIDs);
                        if (userEntity.Roles.Count > 0)
                        {
                            foreach (RoleEntity r in userEntity.Roles)
                            {
                                allMenus = allMenus.Merge(r.Menus, compare);
                            }
                        }
                    }
                    if (!string.IsNullOrEmpty(userInfo.GroupIDs))
                    {
                        userEntity.Groups = GroupService.GetGroupByKeys(userInfo.GroupIDs);
                        if (userEntity.Groups.Count > 0)
                        {
                            foreach (GroupEntity r in userEntity.Groups)
                            {
                                allMenus = allMenus.Merge(r.Menus, compare);
                            }
                        }
                    }
                    userEntity.Menus = allMenus;
                }
                else
                {
                    //当天咨询量
                    List<InquiryEntity> list = InquiryService.GetInquiryByRule("", "", "", " AND AddDate Between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59'", "新", userEntity.UserID.ToString());
                    userEntity.currentSalesCount = list != null && list.Count > 0 ? list.Count : 0;
                }
            }


            return userEntity;
        }

        public static bool ModifyUser(UserEntity userEntity)
        {
            int result = 0;
            if (userEntity != null)
            {
                UserRepository mr = new UserRepository();

                UserInfo userInfo = TranslateUserInfo(userEntity);


                if (userEntity.UserID > 0)
                {
                    result = mr.ModifyUser(userInfo);
                }
                else
                {
                    userInfo.CreateDate = DateTime.Now;
                    userInfo.Password = EncryptHelper.MD5Encrypt("123456");
                    result = mr.CreateNew(userInfo);
                }

                List<UserInfo> miList = mr.GetAllUser();//刷新缓存
                Cache.Add("UserALL", miList);
            }
            return result > 0;
        }

        public static UserEntity GetUserById(long uid)
        {
            UserEntity result = new UserEntity();
            UserRepository mr = new UserRepository();
            UserInfo info = mr.GetUserByKey(uid);
            result = TranslateUserEntity(info, true);
            return result;
        }

        public static List<UserEntity> GetUserAll(bool isRead = true)
        {
            List<UserEntity> all = new List<UserEntity>();
            UserRepository mr = new UserRepository();
            List<UserInfo> miList = null;// Cache.Get<List<UserInfo>>("UserALL");
            if (miList.IsEmpty())
            {
                miList = mr.GetAllUser();
                Cache.Add("UserALL", miList);
            }
            if (!miList.IsEmpty())
            {
                foreach (UserInfo mInfo in miList)
                {
                    UserEntity userEntity = TranslateUserEntity(mInfo, isRead);
                    all.Add(userEntity);
                }
            }

            return all;

        }

        public static List<UserEntity> GetUserByRule(string name, int status, string nickname="")
        {
            List<UserEntity> all = new List<UserEntity>();
            UserRepository mr = new UserRepository();
            List<UserInfo> miList = mr.GetUsersByRule(name, nickname, status);

            if (!miList.IsEmpty())
            {
                foreach (UserInfo mInfo in miList)
                {
                    UserEntity userEntity = TranslateUserEntity(mInfo, false);
                    all.Add(userEntity);
                }
            }

            return all;

        }

        public static void Remove(long uid)
        {
            UserRepository mr = new UserRepository();
            mr.RemoveUser(uid);
            List<UserInfo> miList = mr.GetAllUser();//刷新缓存
            //Cache.Add("UserALL", miList);
        }

        public static int ModifyPassword(long uid, string pwd)
        {
            UserRepository mr = new UserRepository();
            return mr.ModifyPassword(uid,pwd);
        }

        public static int ModifySalesCountByID(long uid, int salesCount)
        {
            UserRepository mr = new UserRepository();
            return mr.ModifySalesCountByID(uid, salesCount);
        }

        public static UserEntity GetLoginUser(string name, string pwd)
        {
            UserEntity result = new UserEntity();
            try
            {
                UserRepository mr = new UserRepository();
                UserInfo info = mr.GetLoginUser(name, pwd);
                result = TranslateUserEntity(info, true);
            }
            catch (Exception ex)
            {
                Log.WriteTextLog(ex.ToString(), DateTime.Now);               
            }
            return result;
        }

        public static bool IsExists(string name)
        {
            return  GetUserAll().Exists(t=>t.Status==1 && t.UserName==name);
        }
    }
}
