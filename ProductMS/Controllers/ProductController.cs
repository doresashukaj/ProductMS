using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMS.DTO;
using ProductMS.Entities;


namespace ProductMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext databaseContext;

        public ProductController(DatabaseContext databaseContext)
        {
            this.databaseContext = databaseContext;
        }
       
        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] ProductDto request)
        {
            
            if (request == null)
            {
                return BadRequest("Invalid product data.");
            }

            var product = new Product
            {
                Id = Guid.NewGuid(), 
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                Category = request.Category
            };

           
            await databaseContext.Products.AddAsync(product);
            await databaseContext.SaveChangesAsync();

            
            return CreatedAtAction(nameof(GetProductById), new { id = product.Id }, product);
        }

        
        [HttpGet("{id}")]
        public IActionResult GetProductById(Guid id)
        {
            var product = databaseContext.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }

    }
}
