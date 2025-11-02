namespace XADAD7112_Application.Models.Booking
{
    public class Cart
    {
        public class BookingItem
        {
            public string Name { get; set; }
            public decimal Price { get; set; }
            public int Quantity { get; set; }
        }

        public class BookingRequest
        {
            public List<BookingItem> Items { get; set; }
            public decimal Total { get; set; }
            public TimeSpan AppointmentTime { get; set; }
            public DateTime AppointmentDate { get; set; }
        }
    }
}
