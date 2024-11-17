using Food.Web.Models;
using Food.Web.Services.IServices;
using static Food.Web.Utilities.StaticDetails;


namespace Food.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IBaseService _baseService;
        public ProductService(IBaseService baseService)
        {
            _baseService = baseService;
                
        }
        public async Task<ResponseDto> CreateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.POST,
                Data = productDto,
                Url = ProductAPIBase + "/api/product/"

            });
        }

        public async Task<ResponseDto> DeleteProductAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.DELETE,
                Url = ProductAPIBase + "/api/product/" + productId

            });
        }

        public async Task<ResponseDto> GetAllProductAsync()
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = ProductAPIBase + "/api/product/"

            });
        }

        public async Task<ResponseDto> GetProductByIdAsync(int productId)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.GET,
                Url = ProductAPIBase + "/api/product/" + productId,

            });
        }

        public async Task<ResponseDto> UpdateProductAsync(ProductDto productDto)
        {
            return await _baseService.SendAsync(new RequestDto()
            {
                ApiType = ApiType.PUT,
                Data = productDto,
                Url = ProductAPIBase + "/api/product/"

            });
        }
    }
}
