using Microsoft.EntityFrameworkCore;
using Assignment_3_CRUD.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace Assignment_3_CRUD.Data
{
    public class LMA_DBcontext : IdentityDbContext<User>
    {
        public LMA_DBcontext(DbContextOptions<LMA_DBcontext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; } 
        public DbSet<Reader> Readers { get; set; }
        public DbSet<Borrowing> Borrowings { get; set; }
    }
}
