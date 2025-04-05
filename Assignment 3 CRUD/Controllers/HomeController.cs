using Assignment_3_CRUD.Migrations;
using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_3_CRUD.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("Index")]
        public IActionResult Index()
        {
            if(!User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Login", "Login");
            }
            return View();
        }
    }
}
