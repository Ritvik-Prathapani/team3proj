using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MonolithicApp.Data;
using MonolithicApp.Models;
using System.Linq;

namespace MonolithicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CartController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/cart
        [HttpPost]
        public IActionResult AddToCart([FromBody] AddToCartRequest request)
        {
            if (request.ProductId <= 0)
                return BadRequest("Invalid product ID.");

            // Fetch product by ID from ProductController
            var product = _context.Products.Find(request.ProductId);
            if (product == null)
                return NotFound("Product not found.");

            var cartItem = new CartItem
            {
                ProductId = request.ProductId
            };

            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return Ok(new { Message = "Product added to cart", ProductId = product.Id });
        }

        // GET: api/cart
        [HttpGet]
        public IActionResult GetCartItems()
        {
            // Retrieve cart items with related product details
            var cartItems = _context.CartItems
                .Include(c => c.Product)  // Load related product details
                .ToList();

            if (!cartItems.Any())
                return NotFound("No items in the cart.");

            return Ok(cartItems.Select(item => new
            {
                item.Id,
                item.ProductId,
                item.Product.Name,
                item.Product.Price,
                item.Product.ImageUrl
            }));
        }

        // DELETE: api/cart/{productId}
        [HttpDelete("{productId}")]
        public IActionResult RemoveFromCart(int productId)
        {
            // Find cart item using product ID
            var cartItem = _context.CartItems.FirstOrDefault(c => c.ProductId == productId);
            if (cartItem == null)
                return NotFound("Cart item not found.");

            // Remove the cart item
            _context.CartItems.Remove(cartItem);
            _context.SaveChanges();

            return Ok($"Product with ID {productId} removed from the cart.");
        }
    }

    // DTO class for adding a product to the cart
    public class AddToCartRequest
    {
        public int ProductId { get; set; }
    }
}
