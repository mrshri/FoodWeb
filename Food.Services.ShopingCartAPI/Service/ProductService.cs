using Food.Services.ShoppingCartAPI.Models.DTO;
using Food.Services.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;
using System.Net.Http;

namespace Food.Services.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        public ProductService(IHttpClientFactory client)
        {
            _httpClientFactory = client;
        }
        public async Task<IEnumerable<ProductDTO>> GetProducts()
        {
            var client = _httpClientFactory.CreateClient("Product");
            var apiResponse = await client.GetAsync($"api/product");
            var apiContent = await apiResponse.Content.ReadAsStringAsync();
            var response = JsonConvert.DeserializeObject<ResponseDto>(apiContent);

            if (response.IsSuccess) {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDTO>>(Convert.ToString(response.Result));
            }
         return new List<ProductDTO>();
        }
    }
}
