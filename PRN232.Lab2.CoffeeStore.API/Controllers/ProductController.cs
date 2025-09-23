using Microsoft.AspNetCore.Mvc;
using PRN232.Lab2.CoffeeStore.Services.Service.IService;

namespace PRN232.Lab2.CoffeeStore.API.Controllers
{
    [ApiController]
    [Route("/api/products")]
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
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            return Ok(product);
        }
    }
}
