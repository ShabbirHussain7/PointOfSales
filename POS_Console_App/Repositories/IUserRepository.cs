using POS.Models;
using System.Collections.Generic;

namespace POS.Repositories
{
    public interface IUserRepository
    {
        User AddUser(User user);
        User Authenticate(string email, string password);
    }
}