using Food.Web.Models;
using Food.Web.Services.IServices;
using Food.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Food.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        [Authorize]
        public IActionResult OrderIndex()
        {
            return View();
        }

        public async Task<IActionResult> orderDetail(int orderId)
        {
            OrderHeaderDto orderHeaderDto = new OrderHeaderDto();
            string userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            var response = await _orderService.GetOrder(orderId);
            if(response != null && response.IsSuccess)
            {
                orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            }
            if (!User.IsInRole(StaticDetails.RoleAdmin) && userId != orderHeaderDto.UserId)
            {
                return NotFound();
            }

            return View(orderHeaderDto);
        }

        [HttpPost("ReadyForPickup")]
        public async Task<IActionResult> ReadyForPickup(int orderId) {
            var response = await _orderService.UpdateOderStatus(orderId, StaticDetails.Status_ReadyForPickup);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(orderDetail), new {orderId = orderId});
            }
            return View();
        }

        [HttpPost("CompleteOrder")]
        public async Task<IActionResult> CompleteOrder(int orderId) {
            var response = await _orderService.UpdateOderStatus(orderId, StaticDetails.Status_Completed);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(orderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpPost("CancelOrder")]
        public async Task<IActionResult> CancelOrder(int orderId) {
            var response = await _orderService.UpdateOderStatus(orderId, StaticDetails.Status_Cancelled);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(orderDetail), new { orderId = orderId });
            }
            return View();
        }

        [HttpGet]
        public IActionResult GetAll(string status) {
            IEnumerable<OrderHeaderDto> list;
            string userId = "";
            if (!User.IsInRole(StaticDetails.RoleAdmin))
            {
                userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            }

            ResponseDto response = _orderService.GetAllOrders(userId).GetAwaiter().GetResult();
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result));
                switch (status)
                {
                    case "approved":
                        list = list.Where(u => u.Status == StaticDetails.Status_Approved);
                        break;
                    case "readyforpickup":
                        list = list.Where(u => u.Status == StaticDetails.Status_ReadyForPickup);
                        break;
                    case "cancelled":
                        list = list.Where(u => u.Status == StaticDetails.Status_Cancelled || u.Status == StaticDetails.Status_Refunded);
                        break;
                    default:
                        break;
                }
            }
            else {
              list = new List<OrderHeaderDto>();
            }

            return Json(new { data = list });
        }
    }
}
