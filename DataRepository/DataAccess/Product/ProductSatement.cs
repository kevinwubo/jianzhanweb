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

namespace DataRepository.DataAccess.Product
{
    public class ProductSatement
    {

        public static string GetProductByProductID= @"SELECT * FROM dt_Product(NOLOCK) WHERE ProductID=@ProductID";

        public static string GetAllProduct = @"SELECT * FROM dt_Product(NOLOCK)";

        public static string GetAllProductByRule = @"SELECT * FROM dt_Product(NOLOCK) WHERE 1=1 ";

        public static string GetAllProductTopCountByRule = @"SELECT {0} * FROM dt_Product(NOLOCK) WHERE 1=1 ";

        public static string GetProductByKey = @"SELECT * FROM dt_Product(NOLOCK) WHERE PPId=@PPId";

        public static string Remove = @"UPDATE dt_Product SET datastatus=0 WHERE PPId=@PPId";

        public static string GetProductByKeys = @"SELECT * FROM dt_Product(NOLOCK) WHERE PPId IN (#ids#)";

        public static string CreateNewProduct = @"INSERT INTO [dbo].[dt_Product]
                                                ([ProductID],[ProductName],[SubTitle],[Type1],[Type2],[Type3],[Type4],[Type5],[Type6],[Type7],[Images],[summary]
                                                ,[ProductDetail],[InventoryCount],[Material],[Volume],[CostPrice],[MarketPrice],[LowPrice],[ArtisanID],[VideoUrl]
                                                ,[VideoDetail],[PlatePosition],[Author],[ProImageDetail],[IsPushMall],[AddDate],[UpdateDate])
                                                VALUES(@ProductID,@ProductName,@SubTitle,@Type1,@Type2,@Type3,@Type4,@Type5,@Type6,@Type7,@Images,@summary
                                                ,@ProductDetail,@InventoryCount,@Material,@Volume,@CostPrice,@MarketPrice,@LowPrice,@ArtisanID,@VideoUrl
                                                ,@VideoDetail,@PlatePosition,@Author,@ProImageDetail,@IsPushMall,@AddDate,@UpdateDate) SELECT @@IDENTITY";

        public static string ModifyProduct = @"UPDATE [dbo].[dt_Product]   SET [ProductID] = @ProductID,[ProductName] = @ProductName,[SubTitle] = @SubTitle,[Type1] = @Type1
                                                ,[Type2] = @Type2,[Type3] = @Type3,[Type4] = @Type4,[Type5] = @Type5,[Type6] = @Type6,[Type7] = @Type7,[Images] = @Images
                                                ,[summary] = @summary,[ProductDetail] = @ProductDetail,[InventoryCount] = @InventoryCount,[Material] = @Material,[Volume] = @Volume
                                                ,[CostPrice] = @CostPrice,[MarketPrice] = @MarketPrice,[LowPrice] = @LowPrice,[ArtisanID] = @ArtisanID,[VideoUrl] = @VideoUrl
                                                ,[VideoDetail] = @VideoDetail,[PlatePosition] = @PlatePosition,[Author] = @Author,[ProImageDetail] = @ProImageDetail,[IsPushMall] = @IsPushMall
                                                ,[UpdateDate] = @UpdateDate WHERE ID=@ID";


        #region 分页相关
        public static string GetProductCount = @"SELECT COUNT(1) AS C FROM dt_Product(NOLOCK) WHERE 1=1 ";

        public static string GetAllProductInfoPager = @"	  DECLARE @UP INT

                                                      IF(@recordCount<@PageSize*(@PageIndex-1)) 
                                                      BEGIN
                                                        SET @PageIndex= @recordCount/@PageSize+1
                                                      END
   
	                                                  IF(@PageIndex<1)
	                                                     SET @PageIndex=1
             
	                                                  IF(@recordCount>@PageSize*(@PageIndex-1))
	                                                  BEGIN
		                                                  SET @UP=@PageSize*(@PageIndex-1);

		                                                  WITH Product AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Adddate,InventoryCount Desc) AS RowNumber FROM (SELECT * FROM dt_Product WHERE 1=1 )as T ) 
		                                                  SELECT *  FROM Product 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";

        public static string GetAllProductInfoPagerHeader = @"	  DECLARE @UP INT
        
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
		                                                  WITH Product AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Adddate,InventoryCount Desc) AS RowNumber FROM (SELECT * FROM dt_Product WHERE 1=1 ";
        public static string GetAllProductInfoPagerFooter = @")as T ) 
		                                                  SELECT *  FROM Product 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";


        public static string GetAllProductInfoSouChangPager = @"	  DECLARE @UP INT

                                                      IF(@recordCount<@PageSize*(@PageIndex-1)) 
                                                      BEGIN
                                                        SET @PageIndex= @recordCount/@PageSize+1
                                                      END
   
	                                                  IF(@PageIndex<1)
	                                                     SET @PageIndex=1
             
	                                                  IF(@recordCount>@PageSize*(@PageIndex-1))
	                                                  BEGIN
		                                                  SET @UP=@PageSize*(@PageIndex-1);

		                                                  WITH Product AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY MarketPrice Desc) AS RowNumber FROM (SELECT * FROM dt_Product WHERE 1=1 )as T ) 
		                                                  SELECT *  FROM Product 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";

        public static string GetAllProductInfoSouChangPagerHeader = @"	  DECLARE @UP INT
        
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
		                                                  WITH Product AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY MarketPrice Desc) AS RowNumber FROM (SELECT * FROM dt_Product WHERE 1=1 ";
        public static string GetAllProductInfoSouChangPagerFooter = @")as T ) 
		                                                  SELECT *  FROM Product 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";
        #endregion     


        #region 自定义SQL
        public static string GetProductBySqlWhere = @"SELECT DISTINCT top {0} b.* FROM dt_Product AS a  CROSS APPLY(   SELECT TOP {1} ProductID,IsPushMall,'' as summary ,'' as ProImageDetail,'' as ProductDetail,ProductName,SubTitle,Type1,Type2,Type3,Type4,Type5,Type6,Type7,Images,Material ,Volume,CostPrice,MarketPrice,LowPrice,ArtisanID,VideoUrl,VideoDetail,AddDate,UpdateDate,PlatePosition,Author,InventoryCount FROM dt_Product WHERE a.Author=Author {2} ORDER BY AddDate DESC ) AS b order by AddDate desc ";

        public static string GetProductBySqlWhere2 = @"SELECT {0} ProductID,ProductName,Images,Author,case InventoryCount when 0 then '已结缘' else Author end as ShowTitle,InventoryCount, 
                 (select sort from dt_Artisan  where artisanName=dt_Product.Author) as ArtisanSort ,
                (select COUNT(1) from dt_Product where ProductID=dt_Product.ProductID) as ProductCount,Type3,(select CountAll from dt_VisitCount where OID=dt_Product.ProductID) as CountAll FROM dt_Product
                 where 1=1 and InventoryCount>=0 {1}
                order by {2}";
        #endregion
    }
}
