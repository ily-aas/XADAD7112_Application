using XADAD7112_Application.Models;
using System.Security.Claims;
using static XADAD7112_Application.Models.System.Enums;

namespace APDS_POE.Services
{
    public interface IHelperService
    {
        User GetSignedInUser();
    }

    public class Helper : IHelperService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Helper(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public User GetSignedInUser()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context?.User?.Identity is { IsAuthenticated: true })
            {
                return new User
                {
                    Username = context.User.Identity.Name,
                    Id = int.TryParse(context.User.FindFirstValue(ClaimTypes.Sid), out var id) ? id : 0,
                    UserRole = int.TryParse(context.User.FindFirstValue(ClaimTypes.Role), out var role) ? role : 0
                };
            }

            return null;
        }
    }


}
