using Microsoft.AspNetCore.Mvc;
using MonolithicApp.Data;
using MonolithicApp.Models;

namespace MonolithicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        // Get all products
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _context.Products.ToList();
            if (!products.Any())
            {
                return NotFound("No products found.");
            }
            return Ok(products);
        }

        // Add a new product
        [HttpPost]
        public IActionResult AddProduct([FromBody] Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            _context.Products.Add(product);
            _context.SaveChanges();
            return Ok(new { Message = "Product added successfully.", Product = product });
        }

        // Update an existing product
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, [FromBody] Product product)
        {
            if (product == null || string.IsNullOrEmpty(product.Name) || product.Price <= 0)
            {
                return BadRequest("Invalid product data.");
            }

            var existingProduct = _context.Products.Find(id);
            if (existingProduct == null)
            {
                return NotFound("Product not found.");
            }

            existingProduct.Name = product.Name;
            existingProduct.Price = product.Price;
            existingProduct.ImageUrl = product.ImageUrl;

            _context.SaveChanges();
            return Ok(new { Message = "Product updated successfully.", Product = existingProduct });
        }

        // Delete a product
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var product = _context.Products.Find(id);
            if (product == null)
            {
                return NotFound("Product not found.");
            }

            _context.Products.Remove(product);
            _context.SaveChanges();
            return Ok(new { Message = "Product deleted successfully." });
        }
    }
}
