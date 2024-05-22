using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace EBook.Controllers
{
    [ApiController]
    [Route("/api/controller")]
    public class EbookDbController : Controller
    {
        private readonly EbookDbService _ebookDbService;

        public EbookDbController(EbookDbService ebookDbService)
        {
            _ebookDbService = ebookDbService;
        }

        [HttpPost("AddAuthor")]
        public IActionResult AddAuthors([FromBody] AuthorDto author)
        {
            _ebookDbService.AddAuthor(author);
            return Ok(author);
        }

        [HttpDelete("DeleteAuthor")]
        public IActionResult DeleteAuthors(int AuthorId)
        {
            var res = _ebookDbService.DeleteAuthor(AuthorId);
            return Ok(res);
        }

        [HttpPut("UpdateAuthor")]
        public IActionResult UpdateAuthor(int AuthorId, [FromBody] AuthorDto author)
        {
            var res = _ebookDbService.UpdateAuthor(AuthorId, author);
            if (res == null)
            {
                return NotFound($"Author with Id = {AuthorId} not found");
            }
            return Ok(res);
        }

        [HttpGet("GetAuthorList")]
        public IActionResult GetAuthorList()
        {
            var res = _ebookDbService.GetAuthorList();
            return Ok(res);
        }

        [HttpPost("AddEbook")]
        public IActionResult AddBookDetails([FromBody] EbookDto ebookDto, [FromQuery] List<int> AuthorId)
        {
            var res = _ebookDbService.AddEbook(ebookDto, AuthorId);
            return Ok(res);
        }

        [HttpGet("GetEbook")]
        public IActionResult GetEbookDetails()
        {
            var res = _ebookDbService.GetEbookList();
            return Ok(res);
        }

        [HttpPut("UpdateEbook")]
        public IActionResult UpdateEbook(int EbookId, [FromBody] EbookDto ebook)
        {
            var res = _ebookDbService.UpdateEbook(EbookId, ebook);
            return Ok(res);
        }

        [HttpDelete("DeleteEbook")]
        public IActionResult DeleteEbook(int EbookId)
        {
            var res = _ebookDbService.DeleteEbook(EbookId);
            return Ok(res);
        }

        [HttpGet("GetEbookByTitle")]
        public IActionResult GetEbooksByTitlename(string Titlename)
        {
            var res = _ebookDbService.SearchEbookByTitle(Titlename);
            return Ok(res);
        }

        [HttpGet("/SearchEbookByGenere")]
        public IActionResult SearchBookByGenere(string Genere)
        {
            var r = _ebookDbService.SearchEbooksByGenre(Genere);
            return Ok(r);
        }

        [HttpGet("/SearchEbooksByLanguage")]
        public IActionResult SearchEbook(string Language)
        {
            var res = _ebookDbService.SearchEbooksByLanguage(Language);
            return Ok(res);
        }

        [HttpGet("/SearchAuthorByBook")]
        public IActionResult actionResult(string BookName) {
            var authorList = _ebookDbService.SearchAuthorByBook(BookName);
            return Ok(authorList);
        }

        [HttpGet]
        public IActionResult Search(string AuthorName)
        {
            var bookList = _ebookDbService.SearchBookByAuthorName(AuthorName);
            return Ok(bookList);
        }
    }
}
