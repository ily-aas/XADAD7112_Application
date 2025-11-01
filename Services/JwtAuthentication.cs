using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Http;
using static XADAD7112_Application.Models.System.Enums;
using static XADAD7112_Application.Models.System.EnumExtensions;
using Microsoft.AspNetCore.Authorization;
using XADAD7112_Application.Models;

namespace APDS_POE.Services
{
    public class JwtAuthentication
    {
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public JwtAuthentication(IHttpContextAccessor httpContextAccessor)
        {

            var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory()) // or AppContext.BaseDirectory
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

            _secretKey = config["JwtSettings:SecretKey"];
            _issuer = config["JwtSettings:Issuer"];
            _audience = config["JwtSettings:Audience"];
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerateToken(User user, UserRole role)
        {
            var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Username),
                            new Claim(ClaimTypes.Role, role.GetEnumDescription()),
                            new Claim(ClaimTypes.Sid,user.Id.ToString())
                        };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _issuer,
                audience: _audience,
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: creds
            );

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            // Save to session
            _httpContextAccessor.HttpContext.Session.SetString("JWToken", tokenString);

            return tokenString;
        }

        public string GetTokenFromSession()
        {
            return _httpContextAccessor.HttpContext.Session.GetString("JWToken");
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_secretKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = _issuer,
                ValidAudience = _audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
                return principal;
            }
            catch
            {
                return null; // Invalid token
            }
        }
    }

    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _secretKey;
        private readonly string _issuer;
        private readonly string _audience;

        public JwtMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _secretKey = config["JwtSettings:SecretKey"];
            _issuer = config["JwtSettings:Issuer"];
            _audience = config["JwtSettings:Audience"];
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            if (endpoint == null)
            {
                await _next(context);
                return;
            }


            // Allow anonymous endpoints
            if (endpoint?.Metadata?.GetMetadata<AllowAnonymousAttribute>() != null)
            {
                await _next(context);
                return;
            }

            var token = context.Request.Cookies["jwt_token"];

            if (string.IsNullOrEmpty(token))
            {
                context.Response.Redirect("/Home/Index");
                return;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_secretKey);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _issuer,
                    ValidAudience = _audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
                var principal = new ClaimsPrincipal(identity);
                context.User = principal;

                // Optional: Check role-based authorization
                var authorizeMetadata = endpoint.Metadata
                    .OfType<AuthorizeAttribute>()
                    .FirstOrDefault();

                if (authorizeMetadata != null && !string.IsNullOrEmpty(authorizeMetadata.Roles))
                {
                    var roles = principal.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList();
                    var requiredRoles = authorizeMetadata.Roles.Split(',');

                    if (!requiredRoles.Any(role => roles.Contains(role)))
                    {
                        context.Response.StatusCode = StatusCodes.Status403Forbidden;
                        await context.Response.WriteAsync("Access Denied: Insufficient role.");
                        return;
                    }
                }
            }
            catch
            {
                context.Response.Redirect("/Home/Index");
                return;
            }

            await _next(context);
        }


    }
}
