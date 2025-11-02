using APDS_POE.Repositories;
using APDS_POE.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;
using XADAD7112_Application.Models;
using XADAD7112_Application.Models.Account;
using static XADAD7112_Application.Models.Account.Dtos;
using static XADAD7112_Application.Models.System.Enums;

namespace APDS_POE.Controllers
{
    public class AccountController : Controller
    {

        private readonly IUserRepository Repo;
        private readonly JwtAuthentication _auth;

        public AccountController(IUserRepository userRepository, JwtAuthentication jwtAuthentication)
        {
            Repo = userRepository;
            _auth = jwtAuthentication;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult LoginAccount(AccountLoginDto Dto)
        {
            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(string username, string password, bool IsAdmin)
        {

            UserRole Role = IsAdmin ? UserRole.Admin : UserRole.User;

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                ViewBag.Error = "Invalid Credentials";
                return View("Login");
            }

            var user = Repo.Login(username, password, IsAdmin);

            if (user == null)
            {
                ViewBag.Error = "Invalid Credentials";
                return View("Login");
            }

            //Generate the token
            var token = _auth.GenerateToken(user, Role);

            // Set up claims
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, (IsAdmin ? "Admin" : "User")),
                new Claim(ClaimTypes.Sid, user.Id.ToString())
            };

            // Create identity & principal
            var identity = new ClaimsIdentity(claims, "MyCookieAuth");
            var principal = new ClaimsPrincipal(identity);

            // Sign in with cookie
            await HttpContext.SignInAsync("MyCookieAuth", principal, new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTime.UtcNow.AddHours(1)
            });

            return RedirectToAction("Index", "Booking");
        }

        [AllowAnonymous]
        public IActionResult Logout()
        {
            Response.Cookies.Delete(".AspNetCore.MyCookieAuth");
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult CreateAccount(AccountCreateDto dto)
        {

            if (!ModelState.IsValid)
            {
                ViewBag.Error = "Invalid Form Submission";
                return View("Create");
            }

            var response = Repo.AddUser(dto);

            if (response.IsSuccess)
            {
                ViewBag.Success = response.Message;
                return View("Login");
            }

            ViewBag.Error = response.Message;
            return View("Create");
        }


        [HttpPost]
        public IActionResult UpdateAccount(AccountUpdateDto dto)
        {
            var response = Repo.UpdateUser(dto.user);
            return Ok(response);
        }

        [HttpPost]
        public IActionResult DeleteAccount(AccountDeleteDto dto)
        {
            var response = Repo.DeleteUser(dto.Id);
            return Ok(response);
        }


    }
}
