using APDS_POE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XADAD7112_Application.Models;
using XADAD7112_Application.Models.Admin;
using XADAD7112_Application.Repositories;
using XADAD7112_Application.Services;

namespace XADAD7112_Application.Controllers
{
    [Authorize]
    public class AdminController:Controller
    {

        private readonly IAdminRepository repo;
        private readonly IBookingRepository bookingRepo;
        private readonly IUserRepository _userRepo;

        public AdminController(IAdminRepository adminRepository, IBookingRepository bookingRepository, IUserRepository userRepo)
        {
            repo = adminRepository;
            bookingRepo = bookingRepository;
            _userRepo = userRepo;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {

            var vm = new AdminDashboardViewModel()
            {
                TraceLogs = repo.ListTraceLogs(),
                Bookings = repo.ListBookings(),
                Users = repo.ListUsers()
            };

            return View(vm);
        }

        [AllowAnonymous]
        public IActionResult RescheduleBooking(int bookingId, DateTime bookingDate, TimeSpan bookingTime)
        {
            try
            {
                // Update booking in DB
                var booking = bookingRepo.GetBookingById(bookingId);

                if (booking == null)
                {
                    ViewBag.Error = "Booking not found";
                    return Json(new { success = false, message = "Booking not found" });
                }

                booking.Date = bookingDate;
                booking.Time = bookingTime;

                var response = bookingRepo.RescheduleBooking(booking);

                if (response.IsSuccess)
                {
                    ViewBag.Success = "Booking updated successfully";

                    var vm = new AdminDashboardViewModel
                    {
                        Bookings = repo.ListBookings(),
                        Users = repo.ListUsers(),
                        TraceLogs = repo.ListTraceLogs()
                    };

                    return Json(new { success = true, message = "Booking updated successfully" });
                }
                else
                {
                    ViewBag.Error = "An error occurred while Updating the booking";
                    return Json(new { success = false, message = "An error occurred while updating the booking" });
                }

            }
            catch (Exception ex)
            {
                ViewBag.Error = "An error occurred while Updating the booking";
                return View("Index");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult UpdateUser(User user)
        {
            try
            {
                var existingUser = _userRepo.GetUser(user.Id);
                if (existingUser == null)
                    return Json(new { success = false, message = "User not found" });

                existingUser.FullName = user.FullName;
                existingUser.Username = user.Username;
                existingUser.Email = user.Email;
                existingUser.MobileNo = user.MobileNo;
                existingUser.Address = user.Address;
                existingUser.UserRole = user.UserRole;

                var result =_userRepo.UpdateUser(existingUser);

                if (result.IsSuccess)
                {
                    ViewBag.Success = "User updated sucessfully";
                }
                else
                {
                    ViewBag.Success = "An error occurred while updating the user";
                }
                   

                return Json(new
                {
                    success = true,
                    user = new
                    {
                        Id = existingUser.Id,
                        Username = existingUser.Username,
                        Email = existingUser.Email,
                        DateCreatedFormatted = existingUser.DateCreated?.ToString("dd MMMM yyyy")
                    }
                });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

    }
}
