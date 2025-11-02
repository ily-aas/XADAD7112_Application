namespace XADAD7112_Application.Models.System
{
    public class Session
    {
        public int Id { get; set; }
        public DateTime SessionStart { get; set; }
        public DateTime SessionEnd { get; set; }
    }

    public class SessionDto
    {
        public string Time { get; set; }      // "08:00", "09:00" etc.
        public bool IsBooked { get; set; }    // true if already booked
    }
}
