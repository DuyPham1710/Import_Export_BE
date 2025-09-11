using BE.Database;
using BE.dto;
using BE.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BE.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/product
        [HttpPost("")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductRequest request)
        {
            if (request.Products == null || request.Products.Count == 0)
                return BadRequest("Danh sách sản phẩm rỗng");

            var entities = request.Products.Select(p => new Product
            {
                ProductName = p.ProductName,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                Images = p.Images
            }).ToList();

            _context.Products.AddRange(entities);
            await _context.SaveChangesAsync();

            return Ok(entities);
        }

        // GET: api/Product/products
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Product>>> GetAllProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Ok(products);
        }

        // GET: api/Product/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProductById(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound();

            return Ok(product);
        }

    }
}
