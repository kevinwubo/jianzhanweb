using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.News
{
    public class InquiryLogStatement
    {
        public static string GetAllInquiry = @"SELECT * FROM dt_proInquiry_history_Log(NOLOCK)";

        public static string GetAllInquiryByRule = @"SELECT * FROM dt_proInquiry_history_Log(NOLOCK) WHERE 1=1 ";

        public static string GetInquiryByKey = @"SELECT * FROM dt_proInquiry_history_Log(NOLOCK) WHERE PPId=@PPId";

        public static string Remove = @"UPDATE dt_proInquiry_history_Log SET datastatus=0,ChangeDate=GETDATE() WHERE PPId=@PPId";

        public static string GetInquiryByKeys = @"SELECT * FROM dt_proInquiry_history_Log(NOLOCK) WHERE PPId IN (#ids#)";


        public static string CreateNewInquiry = @"INSERT INTO dt_proInquiry_history_Log
                                                    (ProductID,telphone,WebChartID,ProcessingState,Provence,City,TraceState,CustomerName,status,SourceForm,AddDate,OperatorID,HistoryOperatorID
                                                    ,datastatus,ChangeDate)     VALUES
                                                    (@ProductID,@telphone,@WebChartID,@ProcessingState,@Provence,@City,@TraceState,@CustomerName,@status,@SourceForm,@AddDate,@OperatorID,@HistoryOperatorID
                                                    ,@datastatus,@ChangeDate) select @@IDENTITY";


        #region 分页相关
        public static string GetInquiryCount = @"SELECT COUNT(1) AS C FROM dt_proInquiry_history_Log(NOLOCK) WHERE 1=1 ";

        public static string GetAllInquiryInfoPager = @"	  DECLARE @UP INT

                                                      IF(@recordCount<@PageSize*(@PageIndex-1)) 
                                                      BEGIN
                                                        SET @PageIndex= @recordCount/@PageSize+1
                                                      END
   
	                                                  IF(@PageIndex<1)
	                                                     SET @PageIndex=1
             
	                                                  IF(@recordCount>@PageSize*(@PageIndex-1))
	                                                  BEGIN
		                                                  SET @UP=@PageSize*(@PageIndex-1);

		                                                  WITH Inquiry AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Adddate Desc) AS RowNumber FROM (SELECT * FROM dt_proInquiry_history_Log WHERE 1=1 )as T ) 
		                                                  SELECT *  FROM Inquiry 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";

        public static string GetAllInquiryInfoPagerHeader = @"	  DECLARE @UP INT
        
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
		                                                  WITH Inquiry AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Adddate Desc) AS RowNumber FROM (SELECT * FROM dt_proInquiry_history_Log WHERE 1=1 ";
        public static string GetAllInquiryInfoPagerFooter = @")as T ) 
		                                                  SELECT *  FROM Inquiry 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";
        #endregion     

    }
}
