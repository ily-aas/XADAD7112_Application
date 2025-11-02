using XADAD7112_Application.Models.Booking;
using XADAD7112_Application.Services;
using static XADAD7112_Application.Models.Booking.Cart;
using APDS_POE.Services;
using XADAD7112_Application.Models.System;

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

        public BookingRepository(AppDbContext dbContext, IHelperService helpers)
        {
            _db = dbContext;
            _helpers = helpers;
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

                return new AppResponse()
                {
                    IsSuccess = true,
                    Message = "Booking created successfully"
                };
                    
            }
            catch (Exception ex)
            {
                return new AppResponse()
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }

        }

    }
}
