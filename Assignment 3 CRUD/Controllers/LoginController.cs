using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Mvc;
using Assignment_3_CRUD.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using System.Net;

namespace Assignment_3_CRUD.Controllers
{
    public class LoginController : Controller
    {

        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;

        public LoginController(SignInManager<User> signInManager, UserManager<User> userManager) {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }


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
            if (ModelState.IsValid)  // Check if the model is valid
            {
                //return View("Login");  // Return to the login view with validation errors
                var result = await signInManager.PasswordSignInAsync(
                model.Username!,  // Get username from the model
                model.Password!,  // Get password from the model
                false,  // Do not remember the user
                false);  // Do not lock out on failure
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");  // Redirect to dashboard after login
                }
                ModelState.AddModelError("", "Invalid login attempt"); // Set error message for invalid login
            }

            
            return View(model);
        }
        [HttpGet]
       
        // View register page
        public IActionResult Register()
        {
            return View();
        }

        // Register user
        [HttpPost]
 
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (ModelState.IsValid)
            {
                User user = new()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    PhoneNumber = model.PhoneNumber,
                    UserName = model.Username,
                    PasswordHash = model.Password

                    //Address = model.Address,

                };
                var result = await userManager.CreateAsync(user, model.Password!);
                if (result.Succeeded) {
                    await signInManager.SignInAsync(user, false);
                    return RedirectToAction("Index", "Home");
                }
                foreach (var error in result.Errors) {
                    ModelState.AddModelError("",error.Description);
                }
                
            }
            
            return View(model);
        }
        [HttpGet]

        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();  // Sign out the user
            return RedirectToAction("Login", "Login");  // Redirect to login page
        }
    }
}