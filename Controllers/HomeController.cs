using System.Diagnostics;
using APDS_POE.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using XADAD7112_Application.Models;

namespace XADAD7112_Application.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IUserRepository _userRepo;

    public HomeController(ILogger<HomeController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepo = userRepository;
    }

    [AllowAnonymous]
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [AllowAnonymous]
    [HttpPost]
    public IActionResult AddInquiry(Inquiry inquiry)
    {
        if (!ModelState.IsValid)
        {
            TempData["Error"] = "Invalid Form";
            return RedirectToAction("Index");
        }

        var response = _userRepo.AddUserInquiry(inquiry);

        if (!response.IsSuccess)
        {
            TempData["Error"] = "Invalid Form";
            return RedirectToAction("Index");
        }
        else
        {
            TempData["Success"] = "Submission Successful";
            return RedirectToAction("Index");
        }
    }
}
