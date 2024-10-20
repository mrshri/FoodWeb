using Food.Web.Models;

namespace Food.Web.Services.IServices
{
    public interface IProductService
    {
        Task<ResponseDto> GetAllProductAsync();
        Task<ResponseDto>GetProductByIdAsync(int productId);
        Task<ResponseDto> CreateProductAsync(ProductDto productDto);
        Task<ResponseDto> UpdateProductAsync(ProductDto productDto);
        Task<ResponseDto> DeleteProductAsync(int productId);
    }
}
