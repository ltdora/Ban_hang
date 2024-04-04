using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IOrderService
    {
        BaseRespone<Order> CreateOrder(List<OrderDetail>orderDetails,int IdUser);
        BaseRespone<List<Order>> DisplayOrder(int userID);
        BaseRespone<OrderDetail> SaveOrder(int quantity, int productID, int orderID);
        BaseRespone<OrderDetail> DeleteOrder(int quantity, int productID, int orderID);
    }
    public class OrderService : IOrderService
    {
        private readonly ShopContext _context;
        public OrderService(ShopContext context)
        {
            _context = context;
        }
        public BaseRespone<Order> CreateOrder(List<OrderDetail> orderDetails, int userID)
        {
            BaseRespone<Order> respone = new BaseRespone<Order>();
            
            try
            {
                var liProID = orderDetails.Select(e => e.ProductID).ToList();  ///1,2,3,4,5,6,2

                foreach(var item in liProID)
                {
                    var checkCoSp = _context.Products.Where(i => i.ProductID == item).ToList();
                    if (checkCoSp == null)
                    {
                        respone.Message = "Lỗi chứa sản phẩm không hợp lệ";
                        return respone;
                    }
                    
                    var checkTrungSP = orderDetails.Where(e => e.ProductID == item).ToList().Count();
                    if (checkTrungSP > 1)
                    {
                        respone.Message = "Lỗi sản phẩm trùng";
                        return respone;
                    }

                }
                Order order = new Order();
                order.Status = 0;
                order.UserID = userID;
                order.CreatedTime = DateTime.Now;
                //order.Total = orderDetails.Sum(s => s.Price * s.Quantity);
                foreach (var i in orderDetails)
                {
                    i.OrderID = order.OrderID;
                    order.Total = order.Total + i.Price * i.Quantity;
                }
                _context.Add(order);
                _context.SaveChanges();

                foreach(var i in orderDetails)
                {
                    i.OrderID = order.OrderID;
                }

                _context.AddRange(orderDetails);
                _context.SaveChanges();
                respone.Message = "Mua hàng thành công";
                respone.Data = order;
                return respone;
            }
            catch (Exception ex)
            {
                respone.Type = "Error";
                respone.Message = ex.Message;
                return respone;
            }
        }
        public BaseRespone<List<Order>> DisplayOrder(int userID)
        {
            BaseRespone<List<Order>> response = new BaseRespone<List<Order>>();
            List<Order> ord = new List<Order>();
            try
            {
                ord = _context.Orders.Where(x => x.UserID == userID).ToList();
                if (ord != null)
                {
                    foreach (var itemDonHang in ord)
                    {
                        itemDonHang.orderDetail = _context.OrderDetails.Where(e => e.OrderID == itemDonHang.OrderID).ToList();
                    }
                }
                response.Data = ord;
                response.Message = "Thành công";
                return response;
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = ex.Message;
                return response;
            }
        }
        public BaseRespone<OrderDetail> SaveOrder(int quantity, int productID, int orderID)
        {
            BaseRespone<OrderDetail> response = new BaseRespone<OrderDetail>();
            var liOrderDetail = _context.OrderDetails.Where(x => x.OrderID == orderID).ToList();
            var liOrderID = _context.Orders.Select(x => x.OrderID).ToList();
            try
            {
                foreach(var item in liOrderID)
                {
                    var checkOrderID = _context.Orders.Where(x => x.OrderID == item).ToList();
                    if(checkOrderID == null)
                    {
                        response.Message = "Lỗi chưa có đơn hàng";
                        return response;
                    }
                }
                bool isExist = false;
                foreach(var item in liOrderDetail)
                {
                    //var orderDetail = _context.OrderDetails.FirstOrDefault(od => od.OrderID == orderID && od.ProductID == item.ProductID);
                    if (item.ProductID == productID)
                    {
                        item.Quantity = item.Quantity + quantity;
                        isExist = true;
                        response.Message = "Thêm sản phẩm vào đơn hàng thành công";
                    }
                    
                }

                if (isExist == false)
                {
                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderID = orderID,
                        ProductID = productID,
                        Quantity = quantity
                    });
                }
                _context.SaveChanges();

                return response;
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = ex.Message;
                return response;
            }
        }
        public OrderDetail GetOrderDetailsById(int productID)
        {
            OrderDetail orderDetail;
            try
            {
                orderDetail = _context.Find<OrderDetail>(productID);
            }
            catch (Exception)
            {
                throw;
            }
            return orderDetail;
        }
        public BaseRespone<OrderDetail> DeleteOrder(int quantity, int productID, int orderID)
        {
            try
            {
                BaseRespone<OrderDetail> response = new BaseRespone<OrderDetail>();
                var liOrderDetail = _context.OrderDetails.Where(x => x.OrderID == orderID).ToList();

                var checkOrderID = _context.Orders.Where(x => x.OrderID == orderID).ToList();
                if (checkOrderID == null)
                {
                    response.Message = "Lỗi chưa có đơn hàng";
                    return response;
                }

                bool isExist = false;
                foreach (var item in liOrderDetail)
                {
                    if(item.ProductID == productID)
                    {
                        isExist = true;
                        if (item.Quantity < quantity)
                        {
                            response.Message = "Lỗi số lượng không hợp lệ";
                            return response;
                        }
                        else
                        {
                            if (item.Quantity == quantity)
                            {
                                //var _temp = GetOrderDetailsById(productID);
                                //_context.Remove<OrderDetail>(_temp);
                                _context.Remove<OrderDetail>(item);
                                response.Message = "Xoá sản phẩm thành công";
                            }
                            else
                            {
                                item.Quantity = item.Quantity - quantity;
                                response.Message = "Giảm số lượng sản phẩm thành công";
                            }
                        }
                        _context.SaveChanges();
                    }
                    
                }
                if (isExist == false)
                {
                    response.Message = "Lỗi sản phẩm không hợp lệ";
                    return response;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
