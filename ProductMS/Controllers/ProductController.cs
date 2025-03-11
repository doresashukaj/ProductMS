using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ProductMS.DTO;
using ProductMS.Entities;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ProductMS.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly DatabaseContext _databaseContext;
        private readonly UserManager<IdentityUser> _userManager;

        public ProductController(DatabaseContext databaseContext, UserManager<IdentityUser> userManager)
        {
            _databaseContext = databaseContext;
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllProducts()
        {
            var allProducts = _databaseContext.Products.ToList();
            return Ok(allProducts);
        }

       
        [HttpGet("{id:guid}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult GetProductById(Guid id)
        {
            var product = _databaseContext.Products.Find(id);
            if (product is null) return NotFound();

           
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (User.IsInRole("User") && product.CreatedByUserId != currentUserId)
            {
                return Forbid();
            }

            return Ok(product);
        }

        
        [HttpPost]
        [Authorize(Roles = "Admin,User")]
        public IActionResult AddProduct(ProductDto productDto)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var productEntity = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                Category = productDto.Category,
                CreatedByUserId = currentUserId
            };

            _databaseContext.Products.Add(productEntity);
            _databaseContext.SaveChanges();

            return Ok(productEntity);
        }

       
        [HttpPut("{id:guid}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult UpdateProduct(Guid id, UpdateProductDto updateProductDto)
        {
            var product = _databaseContext.Products.Find(id);
            if (product is null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("User") && product.CreatedByUserId != currentUserId)
            {
                return Forbid();
            }

            product.Name = updateProductDto.Name;
            product.Description = updateProductDto.Description;
            product.Price = updateProductDto.Price;
            product.Category = updateProductDto.Category;

            _databaseContext.SaveChanges();

            return Ok(product);
        }

        // Delete a product (Admin or owner only)
        [HttpDelete("{id:guid}")]
        [Authorize(Roles = "Admin,User")]
        public IActionResult DeleteProduct(Guid id)
        {
            var product = _databaseContext.Products.Find(id);
            if (product is null) return NotFound();

            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (User.IsInRole("User") && product.CreatedByUserId != currentUserId)
            {
                return Forbid();
            }

            _databaseContext.Products.Remove(product);
            _databaseContext.SaveChanges();
            return Ok();
        }
    }
}
