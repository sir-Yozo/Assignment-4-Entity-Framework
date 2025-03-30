using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Mvc;

namespace Assignment_3_CRUD.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
