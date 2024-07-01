using POS.Models;
using POS.Repositories;

namespace POS.Services
{
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public void AddUser(string name, string email, string password, UserRole role)
        {
            var user = new User { Name = name, Email = email, Password = password, Role = role };
            _userRepository.AddUser(user);
        }

        public User Authenticate(string email, string password)
        {
            return _userRepository.Authenticate(email, password);
        }
    }
}