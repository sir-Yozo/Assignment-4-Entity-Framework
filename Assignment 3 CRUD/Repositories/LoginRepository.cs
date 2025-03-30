using System;
using System.Collections.Generic;
using System.Linq;
using Assignment_3_CRUD.Models;
using Assignment_3_CRUD.Repositories;
namespace Assignment_3_CRUD.Repositories
{

    public class LoginRepository : ILoginRepository
    {
        private List<User> userList = new List<User>
        {
            new User { Username = "admin", Password = "admin" },
            new User { Username = "admin2", Password = "admin2" }
        };

        public User ValidateUserLogin(string username, string password)
        {
            return userList.FirstOrDefault(s => s.Username == username && s.Password == password);
        }

        public bool RegisterUser(string username, string password)
        {
            if (userList.Any(s => s.Username == username))
                return false; // Username already exists

            userList.Add(new User { Username = username, Password = password });
            return true;
        }
    }
}
