using Assignment_3_CRUD___Model.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the repository for dependency injection
builder.Services.AddScoped<IBookRepository, BookRepository>(); 

var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers(); // Ensure controllers are mapped correctly

app.Run();