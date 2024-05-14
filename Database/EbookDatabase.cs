using Microsoft.Data.SqlClient;
using Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Database
{
    public class EbookDatabase : IEbook
    {
        //private readonly string _connectionString = "Data Source = 192.168.10.28\\SQLEX2017;Initial Catalog=Varshita;User ID=sysfore.ea;Password=Sys@2024#;Encrypt = false";
        private readonly string _connectionString = "Data Source=BLRSFLT277\\SQLEXPRESS;Initial Catalog=first;Integrated Security=True;Trusted_Connection=true;encrypt=false;";

        public Author AddAuthor(AuthorDto autordto)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

   

            Author author = new Author()
            {
                FirstName = autordto.FirstName,
                LastName = autordto.LastName,
                Biography = autordto.Biography,
                Birthday = autordto.Birthday,
                Country = autordto.Country,
                UpdatedAt = autordto.UpdatedAt,
                isActive = autordto.isActive,
                Email = autordto.Email,
                ContactInfo = autordto.ContactInfo,
                SocialMedia = autordto.SocialMedia,
            };

            string query = "InsertIntoAuthor";
            using SqlCommand command = new SqlCommand(query, connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@AuthorId" , author.AuthorId);
            command.Parameters.AddWithValue("@FirstName", author.FirstName);
            command.Parameters.AddWithValue("@LastName", author.LastName);
            command.Parameters.AddWithValue("@Biography" , author.Biography);
            command.Parameters.AddWithValue("@Birthday", author.Birthday);
            command.Parameters.AddWithValue("@Country", author.Country);
            command.Parameters.AddWithValue("@UpdatedAt", author.UpdatedAt);
            command.Parameters.AddWithValue("@isActive", author.isActive);
            command.Parameters.AddWithValue("@Email" , author.Email);
            command.Parameters.AddWithValue("ContactInfo" , author.ContactInfo);
            command.Parameters.AddWithValue("SocialMedia" , author.SocialMedia);


            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {


                return author;
            }
            else
            {

                return null;
            }


        }
    }
}
