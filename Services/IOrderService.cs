using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IOrderService
    {
        BaseRespone<OrderDetail> CreateOrder(List<OrderDetail>orderDetails,int IdUser);
        BaseRespone<OrderDetail> SaveOrder(int quantity, int productID, int orderID);
    }
    public class OrderService : IOrderService
    {
        private readonly ShopContext _context;
        public OrderService(ShopContext context)
        {
            _context = context;
        }
        public BaseRespone<OrderDetail> CreateOrder(List<OrderDetail> orderDetails, int userID)
        {
            
            try
            {
                BaseRespone<OrderDetail> respone = new BaseRespone<OrderDetail>();
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
                _context.Add(order);
                _context.SaveChanges();

                foreach(var i in orderDetails)
                {
                    i.OrderID = order.OrderID;
                }

                _context.AddRange(orderDetails);
                _context.SaveChanges();

                return new BaseRespone<OrderDetail>();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public BaseRespone<OrderDetail> SaveOrder(int quantity, int productID, int orderID)
        {
            try
            {
                BaseRespone<OrderDetail> response = new BaseRespone<OrderDetail>();
                var liProID = _context.OrderDetails.Select(x => x.ProductID).ToList();
                var liOrderID = _context.Orders.Select(x => x.OrderID).ToList();
                foreach(var item in liOrderID)
                {
                    var checkOrderID = _context.Orders.Where(x => x.OrderID == item).ToList();
                    if(checkOrderID == null)
                    {
                        response.Message = "Lỗi chưa có đơn hàng";
                        return response;
                    }
                }
                foreach(var item in liProID)
                {
                    var orderDetail = _context.OrderDetails.FirstOrDefault(od => od.OrderID == orderID && od.ProductID == item);
                    if (orderDetail != null)
                    {
                        orderDetail.Quantity = orderDetail.Quantity + quantity;
                    }
                    else
                    {
                        orderDetail = new OrderDetail
                        {
                            OrderID = orderID,
                            ProductID = productID,
                            Quantity = quantity
                        };
                        _context.OrderDetails.Add(orderDetail);
                    }

                    _context.SaveChanges();
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
