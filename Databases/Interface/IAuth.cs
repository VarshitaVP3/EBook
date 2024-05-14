using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Databases.Interface
{
    public interface IAuthService
    {
        Users AddUsers(UserDto user);
        

        string Login(Login login);


        Users Authentication(string username, string password);
        bool IsAdmin(string username);
    }
}
