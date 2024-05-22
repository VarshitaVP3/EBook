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

        public EbookServices(EbookDatabase ebookDatabase)
        {
            _ebookDatabase = ebookDatabase;
        }
        
        public Author AddAuthor(AuthorDto autordto)
        {
            try
            {
                if (_ebookDatabase.ValidateAuthor(autordto))
                {
                    var abc = _ebookDatabase.AddAuthor(autordto);
                    return abc;
                }
                else
                {
                    throw new CustomException("Invalid input");
                }
            }
            catch(CustomException  ex)
            {
                throw new Exception("Some error occured", ex);
            }
            catch (Exception ex)
            {
                throw new Exception($"{ex.Message}", ex);
            }
  

        }

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
            try
            {
                return _ebookDatabase.DeleteAuthor(AuthorId);
                throw new CustomException("Author deleted");
            }
            catch(CustomException ex)
            {
                throw new Exception("Unable to delete the author details");
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to delete as it has the foreign key reference");
            }
        }

        public List<Author> GetAuthor()
        {
           return _ebookDatabase.GetAuthor();
        }

        public Author UpdateAuthor(Author autor)
        {
            try
            {
                if (_ebookDatabase.ValidateAuthorDetails(autor))
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


        public Ebook UpdateEbook(Ebook ebook , List<int> AuthorList)
        {
            var res = _ebookDatabase.UpdateEbook(ebook , AuthorList);
            return res;
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
