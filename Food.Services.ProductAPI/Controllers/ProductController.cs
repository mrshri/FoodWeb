using AutoMapper;
using Food.Services.ProductAPI.Data;
using Food.Services.ProductAPI.Models;
using Food.Services.ProductAPI.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Food.Services.ProductAPI.Controllers
{
    [Route("api/product")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ApplicationDBContext _db;
        private readonly ResponseDto _response;
        IMapper _mapper;
        public ProductController(ApplicationDBContext applicationDBContext, IMapper mapper)
        {
            _db = applicationDBContext;
            _response = new ResponseDto();
            _mapper = mapper;
        }

        [HttpGet]
        public ResponseDto GetProducts()
        {
            try
            {
                IEnumerable<Product> productsList = _db.Products.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDTO>>(productsList);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;

            }
            return _response;
        }

        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto GetProduct(int id)
        {

            try
            {
                Product product = _db.Products.FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Product not found";
                }
                _response.Result = _mapper.Map<ProductDTO>(product);

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;

            }

            return _response;

        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto AddProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                _db.Products.Add(product);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDTO>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            return _response;
        }
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto UpdateProduct([FromBody] ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                _db.Products.Update(product);
                _db.SaveChanges();
                _response.Result = _mapper.Map<ProductDTO>(product);

            }
            catch (Exception ex)
            {

                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}") ]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto DeleteProduct(int id)
        {
            try
            {
                Product product = _db.Products.FirstOrDefault(x => x.ProductId == id);
                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Product not found";
                }
                else
                {
                    _db.Products.Remove(product);
                    _db.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.ErrorMessage = ex.Message;
            }
            return _response;
        }
    }
}