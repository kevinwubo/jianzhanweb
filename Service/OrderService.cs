using Common;
using DataRepository.DataAccess.News;
using DataRepository.DataModel;
using Entity.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Helper;
using DataRepository.DataAccess.Order;
using Service.BaseBiz;
namespace Service
{
    public class OrderService
    {
        public static OrderEntity GetOrderEntityById(long cid)
        {
            OrderEntity result = new OrderEntity();
            OrderInfoRepository mr = new OrderInfoRepository();
            OrderInfo info = mr.GetOrderByKey(cid);
            result = TranslateOrderEntity(info);
            result.orderDetailList = GetAllOrderDetailInfoByRule(info.ID.ToString());
            return result;
        }


        public static List<OrderDetailEntity> GetAllOrderDetailInfoByRule(string orderid)
        {
            List<OrderDetailEntity> list = new List<OrderDetailEntity>();
                OrderInfoRepository mr = new OrderInfoRepository();
                List<OrderDetailInfo> orderDetailList = mr.GetAllOrderDetailInfoByRule(orderid.ToInt(0));
            if (orderDetailList != null && orderDetailList.Count > 0)
            {
                foreach (OrderDetailInfo info in orderDetailList)
                {
                    OrderDetailEntity entity= TranslateOrderDetailEntity(info);
                    if (entity != null)
                    {
                        list.Add(entity);
                    }
                }
            }
            return list;
        }


        private static OrderDetailEntity TranslateOrderDetailEntity(OrderDetailInfo info)
        {
            OrderDetailEntity entity = new OrderDetailEntity();
            entity.DetailID = info.DetailID;
            entity.OrderID = info.OrderID;
            entity.Author = info.Author;
            entity.ProductID = info.ProductID;
            entity.ProductName = info.ProductName;
            entity.Quantity = info.Quantity;
            entity.CostPrice = info.CostPrice;
            entity.SalePrice = info.SalePrice;
            entity.CollectedSalePrice = info.CollectedSalePrice;
            entity.Rate = info.Rate;

            entity.Adddate = info.Adddate;
            return entity;
        }


        private static OrderEntity TranslateOrderEntity(OrderInfo info)
        {
            OrderEntity entity = new OrderEntity();
            entity.ID = info.ID;
            entity.OrderNo = info.OrderNo;
            entity.PayWay = info.PayWay;
            entity.CollectedAmount = info.CollectedAmount;
            entity.CollectedDate = info.CollectedDate;
            entity.TransportNumber = info.TransportNumber;
            entity.DeliveryDate = info.DeliveryDate;
            entity.Province = info.Province;
            entity.City = info.City;
            entity.Address = info.Address;
            entity.ReceiverName = info.ReceiverName;
            entity.Telephone = info.Telephone;
            entity.OperatorID = info.OperatorID;
            entity.Remark = info.Remark;
            entity.Status = info.Status;
            entity.Adddate = info.Adddate;

            City city = BaseDataService.GetAllCity().FirstOrDefault(t => t.CityID == info.City.ToInt(0)) ?? new City();
            Province province = BaseDataService.GetAllProvince().FirstOrDefault(t => t.ProvinceID == info.Province.ToInt(0)) ?? new Province();
            entity.provinceinfo = province;
            entity.cityinfo = city;
            entity.provinceandCity = (province != null ? province.ProvinceName : "") + (city != null ? city.CityName : "");
            return entity;
        }

        private static OrderInfo TranslateOrderInfo(OrderEntity entity)
        {
            OrderInfo info = new OrderInfo();
            if (info != null)
            {
                info.ID = entity.ID;
                info.OrderNo = entity.OrderNo;
                info.PayWay = entity.PayWay;
                info.CollectedAmount = entity.CollectedAmount;
                info.CollectedDate = entity.CollectedDate;
                info.TransportNumber = entity.TransportNumber;
                info.DeliveryDate = entity.DeliveryDate;
                info.Province = entity.Province;
                info.City = entity.City;
                info.Address = entity.Address;
                info.ReceiverName = entity.ReceiverName;
                info.Telephone = entity.Telephone;
                info.OperatorID = entity.OperatorID;
                info.Remark = entity.Remark;
                info.Status = entity.Status;
                info.Adddate = entity.Adddate;
            }

            return info;
        }

