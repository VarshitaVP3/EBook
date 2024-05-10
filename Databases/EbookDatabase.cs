using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Database
{
    public class EbookDatabase : IEbook
    {
        //private readonly string _connectionString = "Data Source = 192.168.10.28\\SQLEX2017;Initial Catalog=Varshita;User ID=sysfore.ea;Password=Sys@2024#;Encrypt = false";
        //private readonly string _connectionString = "Data Source=BLRSFLT277\\SQLEXPRESS;Initial Catalog=Varsha;Integrated Security=True;Trusted_Connection=true;encrypt=false";

        private  IConfiguration _configuration;
        public string _connectionString;

        public EbookDatabase(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DevConnection");
        }



        public Author AddAuthor(AuthorDto autordto)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();


            Author author = new Author()
            {
                FirstName = autordto.FirstName,
                LastName = autordto.LastName,
                Biography = autordto.Biography,
                BirthDate = autordto.BirthDate,
                Country = autordto.Country,
                //CreatedAt = DateTime.Now,
                //UpdatedAt = autordto.UpdatedAt,
                isActive = autordto.isActive,
                Email = autordto.Email,
                ContactInfo = autordto.ContactInfo,
                SocialMedia = autordto.SocialMedia,
            };

            string query = "InsertIntoAuthor";
            using SqlCommand command = new SqlCommand(query, connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            
            command.Parameters.AddWithValue("@FirstName", author.FirstName);
            command.Parameters.AddWithValue("@LastName", author.LastName);
            command.Parameters.AddWithValue("@Biography", author.Biography);
            command.Parameters.AddWithValue("@BirthDate", author.BirthDate);
            command.Parameters.AddWithValue("@Country", author.Country);
            //command.Parameters.AddWithValue("@UpdatedAt", author.UpdatedAt);
            command.Parameters.AddWithValue("@isActive", author.isActive);
            command.Parameters.AddWithValue("@Email", author.Email);
            command.Parameters.AddWithValue("ContactInfo", author.ContactInfo);
            command.Parameters.AddWithValue("SocialMedia", author.SocialMedia);
            //command.Parameters.AddWithValue("@CreatedAt", author.CreatedAt);

            


            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {


                return author;
            }
            else
            {

                return null ;
            }


        }

        public List<Author> GetAuthor()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = "GetAuthors";
            using SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            var authorList = new List<Author>();
            while (reader.Read())
            {
                Console.WriteLine($"Column1: {reader.GetValue(0)}, Column2: {reader.GetValue(1)},  Column3: {reader.GetValue(2)}, Column4: {reader.GetValue(3)}" +
                    $"Column5: {reader.GetValue(4)},Column6: {reader.GetValue(5)},Column7: {reader.GetValue(6)}," +
                    $"Column8: {reader.GetValue(7)}, Column9: {reader.GetValue(8)},Column10: {reader.GetValue(9)} ,Column11 : {reader.GetValue(10)},Column12 : {reader.GetValue(11)}");

                var author = new Author();
                author.AuthorId = reader.GetInt32(0);
                author.FirstName = reader.GetString(1);
                author.LastName = reader.GetString(2);
                author.Biography = reader.GetString(3);
                author.BirthDate = reader.GetDateTime(4);
                author.Country = reader.GetString(5);
                author.CreatedAt = reader.GetDateTime(6);
                author.UpdatedAt = reader.GetDateTime(7);
                author.isActive = reader.GetBoolean(8);
                author.Email = reader.GetString(9);
                author.ContactInfo = reader.GetString(10);
                author.SocialMedia = reader.GetString(11);

                authorList.Add(author);

            }

            return authorList;

        }

        public string DeleteAuthor(int AuthorId)
        {
            try
            {
                
                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string query = "DeleteAuthor";
                using SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@AuthorId", AuthorId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return "Author is deleted successfully";
                }
                else
                {
                    return "Author not found or deletion failed.";
                }
            }
            catch(Exception ex)
            {
                return ex.Message;  
            }

            
        }

        public Author UpdateAuthor(Author autordto)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = "UpdateAuthor";
            using SqlCommand command = new SqlCommand(query, connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@AuthorId", autordto.AuthorId);
            command.Parameters.AddWithValue("@FirstName" , autordto.FirstName);
            command.Parameters.AddWithValue("@LastName" , autordto .LastName);
            command.Parameters.AddWithValue("@Biography" , autordto.Biography);
            command.Parameters.AddWithValue("@Country", autordto.Country);
            //command.Parameters.AddWithValue("@UpdatedAt" , autordto.UpdatedAt);
            command.Parameters.AddWithValue("@isActive", autordto.isActive);
            command.Parameters.AddWithValue("@Email", autordto.Email);
            command.Parameters.AddWithValue("@ContactInfo" , autordto.ContactInfo);
            command.Parameters.AddWithValue("@SocialMedia", autordto.SocialMedia);

            

            int rowsAffected = command.ExecuteNonQuery();
            

            if (rowsAffected > 0)
            {
                Console.WriteLine("Author found and is updated");
                return autordto;
            }
            else
            {
                return null;
            }
        }

        //public Ebook AddEbook(EbookDto ebookdto)
        //{
        //    using SqlConnection connection = new SqlConnection(_connectionString);
        //    connection.Open();

        //    Ebook ebook = new Ebook()
        //    {
        //        Name = ebookdto.Name,
        //        Description = ebookdto.Description,
        //        ISBN = ebookdto.ISBN,
        //        Price = ebookdto.Price,
        //        Language = ebookdto.Language,
        //        PublicationDate = DateTime.Now,
        //        Publisher = ebookdto.Publisher,
        //        PageCount = ebookdto.PageCount,
        //        AverageCounting = ebookdto.AverageCounting,
        //        UpdatedAt = DateTime.Now,
        //        isAvailable = ebookdto.isAvailable,
        //        edition = ebookdto.edition ,

        //        GenereId = ebookdto.GenereId,
        //        CreatedAt = DateTime.Now,
        //    };

        //    string query = "InsertIntoEbook";

        //    using SqlCommand command = new SqlCommand(query, connection);

        //    command.CommandType = System.Data.CommandType.StoredProcedure;

        //    //command.Parameters.AddWithValue("@EbookId", ebook.EbookId);
        //    command.Parameters.AddWithValue("@Name", ebook.Name);
        //    command.Parameters.AddWithValue("@Description", ebook.Description);
        //    command.Parameters.AddWithValue("@ISBN", ebook.ISBN);
        //    command.Parameters.AddWithValue("@PublicationDate", ebook.PublicationDate);
        //    command.Parameters.AddWithValue("@Price", ebook.Price);
        //    command.Parameters.AddWithValue("@Language", ebook.Language);
        //    command.Parameters.AddWithValue("@Publisher", ebook.Publisher);
        //    command.Parameters.AddWithValue("@PageCount", ebook.PageCount);
        //    command.Parameters.AddWithValue("@AverageCounting", ebook.AverageCounting);
        //    command.Parameters.AddWithValue("@CreatedAt", ebook.CreatedAt);
        //    command.Parameters.AddWithValue("@UpdatedAt", ebook.UpdatedAt);

        //    command.Parameters.AddWithValue("@GenereId", ebook.GenereId);
        //    command.Parameters.AddWithValue("@isAvailable", ebook.isAvailable);
        //    command.Parameters.AddWithValue("@edition", ebook.edition);



        //    int rowsAffected = command.ExecuteNonQuery();

        //    if(rowsAffected > 0)
        //    {
        //        string getEbookIdQuery = "SELECT E.EbookId from Ebook E where E.Name = @Name";
        //        //command.Parameters.AddWithValue("@Name", ebook.Name);
        //        SqlCommand getEbookIdCommand = new SqlCommand(getEbookIdQuery, connection);
        //        getEbookIdCommand.Parameters.AddWithValue("@Name", ebook.Name);

        //        int ebookId = Convert.ToInt32(getEbookIdCommand.ExecuteScalar());

        //        string mappingQuery = "InsertIntoEbookAuthorMapping";
        //        using SqlCommand mappingCommand = new SqlCommand(mappingQuery, connection);
        //        mappingCommand.CommandType = System.Data.CommandType.StoredProcedure;
        //        mappingCommand.Parameters.AddWithValue("@BookID", ebookId);
        //        //mappingCommand.Parameters.AddWithValue("@AuthorID", ebook.AuthorId);
        //        int mappingRowsAffected = mappingCommand.ExecuteNonQuery();

        //        if (mappingRowsAffected > 0)
        //        {
        //            return ebook;
        //        }
        //        else
        //        {
        //            return null;
        //        }



        //        return ebook;
        //    }
        //    else
        //    {
        //        Console.WriteLine("Not Added");
        //        return null;
        //    }

        //}


        public Ebook AddEbook(EbookDto ebookdto, List<int> AuthorList)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            Ebook ebook = new Ebook()
            {
                Name = ebookdto.Name,
                Description = ebookdto.Description,
                ISBN = ebookdto.ISBN,
                Price = ebookdto.Price,
                Language = ebookdto.Language,
                PublicationDate = DateTime.Now,
                Publisher = ebookdto.Publisher,
                PageCount = ebookdto.PageCount,
                AverageCounting = ebookdto.AverageCounting,
                //UpdatedAt = DateTime.Now,
                isAvailable = ebookdto.isAvailable,
                edition = ebookdto.edition,

                GenereId = ebookdto.GenereId,
                //CreatedAt = DateTime.Now,
            };

            string query = "InsertIntoEbook";

            using SqlCommand command = new SqlCommand(query, connection);

            command.CommandType = System.Data.CommandType.StoredProcedure;

            //command.Parameters.AddWithValue("@EbookId", ebook.EbookId);
            command.Parameters.AddWithValue("@Name", ebook.Name);
            command.Parameters.AddWithValue("@Description", ebook.Description);
            command.Parameters.AddWithValue("@ISBN", ebook.ISBN);
            command.Parameters.AddWithValue("@PublicationDate", ebook.PublicationDate);
            command.Parameters.AddWithValue("@Price", ebook.Price);
            command.Parameters.AddWithValue("@Language", ebook.Language);
            command.Parameters.AddWithValue("@Publisher", ebook.Publisher);
            command.Parameters.AddWithValue("@PageCount", ebook.PageCount);
            command.Parameters.AddWithValue("@AverageCounting", ebook.AverageCounting);
            //command.Parameters.AddWithValue("@CreatedAt", ebook.CreatedAt);
            //command.Parameters.AddWithValue("@UpdatedAt", ebook.UpdatedAt);

            command.Parameters.AddWithValue("@GenereId", ebook.GenereId);
            command.Parameters.AddWithValue("@isAvailable", ebook.isAvailable);
            command.Parameters.AddWithValue("@edition", ebook.edition);




            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
        
                string getEbookIdQuery = "SELECT EbookId from Ebook  where Name = @Name";
               

                SqlCommand getEbookIdCommand = new SqlCommand(getEbookIdQuery, connection);
                getEbookIdCommand.Parameters.AddWithValue("@Name", ebook.Name);
                int ebookId = Convert.ToInt32(getEbookIdCommand.ExecuteScalar());

             



                DataTable authorDataTable = new DataTable();
                authorDataTable.Columns.Add("AuthorId", typeof(int));
                foreach (int authorId in AuthorList)
                {
                    authorDataTable.Rows.Add(authorId);
                }

                string mappingQuery = "InsertIntoEbookAuthorMapping";
                using SqlCommand mappingCommand = new SqlCommand(mappingQuery, connection);
                mappingCommand.CommandType = CommandType.StoredProcedure;
                mappingCommand.Parameters.AddWithValue("@EbookId", ebookId);
                mappingCommand.Parameters.AddWithValue("@AuthorGenre", authorDataTable);
                mappingCommand.ExecuteNonQuery();

                return ebook;


            }
            else
            {
                Console.WriteLine("Not Added");
                return null;
            }
        }


        public List<Ebook> GetEbooks()
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = "GetBooks";
            using SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            SqlDataReader reader = command.ExecuteReader();
            var bookList = new List<Ebook>();
            while (reader.Read())
            {
                Console.WriteLine($"Column1: {reader.GetValue(0)}, Column2: {reader.GetValue(1)},  Column3: {reader.GetValue(2)}, Column4: {reader.GetValue(3)}" +
                    $"Column5: {reader.GetValue(4)},Column6: {reader.GetValue(5)},Column7: {reader.GetValue(6)}," +
                    $"Column8: {reader.GetValue(7)}, Column9: {reader.GetValue(8)},Column10: {reader.GetValue(9)} ,Column11 : {reader.GetValue(10)},Column12 : {reader.GetValue(11)} , Column13 : {reader.GetValue(12)} , Column14:{reader.GetValue(13)} , Column15:{reader.GetValue(14)}");

                var books = new Ebook();
                books.EbookId = reader.GetInt32(0);
                books.Name = reader.GetString(1);
                books.Description = reader.GetString(2);
                books.ISBN = reader.GetInt32(3);
                books.PublicationDate = reader.GetDateTime(4);
                books.Price = reader.GetDecimal(5);
                books.Language = reader.GetString(6);
                books.Publisher = reader.GetString(7);
                books.PageCount = reader.GetInt32(8);
                books.AverageCounting = reader.GetInt32(9);
                books.CreatedAt = reader.GetDateTime(10);
                books.UpdatedAt = reader.GetDateTime(11);
                
                books.GenereId = reader.GetInt32(12);
                books.isAvailable = reader.GetBoolean(13);
                books.edition = reader.GetString(14);

                bookList.Add(books);

            }

            return bookList;

        }

        public string DeleteBook(int EbookId)
        {
            try
            {

                using SqlConnection connection = new SqlConnection(_connectionString);
                connection.Open();

                string query = "DeleteBooks";
                using SqlCommand command = new SqlCommand(query, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@EbookId", EbookId);

                int rowsAffected = command.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return "Book details is present and is deleted successfully";
                }
                else
                {
                    return "Book details not found or deletion failed.";
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }


        }

        //public Ebook UpdateEbook(Ebook ebook)
        //{
        //    using SqlConnection connection = new SqlConnection(_connectionString);
        //    connection.Open();

        //    string query = "UpdateBook";
        //    using SqlCommand command = new SqlCommand(query, connection);
        //    command.CommandType = System.Data.CommandType.StoredProcedure;

        //    command.Parameters.AddWithValue("@EbookId", ebook.EbookId);
        //    command.Parameters.AddWithValue("@Name", ebook.Name);
        //    command.Parameters.AddWithValue("@Description", ebook.Description);
        //    command.Parameters.AddWithValue("@ISBN", ebook.ISBN);
        //    command.Parameters.AddWithValue("@PublicationDate", ebook.PublicationDate);
        //    command.Parameters.AddWithValue("@Price", ebook.Price);
        //    command.Parameters.AddWithValue("@Language", ebook.Language);
        //    command.Parameters.AddWithValue("@Publisher", ebook.Publisher);
        //    command.Parameters.AddWithValue("@PageCount", ebook.PageCount);
        //    command.Parameters.AddWithValue("@AverageCounting", ebook.AverageCounting);
        //    //command.Parameters.AddWithValue("@CreatedAt", ebook.CreatedAt);
        //    //command.Parameters.AddWithValue("@UpdatedAt", ebook.UpdatedAt);
          
        //    command.Parameters.AddWithValue("@GenereId", ebook.GenereId);
        //    command.Parameters.AddWithValue("@isAvailable", ebook.isAvailable);
        //    command.Parameters.AddWithValue("@edition", ebook.edition);


        //    int rowsAffected = command.ExecuteNonQuery();

        //    if (rowsAffected > 0)
        //    {
        //        Console.WriteLine("Book found and updated");
        //        return ebook;
        //    }
        //    else
        //    {
        //        return null;
        //    }

        //}

        public Ebook UpdateEbook(Ebook ebook , List<int> AuthorList)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = "UpdateBook";
            using SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@EbookId", ebook.EbookId);
            command.Parameters.AddWithValue("@Name", ebook.Name);
            command.Parameters.AddWithValue("@Description", ebook.Description);
            command.Parameters.AddWithValue("@ISBN", ebook.ISBN);
            command.Parameters.AddWithValue("@PublicationDate", ebook.PublicationDate);
            command.Parameters.AddWithValue("@Price", ebook.Price);
            command.Parameters.AddWithValue("@Language", ebook.Language);
            command.Parameters.AddWithValue("@Publisher", ebook.Publisher);
            command.Parameters.AddWithValue("@PageCount", ebook.PageCount);
            command.Parameters.AddWithValue("@AverageCounting", ebook.AverageCounting);
            //command.Parameters.AddWithValue("@CreatedAt", ebook.CreatedAt);
            //command.Parameters.AddWithValue("@UpdatedAt", ebook.UpdatedAt);

            command.Parameters.AddWithValue("@GenereId", ebook.GenereId);
            command.Parameters.AddWithValue("@isAvailable", ebook.isAvailable);
            command.Parameters.AddWithValue("@edition", ebook.edition);

           

            int rowsAffected = command.ExecuteNonQuery();

            if (rowsAffected > 0)
            {
                string getEbookIdQuery = "SELECT EbookId from Ebook  where Name = @Name";


                SqlCommand getEbookIdCommand = new SqlCommand(getEbookIdQuery, connection);
                getEbookIdCommand.Parameters.AddWithValue("@Name", ebook.Name);
                int ebookId = Convert.ToInt32(getEbookIdCommand.ExecuteScalar());

                DataTable authorDataTable = new DataTable();
                authorDataTable.Columns.Add("AuthorId", typeof(int));
                foreach (int authorId in AuthorList)
                {
                    authorDataTable.Rows.Add(authorId);
                }

                string mappingQuery = "UpdateInsertintoMapping";
                using SqlCommand mappingCommand = new SqlCommand(mappingQuery, connection);
                mappingCommand.CommandType = CommandType.StoredProcedure;
                mappingCommand.Parameters.AddWithValue("@EbookId", ebookId);
                mappingCommand.Parameters.AddWithValue("@AuthorGenre", authorDataTable);
                mappingCommand.ExecuteNonQuery();

                Console.WriteLine("Book found and updated");
               return ebook;
            }
            else
            {
                return null;
            }

        }

        public bool ValidateAuthorDetails(Author author)
        {
            if (!ValidateName(author.FirstName, author.LastName))
            {
                return false;
            }
            if (!ValidateContactInfo(author.ContactInfo))
            { return false; }

            if(!ValidateEmail(author.Email)) {  return false; }

            return true;

        }

        public bool ValidateAuthor(AuthorDto authordto)
        {
            if(!ValidateName(authordto.FirstName, authordto.LastName))
            {
                return false;
            }
            if(!ValidateContactInfo(authordto.ContactInfo))
            { return false; }

            if(!ValidateEmail(authordto.Email))
            { return false; }

            return true;

        }

        public bool ValidateName(string FirstName , string LastName )
        {
            const string pattern = @"^[A-Za-z]+$";
            if(Regex.IsMatch(FirstName, pattern))
            {
                return true;
            }
            if(Regex.IsMatch(LastName, pattern))
            {
                return true;
            }

            return false;
            
        }

        public bool ValidateEmail(string Email)
        {
            const string email = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            if (Regex.IsMatch(Email, email))
            {
                return true;
            }

            return false;

        }

        public bool ValidateContactInfo(string ContactInfo)
        {
            const string contact = @"^([0-9]{10})$";
            if(!Regex.IsMatch(ContactInfo, contact))
                return false;

            return true;
        }

        public bool ValidateEbookDto(EbookDto ebook)
        {
            const string name = @"^[A-Za-z]+$";
            if(!Regex.IsMatch(ebook.Name , name))
                return false;

            return true;

        }

        public bool ValidateEbook(Ebook ebook)
        {
            const string name = @"^[A-Za-z]+$";
            if (!Regex.IsMatch(ebook.Name , name))
                return false;

            return true;
        }

        public List<Ebook> SearchEbooksByTitle(string title)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();

            string query = "SearchEbookByName";
            using SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@Name", title);
            using SqlDataReader reader = command.ExecuteReader();
            var ebookList = new List<Ebook>();

            while (reader.Read())
            {
                Console.WriteLine($"Column1: {reader.GetValue(0)}, Column2: {reader.GetValue(1)},  Column3: {reader.GetValue(2)}, Column4: {reader.GetValue(3)}" +
                    $"Column5: {reader.GetValue(4)},Column6: {reader.GetValue(5)},Column7: {reader.GetValue(6)}," +
                    $"Column8: {reader.GetValue(7)}, Column9: {reader.GetValue(8)},Column10: {reader.GetValue(9)} ,Column11 : {reader.GetValue(10)},Column12 : {reader.GetValue(11)} , Column13 : {reader.GetValue(12)} , Column14:{reader.GetValue(13)} , Column15:{reader.GetValue(14)}");

                var books = new Ebook();
                books.EbookId = reader.GetInt32(0);
                books.Name = reader.GetString(1);
                books.Description = reader.GetString(2);
                books.ISBN = reader.GetInt32(3);
                books.PublicationDate = reader.GetDateTime(4);
                books.Price = reader.GetDecimal(5);
                books.Language = reader.GetString(6);
                books.Publisher = reader.GetString(7);
                books.PageCount = reader.GetInt32(8);
                books.AverageCounting = reader.GetInt32(9);
                books.CreatedAt = reader.GetDateTime(10);
                books.UpdatedAt = reader.GetDateTime(11);
               
                books.GenereId = reader.GetInt32(12);
                books.isAvailable = reader.GetBoolean(13);
                books.edition = reader.GetString(14);

                ebookList.Add(books);

            }

            return ebookList;

        }

        public List<string> SearchEbooksByGenre(string GenereName)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open();
            string query = "SearchEbooksByGenere";
            using SqlCommand command = new SqlCommand(query, connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@GenereName", GenereName);
            using SqlDataReader reader = command.ExecuteReader();
            var ebookList = new List<string>(); 

            while (reader.Read())
            {
                
                string ebookName = reader["Name"].ToString();

                ebookList.Add(ebookName);
            }

            return ebookList;


        }


        public List<string> SearchBookByLanguage(string Language)
        {
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open ();
            string query = "SearchByLanguage";
            using SqlCommand command = new SqlCommand( query,connection);
            command.CommandType = System.Data.CommandType.StoredProcedure ;
            command.Parameters.AddWithValue("@Language" , Language);
            using SqlDataReader reader = command.ExecuteReader();
            var bookListLanguage = new List<string>();

            while(reader.Read())
            {
                string bookName = reader["Name"].ToString();
                bookListLanguage.Add(bookName);
            }

            return bookListLanguage;

        }

        public List<string> SearchAuthorByBook(string BookName)
        {
            List<string> authorname = new List<string>();
            using SqlConnection connection = new SqlConnection(_connectionString);
            connection.Open ();

            string query = "GetAuthorsByBookName";
            using SqlCommand command = new SqlCommand(query,connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@Name", BookName);

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string firstName = reader["FirstName"].ToString();
                string lastName = reader["LastName"].ToString() ;
                string authorName = $"{firstName} {lastName}";
                authorname.Add(authorName);
            }

            return authorname;
        }

        public List<string> SearchBookByAuthorName(string AuthorName)
        {
            List<string> bookname = new List<string>();
            using SqlConnection connection = new SqlConnection (_connectionString);
            connection.Open();

            string query = "GetBookByAuthor";
            using SqlCommand command = new SqlCommand(query,connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@AuthorName", AuthorName);

            using SqlDataReader reader = command.ExecuteReader();
            while(reader.Read())
            {
                string ename = reader.GetString("Name");

                bookname.Add(ename);
            }

            return bookname;
        }
    }
}