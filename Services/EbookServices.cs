using Database;
using Microsoft.Extensions.Configuration;
using Models;
using Services.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Services
{
    public class EbookServices : IEbook
    {
        public IEbook _ebookDatabase;

        //public EbookServices(IConfiguration configuration) {
        //    _ebookDatabase = new EbookDatabase(configuration);
        //}

        public EbookServices(EbookDatabase ebookDatabase)
        {
            _ebookDatabase = ebookDatabase;
        }
        
        public Author AddAuthor(AuthorDto autordto)
        {
            if(_ebookDatabase.ValidateAuthor(autordto))
            {
                var res = _ebookDatabase.AddAuthor(autordto);
                return res;
            }

            return null;

        }

        //public Ebook AddEbook(EbookDto ebook)
        //{


        //    try
        //    {
        //        if(_ebookDatabase.ValidateEbookDto(ebook))
        //        {
        //            var res= _ebookDatabase.AddEbook(ebook);
        //            return res;
        //        }

        //        return null;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex; 
        //    }
        //}

        public Ebook AddEbook(EbookDto ebookDto, List<int> AuthorList)
        {
            try
            {
                if (_ebookDatabase.ValidateEbookDto(ebookDto))
                {
                    var res = _ebookDatabase.AddEbook(ebookDto, AuthorList);
                    return res;
                }
                else
                {
                    throw new CustomException("Invalid input");
                }
            }

            catch(CustomException ex)
            {
                throw new CustomException("Error adding Ebook", ex);
            }
            catch (Exception ex)
            {
                throw new CustomException($"{ex.Message}", ex);
            }

           
        }


        public string DeleteAuthor(int AuthorId)
        {
            return _ebookDatabase.DeleteAuthor(AuthorId);
        }

        public List<Author> GetAuthor()
        {
           return _ebookDatabase.GetAuthor();
        }

        public Author UpdateAuthor(Author autor)
        {
            try
            {
                if(_ebookDatabase.ValidateAuthorDetails(autor))
                {
                    var result = _ebookDatabase.UpdateAuthor(autor);
                    return result;
                }

                return null;

            }
            catch(Exception ex)
            {
                Console.WriteLine("Error occured in updating");
                return null;
            }
               
       
        }

        public List<Ebook> GetEbooks()
        {
            return _ebookDatabase.GetEbooks();
        }

        public string DeleteBook(int EbookId)
        {
            return _ebookDatabase.DeleteBook(EbookId);
        }

        public Ebook UpdateEbook(Ebook ebook)
        {
            try
            {
                if (_ebookDatabase.ValidateEbook(ebook))
                {
                    var result = _ebookDatabase.UpdateEbook(ebook);
                    return result;
                }

                return null;

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error occured in updating");
                return null;
            }
        }

        public bool ValidateAuthor(AuthorDto authordto)
        {
            throw new NotImplementedException();
        }

        public bool ValidateAuthorDetails(Author author)
        {
            throw new NotImplementedException();
        }

        public bool ValidateEbook(EbookDto ebook)
        {
            throw new NotImplementedException();
        }

        public bool ValidateEbookDto(EbookDto ebook)
        {
            throw new NotImplementedException();
        }

        public bool ValidateEbook(Ebook ebook)
        {
            throw new NotImplementedException();
        }

        public List<Ebook> SearchEbooksByTitle(string title)
        {
            return _ebookDatabase.SearchEbooksByTitle(title);
        }

        public List<string> SearchEbooksByGenre(string GenereName)
        {
            return _ebookDatabase.SearchEbooksByGenre(GenereName);
        }

       
        public List<string> SearchBookByLanguage(string Language)
        {
            return _ebookDatabase.SearchBookByLanguage(Language);
        }

        public Ebook AddEbooks(EbookDto ebook, DataTable AuthorIds)
        {
            throw new NotImplementedException();
        }

        public List<string> SearchAuthorByBook(string BookName)
        {
            return _ebookDatabase.SearchAuthorByBook(BookName);
        }

        public List<string> SearchBookByAuthorName(string AuthorName)
        {
            return _ebookDatabase.SearchBookByAuthorName(AuthorName);  
        }
    }
}
