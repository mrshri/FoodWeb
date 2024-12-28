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
   // [Authorize]
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
        public ResponseDto AddProduct( ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                _db.Products.Add(product);
                _db.SaveChanges();

                if (productDTO.Image != null)
                {
                    string fileName = product.ProductId + Path.GetExtension(productDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using(var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDTO.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
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
        [HttpPut]
        [Authorize(Roles = "ADMIN")]
        public ResponseDto UpdateProduct( ProductDTO productDTO)
        {
            try
            {
                Product product = _mapper.Map<Product>(productDTO);
                if (productDTO.Image != null)
                {
                    //deleting old image
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFilePathDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    string fileName = product.ProductId + Path.GetExtension(productDTO.Image.FileName);
                    string filePath = @"wwwroot\ProductImages\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDTO.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = baseUrl + "/ProductImages/" + fileName;
                    product.ImageLocalPath = filePath;
                }
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
                if (!string.IsNullOrEmpty(product.ImageLocalPath))
                {
                    var oldFilePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                    FileInfo file = new FileInfo(oldFilePathDirectory);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }


                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.ErrorMessage = "Product not found";
                }
                
              _db.Products.Remove(product);
              _db.SaveChanges();
               

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