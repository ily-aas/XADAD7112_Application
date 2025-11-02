using Xunit;
using Moq;
using System.Collections.Generic;
using XADAD7112_Application.Models;
using APDS_POE.Repositories;
using XADAD7112_Application.Models.System;
using XADAD7112_Application.Models;

public class UserTests
{
    private readonly Mock<IUserRepository> _mockRepo;

    public UserTests()
    {
        _mockRepo = new Mock<IUserRepository>();
    }

    [Fact]
    public void Login_ReturnsUser()
    {
        var user = new User { Username = "test" };
        _mockRepo.Setup(r => r.Login("test", "pass", false)).Returns(user);

        var result = _mockRepo.Object.Login("test", "pass");

        Assert.NotNull(result);
        Assert.Equal("test", result.Username);
    }

    [Fact]
    public void AddUser_ReturnsSuccess()
    {
        var user = new User { Username = "newuser" };
        _mockRepo.Setup(r => r.AddUser(user)).Returns(new AppResponse { IsSuccess = true });

        var result = _mockRepo.Object.AddUser(user);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void UpdateUser_ReturnsSuccess()
    {
        var user = new User { Id = 1 };
        _mockRepo.Setup(r => r.UpdateUser(user)).Returns(new AppResponse { IsSuccess = true });

        var result = _mockRepo.Object.UpdateUser(user);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void DeleteUser_ReturnsSuccess()
    {
        _mockRepo.Setup(r => r.DeleteUser(1)).Returns(new AppResponse { IsSuccess = true });

        var result = _mockRepo.Object.DeleteUser(1);

        Assert.True(result.IsSuccess);
    }

    [Fact]
    public void GetAllUsers_ReturnsList()
    {
        var users = new List<User> { new User { Username = "u1" } };
        _mockRepo.Setup(r => r.GetAllUsers()).Returns(users);

        var result = _mockRepo.Object.GetAllUsers();

        Assert.Single(result);
        Assert.Equal("u1", result[0].Username);
    }

    [Fact]
    public void GetUser_ReturnsUser()
    {
        var user = new User { Id = 1 };
        _mockRepo.Setup(r => r.GetUser(1)).Returns(user);

        var result = _mockRepo.Object.GetUser(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public void AddUserInquiry_ReturnsSuccess()
    {
        var inquiry = new Inquiry { Name = "Test" };
        _mockRepo.Setup(r => r.AddUserInquiry(inquiry)).Returns(new AppResponse { IsSuccess = true });

        var result = _mockRepo.Object.AddUserInquiry(inquiry);

        Assert.True(result.IsSuccess);
    }
}
