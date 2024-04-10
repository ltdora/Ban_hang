using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace He_thong_ban_hang
{
    public class VnPayController : Controller
    {
        IVnPayService _vnPayService;
        IOrderService _orderService;
        IUserService _UserService;
        ICheckoutService _checkoutService;

        public VnPayController(IVnPayService vnPayService, IOrderService orderService, IUserService userService, ICheckoutService checkoutService)
        {
            _vnPayService = vnPayService;
            _orderService = orderService;
            _UserService = userService;
            _checkoutService = checkoutService;
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult CreatePaymentUrl(int orderID)
        {
            Order order = _orderService.GetOrdersById(orderID);
            // kiem tra order == null
            PaymentInformationModel model = new PaymentInformationModel();
            model.Amount = order.Total;
            model.OrderType = "electric";
            model.Name = _UserService.GetUserById(order.UserID).UserName;
            model.OrderDescription = order.OrderID.ToString();

            var url = _vnPayService.CreatePaymentUrl(model, HttpContext);

            return Ok(url);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult PaymentCallback()
        {
            var response = _vnPayService.PaymentExecute(Request.Query);
            int orderID = int.Parse(Request.Query["vnp_OrderInfo"]);
            if (response.Success == true)
            {
                _checkoutService.CheckoutOrder(orderID, 1);
            }
            return Json(response);
        }
    }
}
