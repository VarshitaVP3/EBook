using Database;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Services.Interface;
using System.Data;

namespace EBook.Controllers
{
    public class EbookController : Controller
    {
        private readonly IEbook _ebookDatabase;
        private readonly IEbook _ebookService;

        //public EbookController(IConfiguration configuration )
        //{

        //    //_ebookDatabase = new EbookDatabase();
        //    _ebookService = new EbookServices(configuration);
          

        //}

        public EbookController(IEbook EbookService)
        {
            _ebookService = EbookService;
        }

        [HttpGet]
        [Route("/GetAuthor")]
        public IActionResult GetAuthorDetails()
        {
            
            var authList = _ebookService.GetAuthor();
            return Ok(authList);
        }

        [HttpPost]
        [Route("/PostAuthor")]
        public IActionResult Index([FromBody] AuthorDto authorDto)
        {
            
                var res = _ebookService.AddAuthor(authorDto);
            if(res!=null)
            {
                return Ok(res);
            }
                return Ok("entered details are not valid");
            
            
        }

        [HttpDelete]
        [Route("/DeleteAuthor")]
        public IActionResult Delete(int AuthorId )
        {
            string result = _ebookService.DeleteAuthor(AuthorId);

            return Ok(result);
        }

        [HttpPut]
        [Route("/EditAuthor")]
        public IActionResult Edit(int AuthorId , [FromBody] Author author )
        {
 
            var aith = _ebookService.UpdateAuthor(author);
            if(aith!=null) { return Ok(aith); }

            return Ok("Entered details are not valid");
        }

        [HttpGet]
        [Route("/GetBooks")]
        public IActionResult GetBooks()
        {
            var res = _ebookService.GetEbooks();
            return Ok(res);
        }

        //[HttpPost]
        //[Route("/AddBook")]
        //public IActionResult AddBook([FromBody] EbookDto ebookDto , List<int> AuthorList)
        //{

        //    var result = _ebookService.AddEbook(ebookDto);
        //    if(result != null)
        //    {
        //        return Ok(result);
        //    }
        //    return Ok("Entered details are invalid");  
        //}

        [HttpPost]
        [Route("/AddBook")]
        public IActionResult AddBook([FromBody] EbookDto ebookDto, List<int> AuthorList)
        {
            var result = _ebookService.AddEbook(ebookDto, AuthorList);
            if (result != null)
            {
                return Ok(result);
            }
            return Ok("Entered details are invalid");
        }


        [HttpDelete]
        [Route("/DeleteBook")]
        public IActionResult DeleteBooks(int EbookId )
        {
            string res= _ebookService.DeleteBook(EbookId);
            return Ok(res);
        }

        [HttpPut]
        [Route("/UpdateBooks")]
        public IActionResult UpdateBook(int EbookId, [FromBody] Ebook ebook, List<int> AuthorList)
        {
            //var res = _ebookService.UpdateEbook(ebook);
            var res = _ebookService.UpdateEbook(ebook, AuthorList);
            if (res != null) { return Ok(res); }
            return Ok("Entered details are not valid");
        }





        [HttpGet]
        [Route("/GetBookByName")]
        public IActionResult GetBookByName(string title)
        {
            var resList = _ebookService.SearchEbooksByTitle(title);
            return Ok(resList);
        }

        [HttpGet]
        [Route("/GetBookByGenre")]
        public IActionResult GetBookByGenere(string genere)
        {
            var res = _ebookService.SearchEbooksByGenre(genere);
            return Ok(res);
        }

      

        [HttpGet]
        [Route("/GetBookByLanguage")]
        public IActionResult GetBookByLanguage(string LanguageName)
        {
            var books = _ebookService.SearchBookByLanguage(LanguageName);
            return Ok(books);
        }

        [HttpGet]
        [Route("/SearchAuthorByName")]
        public IActionResult GetAuthorDetails(string BookName)
        {
            var res = _ebookService.SearchAuthorByBook(BookName);
            return Ok(res);

        }

        [HttpGet]
        [Route("/SearchBookByAuthor")]
        public IActionResult GetBookNames(string AuthorName)
        {
            var res = _ebookService.SearchBookByAuthorName(AuthorName);
            return Ok(res);
        }


      

    }
}
