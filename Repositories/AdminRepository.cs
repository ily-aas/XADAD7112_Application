using XADAD7112_Application.Models;
using XADAD7112_Application.Models.Booking;
using XADAD7112_Application.Models.System;
using XADAD7112_Application.Services;

namespace XADAD7112_Application.Repositories
{

    public interface IAdminRepository
    {
        public List<TraceLogs> ListTraceLogs();
        public List<User> ListUsers();
        public List<Booking> ListBookings();
        public List<Inquiry> ListInquiries();
    }

    public class AdminRepository : IAdminRepository
    {

        private readonly AppDbContext _db;

        public AdminRepository(AppDbContext dbContext)
        {
            _db = dbContext;
        }

        public List<TraceLogs> ListTraceLogs()
        {
            return _db.Logs.ToList();
        }

        public List<User> ListUsers()
        {
            return _db.User.ToList();
        }

        public List<Booking> ListBookings()
        {
            return _db.Booking.ToList();
        }

        public List<Inquiry> ListInquiries()
        {
            return _db.Inquiries.ToList();
        }

    }
}
