using LibraryMVC.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LibraryMVC.Domain
{
    public class LibraryDbContext : DbContext
    {
        public LibraryDbContext(DbContextOptions<LibraryDbContext> options)
            : base(options){ }
        
        public DbSet<Book> Books { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<BorrowedBook> BorrowedBooks { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BorrowedBook>()
                .HasKey(bb => new { bb.ID});

            modelBuilder.Entity<BorrowedBook>()
                .HasOne(bb => bb.Book)
                .WithMany(b => b.BorrowingUsers)
                .HasForeignKey(bb => bb.BookId);

            modelBuilder.Entity<BorrowedBook>()
                .HasOne(bb => bb.User)
                .WithMany(u => u.BorrowedBooks)
                .HasForeignKey(bb => bb.UserId);

            modelBuilder.Entity<User>().ToTable("Users");

        }

        
    }
}