using Databases.Interface;
using EBook.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;

namespace EBook.Controllers
{
    public class AuthController : Controller
    {
        private IConfiguration _configuration;
        private IAuthService _authService;
        private readonly string _connectionString;


        public AuthController(IConfiguration configuration, IAuthService auth)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DevConnection");
            _authService = auth;

        }

       

        [HttpPost]
        [Route("/SignUp")]
        public IActionResult AddUsers([FromBody] UserDto user)
        {
            try
            {
                var res = _authService.AddUsers(user);
                return Ok(res);
            }
            catch( Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string GenerateToken(Users user)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string HashPassword(string password)
        {
            using SHA256 sha256 = SHA256.Create();
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashedBytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("/Login")]
        public string Login([FromBody] Login login)
        {
            
            var response = _authService.Login(login);
            return response.ToString();
        }

        [HttpPost]
        [Route("/IsAdmin")]
        public IActionResult GetAdmin([FromBody] string Username)
        {
            try
            {
                var res = _authService.IsAdmin(Username);
                return Ok(res);
            }
            catch ( InValidNameException ex) {
                return BadRequest(ex.Message);
            }

            
        }

        [HttpPost]
        [Route("/AddAdmin")]
        public IActionResult AddAdmin([FromBody] UserDto user )
        {
            try
            {
                var res = _authService.AddAdmin(user);
                return Ok(res);
            }
            catch (SqlException ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}
