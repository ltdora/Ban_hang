using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface ICheckoutService
    {
        BaseRespone<Order> CheckoutOrder(int orderID);
    }
    public class CheckoutService : ICheckoutService
    {
        private readonly ShopContext _context;
        public CheckoutService(ShopContext context)
        {
            _context = context;
        }
        public BaseRespone<Order> CheckoutOrder(int orderID)
        {
            BaseRespone<Order> respone = new BaseRespone<Order>();
            List<Order> ord = new List<Order>();

            var liOrder = _context.Orders.Where(x => x.OrderID == orderID).ToList();
            try
            {

                if (liOrder == null)
                {
                    respone.Message = "Đơn hàng không tồn tại";
                    return respone;
                }
                else
                {
                    foreach(var item in liOrder)
                    {
                        item.Status = 1;
                        respone.Message = "Thanh toán thành công";
                        respone.Data = item;

                        ord = _context.Orders.Where(x => x.OrderID == orderID).ToList();

                        foreach (var itemDonHang in ord)
                    {
                        itemDonHang.orderDetail = _context.OrderDetails.Where(e => e.OrderID == itemDonHang.OrderID).ToList();
                    }
                    }
                }
                _context.SaveChanges();
                return respone;
            }
            catch(Exception ex)
            {
                respone.Type = "Error";
                respone.Message = ex.Message;
                return respone;
            }
        }
    }
}
