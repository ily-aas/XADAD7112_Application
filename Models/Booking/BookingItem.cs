namespace XADAD7112_Application.Models.Booking
{
    public class BookingItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Quantity { get; set; }

        public int BookingId { get; set; }

        public decimal Price { get; set; }

        // Navigation property
        public Booking Booking { get; set; }
    }
}
