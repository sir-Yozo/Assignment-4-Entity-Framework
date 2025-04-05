using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Assignment_3_CRUD.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace Assignment_3_CRUD.Controllers
{
    public class LoginController : Controller
    {

        private readonly SignInManager<User> signInManager;
        // View login page
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // Login - Authenticate user
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)  // Check if the model is valid
            {
                return View("Login");  // Return to the login view with validation errors
            }

            var result = await signInManager.PasswordSignInAsync(
                model.Username,  // Get username from the model
                model.Password,  // Get password from the model
                isPersistent: false,  // Do not remember the user
                lockoutOnFailure: false);  // Do not lock out on failure
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");  // Redirect to dashboard after login
            }

            ModelState.AddModelError("", "Invalid login attempt"); // Set error message for invalid login
            return View("Login");
        }

        //// View register page
        //public IActionResult Register()
        //{
        //    return View();
        //}

        //// Register user
        //[HttpPost]
        //public IActionResult Register(Register usr)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //var user = new User { Username = usr.Username, Password = usr.Password };
        //        var success = _loginRepository.RegisterUser(usr.Username, usr.Password);
        //        if (success)
        //        {
        //            return RedirectToAction("Login");
        //        }
        //        ViewData["ErrorMessage"] = "Registration failed. Please try again.";
        //        return View(usr);
        //    }
        //    ViewData["ErrorMessage"] = "Invalid input. Please check your details.";
        //    return View(usr);
        //}

        //public IActionResult Logout()
        //{
        //    HttpContext.Session.Clear();  // Clear session
        //    return RedirectToAction("Login");
        //}
    }
}