using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    [Route("api/[controller]")]
    //[Authorize]
    [ApiController]
    public class OrderController : Controller
    {
        IOrderService _orderService;
        public OrderController(IOrderService service)
        {
            _orderService = service;
        }
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

        [HttpGet]
        [Route("[action]")]
        public IActionResult DisplayOrder(int userID)
        {
            try
            {
                var ord = _orderService.DisplayOrder(userID);
                if (ord == null) return NotFound();
                return Ok(ord);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult SaveOrder(int quantity, int productID, int orderID)
        {
            try
            {
                var model = _orderService.SaveOrder(quantity, productID, orderID);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
        [HttpPost]
        [Route("[action]")]
        public IActionResult DeleteOrder(int quantity, int productID, int orderID)
        {
            try
            {
                var model = _orderService.DeleteOrder(quantity, productID, orderID);
                return Ok(model);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
