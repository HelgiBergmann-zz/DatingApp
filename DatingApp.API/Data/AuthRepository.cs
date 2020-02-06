using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data
{
  public class AuthRepository : IAuthUserRepository
  {
    private readonly DataContext _context;
    public AuthRepository(DataContext context)
    {
      _context = context;

    }
    public async Task<User> Login(string username, string password)
    {
       var user = await _context.Users.FirstOrDefaultAsync(x => username == x.UserName);
       if (user == null) {
         return null;
       }
       if (!VerifyUserPassword(password, user.PasswordHash, user.PasswordSalt)) return null;
       return user;
    }

    private bool VerifyUserPassword(string password, byte[] passwordHash, byte[] passwordSalt)
    {
       byte[] computedHash;
       using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt)) {
         computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
         for (var i = 0; i < computedHash.Length; i++) {
           if (computedHash[i] != passwordHash[i]) return false;
         }
       }
       return true;
    }

    public async Task<User> Register(User user, string password)
    {
      byte[] passwordHash, passwordSalt;
      CreatePasswordHash(password, out passwordHash, out passwordSalt);
      user.PasswordHash = passwordHash;
      user.PasswordSalt = passwordSalt;
      await _context.AddAsync(user);
      await _context.SaveChangesAsync();
      return user;
    }

    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
       using(var hmac = new System.Security.Cryptography.HMACSHA512()) {
         passwordSalt = hmac.Key;
         passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
       }
    }

    public async Task<bool> UserExist(string username)
    {
      return await Task.Run( () => Array.Exists(_context.Users.ToArray(), x => x.UserName == username));
    }
  }
}