using Food.Web.Models;

namespace Food.Web.Services.IServices
{
    public  interface IBaseService
    {
        Task<ResponseDto?> SendAsync(RequestDto requestDto, bool withBearer = true );
    }
}
