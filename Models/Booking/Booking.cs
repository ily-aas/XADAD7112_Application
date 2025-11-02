namespace XADAD7112_Application.Models.Booking
{
    public class Booking
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public bool IsCancelled { get; set; }

        // Navigation property
        public ICollection<BookingItem> Items { get; set; }
    }
}