        public static bool ModifyOrder(OrderEntity entity)
        {
            long result = 0;
            if (entity != null)
            {
                OrderInfoRepository mr = new OrderInfoRepository();

                OrderInfo OrderInfo = TranslateOrderInfo(entity);

                OrderJsonEntity jsonlist = null;
                if (!string.IsNullOrEmpty(entity.OrderDetailJson))
                {
                    try
                    {
                        jsonlist = (OrderJsonEntity)JsonHelper.FromJson(entity.OrderDetailJson, typeof(OrderJsonEntity));

                    }
                    catch (Exception ex)
                    {
                        string str = ex.ToString();
                    }
                }

                if (entity.ID > 0)
                {
                    OrderInfo.ID = entity.ID;
                    OrderInfo.ChangeDate = DateTime.Now;
                    result = mr.ModifyOrder(OrderInfo);
                }
                else
                {
                    OrderInfo.ChangeDate = DateTime.Now;
                    OrderInfo.Adddate = DateTime.Now;
                    result = mr.CreateNew(OrderInfo);
                }


                #region 更新产品信息
                if (jsonlist != null)
                {
                    OrderInfoRepository cr = new OrderInfoRepository();
                    cr.RemoveOrderDetailByOrderID(entity.ID > 0 ? entity.ID : int.Parse(result.ToString()));

                    List<OrderDetailJsonEntity> list = jsonlist.orderDetail;
                    if (list != null && list.Count > 0)
                    {
                        foreach (OrderDetailJsonEntity cc in list)
                        {
                            
                            OrderDetailInfo orderdetail = new OrderDetailInfo();
                            orderdetail.Author = cc.Author;
                            orderdetail.ProductID = cc.ProductID;
                            orderdetail.ProductName = cc.ProductName;
                            orderdetail.Quantity = string.IsNullOrEmpty(cc.Quantity) ? 0 : int.Parse(cc.Quantity);
                            orderdetail.SalePrice = string.IsNullOrEmpty(cc.SalePrice) ? 0 : decimal.Parse(cc.SalePrice);
                            orderdetail.CollectedSalePrice = string.IsNullOrEmpty(cc.CollectedSalePrice) ? 0 : decimal.Parse(cc.CollectedSalePrice);
                            orderdetail.CostPrice =0;
                            orderdetail.Rate = 0;
                            orderdetail.OrderID = entity.ID > 0 ? entity.ID : int.Parse(result.ToString());
                            orderdetail.Adddate = DateTime.Now;
                            cr.CreateNewOrderDetail(orderdetail);

                        }
                    }
                }
                #endregion  

            }
            return result > 0;
        }

        public static OrderEntity GetOrderById(long gid)
        {
            OrderEntity result = new OrderEntity();
            OrderInfoRepository mr = new OrderInfoRepository();
            OrderInfo info = mr.GetOrderByKey(gid);
            result = TranslateOrderEntity(info);
            return result;
        }

        public static List<OrderEntity> GetOrderAll()
        {
            List<OrderEntity> all = new List<OrderEntity>();
            OrderInfoRepository mr = new OrderInfoRepository();
            List<OrderInfo> miList = mr.GetAllOrder();//Cache.Get<List<OrderInfo>>("OrderALL");
            //if (miList.IsEmpty())
            //{
            //    miList = mr.GetAllOrder();
            //    Cache.Add("OrderALL", miList);
            //}
            if (!miList.IsEmpty())
            {
                foreach (OrderInfo mInfo in miList)
                {
                    OrderEntity OrderEntity = TranslateOrderEntity(mInfo);
                    all.Add(OrderEntity);
                }
            }

            return all;

        }

        public static List<OrderEntity> GetOrderByRule(string name, int status)
        {
            List<OrderEntity> all = new List<OrderEntity>();
            OrderInfoRepository mr = new OrderInfoRepository();
            List<OrderInfo> miList = mr.GetOrderByRule(name, status);

            if (!miList.IsEmpty())
            {
                foreach (OrderInfo mInfo in miList)
                {
                    OrderEntity OrderEntity = TranslateOrderEntity(mInfo);
                    all.Add(OrderEntity);
                }
            }

            return all;

        }

        public static List<OrderEntity> GetOrderByKeys(string ids)
        {
            List<OrderEntity> all = new List<OrderEntity>();
            OrderInfoRepository mr = new OrderInfoRepository();
            List<OrderInfo> miList = mr.GetOrderByKeys(ids);

            if (!miList.IsEmpty())
            {
                foreach (OrderInfo mInfo in miList)
                {
                    OrderEntity entity = TranslateOrderEntity(mInfo);
                    all.Add(entity);
                }
            }

            return all;
        }

        public static int RemoveOrderDetail(long gid)
        {
            OrderInfoRepository mr = new OrderInfoRepository();
            int i = mr.RemoveOrderDetail(gid);
            //List<OrderInfo> miList = mr.GetAllOrder();//刷新缓存
            //Cache.Add("OrderALL", miList);
            return i;
        }

        public static int Remove(long gid)
        {
            OrderInfoRepository mr = new OrderInfoRepository();
            int i = mr.Remove(gid);
            //List<OrderInfo> miList = mr.GetAllOrder();//刷新缓存
            //Cache.Add("OrderALL", miList);
            return i;
        }

        #region 分页相关
        public static int GetOrderCount(string name, string tracestate, int status)
        {
            return new OrderInfoRepository().GetOrderCount(name, tracestate, -1);
        }

        public static List<OrderEntity> GetOrderInfoPager(PagerInfo pager)
        {
            List<OrderEntity> all = new List<OrderEntity>();
            OrderInfoRepository mr = new OrderInfoRepository();
            List<OrderInfo> miList = mr.GetAllOrderInfoPager(pager);
            foreach (OrderInfo mInfo in miList)
            {
                OrderEntity carEntity = TranslateOrderEntity(mInfo);
                all.Add(carEntity);
            }
            return all;
        }

        public static List<OrderEntity> GetOrderInfoByRule(string name, string receivername, int status, PagerInfo pager)
        {
            List<OrderEntity> all = new List<OrderEntity>();
            OrderInfoRepository mr = new OrderInfoRepository();
            List<OrderInfo> miList = mr.GetAllOrderInfoByRule(name, receivername, status, pager);

            if (!miList.IsEmpty())
            {
                foreach (OrderInfo mInfo in miList)
                {
                    OrderEntity storeEntity = TranslateOrderEntity(mInfo);
                    all.Add(storeEntity);
                }
            }

            return all;
        }
        #endregion
    }
}
