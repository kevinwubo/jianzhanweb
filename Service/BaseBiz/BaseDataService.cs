﻿using DataRepository.DataAccess.BaseData;
using DataRepository.DataModel;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Helper;
using System.Data;

namespace Service.BaseBiz
{
    public class BaseDataService : BaseService
    {
        private static BaseDataInfo TranslateBaseDataInfo(BaseDataEntity baseDataEntity)
        {
            BaseDataInfo baseDataInfo = new BaseDataInfo();
            if (baseDataEntity != null)
            {
                baseDataInfo.ID = baseDataEntity.ID;
                baseDataInfo.TypeCode = baseDataEntity.TypeCode ?? "";
                baseDataInfo.PCode = baseDataEntity.PCode??"";
                baseDataInfo.ValueInfo = baseDataEntity.ValueInfo ?? "";
                baseDataInfo.Description = baseDataEntity.Description ?? "";
                baseDataInfo.Status = baseDataEntity.Status;
            }

            return baseDataInfo;
        }

        private static BaseDataEntity TranslateBaseDataEntity(BaseDataInfo baseDataInfo)
        {
            BaseDataEntity baseDataEntity = new BaseDataEntity();
            if (baseDataInfo != null)
            {
                baseDataEntity.ID = baseDataInfo.ID;
                baseDataEntity.TypeCode = baseDataInfo.TypeCode;
                baseDataEntity.PCode = baseDataInfo.PCode;
                baseDataEntity.ValueInfo = baseDataInfo.ValueInfo;
                baseDataEntity.Description = baseDataInfo.Description;
                baseDataEntity.Status = baseDataInfo.Status;
            }

            return baseDataEntity;
        }

        private static AttachmentEntity TranslateAttachmentInfo(AttachmentInfo attachmentInfo,bool isAPI)
        {
            AttachmentEntity result = new AttachmentEntity();
            if (attachmentInfo != null)
            {
                result.AttachmentID = attachmentInfo.AttachmentID;
                result.FileName = attachmentInfo.FileName;
                result.FileExtendName = attachmentInfo.FileExtendName;
                if (isAPI)
                    result.FilePath = attachmentInfo.FilePath.Replace("~", "");
                else
                    result.FilePath = attachmentInfo.FilePath;
                result.UploadDate = attachmentInfo.UploadDate;
                result.FileType = attachmentInfo.FileType;
                result.BusinessType = attachmentInfo.BusinessType;
                result.Channel = attachmentInfo.Channel;
                result.FileSize = attachmentInfo.FileSize;
                result.Remark = attachmentInfo.Remark;
                result.Operator = attachmentInfo.Operator;
            }


            return result;
        }

        private static AttachmentInfo TranslateAttachmentEntity(AttachmentEntity attachmentInfo)
        {
            AttachmentInfo result = new AttachmentInfo();
            if (attachmentInfo != null)
            {
                result.AttachmentID = attachmentInfo.AttachmentID;
                result.FileName = attachmentInfo.FileName;
                result.FileExtendName = attachmentInfo.FileExtendName;
                result.FilePath = attachmentInfo.FilePath;
                result.UploadDate = attachmentInfo.UploadDate;
                result.FileType = attachmentInfo.FileType;
                result.BusinessType = attachmentInfo.BusinessType;
                result.Channel = attachmentInfo.Channel;
                result.FileSize = attachmentInfo.FileSize;
                result.Remark = attachmentInfo.Remark;
                result.Operator = attachmentInfo.Operator;
            }


            return result;
        }

        public static long CreateAttachment(AttachmentEntity attachmentInfo)
        {
            long result=0;
            BaseDataRepository mr = new BaseDataRepository();

            AttachmentInfo attachment = TranslateAttachmentEntity(attachmentInfo);
            attachment.CreateDate = DateTime.Now;
            result = mr.CreateNewAttachment(attachment);

            return result;
        }

        public static List<AttachmentEntity> GetAttachmentInfoByKyes(string ids, bool isAPI = false)
        {
            List<AttachmentEntity> all = new List<AttachmentEntity>();
            BaseDataRepository mr = new BaseDataRepository();
            List<AttachmentInfo> miList = mr.GetAttachments(ids);

            if (!miList.IsEmpty())
            {
                foreach (AttachmentInfo mInfo in miList)
                {
                    AttachmentEntity attachEntity = TranslateAttachmentInfo(mInfo, isAPI);
                    all.Add(attachEntity);
                }
            }

            return all;
        }


        /// <summary>
        /// API接口返回  包含完整路径
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public static List<AttachmentEntity> GetAttachmentAPIInfoByKyes(string ids)
        {
            List<AttachmentEntity> all = new List<AttachmentEntity>();
            BaseDataRepository mr = new BaseDataRepository();
            List<AttachmentInfo> miList = mr.GetAttachments(ids);

            if (!miList.IsEmpty())
            {
                foreach (AttachmentInfo mInfo in miList)
                {
                    AttachmentEntity attachEntity = TranslateAttachmentInfo(mInfo, true);
                    all.Add(attachEntity);
                }
            }

            return all;
        }

