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
    public class OrderDisplayController : Controller
    {
        IOrderDisplayService _orderDisplay;
        public OrderDisplayController(IOrderDisplayService service)
        {
            _orderDisplay = service;
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult DisplayOrder(int userID)
        {
            try
            {
                var ord = _orderDisplay.DisplayOrder(userID);
                if (ord == null) return NotFound();
                return Ok(ord);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }
    }
}
