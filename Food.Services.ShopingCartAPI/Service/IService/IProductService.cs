using Food.Services.ShoppingCartAPI.Models.DTO;

namespace Food.Services.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
    }
}
