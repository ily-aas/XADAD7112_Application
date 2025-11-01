namespace XADAD7112_Application.Models.Booking
{
    public class Cart
    {
        public class CartModel
        {
            public Dictionary<string, CartItem> Items { get; set; }
            public decimal Total { get; set; }
        }

        public class CartItem
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }

        public class BookingRequest
        {
            public List<CartItem> Items { get; set; }
            public decimal Total { get; set; }
        }
    }
}
