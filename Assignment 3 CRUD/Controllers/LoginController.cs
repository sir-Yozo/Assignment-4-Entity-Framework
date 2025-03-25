using Assignment_3_CRUD___Model.Models;
using Microsoft.AspNetCore.Mvc;
using Assignment_3_CRUD.Repositories;
using Assignment_3_CRUD___Model.Repositories;

namespace Assignment_3_CRUD.Controllers
{
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;

        public LoginController(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        // View login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login - Authenticate user
        [HttpPost]
        public IActionResult LoginSubmit(User user)
        {
            if (!ModelState.IsValid)  // Check if the model is valid
            {
                return View("Login");  // Return to the login view with validation errors
            }

            var usr = _loginRepository.ValidateUserLogin(user.Username, user.Password);
            if (usr != null)
            {
                HttpContext.Session.SetString("Username", usr.Username);  // Store in session
                return RedirectToAction("Index", "Home");  // Redirect to dashboard after login
            }

            ViewData["ErrorMessage"] = "Invalid credentials";  // Set error message for invalid login
            return View("Login");
        }

        // View register page
        public IActionResult Register()
        {
            return View();
        }

        // Register user
        [HttpPost]
        public IActionResult Register(Register usr)
        {
            if (ModelState.IsValid)
            {
                //var user = new User { Username = usr.Username, Password = usr.Password };
                var success = _loginRepository.RegisterUser(usr.Username, usr.Password);
                if (success)
                {
                    return RedirectToAction("Login");
                }
                ViewData["ErrorMessage"] = "Registration failed. Please try again.";
                return View(usr);
            }
            ViewData["ErrorMessage"] = "Invalid input. Please check your details.";
            return View(usr);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();  // Clear session
            return RedirectToAction("Login");
        }
    }
}