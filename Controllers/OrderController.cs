using He_thong_ban_hang.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        IOrderService _orderService;
        [HttpPost]
        [Route("[action]")]
        public IActionResult CreateOrder(List<OrderDetail> liOrderDetail, int userID)
        {
            try
            {
                var model = _orderService.CreateOrder(liOrderDetail, userID);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
