using Food.Web.Models;
using Food.Web.Services;
using Food.Web.Services.IServices;
using Food.Web.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Food.Web.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly IShoppingCartService _shoppingCartService;
        private readonly IOrderService _orderService;
        public ShoppingCartController(IShoppingCartService shoppingCartService,IOrderService orderService )
        {
            _shoppingCartService = shoppingCartService;
            _orderService = orderService;
        }

        [Authorize]
        public async Task<IActionResult> ShoppingCartIndex()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        [Authorize]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartBasedOnLoggedInUser());
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart = await LoadCartBasedOnLoggedInUser();
            cart.CartHeader.Phone = cartDto.CartHeader.Phone;
            cart.CartHeader.Email = cartDto.CartHeader.Email;
            cart.CartHeader.Name = cartDto.CartHeader.Name;

            var response = await _orderService.CreateOrder(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            if (response != null && response.IsSuccess)
            {
                //get stripe session and redirect to stripe to place order
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                StripeRequestDto stripeRequest = new()
                {
                    ApprovedUrl = domain+ "ShoppingCart/Confirmation?orderId=" + orderHeaderDto.OrderHeaderId,
                    CanceledUrl = domain+ "ShoppingCart/Checkout",
                    OrderHeader = orderHeaderDto
                };

                var stripeResponse = await _orderService.CreateStripeSession(stripeRequest);
                StripeRequestDto stripeRequestResult = JsonConvert.DeserializeObject<StripeRequestDto>
                                                       (Convert.ToString(stripeResponse.Result));
                Response.Headers.Add("Location", stripeRequestResult.StripeSessionUrl);

                return new StatusCodeResult(303);
            }

            return View();
        }

        public async Task<IActionResult> Confirmation(int orderId)
        {
            ResponseDto? response = await _orderService.ValidateStripeSession(orderId);
            if (response != null && response.IsSuccess)
            {
                OrderHeaderDto orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
                if (orderHeader.Status == StaticDetails.Status_Approved)
                {
                 return View(orderId);
                }

            }
            //redirect to some error page based on status
            return View(orderId);
        }

        private async Task<CartDto> LoadCartBasedOnLoggedInUser()
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _shoppingCartService.GetCartByUserIDAsync(userId);
            if (response != null && response.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(response.Result));
                return cartDto;
            }
            return new CartDto();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Sub)?.FirstOrDefault()?.Value;
            ResponseDto? response = await _shoppingCartService.RemoveFromCartAsync(cartDetailsId);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Item removed from cart successfully";
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            ResponseDto? response = await _shoppingCartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Coupon applied successfully";
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> RemoveCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.CouponCode = "";
            ResponseDto? response = await _shoppingCartService.ApplyCouponAsync(cartDto);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Coupon applied successfully";
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            return View();
        }

        //sending email
        [HttpPost]
        public async Task<IActionResult> EmailCart(CartDto cartDto)
        {
            CartDto cart = await LoadCartBasedOnLoggedInUser();
            cart.CartHeader.Email = User.Claims.Where(u => u.Type == JwtRegisteredClaimNames.Email)?.FirstOrDefault()?.Value;

            ResponseDto? response = await _shoppingCartService.EmailShoppingCart(cart);
            if (response != null && response.IsSuccess)
            {
                TempData["Success"] = "Email will be processed and sent Sortly!";
                return RedirectToAction(nameof(ShoppingCartIndex));
            }
            return View();
        }

    }
}