﻿using Assignment_3_CRUD___Model.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_3_CRUD___Model.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public string Index()
        {
            return "Home Page";
        }
    }
}
