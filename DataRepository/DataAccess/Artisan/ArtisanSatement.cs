/* ==============================================================================
 * Copyright (C) CtripCorpBiz OR Author. All rights reserved.
 * 
 * 类名称：MenuSatement
 * 类描述：
 * 创建人：Ethen Shen
 * 创建时间：4/28/2018 9:53:40 AM
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

namespace DataRepository.DataAccess.Artisan
{
    public class ArtisanSatement
    {
        public static string GetAllArtisan = @"SELECT * FROM dt_Artisan(NOLOCK) ";

        public static string GetAllArtisanByRule = @"SELECT * FROM dt_Artisan(NOLOCK) WHERE 1=1 ";

        public static string GetArtisanByArtisanID= @"SELECT * FROM dt_Artisan(NOLOCK) WHERE ArtisanID=@ArtisanID";

        public static string Remove = @"UPDATE dt_Artisan SET Status=0 WHERE ArtisanID=@ArtisanID";

        public static string GetArtisanByKeys = @"SELECT * FROM dt_Artisan(NOLOCK) WHERE ID IN (#ids#)";

    }
}
