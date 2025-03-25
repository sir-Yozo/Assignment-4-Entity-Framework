using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Assignment_3_CRUD___Model.Middleware
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Allow access to login and register pages without authentication
            if (context.Request.Path.StartsWithSegments("/Login") ||
                context.Request.Path.StartsWithSegments("/Register"))
            {
                await _next(context);  // Skip authentication check for login/register
                return;
            }

            // Check if the user is logged in by checking the session
            if (string.IsNullOrEmpty(context.Session.GetString("Username")))
            {
                context.Response.Redirect("/Login/Login");  // Redirect to login page if not logged in
                return;
            }

            // Pass the request to the next middleware if the user is logged in
            await _next(context);
        }
    }
}
