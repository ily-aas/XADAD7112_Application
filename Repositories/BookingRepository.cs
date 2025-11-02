using XADAD7112_Application.Models.Booking;
using XADAD7112_Application.Services;
using static XADAD7112_Application.Models.Booking.Cart;
using APDS_POE.Services;
using XADAD7112_Application.Models.System;
using XADAD7112_Application.Models.Account;
using Microsoft.EntityFrameworkCore;

namespace XADAD7112_Application.Repositories
{

    public interface IBookingRepository
    {
        Task<AppResponse> CreateBookingAsync(BookingRequest request);
        public Booking? GetBookingById(int Id);
        public AppResponse RescheduleBooking(Booking booking);
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

        public Booking? GetBookingById(int Id)
        {
            return _db.Booking.Where(x => x.Id == Id).FirstOrDefault();
        }

        public AppResponse RescheduleBooking(Booking booking)
        {
            try
            {
                _db.Booking
                .Where(x => x.Id == booking.Id)
                .ExecuteUpdate(b => b
                    .SetProperty(p => p.Date, booking.Date)
                    .SetProperty(p => p.Time, booking.Time)
                );

                _db.SaveChanges();

                return new AppResponse()
                {
                    IsSuccess = true,
                    Message = "Booking updated sucessfully",
                };
            }
            catch (Exception ex)
            {
                return new AppResponse()
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the booking",
                };
            }
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
