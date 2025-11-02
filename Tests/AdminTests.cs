using Xunit;
using Moq;
using System.Collections.Generic;
using XADAD7112_Application.Models;
using XADAD7112_Application.Repositories;
using XADAD7112_Application.Models.System;
using XADAD7112_Application.Models.Booking; // Replace with your actual namespace

public class AdminTests
{
    private readonly Mock<IAdminRepository> _mockRepo;
    public AdminTests()
    {
        _mockRepo = new Mock<IAdminRepository>();
    }

    [Fact]
    public void ListTraceLogs_ReturnsList()
    {
        // Arrange
        var logs = new List<TraceLogs> { new TraceLogs(), new TraceLogs() };
        _mockRepo.Setup(r => r.ListTraceLogs()).Returns(logs);

        // Act
        var result = _mockRepo.Object.ListTraceLogs();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public void ListUsers_ReturnsUsers()
    {
        var users = new List<User> { new User { Username = "test" } };
        _mockRepo.Setup(r => r.ListUsers()).Returns(users);

        var result = _mockRepo.Object.ListUsers();

        Assert.Single(result);
        Assert.Equal("test", result[0].Username);
    }

    [Fact]
    public void ListBookings_ReturnsBookings()
    {
        var bookings = new List<Booking> { new Booking { Id = 1 } };
        _mockRepo.Setup(r => r.ListBookings()).Returns(bookings);

        var result = _mockRepo.Object.ListBookings();

        Assert.Single(result);
        Assert.Equal(1, result[0].Id);
    }

    [Fact]
    public void ListInquiries_ReturnsInquiries()
    {
        var inquiries = new List<Inquiry> { new Inquiry { Name = "Test" } };
        _mockRepo.Setup(r => r.ListInquiries()).Returns(inquiries);

        var result = _mockRepo.Object.ListInquiries();

        Assert.Single(result);
        Assert.Equal("Test", result[0].Name);
    }
}
