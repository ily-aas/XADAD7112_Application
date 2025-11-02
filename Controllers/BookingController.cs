using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XADAD7112_Application.Models.Booking;
using XADAD7112_Application.Repositories;
using static XADAD7112_Application.Models.Booking.Cart;

namespace XADAD7112_Application.Controllers
{
    public class BookingController : Controller
    {

        private readonly IBookingRepository repo;

        public BookingController(IBookingRepository bookingRepository)
        {
            repo = bookingRepository;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View("~/Views/Booking/Index.cshtml");
        }

        [HttpPost]
        [AllowAnonymous]
        public IActionResult SubmitBooking(BookingRequest request)
        {
            if (request.Items == null || request.Items.Count == 0)
            {
                ViewBag.Error = "Your cart is empty!";
                return View("Book");
            }

            var response = repo.CreateBookingAsync(request);

            if (response.Result.IsSuccess)
            {
                ViewBag.Success = "Booking successful!";
                return View("Index");
            }
            else
            {
                ViewBag.Success = "An error occurred while capturing the booking!";
                return View("Index");
            }

                
        }

    }
}
