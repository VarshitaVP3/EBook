using Databases.Interface;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class EbookDbService : IDatabase
    {
        private readonly EbookDbContext _context;

        public EbookDbService(EbookDbContext context)
        {
            _context = context;
        }
        public Author AddAuthor(AuthorDto authordto)
        {
            Author author = new Author()
            {
                FirstName = authordto.FirstName,
                LastName = authordto.LastName,
                Biography = authordto.Biography,
                BirthDate = authordto.BirthDate,
                Country = authordto.Country,
                isActive = authordto.isActive,
                Email = authordto.Email,
                ContactInfo = authordto.ContactInfo,
                SocialMedia = authordto.SocialMedia
            };

            var auth = _context.AuthorsEf.Add(author);
            _context.SaveChanges();
            return auth.Entity;
        }

        public string DeleteAuthor(int AuthorId)
        {
            var auth = _context.AuthorsEf.FirstOrDefault(authr => authr.AuthorId == AuthorId);
            if (auth == null)
            {
                throw new Exception("User not found");
                return "Unable to delete";
            }
            else
            {
                _context.AuthorsEf.Remove(auth);
                _context.SaveChanges();
                return "Author deleted Successfully";
            }

            return null;
        }

        public Author GetAuthor(int AuthorId)
        {
            var auth = _context.AuthorsEf.FirstOrDefault(s => s.AuthorId == AuthorId);
            return auth;
        }

        public List<Author> GetAuthorList()
        {
            var authorList = _context.AuthorsEf.ToList();
            return authorList;
        }

        public Author UpdateAuthor(int AuthorId, AuthorDto authordto)
        {

            var author = _context.AuthorsEf.FirstOrDefault(s => s.AuthorId == AuthorId);
            if (author == null)
                return null;

            author.FirstName = authordto.FirstName;
            author.LastName = authordto.LastName;
            author.Biography = authordto.Biography;
            author.BirthDate = authordto.BirthDate;
            author.Country = authordto.Country;
            author.isActive = authordto.isActive;
            author.Email = authordto.Email;
            author.ContactInfo = authordto.ContactInfo;
            author.SocialMedia = authordto.SocialMedia;
            author.UpdatedAt = DateTime.Now;


            var updatedAuthor = _context.AuthorsEf.Update(author);
            _context.SaveChanges();
            return updatedAuthor.Entity;
        }

        public Ebook AddEbook(EbookDto ebookdto, List<int> authorIds)
        {
            Ebook ebooks = new Ebook()
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

            foreach (var authorId in authorIds)
            {
                ebooks.AuthorEbooks.Add(new AuthorEbook
                {
                    AuthorId = authorId,
                    Ebook = ebooks
                });
            }

            var addedEbook = _context.EbooksEf.Add(ebooks);
            _context.SaveChanges();
            return addedEbook.Entity;
        }

        public List<Ebook> GetEbookList()
        {
            //var ebookList = _context.EbooksEf.ToList();
            //return ebookList;
            var ebookList = _context.EbooksEf
            .Include(ebook => ebook.AuthorEbooks)
            .ToList();

            return ebookList;
        }

        public Ebook UpdateEbook(int EbookId, EbookDto ebookdto)
        {
            var ebook = _context.EbooksEf.FirstOrDefault(s => s.EbookId == EbookId);
            if (ebook == null)
            {
                return null;
            }

            ebook.Name = ebookdto.Name;
            ebook.Description = ebookdto.Description;
            ebook.ISBN = ebookdto.ISBN;
            ebook.Price = ebookdto.Price;
            ebook.Language = ebookdto.Language;
            //ebook.PublicationDate = DateTime.Now
            ebook.Publisher = ebookdto.Publisher;
            ebook.PageCount = ebookdto.PageCount;
            ebook.AverageCounting = ebookdto.AverageCounting;
            //UpdatedAt = DateTime.Now,
            ebook.isAvailable = ebookdto.isAvailable;
            ebook.edition = ebookdto.edition;

            ebook.GenereId = ebookdto.GenereId;
            //CreatedAt = DateTime.Now,

            var updatedEbook = _context.EbooksEf.Update(ebook);
            _context.SaveChanges();
            return updatedEbook.Entity;


        }

        public string DeleteEbook(int EbookId)
        {
            var ebook = _context.EbooksEf.FirstOrDefault(authr => authr.EbookId == EbookId);
            if (ebook == null)
            {
                throw new Exception("User not found");
                return "Unable to delete";
            }
            else
            {
                _context.EbooksEf.Remove(ebook);
                _context.SaveChanges();
                return "Book deleted Successfully";
            }
        }

        public List<Ebook> SearchEbookByTitle(string Title)
        {
            try
            {
                var ebookTitle = _context.EbooksEf.Where(e => e.Name.Contains(Title)).ToList();

                if(ebookTitle == null)
                {
                    throw new Exception("Not found");
                }

                return ebookTitle;
            }
            catch (Exception ex)
            {
                return new List<Ebook>();
            }
        }

        public List<Ebook> SearchEbooksByGenre(string genreName)
        {
            try
            {
                var ebooks = _context.EbooksEf
                    .Include(e => e.Genere)
                    .Where(e => e.Genere.GenereName.Equals(genreName))
                    .ToList();

                if (ebooks == null || !ebooks.Any())
                {
                    throw new Exception("No books found in this genre");
                }

                return ebooks;
            }
            catch (Exception ex)
            {
                // Log the exception (ex) here if needed
                return new List<Ebook>();
            }

        }

        public List<string> SearchEbooksByLanguage(string Language)
        {
            try
            {

                var languageEbook = _context.EbooksEf.Where(e => e.Language.Equals(Language)).Select(e => e.Name).ToList();
                if (languageEbook == null)
                {
                    throw new Exception("No book found in that Language");
                }

                return languageEbook;
            }
            catch (Exception ex)
            {
                throw new Exception("Exceprion found");
                return new List<string>();

            }

        }

        public List<string> SearchAuthorByBook(string bookName)
        {
            try
            {
                var authors = _context.EbooksEf
                    .Where(e => e.Name == bookName)
                    .SelectMany(e => e.AuthorEbooks)
                    .Select(eam => new { eam.Author.FirstName, eam.Author.LastName })
                    .ToList();

                if (authors == null || !authors.Any())
                {
                    throw new Exception("No authors found for the specified book name");
                }

                return authors.Select(a => $"{a.FirstName} {a.LastName}").ToList();
            }
            catch (Exception ex)
            {
                return new List<string>();
                throw new Exception("invalid");
            }
        }

        public List<string> SearchBookByAuthorName(string authorName)
        {
            try
            {
                //   var books = _context.EbooksEf
                //.Where(e => e.AuthorEbooks
                //    .Any(ae => ae.Author.FirstName + " " + ae.Author.LastName == authorName))
                //.Select(e => e.Name)
                //.ToList();

                var books = _context.EbooksEf
            .Where(e => e.AuthorEbooks
                .Any(ae => $"{ae.Author.FirstName} {ae.Author.LastName}" == authorName))
            .Select(e => e.Name)
            .ToList();

                if (books == null || !books.Any())
                    throw new Exception("No such book");

                return books;
            }

            catch (Exception ex) 
            { 
                return new List<string>();
                throw new Exception("No such Book details are found");
            }

        }
    }
}
