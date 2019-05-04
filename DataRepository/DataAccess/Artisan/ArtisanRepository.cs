/* ==============================================================================
 * Copyright (C) CtripCorpBiz OR Author. All rights reserved.
 * 
 * 类名称：ArtisanRepository
 * 类描述：
 * 创建人：Ethen Shen
 * 创建时间：4/28/2018 9:56:46 AM
 * 修改人：
 * 修改时间：
 * 修改备注：
 * 代码请保留相关关键处的注释
 * ==============================================================================*/

using DataRepository.DataAccess.Artisan;
using DataRepository.DataAccess.Artisan;
using DataRepository.DataModel;
using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.Artisan
{
    public class ArtisanRepository : DataAccessBase
    {



        public List<ArtisanInfo> GetAllArtisan()
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.GetAllArtisan, "Text"));
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }

        public List<ArtisanInfo> GetArtisansByRule(string artisanType, string IsCooperation)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            string sqlText=ArtisanSatement.GetAllArtisanByRule;
            if (!string.IsNullOrEmpty(artisanType))
            {
                sqlText += " AND artisanType =@artisanType";
            }

            if (!string.IsNullOrEmpty(IsCooperation))
            {
                sqlText += " AND IsCooperation =@IsCooperation";
            }


            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(artisanType))
            {
                command.AddInputParameter("@artisanType", DbType.String, artisanType);
            }
            if (!string.IsNullOrEmpty(IsCooperation))
            {
                command.AddInputParameter("@IsCooperation", DbType.String, IsCooperation);
            }
           
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }

        public ArtisanInfo GetArtisanByKey(string artisanID)
        {
            ArtisanInfo result = new ArtisanInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.GetArtisanByArtisanID, "Text"));
            command.AddInputParameter("@artisanID", DbType.String, artisanID);
            result = command.ExecuteEntity<ArtisanInfo>();
            return result;
        }

        public List<ArtisanInfo> GetArtisanByKeys(string keys)
        {
            List<ArtisanInfo> result = new List<ArtisanInfo>();
            string sqlText = ArtisanSatement.GetArtisanByKeys;
            sqlText = sqlText.Replace("#ids#", keys);
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            result = command.ExecuteEntityList<ArtisanInfo>();
            return result;
        }

       

        public int RemoveArtisan(long mid)
        {
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ArtisanSatement.Remove, "Text"));
            command.AddInputParameter("@ArtisanID", DbType.Int64, mid);
            int result=command.ExecuteNonQuery();
            return result;
        }

    }
}
