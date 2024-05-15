using Databases.Interface;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Databases
{
    public class AuthDatabase : IAuthService
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly appsetting _appsetting;

        public AuthDatabase(IConfiguration configuration , IOptions<appsetting> appsetting)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DevConnection");
            _appsetting = appsetting.Value;

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

           
                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return users;
                }
                else
                {

                    return null;
                }

            

            

            return null;
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
            catch (Exception ex)
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
    }
}
