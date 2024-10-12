using Food.Web.Models;
using Food.Web.Services.IServices;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using static Food.Web.Utilities.StaticDetails;
namespace Food.Web.Services
{
    public class BaseServices : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ITokenProvider _tokenProvider;
        public BaseServices(IHttpClientFactory httpClientFactory,ITokenProvider tokenProvider)
        {
            _httpClientFactory = httpClientFactory;
            _tokenProvider = tokenProvider;
        }
        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearer = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("FoodAPI");
                HttpRequestMessage message = new();
                message.Headers.Add("Accept", "application/json");

                //Token
                if (withBearer)
                {
                    var token = _tokenProvider.GetToken();
                    message.Headers.Add("Authorization", $"Bearer {token}");
                }

                message.RequestUri = new Uri(requestDto.Url);
                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonConvert.SerializeObject(requestDto.Data), Encoding.UTF8, "application/json");
                }

                HttpResponseMessage? apiResponse = null;
                switch (requestDto.ApiType)
                {
                    case ApiType.POST:
                        message.Method = HttpMethod.Post;
                        break;
                    case ApiType.PUT:
                        message.Method = HttpMethod.Put;
                        break;
                    case ApiType.DELETE:
                        message.Method = HttpMethod.Delete;
                        break;
                    default:
                        message.Method = HttpMethod.Get;
                        break;
                }
                apiResponse = await client.SendAsync(message);
                switch (apiResponse.StatusCode)
                {
                    case HttpStatusCode.NotFound:
                        return new() { IsSuccess = false, ErrorMessage = "Not Found" };
                    case HttpStatusCode.Unauthorized:
                        return new() { IsSuccess = false, ErrorMessage = "Unauthorized" };
                    case HttpStatusCode.BadRequest:
                        return new() { IsSuccess = false, ErrorMessage = "Bad Request" };
                    case HttpStatusCode.Forbidden:
                        return new() { IsSuccess = false, ErrorMessage = "Forbidden" };
                    case HttpStatusCode.InternalServerError:
                        return new() { IsSuccess = false, ErrorMessage = "Internal Server Error" };
                    default:
                        var apiContent = await apiResponse.Content.ReadAsStringAsync();
                        var apiResponseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
                        return apiResponseDto;
                }
            }
            catch (Exception ex)
            {

                var dto = new ResponseDto
                {
                    ErrorMessage = ex.Message.ToString(),
                    IsSuccess = false
                };
                return dto;
            }
        }
    }
}
