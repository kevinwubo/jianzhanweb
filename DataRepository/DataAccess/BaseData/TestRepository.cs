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
    public class TestRepository : DataAccessBase
    {

        public DataSet GetDataSet()
        {
            DataSet ds = new DataSet();
            DataCommand command = new DataCommand(ConnectionString, GetDbCommand(TestStatement.GetData, "Text"));
            ds = command.ExecuteDataSet();
            return ds;
        }


    }
}
