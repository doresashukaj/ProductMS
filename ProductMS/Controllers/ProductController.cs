using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductMS.DTO;
using ProductMS.Entities;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

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

        
        [HttpGet]
        [Authorize]
        public IActionResult GetAllProduct()
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value; 
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            
            if (User.IsInRole("Admin"))
            {
                return Ok(databaseContext.Products.ToList());
            }

            
            var userProducts = databaseContext.Products.Where(p => p.UserId == currentUserId).ToList();
            return Ok(userProducts);
        }

      
        [HttpGet("{id:guid}")]
        [Authorize]
        public IActionResult GetProductById(Guid id)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var product = databaseContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

         
            if (User.IsInRole("Admin"))
            {
                return Ok(product);
            }

           
            if (product.UserId != currentUserId)
            {
                return Forbid();  
            }

            return Ok(product);
        }

        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddProduct(ProductDto productDto)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            if (string.IsNullOrEmpty(currentUserId))
            {
                return Unauthorized();
            }

            var productEntity = new Product()
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                UserId = currentUserId  
            };

            databaseContext.Products.Add(productEntity);
            await databaseContext.SaveChangesAsync();  

            return Ok(productEntity);
        }

        
        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDto updateProductDto)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var product = databaseContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            
            if (User.IsInRole("Admin"))
            {
                product.Name = updateProductDto.Name;
                product.Description = updateProductDto.Description;
                product.Price = updateProductDto.Price;
                product.Category = updateProductDto.Category;
            }
            else if (product.UserId == currentUserId)
            {
               
                product.Name = updateProductDto.Name;
                product.Description = updateProductDto.Description;
                product.Price = updateProductDto.Price;
                product.Category = updateProductDto.Category;
            }
            else
            {
                return Forbid();  
            }

            await databaseContext.SaveChangesAsync();  

            return Ok(product);
        }

       
        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var currentUserId = User.Claims.FirstOrDefault(c => c.Type == "userId")?.Value;
            var product = databaseContext.Products.Find(id);

            if (product == null)
            {
                return NotFound();
            }

            
            if (User.IsInRole("Admin"))
            {
                databaseContext.Products.Remove(product);
                await databaseContext.SaveChangesAsync();  
                return Ok();
            }

            
            if (product.UserId == currentUserId)
            {
                databaseContext.Products.Remove(product);
                await databaseContext.SaveChangesAsync();  
                return Ok();
            }

            return Forbid();  
        }
    }
}
