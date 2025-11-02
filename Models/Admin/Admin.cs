using XADAD7112_Application.Models.Booking;
using XADAD7112_Application.Models.System;

namespace XADAD7112_Application.Models.Admin
{
    public class Admin
    {
    }

    public class AdminDashboardViewModel
    {
        public List<User>? Users { get; set; }
        public List<TraceLogs>? TraceLogs { get; set; }
        public List<Booking.Booking>? Bookings { get; set; }
    }
}
