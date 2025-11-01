using XADAD7112_Application.Models;
using XADAD7112_Application.Models.System;
using APDS_POE.Services;
using System;
using static XADAD7112_Application.Models.System.Enums;
using XADAD7112_Application.Services;


namespace APDS_POE.Repositories
{

    public interface IUserRepository
    {
        public User? Login(string Username, string Password, bool IsAdmin = false);
        public AppResponse AddUser(User user);
        public List<User> GetAllUsers();
        public User? GetUser(int ID);
    }
    public class UserRepository : IUserRepository
    {

        private readonly AppDbContext DB;

        public UserRepository(AppDbContext dbContext)
        {
            DB = dbContext;
        }

        public User? Login(string Username, string Password, bool IsAdmin = false)
        {

            User? user = new User();
            AppResponse response = new AppResponse();

            try
            {
                if (IsAdmin)
                {
                    user = DB.User.Where(x => x.Username == Username && x.Password == Password && (UserRole)x.UserRole == UserRole.Employee).FirstOrDefault();
                }
                else
                {
                    user = DB.User.Where(x => x.Username == Username && x.Password == Password && (UserRole)x.UserRole == UserRole.Farmer).FirstOrDefault();
                }

                if (user == null)
                {
                    return null;
                }

                user.HasErrors = false;
                user.Message = "Successfully logged in";
                return user;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public User? GetUser(int ID)
        {
            return DB.User.Where(x => x.Id == ID).FirstOrDefault();
        }

        public List<User> GetAllUsers()
        {
            return DB.User.Where(x => (UserRole)x.UserRole == UserRole.Farmer).ToList();
        }

        public AppResponse AddUser(User user)
        {

            AppResponse response = new AppResponse();

            try
            {
                user.DateCreated = DateTime.Now;

                DB.User.Add(user);
                DB.SaveChanges();

                response.IsSuccess = true;
                response.Message = "Farmer added successfully";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = true;
                response.Message = $"An error occurred while attempting to add a farmer: {ex.Message}";
                return response;
            }
        }
    }
}
