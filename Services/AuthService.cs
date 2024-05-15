using Databases.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;

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
                UserRole = Role.User.ToString(),

            };

            string query = "InsertUser";
            using SqlCommand command = new SqlCommand(query, connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Username", users.Username);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@UserRole", Role.User.ToString());

            try
            {

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

            catch (SqlException ex)
            {
                if(ex.Number == 2627)
                {
                    throw new Exception("User already exits");
                }

               
            }

            return null;

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

      

        

        public string IsAdmin(string username)
        {
            try
            {

                if (username!.IsNullOrEmpty())
                {
                    throw new Exception("");
                }

                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string query = "CheckAdmin";
                using SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@Username", username);

                // Add output parameter to get the result
                var outputParameter = command.Parameters.Add("@IsAdmin", SqlDbType.VarChar, 20);
                outputParameter.Direction = ParameterDirection.Output;

                command.ExecuteNonQuery();

                return outputParameter.Value.ToString();
            }
            catch ( Exception ex)
            {
                throw new Exception("Invalid name");
            }

            
        }

        public string Login(Login login)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string querys = "HashPasswordAndRole";
            using SqlCommand commands = new SqlCommand(querys, connection);
            commands.CommandType = System.Data.CommandType.StoredProcedure;
            commands.Parameters.AddWithValue("@Username", login.Username);

            using SqlDataReader reader = commands.ExecuteReader();
            if (reader.Read())
            {
                string passwordFromDb = reader["Password"].ToString();
                string userRole = reader["UserRole"].ToString();

            

                // Verify the hashed password from the database
                if (BCrypt.Net.BCrypt.EnhancedVerify(login.Password, passwordFromDb))
                {

                    var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsetting.Key));
                    var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_appsetting.Issuer, _appsetting.Audience,
                        new[]
                        {
                    new Claim(ClaimTypes.Name, login.Username),
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

        public Users AddAdmin(UserDto user)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            //string hashedPassword = HashPassword(user.Password);
            string hashedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13);

            Users users = new Users()
            {
                Username = user.Username,
                Password = hashedPassword,
                UserRole = Role.Admin.ToString(),

            };

            string query = "InsertUser";
            using SqlCommand command = new SqlCommand(query, connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Username", users.Username);
            command.Parameters.AddWithValue("@Password", hashedPassword);
            command.Parameters.AddWithValue("@UserRole", Role.Admin.ToString());

            try
            {

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

            catch (SqlException ex)
            {
                if (ex.Number == 2627)
                {
                    throw new Exception("User name  already exits");
                }


            }

            return null;
        }

        
    }

   
}
