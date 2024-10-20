using AutoMapper;
using Food.Services.ProductAPI.Models;
using Food.Services.ProductAPI.Models.DTO;

namespace Food.Services.ProductAPI
{
    public class MappingConfig
    {
        public static MapperConfiguration RegisterMaps()
        {
            var mappingConfig = new MapperConfiguration(config =>
            {
                config.CreateMap<ProductDTO, Product>();
                config.CreateMap<Product, ProductDTO>();
            });
            return mappingConfig;
        }
    }
}
