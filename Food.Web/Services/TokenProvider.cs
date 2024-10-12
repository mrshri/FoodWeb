using Food.Web.Services.IServices;
using Food.Web.Utilities;
using Newtonsoft.Json.Linq;

namespace Food.Web.Services
{
    public class TokenProvider : ITokenProvider
    {
         private readonly IHttpContextAccessor _httpContextAccessor;
        public TokenProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        public void ClearToken()
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Delete(StaticDetails.TokenCookie);
        }

        public string GetToken()
        {
            string? token = null;
            bool? hasToken = _httpContextAccessor.HttpContext?.Request.Cookies.TryGetValue(StaticDetails.TokenCookie, out token);

            return hasToken is true? token : null;
        }

        public void SetToken(string token)
        {
            _httpContextAccessor.HttpContext?.Response.Cookies.Append(StaticDetails.TokenCookie,token);
        }
    }
}
