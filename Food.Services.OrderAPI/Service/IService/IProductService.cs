
using Food.Services.OrderAPI.Models.Dto;

namespace Food.Services.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