        /// <summary>
        /// 更新产品队列信息
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public static bool ModifyCodes(CodeSEntity entity)
        {
            BaseDataRepository mr = new BaseDataRepository();
            int result = 0;
            if (entity != null)
            {
                CodeSInfo info = new CodeSInfo();
                info.ID = entity.ID;
                info.CodeValues = entity.CodeValues;
                result = mr.ModifyCodes(info);
            }
            return result > 0;
        }

        public static bool ModifyBaseData(BaseDataEntity baseDataEntity)
        {
            int result = 0;
            if (baseDataEntity != null)
            {
                BaseDataRepository mr = new BaseDataRepository();

                BaseDataInfo baseDataInfo = TranslateBaseDataInfo(baseDataEntity);


                if (baseDataEntity.ID > 0)
                {
                    result = mr.ModifyData(baseDataInfo);
                }
                else
                {
                    baseDataInfo.CreateDate = DateTime.Now;
                    result = mr.CreateNew(baseDataInfo);
                }
                List<BaseDataInfo> miList = mr.GetAllData();//刷新缓存
                Cache.Add("BaseDataALL", miList);
            }
            return result > 0;
        }

        public static List<Province> GetAllProvince()
        {
            List<Province> result = Cache.Get<List<Province>>("ProvinceALL")??new List<Province>();

            if (result.IsEmpty())
            {
               BaseDataRepository mr = new BaseDataRepository();
               List<ProvinceInfo> pInfo = mr.GetAllProvince();

               foreach (var item in pInfo)
               {
                   Province p = new Province();
                   p.ProvinceID = item.ProvinceID;
                   p.ProvinceName = item.ProvinceName;
                   result.Add(p);
               }
               Cache.Add("ProvinceALL", result);
            }

            return result;
        }

