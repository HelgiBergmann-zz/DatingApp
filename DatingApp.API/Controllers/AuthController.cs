using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController: ControllerBase
    {
        private readonly IAuthUserRepository _context;
        private readonly IConfiguration _config;

        public AuthController(IAuthUserRepository context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync(UserForRegisterDto user) {
            user.UserName = user.UserName.ToLower();
            if (!(await _context.UserExist(user.UserName))) {
                var userCreate = new User();
                userCreate.UserName = user.UserName;
                await _context.Register(userCreate, user.Password);
                return StatusCode(201);
            }
            return BadRequest("Username already Exist");
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync(UserForLoginDto user) {

            User loginUser =  await _context.Login(user.UserName, user.Password);
            if (loginUser == null) {
                    return Unauthorized();
            }
            var claims =  new [] {
                new Claim(ClaimTypes.NameIdentifier, loginUser.Id.ToString()),
                new Claim(ClaimTypes.Name, loginUser.UserName)
            };
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_config.GetSection("AppSettings:Token").Value));
            var kreds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = kreds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Ok(new {token = tokenHandler.WriteToken(token)});
        }
    }
}