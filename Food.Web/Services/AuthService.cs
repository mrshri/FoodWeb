using Food.Web.Models;
using Food.Web.Services.IServices;
using Food.Web.Utilities;

namespace Food.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IBaseService _baseService;
        public AuthService(IBaseService baseService)
        {
            _baseService = baseService;            
        }
        public async Task<ResponseDto> AssignRoleAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = registrationRequestDto,
                Url = StaticDetails.AuthAPIBase + "/api/auth/AssignRole"
            });           
        }

        public async Task<ResponseDto> LoginAsync(LoginRequestDto loginRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = loginRequestDto,
                Url = StaticDetails.AuthAPIBase + "/api/auth/login"
            }, withBearer: false);
        }

        public async Task<ResponseDto> RegisterAsync(RegistrationRequestDto registrationRequestDto)
        {
            return await _baseService.SendAsync(new RequestDto
            {
                ApiType = StaticDetails.ApiType.POST,
                Data = registrationRequestDto,
                Url = StaticDetails.AuthAPIBase + "/api/auth/register"
            }, withBearer: false);
        }
    }
}
