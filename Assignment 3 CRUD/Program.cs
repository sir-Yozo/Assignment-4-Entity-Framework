using Assignment_3_CRUD.Data;
using Assignment_3_CRUD.Middleware;
using Assignment_3_CRUD.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
//using Assignment_3_CRUD___Model.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<LMA_DBcontext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//builder.Services.AddSession(options =>
//{
//    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
//    options.Cookie.HttpOnly = true;
//    options.Cookie.IsEssential = true;
//});
builder.Services.AddIdentity<User, IdentityRole>(
    options =>
    {
        options.Password.RequiredUniqueChars = 0;
        options.Password.RequireDigit = false;
        options.Password.RequireLowercase = false;   
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = false;
        options.Password.RequiredLength = 4;
    })
    .AddEntityFrameworkStores<LMA_DBcontext>()
    .AddDefaultTokenProviders();

// Register the repository for dependency injection 
//builder.Services.AddScoped<IBookRepository, BookRepository>();
//builder.Services.AddScoped<IReaderRepository, ReaderRepository>();
//builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();
//builder.Services.AddSingleton<ILoginRepository, LoginRepository>();





var app = builder.Build();

// Register the custom authentication middleware

app.UseSession();  // Enable session
app.UseMiddleware<AuthMiddleware>();


app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Login}/{action=Login}"); // Redirect to Login by default
app.Run();