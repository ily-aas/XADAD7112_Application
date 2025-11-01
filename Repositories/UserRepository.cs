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
        public AppResponse UpdateUser(User user);
        public AppResponse DeleteUser(int id);
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
                user.Password = Helper.PasswordService.Encrypt(user.Password);
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

        public AppResponse UpdateUser(User user)
        {
            AppResponse response = new AppResponse();

            try
            {
                var existingUser = DB.User.FirstOrDefault(x => x.Id == user.Id);

                if (existingUser == null)
                {
                    response.IsSuccess = false;
                    response.Message = "User not found.";
                    return response;
                }

                // Update fields
                existingUser.FullName = user.FullName;
                existingUser.Username = user.Username;
                existingUser.Password = user.Password;
                existingUser.MobileNo = user.MobileNo;
                existingUser.Email = user.Email;
                existingUser.Address = user.Address;
                existingUser.UserRole = user.UserRole;
                // Do NOT update DateCreated — that's historical data

                DB.SaveChanges();

                response.IsSuccess = true;
                response.Message = "User updated successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error updating user: {ex.Message}";
                return response;
            }
        }

        public AppResponse DeleteUser(int id)
        {
            AppResponse response = new AppResponse();

            try
            {
                var user = DB.User.FirstOrDefault(x => x.Id == id);

                if (user == null)
                {
                    response.IsSuccess = false;
                    response.Message = "User not found.";
                    return response;
                }

                DB.User.Remove(user);
                DB.SaveChanges();

                response.IsSuccess = true;
                response.Message = "User deleted successfully.";
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = $"Error deleting user: {ex.Message}";
                return response;
            }
        }


    }
}
