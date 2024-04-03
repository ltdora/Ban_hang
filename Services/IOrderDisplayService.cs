using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public interface IOrderDisplayService
    {
        List<Order> DisplayOrder(int userID);
    }

    public class OrderDisplayService : IOrderDisplayService
    {
        private readonly ShopContext _context;
        public OrderDisplayService(ShopContext context)
        {
            _context = context;
        }
        public List<Order> DisplayOrder(int userID)
        {
            List<Order> ord = new List<Order>();
            try
            {
                ord = _context.Orders.Where(x => x.UserID == userID).ToList();
            }
            catch (Exception)
            {
                throw;
            }
            return ord;
        }

    }
}
