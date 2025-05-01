using Microsoft.AspNetCore.Mvc;
using MonolithicApp.Data;
using MonolithicApp.Models;
using System.Linq;
using System.Collections.Generic;

namespace MonolithicApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillingController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BillingController(AppDbContext context)
        {
            _context = context;
        }

        // POST: api/billing
        [HttpPost]
        public IActionResult Bill([FromQuery] int userId)
        {
            // Fetch cart items for the given userId
            var items = _context.CartItems.Where(c => c.ProductId == userId).ToList();
            if (!items.Any())
                return NotFound("Cart is empty.");

            // Get the list of product IDs and retrieve corresponding products in a single query
            var productIds = items.Select(item => item.ProductId).ToList();
            var products = _context.Products.Where(p => productIds.Contains(p.Id)).ToList();

            // Calculate the total by matching products with the cart items
            decimal total = 0;
            var billingDetails = new List<object>();  // To store product details in the response
            foreach (var item in items)
            {
                var product = products.FirstOrDefault(p => p.Id == item.ProductId);
                if (product != null)
                {
                    total += product.Price;  // Add the price of the product without considering quantity
                    billingDetails.Add(new
                    {
                        ProductName = product.Name,
                        Price = product.Price,
                    });
                }
                else
                {
                    // Handle case where product is missing, could log or alert
                    return NotFound($"Product with ID {item.ProductId} not found.");
                }
            }

            // Remove cart items after billing
            _context.CartItems.RemoveRange(items);
            _context.SaveChanges();

            // Return the detailed billing information
            return Ok(new
            {
                TotalAmount = total,
                Message = $"Total Bill for User {userId}: ₹{total}",
                BillingDetails = billingDetails
            });
        }
    }
}
