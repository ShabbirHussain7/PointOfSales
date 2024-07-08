using POS.Database;
using POS.Models;
using System.Linq;
using POS.Services;

namespace POS.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly POSDbContext _dbContext;

        public UserRepository(POSDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public User AddUser(User user)
        {
            Console.WriteLine("adding user: ", user);
            string encryptedPassword = HashingService.ComputeHash(user.Password);
            user.Password = encryptedPassword;
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
            return user;
        }

        public User Authenticate(string email, string password)
        {
            var user = _dbContext.Users.FirstOrDefault(u => u.Email == email);
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