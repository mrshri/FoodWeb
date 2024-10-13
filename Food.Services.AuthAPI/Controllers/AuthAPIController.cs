using Food.Services.AuthAPI.Models.DTO;
using Food.Services.AuthAPI.Service.IService;
using Food.Services.AuthPI.Models.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Food.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ResponseDto _response;
        public AuthAPIController(IAuthService authService)
        {
            _authService = authService;
            _response = new ();
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegistrationRequestDto model)
        {
            var errorMessage = await _authService.Register(model);
            if (!string.IsNullOrEmpty(errorMessage)) {
                _response.IsSuccess = false;
                _response.ErrorMessage = errorMessage;
                return BadRequest(_response);

            }

            return Ok(_response);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
           var loginResponse = await _authService.Login(loginRequestDto);
            if (loginResponse.User == null)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "UserName or Password is Incorrect";
                return BadRequest(_response);
            }
            _response.Result = loginResponse;
            return Ok(_response);
        }


        [HttpPost("AssignRole")]
        public async Task<IActionResult> AssignRole([FromBody]RegistrationRequestDto model)
        {
            var AssignRoleSuccessfully = await _authService.AssignRole(model.Email,model.Role.ToUpper());
            if (!AssignRoleSuccessfully)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = "Error Encountered";
                return BadRequest(_response);
            }           
            return Ok(_response);
        }
    }
}
