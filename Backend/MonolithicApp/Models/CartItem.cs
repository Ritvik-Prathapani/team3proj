namespace MonolithicApp.Models
{
    public class CartItem
    {
        public int Id { get; set; }        // Unique Id for the cart item
        public int ProductId { get; set; } // Reference to the Product (foreign key)

        // Navigation property to Product
        public Product Product { get; set; }
    }
}
