using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.News
{
    public class InquiryMonitorStatement
    {
        public static string GetAllInquiryMonitorByRule = @"SELECT * FROM dt_proInquiry_Monitor(NOLOCK) WHERE 1=1 ";

        public static string CreateNewInquiry = @"INSERT INTO dt_proInquiry_Monitor(PPId,ProductID,OriginOperatorID,OriginSalesName,NewOperatorID,NewSalesName,Remark,CreateDate)
                                                VALUES(@PPId,@ProductID,@OriginOperatorID,@OriginSalesName,@NewOperatorID,@NewSalesName,@Remark,@CreateDate)";


        #region 分页相关
        public static string GetInquiryMonitorCount = @"SELECT COUNT(1) AS C FROM dt_proInquiry_Monitor(NOLOCK) WHERE 1=1 ";

        public static string GetAllInquiryMonitorInfoPager = @"	  DECLARE @UP INT

                                                      IF(@recordCount<@PageSize*(@PageIndex-1)) 
                                                      BEGIN
                                                        SET @PageIndex= @recordCount/@PageSize+1
                                                      END
   
	                                                  IF(@PageIndex<1)
	                                                     SET @PageIndex=1
             
	                                                  IF(@recordCount>@PageSize*(@PageIndex-1))
	                                                  BEGIN
		                                                  SET @UP=@PageSize*(@PageIndex-1);

		                                                  WITH InquiryMonitor AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY CreateDate Desc) AS RowNumber FROM (SELECT * FROM dt_proInquiry_Monitor WHERE 1=1 )as T ) 
		                                                  SELECT *  FROM InquiryMonitor 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";

        public static string GetAllInquiryMonitorInfoPagerHeader = @"	  DECLARE @UP INT
        
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
		                                                  WITH InquiryMonitor AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY CreateDate Desc) AS RowNumber FROM (SELECT * FROM dt_proInquiry_Monitor WHERE 1=1 ";
        public static string GetAllInquiryMonitorInfoPagerFooter = @")as T ) 
		                                                  SELECT *  FROM InquiryMonitor 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";
        #endregion     
        
    }
}
