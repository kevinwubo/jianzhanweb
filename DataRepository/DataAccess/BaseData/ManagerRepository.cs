/* ==============================================================================
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
    public class ManagerRepository : DataAccessBase
    {

        public List<ManagerInfo> GetAllManager()
        {
            List<ManagerInfo> result = new List<ManagerInfo>();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ManagerStatement.GetAllManager, "Text"));
            result = command.ExecuteEntityList<ManagerInfo>();
            return result;
        }

        public ManagerInfo GetManagerByID(int id)
        {
            ManagerInfo result = new  ManagerInfo();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(ManagerStatement.GetManagerByID, "Text"));
            command.AddInputParameter("@id", DbType.Int32, id);
            result = command.ExecuteEntity<ManagerInfo>();
            return result;
        }

        public List<ManagerInfo> GetBaseDataBySalesName(string salesname)
        {
            List<ManagerInfo> result = new List<ManagerInfo>();
            string sqlText = ManagerStatement.GetBaseDataBySalesName;
            if (!string.IsNullOrEmpty(salesname))
            {
                sqlText += " AND real_name=@real_name";
            }
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(salesname))
            {
                command.AddInputParameter("@real_name", DbType.String, salesname);
            }

            result = command.ExecuteEntityList<ManagerInfo>();
            return result;
        }



        public List<ManagerInfo> GetManagerByRule(string salesname)
        {
            List<ManagerInfo> result = new List<ManagerInfo>();
            string sqlText = ManagerStatement.GetManagerByRule;
            if (!string.IsNullOrEmpty(salesname))
            {
                sqlText += " AND real_name=@real_name";
            }
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(sqlText, "Text"));
            if (!string.IsNullOrEmpty(salesname))
            {
                command.AddInputParameter("@real_name", DbType.String, salesname);
            }

            result = command.ExecuteEntityList<ManagerInfo>();
            return result;
        }

    }
}
