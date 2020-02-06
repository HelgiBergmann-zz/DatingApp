using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public interface IAuthUserRepository
    {
        Task<User> Register(User user, string password);
        Task<User> Login(string usename, string password);
        Task<bool> UserExist(string username);
        
    }
}