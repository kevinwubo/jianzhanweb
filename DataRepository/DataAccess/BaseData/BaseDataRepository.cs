﻿/* ==============================================================================
 * Copyright (C) CtripCorpBiz OR Author. All rights reserved.
 * 
 * 类名称：MenuRepository
 * 类描述：
 * 创建人：Ethen Shen
 * 创建时间：4/28/2018 9:56:46 AM
 * 修改人：
 * 修改时间：
 * 修改备注：
 * 代码请保留相关关键处的注释
 * ==============================================================================*/

using DataRepository.DataModel;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.BaseData
{
    public class BaseDataRepository : DataAccessBase
    {

        /// <summary>
        /// 获取活动手机号 临时使用
        /// </summary>
        /// <returns></returns>
        public long InsertTelephone(string telephone)        {

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.InsertTelephone, "Text"));
            command.AddInputParameter("@Telephone", DbType.String, telephone);

            var o = command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }

        /// <summary>
        /// 获取活动手机号 临时使用
        /// </summary>
        /// <returns></returns>
        public DataSet GetTelephone()
        {
            DataSet ds = new DataSet();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetTelephone, "Text"));
            ds = command.ExecuteDataSet();
            return ds;
        }

        /// <summary>
        /// 获取二维码  PC使用
        /// </summary>
        /// <returns></returns>
        public DataSet GetQRCode()
        {
            DataSet ds = new DataSet();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetQRCode, "Text"));
            ds = command.ExecuteDataSet();
            return ds;
        }

        public List<CityInfo> GetAllCity()
        {
            List<CityInfo> result = new List<CityInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetAllCity, "Text"));
            result = command.ExecuteEntityList<CityInfo>();
            return result;
        }

        public List<CityInfo> GetAllHasCity()
        {
            List<CityInfo> result = new List<CityInfo>();
            string sqlText = BaseDataStatement.GetAllHasCity;
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetAllHasCity, "Text"));
            result = command.ExecuteEntityList<CityInfo>();
            return result;
        }

        public List<ProvinceInfo> GetAllProvince()
        {
            List<ProvinceInfo> result = new List<ProvinceInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetAllProvince, "Text"));
            result = command.ExecuteEntityList<ProvinceInfo>();
            return result;
        }

        public List<AttachmentInfo> GetAttachments(string keys)
        {
            List<AttachmentInfo> result = new List<AttachmentInfo>();
            if (!string.IsNullOrEmpty(keys))
            {
                string sqlText = BaseDataStatement.GetAttachmentByKey;
                sqlText = sqlText.Replace("#ids#", keys.Replace(",,",","));
                DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
                result = command.ExecuteEntityList<AttachmentInfo>();
            }
            return result;
        }

        public long CreateNewAttachment(AttachmentInfo attachment)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.CreateAttachment, "Text"));
            command.AddInputParameter("@FileName", DbType.String, attachment.FileName);
            command.AddInputParameter("@FileExtendName", DbType.String, attachment.FileExtendName);
            command.AddInputParameter("@FilePath", DbType.String, attachment.FilePath);
            command.AddInputParameter("@UploadDate", DbType.String, attachment.UploadDate);
            command.AddInputParameter("@FileType", DbType.String, attachment.FileType);
            command.AddInputParameter("@BusinessType", DbType.String, attachment.BusinessType);
            command.AddInputParameter("@Channel", DbType.String, attachment.Channel);
            command.AddInputParameter("@FileSize", DbType.String, attachment.FileSize);
            command.AddInputParameter("@Remark", DbType.String, attachment.Remark);
            command.AddInputParameter("@Operator", DbType.Int64, attachment.Operator);
            command.AddInputParameter("@CreateDate", DbType.DateTime, attachment.CreateDate);

            var o=command.ExecuteScalar<object>();
            return Convert.ToInt64(o);
        }


        public List<BaseDataInfo> GetAllData()
        {
            List<BaseDataInfo> result = new List<BaseDataInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetAllBaseData, "Text"));
            result = command.ExecuteEntityList<BaseDataInfo>();
            return result;
        }

        public List<BaseDataInfo> GetAllDataByPCode(string pcode)
        {
            List<BaseDataInfo> result = new List<BaseDataInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetBaseDataByPCode, "Text"));
            command.AddInputParameter("@PCode", DbType.String, pcode);
            result = command.ExecuteEntityList<BaseDataInfo>();
            return result;
        }

        public List<BaseDataInfo> GetDataByType(string typeCode)
        {
            List<BaseDataInfo> result = new List<BaseDataInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetBaseDataByType, "Text"));
            command.AddInputParameter("@TypeCode", DbType.String, typeCode);
            result = command.ExecuteEntityList<BaseDataInfo>();
            return result;
        }

        public List<CodeSInfo> GetCodeValuesByRule(string code, string isShow)
        {
            List<CodeSInfo> result = new List<CodeSInfo>();
            string sqlText = BaseDataStatement.GetCodeValuesByRule;
            if (!string.IsNullOrEmpty(code))
            {
                sqlText += " AND code=@code";
            }

            if (!string.IsNullOrEmpty(isShow))
            {
                sqlText += " AND isShow=@isShow";
            }

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(code))
            {
                command.AddInputParameter("@code", DbType.String, code);
            }
            if (!string.IsNullOrEmpty(isShow))
            {
                command.AddInputParameter("@isShow", DbType.String, isShow);
            }

            result = command.ExecuteEntityList<CodeSInfo>();
            return result;
        }

        public List<BaseDataInfo> GetBaseDataByRule(string desc)
        {
            List<BaseDataInfo> result = new List<BaseDataInfo>();
            string sqlText = BaseDataStatement.GetAllBaseDataByRule;
            if (!string.IsNullOrEmpty(desc))
            {
                sqlText += " AND Description LIKE '%'+@key+'%'";
            }
            sqlText += " ORDER BY TypeCode";

            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(desc))
            {
                command.AddInputParameter("@key", DbType.String, desc);
            }

            result = command.ExecuteEntityList<BaseDataInfo>();
            return result;
        }

        public BaseDataInfo GetBaseDataByKey(int id)
        {
            BaseDataInfo result = new BaseDataInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.GetBaseDataByKey, "Text"));
            command.AddInputParameter("@ID", DbType.Int32, id);
            result = command.ExecuteEntity<BaseDataInfo>();
            return result;
        }

        public int CreateNew(BaseDataInfo data)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.CreateNewData, "Text"));
           command.AddInputParameter("@TypeCode",DbType.String,data.TypeCode);
		   command.AddInputParameter("@PCode",DbType.String,data.PCode);
		   command.AddInputParameter("@ValueInfo",DbType.String,data.ValueInfo);
		   command.AddInputParameter("@Description",DbType.String,data.Description);
		   command.AddInputParameter("@Status",DbType.Int32,data.Status);
           command.AddInputParameter("@CreateDate", DbType.DateTime, data.CreateDate);

            return command.ExecuteNonQuery();
        }

        public int ModifyData(BaseDataInfo data)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.ModifyBaseData, "Text"));
            command.AddInputParameter("@ID", DbType.Int32, data.ID);
            command.AddInputParameter("@TypeCode", DbType.String, data.TypeCode);
            command.AddInputParameter("@PCode", DbType.String, data.PCode);
            command.AddInputParameter("@ValueInfo", DbType.String, data.ValueInfo);
            command.AddInputParameter("@Description", DbType.String, data.Description);
            command.AddInputParameter("@Status", DbType.Int32, data.Status);

            return command.ExecuteNonQuery();
        }

        /// <summary>
        /// 更新产品队列数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public int ModifyCodes(CodeSInfo data)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.ModifyCodes, "Text"));
            command.AddInputParameter("@ID", DbType.Int32, data.ID);
            command.AddInputParameter("@CodeValues", DbType.String, data.CodeValues);
            return command.ExecuteNonQuery();
        }
        

        public int Remove(int id)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(BaseDataStatement.Remove, "Text"));
            command.AddInputParameter("@ID", DbType.Int32, id);
            int result=command.ExecuteNonQuery();
            return result;
        }
    }
}
