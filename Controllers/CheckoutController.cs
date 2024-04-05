using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public class CheckoutController : Controller
    {
        ICheckoutService _checkoutService;
        public CheckoutController(ICheckoutService service)
        {
            _checkoutService = service;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CheckoutOrder(int orderID)
        {
            try
            {
                var model = _checkoutService.CheckoutOrder(orderID);
                return Ok(model);
            }
            catch(Exception)
            {
                return BadRequest();
            }
        }
    }
}
