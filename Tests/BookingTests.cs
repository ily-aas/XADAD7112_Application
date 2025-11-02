using Xunit;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using XADAD7112_Application.Models;
using static XADAD7112_Application.Models.Booking.Cart;
using XADAD7112_Application.Models.Booking;
using XADAD7112_Application.Models.System;
using XADAD7112_Application.Repositories;

public class BookingRepositoryTests
{
    private readonly Mock<IBookingRepository> _mockRepo;

    public BookingRepositoryTests()
    {
        _mockRepo = new Mock<IBookingRepository>();
    }

    //[Fact]
    //public async Task CreateBookingAsync_ReturnsSuccess()
    //{
    //    var request = new BookingRequest { Id = 1, SessionTime = DateTime.Now };
    //    _mockRepo.Setup(r => r.CreateBookingAsync(request))
    //             .ReturnsAsync(new AppResponse { IsSuccess = true });

    //    var result = await _mockRepo.Object.CreateBookingAsync(request);

    //    Assert.True(result.IsSuccess);
    //}

    [Fact]
    public void GetBookingById_ReturnsBooking()
    {
        var booking = new Booking { Id = 1 };
        _mockRepo.Setup(r => r.GetBookingById(1)).Returns(booking);

        var result = _mockRepo.Object.GetBookingById(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void RescheduleBooking_ReturnsSuccess()
    {
        var booking = new Booking { Id = 1 };
        _mockRepo.Setup(r => r.RescheduleBooking(booking))
                 .Returns(new AppResponse { IsSuccess = true });

        var result = _mockRepo.Object.RescheduleBooking(booking);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GetSessions_ReturnsSessionList()
    {
        var date = DateTime.Today;
        var sessions = new List<SessionDto> { new SessionDto { Time = "08:00" } };
        _mockRepo.Setup(r => r.GetSessions(date)).Returns(sessions);

        var result = _mockRepo.Object.GetSessions(date);

        Assert.Single(result);
        Assert.Equal("08:00", result[0].Time);
    }
}

