using AutoMapper;
using DsiCode.Micro.Product.API.Datas;
using DsiCode.Micro.Product.API.Model.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using DsiCode.Micro.Product.API.Model;
using DsiCode.Micro.Product.API.Extension;
using Microsoft.EntityFrameworkCore;


namespace DsiCode.Micro.Product.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;
        private ResponseDto _response;
        private IMapper _mapper;

        public ProductController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
            _response = new ResponseDto();
        }

        [AllowAnonymous]
        [HttpGet]
        public ResponseDto Get()
        {
            try
            {
                IEnumerable<DsiCode.Micro.Product.API.Model.Product>
                    objList = _context.Productos.ToList();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(objList);
                _response.IsSuccess = true;
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR")]
        public ResponseDto Post(ProductDto productDto)
        {
            try
            {
                DsiCode.Micro.Product.API.Model.Product product = _mapper.Map<DsiCode.Micro.Product.API.Model.Product>(productDto);
                _context.Productos.Add(product);
                _context.SaveChanges();
                if (productDto.Image != null)
                {
                    string fileName =
                        product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    var filePath = @"wwwroot\ProductImage\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    var directory = Path.GetDirectoryName(filePathDirectory);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }
                    using (var stream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(stream);
                    }

                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = $"{baseUrl}/ProductImage/{fileName}";
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
                _context.Productos.Update(product);
                _context.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpGet("All")]
        [AllowAnonymous]
        public async Task<ResponseDto> All([FromQuery] PagerDto pagerDto)
        {
            try
            {
                var queryable = _context.Productos.AsQueryable();
                await HttpContext.InsertParamPageHeader(queryable);
                var products = await queryable.OrderBy(o => o.Name).Paginar(pagerDto).ToListAsync();
                _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }

            return _response;
        }
        [HttpGet]
        [Route("{id:int}")]
        public ResponseDto Get(int id)
        {
            try
            {
                DsiCode.Micro.Product.API.Model.Product obj = _context.Productos.First(u => u.ProductId == id);
                _response.Result = _mapper.Map<ProductDto>(obj);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpPut]
        [Authorize(Roles = "ADMINISTRADOR")]
        public ResponseDto Put([FromForm] ProductDto productDto)
        {
            try
            {
                DsiCode.Micro.Product.API.Model.Product product = _mapper.Map<DsiCode.Micro.Product.API.Model.Product>(productDto);
                _context.Productos.Update(product);
                _context.SaveChanges();
                if (productDto.Image != null)
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo archivo = new FileInfo(oldFilePath);
                        if (archivo.Exists)
                        {
                            archivo.Delete();
                        }
                    }
                    string fileName =
                        product.ProductId + Path.GetExtension(productDto.Image.FileName);
                    var filePath = @"wwwroot\ProductImage\" + fileName;
                    var filePathDirectory = Path.Combine(Directory.GetCurrentDirectory(), filePath);
                    using (var fileStream = new FileStream(filePathDirectory, FileMode.Create))
                    {
                        productDto.Image.CopyTo(fileStream);
                    }
                    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.Value}{HttpContext.Request.PathBase.Value}";
                    product.ImageUrl = $"{baseUrl}/ProductImage/{fileName}";
                    product.ImageLocalPath = filePath;
                }
                else
                {
                    product.ImageUrl = "https://placehold.co/600x400";
                }
                _context.Productos.Update(product);
                _context.SaveChanges();
                _response.Result = _mapper.Map<ProductDto>(product);
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }

        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public ResponseDto Delete(int id)
        {
            try
            {
                DsiCode.Micro.Product.API.Model.Product product = _context.Productos.FirstOrDefault(u => u.ProductId == id);
                if (product == null)
                {
                    _response.IsSuccess = false;
                    _response.Message = "Product not found";
                }
                else
                {
                    if (!string.IsNullOrEmpty(product.ImageLocalPath))
                    {
                        var oldFileDirectory = Path.Combine(Directory.GetCurrentDirectory(), product.ImageLocalPath);
                        FileInfo file = new FileInfo(oldFileDirectory);
                        if (file.Exists)
                        {
                            file.Delete();
                        }
                    }
                    _context.Productos.Remove(product);
                    _context.SaveChanges();
                    _response.Result = true;
                }
            }
            catch (Exception ex)
            {
                _response.IsSuccess = false;
                _response.Message = ex.Message;
            }
            return _response;
        }
    }
}
