/* ==============================================================================
 * Copyright (C) CtripCorpBiz OR Author. All rights reserved.
 * 
 * 类名称：PagerInfo
 * 类描述：
 * 创建人：Ethen Shen
 * 创建时间：5/9/2018 2:55:11 PM
 * 修改人：
 * 修改时间：
 * 修改备注：
 * 代码请保留相关关键处的注释
 * ==============================================================================*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class PagerInfo
    {
        public string URL { get; set; }

        //第几页 从1开始
        public int PageIndex { get; set; }

        //每页多少条
        public int PageSize { get; set; }

        //总条数
        public int SumCount { get; set; }

        //总页数
        public int PageCount
        {
            get {
                int result = 0;
                if (PageSize > 0 && SumCount > 0)
                {
                    if ((SumCount % PageSize) > 0)
                    {
                        result = (int)(SumCount / PageSize) + 1;
                    }
                    else
                    {
                        result = (int)(SumCount / PageSize);
                    }
                }

                return result;
            
            }
        }
    }
}
