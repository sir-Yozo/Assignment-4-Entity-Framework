using Microsoft.EntityFrameworkCore;
using Assignment_3_CRUD.Models;
namespace Assignment_3_CRUD.Data
{
    public class BookDBcontext : DbContext
    {
        public BookDBcontext(DbContextOptions<BookDBcontext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; }
    }
}
