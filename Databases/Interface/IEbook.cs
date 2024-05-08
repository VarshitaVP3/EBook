using Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interface
{
    public interface IEbook
    {
        public Author AddAuthor(AuthorDto autordto);

        public List<Author> GetAuthor();
        public string DeleteAuthor(int AuthorId);
        public Author UpdateAuthor(Author autor);

        public Ebook AddEbook(EbookDto ebook, List<int> AuthorList);
  
        public List<Ebook> GetEbooks();

        public string DeleteBook(int EbookId);

        //public Ebook UpdateEbook(Ebook ebook);
        public Ebook UpdateEbook(Ebook ebook , List<int> AuthorList);

        public bool ValidateAuthor(AuthorDto authordto);
        public bool ValidateAuthorDetails(Author author);
        public bool ValidateEbookDto(EbookDto ebook);

        public bool ValidateEbook(Ebook ebook);
        public List<Ebook> SearchEbooksByTitle(string title);
        public List<string> SearchEbooksByGenre(string GenereName);
       
        public List<string> SearchAuthorByBook(string BookName);

        public List<string> SearchBookByLanguage(string Language);

        //public List<string> SearchBookByAuthorName(string FirstName,string LastName);

        public List<string> SearchBookByAuthorName(string AuthorName);

    }

}
