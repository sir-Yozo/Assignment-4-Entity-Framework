using System.Collections;
using System.Collections.Generic;
using Assignment_3_CRUD.Models;

namespace Assignment_3_CRUD.Repositories
{
    public interface ILoginRepository
    {
        User ValidateUserLogin(string username, string password);
        bool RegisterUser(string username, string password);
    }
}
