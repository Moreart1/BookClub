using BookClub.Models;
using Microsoft.EntityFrameworkCore;

namespace BookClub.DAL.DBContext
{
    public class DataBaseContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Book> Books { get; set; }

        public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var users = new User[]
            {
                new User() { Id = 1, Name = "Марк", Login = "morearti" },
                new User() { Id = 2, Name = "Маргарита", Login = "margo" },
                new User() { Id = 3, Name = "Максим", Login = "max" },
                new User() { Id = 4, Name = "Николай", Login = "niko" }
            };
            var books = new Book[]
            {
                new Book() { Id = 1, Name = "Война и мир.Том 1"},
                new Book() { Id = 2, Name = "Война и мир.Том 2"},
                new Book() { Id = 3, Name = "Война и мир.Том 3"},
                new Book() { Id = 4, Name = "Война и мир.Том 4"},
                new Book() { Id = 5, Name = "Война и мир.Том 5"},
                new Book() { Id = 6, Name = "Война и мир.Том 6"},
                new Book() { Id = 7, Name = "Война и мир.Том 7"},
                new Book() { Id = 8, Name = "Война и мир.Том 8"}
            };

            modelBuilder.Entity<User>().HasData(users);
            modelBuilder.Entity<Book>().HasData(books);
        }
    }
}
