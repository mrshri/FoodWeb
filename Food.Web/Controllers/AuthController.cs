using Food.Web.Models;
using Food.Web.Services.IServices;
using Food.Web.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace Food.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ITokenProvider _tokenProvider;
        public AuthController(IAuthService authService, ITokenProvider tokenProvider)
        {
            _authService = authService;
            _tokenProvider = tokenProvider;
        }

        [HttpGet]
        public IActionResult Login() {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
           
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto responseDto = await _authService.LoginAsync(obj);
          
            if (responseDto != null && responseDto.IsSuccess)
            {
                LoginResponseDto loginResponseDto =
                    JsonConvert.DeserializeObject<LoginResponseDto>(Convert.ToString(responseDto.Result));

                await SignInUser(loginResponseDto);
                _tokenProvider.SetToken(loginResponseDto.Token);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto.ErrorMessage;
                return View(obj);
            }

        }


        [HttpGet]
        public IActionResult Register()
        {
            var roleList = new List<SelectListItem>
            {
                new SelectListItem{Text= StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin},
                new SelectListItem{Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer}

            };

            ViewBag.RoleList = roleList;
            return View();
        }

        [HttpPost]
        public async  Task<IActionResult> Register(RegistrationRequestDto obj)
        {
            ResponseDto result = await _authService.RegisterAsync(obj);
            ResponseDto assignRole;

            if (result != null && result.IsSuccess)
            {
                if (string.IsNullOrEmpty(obj.Role))
                {
                    obj.Role = StaticDetails.RoleCustomer;
                }
                assignRole = await _authService.AssignRoleAsync(obj);
                if (assignRole != null && assignRole.IsSuccess)
                {
                    TempData["success"] = "Registration Successful";
                    return RedirectToAction(nameof(Login));
                }
            }
            else
            {
                TempData["error"] = result.ErrorMessage;
            }

             var roleList = new List<SelectListItem>
             {
                 new SelectListItem{Text= StaticDetails.RoleAdmin, Value = StaticDetails.RoleAdmin},
                 new SelectListItem{Text = StaticDetails.RoleCustomer, Value = StaticDetails.RoleCustomer}
             };

             ViewBag.RoleList = roleList;
             return View(obj);
        }
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            _tokenProvider.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var handler = new JwtSecurityTokenHandler();

            var jwt = handler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);

            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Email,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email) .Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));

            identity.AddClaim(new Claim(ClaimTypes.Name,
              jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Email).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
              jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principle = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principle);
        }

    }
}
