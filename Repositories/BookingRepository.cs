using XADAD7112_Application.Models.Booking;
using XADAD7112_Application.Services;
using static XADAD7112_Application.Models.Booking.Cart;
using APDS_POE.Services;
using XADAD7112_Application.Models.System;
using XADAD7112_Application.Models.Account;

namespace XADAD7112_Application.Repositories
{

    public interface IBookingRepository
    {
        Task<AppResponse> CreateBookingAsync(BookingRequest request);
    }

    public class BookingRepository : IBookingRepository
    {

        private readonly AppDbContext _db;
        private readonly IHelperService _helpers;
        private readonly ILoggingService logger;

        public BookingRepository(AppDbContext dbContext, IHelperService helpers, ILoggingService loggingService)
        {
            _db = dbContext;
            _helpers = helpers;
            logger = loggingService;
        }

        public async Task<AppResponse> CreateBookingAsync(BookingRequest request)
        {

            try
            {
                var user = _helpers.GetSignedInUser();

                var booking = new Booking
                {
                    UserId = user.Id,
                    Date = request.AppointmentDate,
                    Time = request.AppointmentTime,
                    IsCancelled = false,
                };

                _db.Booking.Add(booking);
                await _db.SaveChangesAsync();

                foreach (var item in request.Items)
                {
                    var BookingItem = new Models.Booking.BookingItem()
                    {
                        Name = item.Name,
                        Quantity = item.Quantity,
                        Price = item.Price,
                        BookingId = booking.Id,

                    };

                    _db.BookingItem.Add(BookingItem);
                    await _db.SaveChangesAsync();
                }

                await logger.LogAsync("Booking", $"User '{booking.UserId}' added a booking '{booking.Id}'");

                return new AppResponse()
                {
                    IsSuccess = true,
                    Message = "Booking created successfully"
                };
                    
            }
            catch (Exception ex)
            {
                await logger.LogAsync("Booking", $"An error occurred while creating a booking: {ex.Message} ");

                return new AppResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }

    }
}
