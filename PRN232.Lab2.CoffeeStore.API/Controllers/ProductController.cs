using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.RequestModel;
using PRN232.Lab2.CoffeeStore.Services.ResponseModel;
using PRN232.Lab2.CoffeeStore.Services.Service.IService;

namespace PRN232.Lab2.CoffeeStore.API.Controllers
{
    [ApiController]
    [Route("/api/products")]
    [Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;
        public ProductController(IProductService productService, ILogger<ProductController> logger)
        {
            _productService = productService;

            _logger = logger;
        }
        
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] string? search,
            [FromQuery] string? orderBy,
            [FromQuery] string? select,
            [FromQuery] int currentPage = 1, [FromQuery] int pageSize = 5)
        {
            var products = await _productService.GetAllAsync(search, currentPage, pageSize, orderBy, select);
            return Ok(SuccessResponse<Paginated<ProductResponse>>.Create(products, "Product list retrieved success"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(SuccessResponse<ProductResponse>.Create(product, $"Product with id #{id} retrieved success"));
        }
        //[Authorize(Roles ="Admin")]
        [Authorize(Policy ="AdminPolicy")]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProductRequest request)
        {
            var product = await _productService.AddAsync(request);
            return CreatedAtAction(
                nameof(Get),
                new { id = product.ProductId },
                SuccessResponse<ProductResponse>.Create(
                    product,
                    "Product CREATED success."));
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProductRequest request)
        {
            if (request == null)
                return BadRequest();
            return Ok(SuccessResponse<ProductResponse>.Create(
                await _productService.UpdateAsync(id, request),
                "Product UPDATED success."));
        }
        [Authorize(Policy = "AdminPolicy")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var product = _productService.GetById(id);
            await _productService.DeleteAsync(product);
            //return Ok(SuccessResponse<object>.Create("", "Product removed success.")); // 204
            return NoContent();
        }
    }
}
