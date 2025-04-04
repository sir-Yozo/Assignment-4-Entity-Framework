using Microsoft.EntityFrameworkCore;
using Assignment_3_CRUD.Models;
namespace Assignment_3_CRUD.Data
{
    public class LMA_DBcontext : DbContext
    {
        public LMA_DBcontext(DbContextOptions<LMA_DBcontext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; } 
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
