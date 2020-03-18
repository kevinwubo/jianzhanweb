using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.News
{
    public class InquiryStatement
    {
        public static string IntoHistoryInquiry = @"insert into dbo.dt_proInquiry_history  select * from  dbo.dt_proInquiry where PPId  in(@ppids)  delete from dt_proInquiry where PPId in(@ppids)";

        public static string GetAllInquiry = @"SELECT * FROM dt_proInquiry(NOLOCK)";

        public static string GetAllInquiryByRule = @"SELECT * FROM dt_proInquiry(NOLOCK) WHERE 1=1 ";

        public static string GetInquiryByKey = @"SELECT * FROM dt_proInquiry(NOLOCK) WHERE PPId=@PPId";

        public static string Remove = @"UPDATE dt_proInquiry SET datastatus=0,ChangeDate=GETDATE() WHERE PPId=@PPId";

        public static string GetInquiryByKeys = @"SELECT * FROM dt_proInquiry(NOLOCK) WHERE PPId IN (#ids#)";

        public static string CreateSimpleInquiry = @"INSERT INTO [dt_proInquiry]
                                                   (ProductID,telphone,WebChartID,Provence,City,InquiryContent,CustomerName,OperatorID,status,ProcessingState,SourceForm,TraceState,HistoryOperatorID,IpAddress,AddDate,transferCount)
                                                    VALUES(@ProductID,@telphone,@WebChartID,@Provence,@City,@InquiryContent,@CustomerName,@OperatorID,@status,@ProcessingState,@SourceForm,@TraceState,@HistoryOperatorID,@IpAddress,@AddDate,@transferCount) select @@IDENTITY";


        public static string CreateNewInquiry = @"INSERT INTO [dt_proInquiry]
                                            ([ProductID],[telphone],[WebChartID],[InquiryContent],[CommentContent],[ProcessingState],[ProcessingTime],[Provence]
                                            ,[City],[TraceContent],[TraceState],[NextVisitTime],[CustomerName],[sex],[status],[SourceForm],[AddDate],[OperatorID],[datastatus])
                                                 VALUES(@ProductID,@telphone,@WebChartID,@InquiryContent,@CommentContent,@ProcessingState,@ProcessingTime,@Provence,@City
                                            ,@TraceContent,@TraceState,@NextVisitTime,@CustomerName,@sex,@status,@SourceForm,@AddDate,@OperatorID,@datastatus) select @@IDENTITY";
        //[SourceForm] = @SourceForm,
        public static string ModifyInquiry = @"UPDATE [dbo].[dt_proInquiry]   SET [ProductID] = @ProductID,[telphone] = @telphone,[WebChartID] = @WebChartID,[InquiryContent] = @InquiryContent
                                                ,[CommentContent] = @CommentContent,[ProcessingState] = @ProcessingState,[ProcessingTime] = @ProcessingTime,[Provence] = @Provence
                                                ,[City] = @City,[TraceContent] = @TraceContent,[TraceState] = @TraceState,[NextVisitTime] = @NextVisitTime,[CustomerName] = @CustomerName
                                                ,[sex] = @sex,[status] = @status,[OperatorID] = @OperatorID,ChangeDate=GETDATE()
                                                 WHERE PPId=@PPId";
        public static string ModifyInquiryStatus = @"UPDATE [dbo].[dt_proInquiry]   SET [CommentContent] = @CommentContent,[status] = @status    WHERE PPId=@PPId";

        #region 分页相关
        public static string GetInquiryCount = @"SELECT COUNT(1) AS C FROM dt_proInquiry(NOLOCK) WHERE 1=1 ";

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
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Adddate Desc) AS RowNumber FROM (SELECT * FROM dt_proInquiry WHERE 1=1 )as T ) 
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
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Adddate Desc) AS RowNumber FROM (SELECT * FROM dt_proInquiry WHERE 1=1 ";
        public static string GetAllInquiryInfoPagerFooter = @")as T ) 
		                                                  SELECT *  FROM Inquiry 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";
        #endregion     

        #region 自定义SQL
        /// <summary>
        /// 获取当前队列销售数据
        /// </summary>
        public static string GetLastSaleName = @"select  b.real_name ,b.salesCount,COUNT(1) as countCurrentDay,a.OperatorID from dt_proInquiry a ,dt_manager b where a.OperatorID=b.id and b.real_name =@real_name  and   status='新' and AddDate between '" + DateTime.Now.ToShortDateString() + " 00:00:01' and '" + DateTime.Now.ToShortDateString() + " 23:59:59' group by real_name,b.salesCount ";

        public static string GetLastSaleNameByCodes = @"select top 1 b.real_name,salesCount,0 as countCurrentDay,a.OperatorID from dbo.dt_proInquiry a,dt_manager b where a.OperatorID=b.id  and status='新' and status!='Hand' and b.real_name in({0}) order by PPId desc ";

        public static string GetLastSaleNameByOperatorID = @"select top 1 b.real_name,a.OperatorID,salesCount,0 as countCurrentDay from dbo.dt_proInquiry a,dt_manager b where a.HistoryOperatorID=b.id  and status='新' and status!='Hand' and a.HistoryOperatorID in({0}) order by PPId desc ";

        public static string UpdateOperatorIDByPPId = @"UPDATE dt_proInquiry SET OperatorID=@OperatorID,ProcessingState='0',ChangeDate=GETDATE() WHERE PPId=@PPId";

        public static string GetDistinctTelephone = @"select distinct telphone from  dbo.dt_proInquiry(NOLOCK) WHERE 1=1";


        #endregion


        #region 资讯日志记录
        public static string CreateInuiryLog = @"INSERT INTO dt_Inquiry_Log(ProductID,Telephone,JMTelephone,SourceForm,CreateDate)VALUES(@ProductID,@Telephone,@JMTelephone,@SourceForm,@CreateDate)";
        #endregion
    }
}
