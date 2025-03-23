using Assignment_3_CRUD___Model.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Register the repository for dependency injection 
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IReaderRepository, ReaderRepository>();
builder.Services.AddScoped<IBorrowingRepository, BorrowingRepository>();


var app = builder.Build();

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

app.Run();