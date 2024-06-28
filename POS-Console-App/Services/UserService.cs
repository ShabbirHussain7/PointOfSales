using POS.Models;
using POS.Database;
namespace POS.Services
{
    public class UserService
    {
        private readonly POSDbContext dbContext;

        public UserService(POSDbContext context)
        {
            dbContext = context;
        }

        public void AddUser(string name, string email, string password, UserRole role)
        {
            string encryptedPassword = HashingService.ComputeHash(password);
            dbContext.Users.Add(new User { Name = name, Email = email, Password = encryptedPassword, Role = role });
            dbContext.SaveChanges();
        }

        public User Authenticate(string email, string password)
        {
            var user = dbContext.Users.FirstOrDefault(u => u.Email == email);
            if (user != null)
            {
                string hashedPassword = HashingService.ComputeHash(password);
                if (hashedPassword == user.Password)
                {
                    return user;
                }
            }
            return null;
        }
    }
}