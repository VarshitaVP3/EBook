using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases.Interface
{
    public interface IDatabase
    {
        public List<Author> GetAuthorList();

        public Author GetAuthor(int AuthorId);
        public Author AddAuthor(AuthorDto author);
        public Author UpdateAuthor(int AuthorId, AuthorDto author);
        public string DeleteAuthor(int AuthorId);
        public Ebook AddEbook(EbookDto ebook, List<int> AuthorIds);
        public List<Ebook> GetEbookList();
        public Ebook UpdateEbook(int EbookId, EbookDto ebookDto);

        public string DeleteEbook(int EbookId);
        public List<Ebook> SearchEbookByTitle(string Title);
        public List<Ebook> SearchEbooksByGenre(string genreName);
        public List<string> SearchEbooksByLanguage(string Language);

        public List<string> SearchAuthorByBook(string bookName);

        public List<string> SearchBookByAuthorName(string authorName);
    }
}
