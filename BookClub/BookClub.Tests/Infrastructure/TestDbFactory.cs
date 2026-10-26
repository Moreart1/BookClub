using BookClub.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BookClub.Tests.Infrastructure
{
    internal static class TestDbFactory
    {
        public static DataBaseContext Create(string? databaseName = null)
        {
            var options = new DbContextOptionsBuilder<DataBaseContext>()
                .UseInMemoryDatabase(databaseName ?? Guid.NewGuid().ToString())
                .Options;

            return new DataBaseContext(options);
        }
    }
}
