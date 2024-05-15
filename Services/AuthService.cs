using Databases;
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
        private readonly  IAuthService _authDatabase ;

        public AuthService( IConfiguration configuration ,IOptions<appsetting> appsetting , AuthDatabase authDatabase)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DevConnection");
            _appsetting = appsetting.Value;
            _authDatabase = authDatabase;
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
            
            try
            {
               
                var res = _authDatabase.AddUsers(user);
                return res;

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
           
            string hashedString = BCrypt.Net.BCrypt.HashPassword(password);

            return hashedString;
        }

      

        

        public string IsAdmin(string username)
        {
            try
            {
                var res = _authDatabase.IsAdmin(username);
                return res;

            }
            catch ( Exception ex)
            {
                throw new Exception("Invalid name");
            }

            
        }

        public string Login(Login login)
        {
            //using SqlConnection connection = new SqlConnection(_connectionString);
            //connection.Open();

            //string querys = "HashPasswordAndRole";
            //using SqlCommand commands = new SqlCommand(querys, connection);
            //commands.CommandType = System.Data.CommandType.StoredProcedure;
            //commands.Parameters.AddWithValue("@Username", login.Username);

            //using SqlDataReader reader = commands.ExecuteReader();
            //if (reader.Read())
            //{
            //    string passwordFromDb = reader["Password"].ToString();
            //    string userRole = reader["UserRole"].ToString();



            //    // Verify the hashed password from the database
            //    if (BCrypt.Net.BCrypt.EnhancedVerify(login.Password, passwordFromDb))
            //    {

            //        var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appsetting.Key));
            //        var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            //        var token = new JwtSecurityToken(_appsetting.Issuer, _appsetting.Audience,
            //            new[]
            //            {
            //        new Claim(ClaimTypes.Name, login.Username),
            //        new Claim(ClaimTypes.Role, userRole)
            //            },
            //            expires: DateTime.Now.AddMinutes(1),
            //            signingCredentials: credentials
            //        );

            //        return new JwtSecurityTokenHandler().WriteToken(token);
            //    }
            //}

            //return null;

            try
            {
                var rs = _authDatabase.Login(login);
                return rs;
            }
            catch ( SqlException ex )
            {
                throw new Exception("User doesnt exists");
            }
            catch
            {
                throw new Exception("Users doesnt exists");
            }
            

        }

        public Users AddAdmin(UserDto user)
        {
           
            try
            {
                var admin = _authDatabase.AddAdmin(user);
                return admin;


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
