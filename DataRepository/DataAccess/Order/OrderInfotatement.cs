using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataRepository.DataAccess.Order
{
    public class OrderInfoStatement
    {


        public static string GetAllOrderInfo = @"SELECT * FROM dt_OrderInfo(NOLOCK)";

        public static string GetAllOrderInfoByRule = @"SELECT * FROM dt_OrderInfo(NOLOCK) WHERE 1=1 ";

        public static string GetOrderInfoByKey = @"SELECT * FROM dt_OrderInfo(NOLOCK) WHERE ID=@ID";

        public static string Remove = @"UPDATE dt_OrderInfo SET Status=0 WHERE ID=@ID";

        public static string GetOrderInfoByKeys = @"SELECT * FROM dt_OrderInfo(NOLOCK) WHERE ID IN (#ids#)";

        public static string CreateNewOrderInfo = @"INSERT INTO [dbo].[dt_OrderInfo]([OrderNo],[PayWay],[CollectedAmount],[CollectedDate],[TransportNumber],[DeliveryDate],[Province],[City],[Address],[ReceiverName],[Telephone],[OperatorID],[Remark],[Status],[Adddate],ChangeDate)     
                                                    VALUES(@OrderNo,@PayWay,@CollectedAmount,@CollectedDate,@TransportNumber,@DeliveryDate,@Province,@City,@Address,@ReceiverName,@Telephone,@OperatorID,@Remark,@Status,@Adddate,@ChangeDate)
                                                    SELECT @@IDENTITY";

        public static string ModifyOrderInfo = @"UPDATE [dbo].[dt_OrderInfo]
                                                    SET [OrderNo] = @OrderNo,[PayWay] = @PayWay,[CollectedAmount] = @CollectedAmount,[CollectedDate] = @CollectedDate,[TransportNumber] = @TransportNumber
                                                    ,[DeliveryDate] = @DeliveryDate,[Province] = @Province,[City] = @City,[Address] = @Address,[ReceiverName] = @ReceiverName,[Telephone] = @Telephone
                                                    ,[OperatorID] = @OperatorID,[Remark] = @Remark,[Status] = @Status,ChangeDate=@ChangeDate WHERE ID=@ID";


        #region 订单明细相关
        public static string CreateNewOrderDetailInfo = @"INSERT INTO [dt_OrderDetailInfo]([OrderID],[Author],[ProductID],[ProductName],[Quantity],[CostPrice],[SalePrice],[CollectedSalePrice],[Rate],[AddDate])
                                                                VALUES(@OrderID,@Author,@ProductID,@ProductName,@Quantity,@CostPrice,@SalePrice,@CollectedSalePrice,@Rate,@AddDate)";

        public static string RemoveOrderDetailInfo = @"DELETE FROM [dt_OrderDetailInfo] WHERE DetailID=@DetailID";

        public static string RemoveOrderDetailInfoByOrderID = @"DELETE FROM [dt_OrderDetailInfo] WHERE OrderID=@OrderID";

        public static string GetAllOrderDetailInfoByRule = @"SELECT * FROM [dt_OrderDetailInfo] (NOLOCK) WHERE 1=1 ";
        #endregion


        #region 分页相关
        public static string GetOrderInfoCount = @"SELECT COUNT(1) AS C FROM dt_OrderInfo(NOLOCK) WHERE 1=1 ";

        public static string GetAllOrderInfoPager = @"	  DECLARE @UP INT

                                                      IF(@recordCount<@PageSize*(@PageIndex-1)) 
                                                      BEGIN
                                                        SET @PageIndex= @recordCount/@PageSize+1
                                                      END
   
	                                                  IF(@PageIndex<1)
	                                                     SET @PageIndex=1
             
	                                                  IF(@recordCount>@PageSize*(@PageIndex-1))
	                                                  BEGIN
		                                                  SET @UP=@PageSize*(@PageIndex-1);

		                                                  WITH OrderInfo AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Adddate Desc) AS RowNumber FROM (SELECT * FROM dt_OrderInfo WHERE 1=1 )as T ) 
		                                                  SELECT *  FROM OrderInfo 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";

        public static string GetAllOrderInfoPagerHeader = @"	  DECLARE @UP INT
        
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
		                                                  WITH OrderInfo AS
		                                                  (SELECT *,ROW_NUMBER() OVER (ORDER BY Adddate Desc) AS RowNumber FROM (SELECT * FROM dt_OrderInfo WHERE 1=1 ";
        public static string GetAllOrderInfoPagerFooter = @")as T ) 
		                                                  SELECT *  FROM OrderInfo 
		                                                  WHERE RowNumber BETWEEN @UP+1 AND @UP+@PageSize
	                                                  END";
        #endregion     

    }
}
