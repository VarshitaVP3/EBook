using Databases.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;

namespace EBook.Controllers
{
    public class UserController : Controller
    {
        private IAuthService _authService;
        public UserController(IAuthService authService)
        {
            _authService = authService;
        }

       
        [HttpPost]
        [Route("/Authorize")]
        [Authorize()] 
        public string LoginDetails( [FromBody] Login login)
        {
            return "varhsita";
        }
    }
}
