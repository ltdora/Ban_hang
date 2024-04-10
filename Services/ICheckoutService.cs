using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface ICheckoutService
    {/// <summary>
    /// Thanh toan don hang
    /// </summary>
    /// <param name="orderID">ID don hang</param>
    /// <param name="status">Trang thai don hang</param>
    /// <returns></returns>
        BaseRespone<Order> CheckoutOrder(int orderID, int status);
    }
    public class CheckoutService : ICheckoutService
    {
        private readonly ShopContext _context;
        public CheckoutService(ShopContext context)
        {
            _context = context;
        }
        public BaseRespone<Order> CheckoutOrder(int orderID, int status)
        {
            BaseRespone<Order> response = new BaseRespone<Order>();
            List<Order> ord = new List<Order>();

            var liOrder = _context.Orders.Where(x => x.OrderID == orderID).ToList();
            try
            {

                if (liOrder.Count != 0)
                {
                    foreach (var item in liOrder)
                    {
                        item.Status = status;
                        switch (status)
                        {
                            case 1:
                                response.Message = "Thanh toán đơn hàng thành công";
                                break;
                            case 2:
                                response.Message = "Hủy đơn hàng thành công";
                                break;
                            default:
                                {
                                    response.Message = "Trạng thái đơn hàng không hợp lệ";
                                    return response;
                                }
                        }
                        response.Type = "Success";
                        response.Data = item;

                        ord = _context.Orders.Where(x => x.OrderID == orderID).ToList();

                        foreach (var itemDonHang in ord)
                        {
                            itemDonHang.orderDetail = _context.OrderDetails.Where(e => e.OrderID == itemDonHang.OrderID).ToList();
                        }
                    }
                }
                else
                {
                    response.Message = "Đơn hàng không tồn tại";
                    response.Type = "Success";
                    return response;
                }
                _context.SaveChanges();
                return response;
            }
            catch(Exception ex)
            {
                response.Type = "Error";
                response.Message = ex.Message;
                return response;
            }
        }
    }
}
