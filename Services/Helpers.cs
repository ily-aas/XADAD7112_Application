using XADAD7112_Application.Models;
using System.Security.Claims;
using static XADAD7112_Application.Models.System.Enums;
using System.Security.Cryptography;
using System.Text;

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

        public class PasswordService
        {
            // MUST be 32 characters for AES-256
            private static readonly string Key = "12345678901234567890123456789012"; // 32 chars
            private static readonly string IV = "1234567890123456"; // 16 chars

            public static string Encrypt(string plainText)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = Encoding.UTF8.GetBytes(IV);
                    aes.Mode = CipherMode.CBC;

                    using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream())
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        byte[] raw = Encoding.UTF8.GetBytes(plainText);
                        cs.Write(raw, 0, raw.Length);
                        cs.FlushFinalBlock();

                        return Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            public static string Decrypt(string encrypted)
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(Key);
                    aes.IV = Encoding.UTF8.GetBytes(IV);
                    aes.Mode = CipherMode.CBC;

                    using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                    using (var ms = new MemoryStream(Convert.FromBase64String(encrypted)))
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    using (var sr = new StreamReader(cs))
                    {
                        return sr.ReadToEnd();
                    }
                }
            }
        }

    }


}
