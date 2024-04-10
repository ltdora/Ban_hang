using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IOrderService
    {
        /// <summary>
        /// Tạo đơn hàng
        /// </summary>
        /// <param name="orderDetails">Thông tin chi tiết của đơn hàng</param>
        /// <param name="IdUser">Mã người dùng</param>
        /// <returns>Dữ liệu kiểu BaseRespone cho biết công việc đã làm</returns>
        BaseRespone<Order> CreateOrder(List<OrderDetail>orderDetails,int IdUser);

        /// <summary>
        /// Hiển thị danh sách đơn hàng theo người dùng
        /// </summary>
        /// <param name="userID">Mã người dùng</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị danh sách đơn hàng theo người dùng</returns>
        BaseRespone<List<Order>> DisplayOrder(int userID);

        /// <summary>
        /// Hiển thị danh sách đơn hàng theo trạng thái thanh toán
        /// </summary>
        /// <param name="status">Trạng thái đơn hàng</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị danh sách đơn hàng theo trạng thái</returns>
        BaseRespone<List<Order>> DisplayOrderStatus(int status);

        /// <summary>
        /// Hiển thị danh sách đơn hàng theo trạng thái và khoảng thời gian
        /// </summary>
        /// <param name="status">Trạng thái đơn hàng</param>
        /// <param name="startTime">Giới hạn thời gian đầu</param>
        /// <param name="endTime">Giới hạn thời gian cuối</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị danh sách đơn dàng theo trạng thái và khoảng thời gian</returns>
        BaseRespone<List<Order>> DisplayOrderStatusTime(int status, DateTime startTime, DateTime endTime);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        BaseRespone<List<Order>> DisplayExcelOrder();

        /// <summary>
        /// Lấy thông tin đơn hàng theo mã đơn hàng
        /// </summary>
        /// <param name="orderID">Mã đơn hàng</param>
        /// <returns>Thông tin đơn hàng dưới dạng Model Order</returns>
        Order GetOrdersById(int orderID);

        /// <summary>
        /// Cập nhật thêm sản phẩm vào đơn hàng
        /// </summary>
        /// <param name="quantity">Số lượng sản phẩm</param>
        /// <param name="productID">Mã sản phẩm</param>
        /// <param name="orderID">Mã đơn hàng</param>
        /// <param name="price">Giá sản phẩm</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị thông tin đơn hàng được cập nhật</returns>
        BaseRespone<Order> SaveOrder(int quantity, int productID, int orderID, decimal price);

        /// <summary>
        /// Cập nhật bớt sản phầm trong đơn hàng
        /// </summary>
        /// <param name="quantity">Số lượng</param>
        /// <param name="productID">Mã sản phẩm</param>
        /// <param name="orderID">Mã đơn hàng</param>
        /// <returns>Dữ liệu kiểu BaseRespone hiển thị thông tin đơn hàng được cập nhật</returns>
        BaseRespone<Order> DeleteOrder(int quantity, int productID, int orderID);

        /// <summary>
        /// Hiển thị danh sách đơn hàng
        /// </summary>
        /// <returns>Danh sách đơn hàng dưới dạng List</returns>
        List<Order> GetOrderList();
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
        public Order GetOrdersById(int orderID)
        {
            Order Order;
            try
            {
                Order = _context.Find<Order>(orderID);
            }
            catch (Exception)
            {
                throw;
            }
            return Order;
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
        public BaseRespone<List<Order>> DisplayOrderStatusTime(int status, DateTime startTime, DateTime endTime)
        {
            BaseRespone<List<Order>> response = new BaseRespone<List<Order>>();
            var ordertime = _context.Orders.Where(o => o.CreatedTime >= startTime && o.CreatedTime <= endTime && o.Status == status).ToList();
            decimal total = 0;
            try
            {
                if (ordertime != null)
                {
                    foreach (var item in ordertime)
                    {
                        item.orderDetail = _context.OrderDetails.Where(e => e.OrderID == item.OrderID).ToList();
                        total = total + item.Total;
                       
                    }
                }
                switch (status)
                {
                    case 0:
                        response.Message = $"Danh sách đơn hàng chưa thanh toán. Tổng tiền các đơn hàng: {total}";
                        break;
                    case 1:
                        response.Message = $"Danh sách đơn hàng đã thanh toán. Tổng tiền các đơn hàng: {total}";
                        break;
                    case 2:
                        response.Message = $"Danh sách đơn hàng bị hủy. Tổng tiền các đơn hàng: {total}";
                        break;
                    default:
                        response.Message = "Trạng thái đơn hàng không hợp lệ";
                        break;
                }
                response.Data = ordertime;
               
                return response;
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = ex.Message;
                return response;
            }
        }
        public BaseRespone<List<Order>> DisplayOrderStatus(int status)
        {
            BaseRespone<List<Order>> response = new BaseRespone<List<Order>>();
            List<Order> ord = new List<Order>();
            decimal total = 0;
            try
            {
                ord = _context.Orders.Where(x => x.Status == status).ToList();
                if(ord != null)
                {
                    foreach(var item in ord)
                    {
                        item.orderDetail = _context.OrderDetails.Where(e => e.OrderID == item.OrderID).ToList();
                        total = total + item.Total;
                    }
                }
                response.Data = ord;
                switch (status)
                {
                    case 0:
                        response.Message = $"Danh sách đơn hàng chưa thanh toán. Tổng tiền các đơn hàng: {total}";
                        break;
                    case 1:
                        response.Message = $"Danh sách đơn hàng đã thanh toán. Tổng tiền các đơn hàng: {total}";
                        break;
                    case 2:
                        response.Message = $"Danh sách đơn hàng bị hủy. Tổng tiền các đơn hàng: {total}";
                        break;
                    default:
                        response.Message = "Trạng thái đơn hàng không hợp lệ";
                        break;
                }
                return response;
            }
            catch (Exception ex)
            {
                response.Type = "Error";
                response.Message = ex.Message;
                return response;
            }
        }
        public BaseRespone<List<Order>> DisplayExcelOrder()
        {
            BaseRespone<List<Order>> response = new BaseRespone<List<Order>>();
            List<Order> ord = new List<Order>();
            try
            {
                ord = _context.Orders.ToList();
                
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
        public BaseRespone<Order> SaveOrder(int quantity, int productID, int orderID, decimal price)
        {
            BaseRespone<Order> response = new BaseRespone<Order>();
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
                    if (item.ProductID == productID)
                    {
                        item.Quantity = item.Quantity + quantity;
                        isExist = true;
                        response.Message = "Tăng số lượng sản phẩm thành công";
                    }
                    
                }

                if (isExist == false)
                {
                    _context.OrderDetails.Add(new OrderDetail
                    {
                        OrderID = orderID,
                        ProductID = productID,
                        Quantity = quantity,
                        Price = price
                    });
                    response.Message = "Thêm mới sản phẩm vào đơn hàng thành công";
                }
                Order order = GetOrdersById(orderID);
                order.Total = order.Total + quantity * price;
                _context.Update<Order>(order);
                response.Data = order;
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
        public BaseRespone<Order> DeleteOrder(int quantity, int productID, int orderID)
        {
            try
            {
                BaseRespone<Order> response = new BaseRespone<Order>();
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
                        Order order = GetOrdersById(orderID);
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
                                order.Total = order.Total - quantity * item.Price;
                                _context.Update<Order>(order);
                                response.Message = "Xoá sản phẩm thành công";
                            }
                            else
                            {
                                item.Quantity = item.Quantity - quantity;
                                order.Total = order.Total - quantity * item.Price;
                                _context.Update<Order>(order);
                                response.Message = "Giảm số lượng sản phẩm thành công";
                            }
                        }
                        _context.SaveChanges();
                        response.Data = order;

                        var liCheckOrder = _context.OrderDetails.Select(x => x.ProductID).ToList();
                        if(order.Total == 0)
                        {
                            _context.Remove<Order>(order);
                            response.Message = "Không còn sản phẩm, xóa đơn hàng thành công";
                            _context.SaveChanges();
                        }
                    }
                    
                }
                if (isExist == false)
                {
                    response.Message = "Lỗi sản phẩm không hợp lệ";
                    response.Type = "Success";
                    return response;
                }
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Order> GetOrderList()
        {
            List<Order> liorder;
            try
            {
                liorder = _context.Set<Order>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return liorder;
        }
    }
}