        /// <summary>
        /// 获取活动手机号--临时使用
        /// </summary>
        /// <returns></returns>
        public static List<string> GetTelephone()
        {
            List<string> list = new List<string>();
            BaseDataRepository mr = new BaseDataRepository();
            DataSet ds = mr.GetTelephone();
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        string tel = dr["Telephone"].ToString();
                        if (!string.IsNullOrEmpty(tel) && !list.Contains(tel))
                        {
                            list.Add(tel);
                        }
                    }
                }
            }
            return list;
        }

        public static long InsertTelephone(string telphone)
        {
            BaseDataRepository mr = new BaseDataRepository();
            return mr.InsertTelephone(telphone);
        }

        public static string GetPCEwmCode()
        {
            String url = "";
            BaseDataRepository mr = new BaseDataRepository();
            DataSet ds = mr.GetQRCode();
            if (ds != null)
            {
                DataTable dt = ds.Tables[0];
                if (dt != null && dt.Rows.Count > 0)
                {
                    url = URL + dt.Rows[0]["QRImage"];
                }
            }
            return url;
        }

        public static List<City> GetAllCity()
        {
            List<City> result = Cache.Get<List<City>>("CityALL") ?? new List<City>();
            List<Province> allProvinces = GetAllProvince();
            if (result.IsEmpty())
            {
                BaseDataRepository mr = new BaseDataRepository();
                List<CityInfo> cInfo = mr.GetAllCity();

                foreach (var item in cInfo)
                {
                    City c= new City();
                    c.CityID = item.CityID;
                    c.CityName = item.CityName;
                    c.ProvinceID = item.ProvinceID;
                    c.ProvinceInfo = allProvinces.FirstOrDefault(t => t.ProvinceID == item.ProvinceID) ?? new Province();
                    result.Add(c);
                }
                Cache.Add("CityALL", result);
            }

            return result;
        }

        public static List<City> GetAllHasCity()
        {
            List<City> result = Cache.Get<List<City>>("GetAllHasCity") ?? new List<City>();            
            if (result.IsEmpty())
            {
                BaseDataRepository mr = new BaseDataRepository();
                List<CityInfo> cInfo = mr.GetAllHasCity();
                foreach (var item in cInfo)
                {
                    City c = new City();
                    c.CityID = item.CityID;
                    c.CityName = item.CityName;
                    c.ProvinceID = item.ProvinceID;                    
                    result.Add(c);
                }
                Cache.Add("GetAllHasCity", result);
            }

            return result;
        }

        public static BaseDataEntity GetBaseDataById(int id)
        {
            BaseDataEntity result = new BaseDataEntity();
            BaseDataRepository mr = new BaseDataRepository();
            BaseDataInfo info = mr.GetBaseDataByKey(id);
            result = TranslateBaseDataEntity(info);
            return result;
        }

        public static List<BaseDataEntity> GetBaseDataAll()
        {
            List<BaseDataEntity> all = new List<BaseDataEntity>();
            BaseDataRepository mr = new BaseDataRepository();
            List<BaseDataInfo> miList = mr.GetAllData();//Cache.Get<List<BaseDataInfo>>("BaseDataALLN");
            //if (miList.IsEmpty())
            //{
            //    miList = mr.GetAllData();
            //    Cache.Add("BaseDataALL", miList);
            //}

            if (!miList.IsEmpty())
            {
                foreach (BaseDataInfo mInfo in miList)
                {
                    BaseDataEntity BaseDataEntity = TranslateBaseDataEntity(mInfo);
                    all.Add(BaseDataEntity);
                }
            }

            return all;
        }



        

        public static List<BaseDataEntity> GetBaseDataByType(string tcode)
        {
            List<BaseDataEntity> all = new List<BaseDataEntity>();
            BaseDataRepository mr = new BaseDataRepository();
            List<BaseDataInfo> miList = mr.GetDataByType(tcode);

            if (!miList.IsEmpty())
            {
                foreach (BaseDataInfo mInfo in miList)
                {
                    BaseDataEntity BaseDataEntity = TranslateBaseDataEntity(mInfo);
                    all.Add(BaseDataEntity);
                }
            }

            return all;

        }

        public static List<BaseDataEntity> GetBaseDataByPCode(string pcode)
        {
            List<BaseDataEntity> all = new List<BaseDataEntity>();
            BaseDataRepository mr = new BaseDataRepository();
            List<BaseDataInfo> miList = mr.GetAllDataByPCode(pcode);

            if (!miList.IsEmpty())
            {
                foreach (BaseDataInfo mInfo in miList)
                {
                    BaseDataEntity BaseDataEntity = TranslateBaseDataEntity(mInfo);
                    all.Add(BaseDataEntity);
                }
            }

            return all;

        }

        public static List<BaseDataEntity> GetBaseDataByRule(string desc)
        {
            List<BaseDataEntity> all = new List<BaseDataEntity>();
            BaseDataRepository mr = new BaseDataRepository();
            List<BaseDataInfo> miList = mr.GetBaseDataByRule(desc);

            if (!miList.IsEmpty())
            {
                foreach (BaseDataInfo mInfo in miList)
                {
                    BaseDataEntity BaseDataEntity = TranslateBaseDataEntity(mInfo);
                    all.Add(BaseDataEntity);
                }
            }

            return all;

        }

        /// <summary>
        /// 获取队列信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static List<CodeSEntity> GetCodeListByRule(string code,string isshow)
        {
            List<CodeSEntity> list = new List<CodeSEntity>();
            BaseDataRepository mr = new BaseDataRepository();
            List<CodeSInfo> miList = mr.GetCodeValuesByRule(code, isshow);

            if (!miList.IsEmpty())
            {
                foreach (CodeSInfo info in miList)
                {
                    CodeSEntity entity = new CodeSEntity();
                    entity.ID = info.ID;
                    entity.Code = info.Code;
                    entity.CodeName = info.CodeName;
                    entity.CodeValues = info.CodeValues;
                    entity.change_date = info.change_date.ToString();
                    list.Add(entity);
                }
            }
            return list;

        }


        public static CodeSEntity GetCodeValuesByRule(string code)
        {
            CodeSEntity entity = new CodeSEntity();
            BaseDataRepository mr = new BaseDataRepository();
            List<CodeSInfo> miList = mr.GetCodeValuesByRule(code,"");

            if (!miList.IsEmpty())
            {
                foreach (CodeSInfo info in miList)
                {
                    entity.ID = info.ID;
                    entity.Code = info.Code;
                    entity.CodeName = info.CodeName;
                    entity.CodeValues = info.CodeValues;
                }
            }

            return entity;

        }


        public static List<BaseDataEntity> GetAllPayType()
        {
            List<BaseDataEntity> payType = GetBaseDataAll().Where(p => p.PCode == "P00" && p.Status == 1).ToList();
            return payType;
        }


        public static void Remove(int id)
        {
            BaseDataRepository mr = new BaseDataRepository();
            mr.Remove(id);
        }

        //public static void AddVerificationCode(VerificationCodeEntity entity)
        //{
        //    BaseDataRepository mr = new BaseDataRepository();
        //    VerificationCodeInfo info = new VerificationCodeInfo();
        //    info.Mobile = entity.Mobile;
        //    info.Email = entity.Email;
        //    info.VCode = entity.VCode;
        //    info.Status = entity.Status;
        //    info.DeadLine = entity.DeadLine;
        //    mr.AddVerificationCode(info);

        //}

        //public static VerificationCodeEntity CheckVerificationCode(string telephone, string vcode)
        //{
        //    BaseDataRepository mr = new BaseDataRepository();
        //    VerificationCodeInfo info=  mr.CheckVerificationCode(telephone, vcode);
        //    VerificationCodeEntity entity = new VerificationCodeEntity();
        //    if (info != null)
        //    {
        //        entity.Mobile = info.Mobile;
        //        entity.Email = info.Email;
        //        entity.VCode = info.VCode;
        //        entity.Status = info.Status;
        //        entity.DeadLine = info.DeadLine;
        //    }
        //    return entity;
        //}
    }
}
