namespace XADAD7112_Application.Models.System
{
    public class TraceLogs
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Details { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
