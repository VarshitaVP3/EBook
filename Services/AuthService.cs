using Databases.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly appsetting _appsetting;

        public AuthService(IConfiguration configuration , IOptions<appsetting> appsetting)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DevConnection");
            _appsetting = appsetting.Value;

        }

        public Users AddUsers(UserDto user)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            //string hashedPassword = HashPassword(user.Password);
            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13);

            Users users = new Users()
            {
                Username = user.Username,
                Password = hashedPassword,
                UserRole = user.UserRole

            };

            string query = "InsertUser";
            using SqlCommand command = new SqlCommand(query, connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Username", users.Username);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@UserRole", users.UserRole);

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                return users;
            }
            else
            {

                return null;
            }

        }

        private string HashPassword(string password)
        {
            //using (SHA256 sha256 = SHA256.Create())
            //{
            //    byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            //    StringBuilder builder = new StringBuilder();
            //    foreach (byte b in hashedBytes)
            //    {
            //        builder.Append(b.ToString("x2"));
            //    }
            //    return builder.ToString();
            //}

            string hashedString = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedString;
        }

      

        public Users Authentication(string username, string password)
        {
            throw new NotImplementedException();
        }

        public bool IsAdmin(string username)
        {
            throw new NotImplementedException();
        }

        public string Login(Login login)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            // string hashedPassword = HashPassword(login.Password);

            string querys = "HashPassword";
            using SqlCommand commands = new SqlCommand(querys, connection);
            commands.CommandType = System.Data.CommandType.StoredProcedure;

            commands.Parameters.AddWithValue("@Username", login.Username);

            string hash = commands.ExecuteScalar() as string;


            if (BCrypt.Net.BCrypt.EnhancedVerify(login.Password, hash))
            {

                string query = "SELECT Username, UserRole FROM Users WHERE Username = @Username AND Password = @Password;";
                using SqlCommand command = new SqlCommand(query, connection);

                command.Parameters.AddWithValue("@Username", login.Username);
                command.Parameters.AddWithValue("@Password", hash);


                using SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    string username = reader["Username"].ToString();
                    string userRole = reader["UserRole"].ToString();

                    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsetting.Key));
                    var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_appsetting.Issuer, _appsetting.Audience
                       ,
                        new[]
                        {
                        new Claim(ClaimTypes.Name, username),
                        new Claim(ClaimTypes.Role, userRole)
                        },
                        expires: DateTime.Now.AddMinutes(1),
                        signingCredentials: credentials
                    );

                    return new JwtSecurityTokenHandler().WriteToken(token);
                }

            }


            return null;
        }
    }
}
