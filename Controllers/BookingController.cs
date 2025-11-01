using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XADAD7112_Application.Models.Booking;
using static XADAD7112_Application.Models.Booking.Cart;

namespace XADAD7112_Application.Controllers
{
    public class BookingController : Controller
    {
        [AllowAnonymous]
        public IActionResult Index()
        {
            return View("~/Views/Booking/Index.cshtml");
        }

        [HttpPost]
        public IActionResult SubmitBooking(BookingRequest model)
        {
            if (!ModelState.IsValid || model.Items.Count == 0)
                return View("BookYourAppointment");

            // Model.Items and Model.Total are successfully bound here ✅

            return RedirectToAction("Confirm");
        }

    }
}
