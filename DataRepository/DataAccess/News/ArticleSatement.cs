using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.New
{
    public class ArticleSatement
    {

        public static string GetArticleByID = @"SELECT * FROM dt_article(NOLOCK) WHERE id=@id";

        public static string GetArticleByRule = @"SELECT {0} * FROM dt_article(NOLOCK) WHERE 1=1 ";

        #region 分页相关
        public static string GetArticleCount = @"SELECT COUNT(1) AS C FROM dt_article(NOLOCK) WHERE 1=1 ";

        public static string GetAllArticleInfoPager = @"	  DECLARE @UP INT

                                                      IF(@recordCount<@PageSize*(@PageIndex-1)) 
                                                      BEGIN
                                                        SET @PageIndex= @recordCount/@PageSize+1
                                                      END
   
	                                                  IF(@PageIndex<1)
	                                                     SET @PageIndex=1
             
	                                                  IF(@recordCount>@PageSize*(@PageIndex-1))
	                                                  BEGIN
		                                                  SET @UP=@PageSize*(@PageIndex-1);

		                                                  WITH article AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY add_time Desc) AS RowNumber FROM (SELECT * FROM dt_article WHERE 1=1 )as T ) 
		                                                  SELECT *  FROM article 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";

        public static string GetAllArticleInfoPagerHeader = @"	  DECLARE @UP INT
        
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
		                                                  WITH article AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY add_time Desc) AS RowNumber FROM (SELECT * FROM dt_article WHERE 1=1 ";
        public static string GetAllArticleInfoPagerFooter = @")as T ) 
		                                                  SELECT *  FROM article 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";


        public static string ModifyArticle = @"UPDATE dbo.dt_article SET channel_id = @channel_id,category_id = @category_id,call_index = @call_index,title = @title
                                        ,link_url = @link_url,img_url = @img_url,seo_title = @seo_title,seo_keywords = @seo_keywords,seo_description = @seo_description
                                        ,zhaiyao = @zhaiyao,content = @content,sort_id = @sort_id,click = @click,status = @status,user_name = @user_name,update_time = @update_time  WHERE id=@id";

        public static string ModifyContent = @"UPDATE dbo.dt_article SET content = @content ,update_time = @update_time  WHERE id=@id";

        public static string ModifyImageUrlByID = @"UPDATE dbo.dt_article SET img_url = @img_url ,update_time = @update_time  WHERE id=@id";
        #endregion     
    }
}
