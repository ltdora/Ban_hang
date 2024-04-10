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
            PaymentInformationModel model = new PaymentInformationModel();
            if (order != null)
            {
                if (order.Status == 0)
                {
                    model.Amount = order.Total;
                    model.OrderType = "110000";
                    model.Name = _UserService.GetUserById(order.UserID).UserName;
                    model.OrderDescription = order.OrderID.ToString();
                }
                else
                {
                    model.OrderDescription = "Trạng thái đơn hàng không hợp lệ";
                    return Ok(model);
                }
            }
            else
            {
                model.OrderDescription = "Đơn hàng không hợp lệ";
                return Ok(model);
            }

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
