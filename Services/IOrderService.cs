using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang.Services
{
    public interface IOrderService
    {
        BaseRespone<OrderDetail> CreateOrder(List<OrderDetail>orderDetails,int IdUser);
    }
    public class OrderService : IOrderService
    {
        private readonly ShopContext _context;
        public OrderService(ShopContext context)
        {
            _context = context;
        }
        public BaseRespone<OrderDetail> CreateOrder(List<OrderDetail> orderDetails, int IdUser)
        {
            
            try
            {
                BaseRespone<OrderDetail> respone = new BaseRespone<OrderDetail>();
                   var lstIdSanPham = orderDetails.Select(e => e.ProductID).ToList();  ///1,2,3,4,5,6,2

                foreach(var item in lstIdSanPham)
                {
                    var checkCoSp = orderDetails.Where(i => i.ProductID == item).ToList();
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
                    orderDetails = _context.OrderDetails.Where(i => i.ProductID == item).ToList();
                    _context.Add(orderDetails);
                    _context.SaveChanges();
                    

                }

                return new BaseRespone<OrderDetail>();
            }
            catch (Exception)
            {
                throw;
                return new BaseRespone<OrderDetail>();
            }
        }
    }
}
