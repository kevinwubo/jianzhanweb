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

        public static string GetAllArtisanByRule = @"SELECT  * FROM dt_Artisan(NOLOCK) WHERE 1=1 ";

        public static string GetTopCountArtisanByRule = @"SELECT {0} * FROM dt_Artisan(NOLOCK) WHERE 1=1 ";

        public static string GetArtisanByArtisanID= @"SELECT * FROM dt_Artisan(NOLOCK) WHERE ArtisanID=@ArtisanID";

        public static string Remove = @"UPDATE dt_Artisan SET Status=0 WHERE ArtisanID=@ArtisanID";

        public static string GetArtisanByKeys = @"SELECT * FROM dt_Artisan(NOLOCK) WHERE ID IN (#ids#)";


        #region 分页相关
        public static string GetArtisanCount = @"SELECT COUNT(1) AS C FROM dt_Artisan(NOLOCK) WHERE 1=1 ";

        public static string GetAllArtisanInfoPager = @"	  DECLARE @UP INT

                                                      IF(@recordCount<@PageSize*(@PageIndex-1)) 
                                                      BEGIN
                                                        SET @PageIndex= @recordCount/@PageSize+1
                                                      END
   
	                                                  IF(@PageIndex<1)
	                                                     SET @PageIndex=1
             
	                                                  IF(@recordCount>@PageSize*(@PageIndex-1))
	                                                  BEGIN
		                                                  SET @UP=@PageSize*(@PageIndex-1);

		                                                  WITH Artisan AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Sort) AS RowNumber FROM (SELECT * FROM dt_Artisan WHERE 1=1 )as T ) 
		                                                  SELECT *  FROM Artisan 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";

        public static string GetAllArtisanInfoPagerHeader = @"	  DECLARE @UP INT
        
	                                                  ---------分页区间计算-------------最大页数
                                                      IF(@recordCount<@PageSize*(@PageIndex-1)) 
                                                      BEGIN
                                                        SET @PageIndex= @recordCount/@PageSize+1
                                                      END
                                                      --最小页数
	                                                  IF(@PageIndex<1)
	                                                     SET @PageIndex=1
                                                      --当前页起始游标值
	                                                  IF(@recordCount>@PageSize*(@PageIndex-1))
	                                                  BEGIN
		                                                  SET @UP=@PageSize*(@PageIndex-1);
		                                                  ---------分页查询-----------
		                                                  WITH Artisan AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Sort) AS RowNumber FROM (SELECT * FROM dt_Artisan WHERE 1=1 ";
        public static string GetAllArtisanInfoPagerFooter = @")as T ) 
		                                                  SELECT *  FROM Artisan 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";
        #endregion     

        #region 自定义SQL

        #endregion

    }
}
