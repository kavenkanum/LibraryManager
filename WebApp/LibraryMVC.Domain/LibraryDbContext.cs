using LibraryMVC.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryMVC.Domain
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options){ }
        
        public DbSet<Book> Books { get; set; }
    }
}