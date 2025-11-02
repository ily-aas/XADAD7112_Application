using XADAD7112_Application.Models.System;

namespace XADAD7112_Application.Services
{

    public interface ILoggingService
    {
        Task LogAsync(string action, string details);
    }

    public class Logger:ILoggingService
    {
        private readonly AppDbContext _db;

        public Logger(AppDbContext db)
        {
            _db = db;
        }

        public async Task LogAsync(string action, string details)
        {
            var log = new TraceLogs
            {
                Action = action,
                Details = details,
                CreatedAt = DateTime.Now
            };

            _db.Logs.Add(log);
            await _db.SaveChangesAsync();
        }
    }
}
