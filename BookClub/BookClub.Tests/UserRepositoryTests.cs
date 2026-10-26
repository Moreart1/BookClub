using BookClub.DAL.Repository;
using BookClub.Models;
using BookClub.Tests.Infrastructure;

namespace BookClub.Tests
{
    public class UserRepositoryTests
    {
        [Fact]
        public async Task GetByLoginAsync_WhenSeededUserExists_ReturnsUserWithBooks()
        {
            await using var context = TestDbFactory.Create();
            var repository = new UserRepository(context);

            var user = await repository.GetByLoginAsync("morearti");

            Assert.NotNull(user);
            Assert.Equal(1, user.Id);
            Assert.Equal("Марк", user.Name);
            Assert.NotNull(user.Books);
        }

        [Fact]
        public async Task GetByLoginAsync_WhenUnknownLogin_ReturnsNull()
        {
            await using var context = TestDbFactory.Create();
            var repository = new UserRepository(context);

            var user = await repository.GetByLoginAsync("missing");

            Assert.Null(user);
        }

        [Fact]
        public async Task GetByIdAsync_WhenSeededUserExists_ReturnsUser()
        {
            await using var context = TestDbFactory.Create();
            var repository = new UserRepository(context);

            var user = await repository.GetByIdAsync(2);

            Assert.NotNull(user);
            Assert.Equal("margo", user.Login);
        }

        [Fact]
        public async Task CreateAsync_PersistsUser()
        {
            var databaseName = Guid.NewGuid().ToString();
            await using (var context = TestDbFactory.Create(databaseName))
            {
                var repository = new UserRepository(context);
                await repository.CreateAsync(new User { Name = "Анна", Login = "anna" });
            }

            await using (var context = TestDbFactory.Create(databaseName))
            {
                var repository = new UserRepository(context);
                var user = await repository.GetByLoginAsync("anna");
                Assert.NotNull(user);
                Assert.Equal("Анна", user.Name);
            }
        }

        [Fact]
        public async Task AddBookToUserAsync_AttachesExistingBook()
        {
            await using var context = TestDbFactory.Create();
            var users = new UserRepository(context);
            var books = new BookRepository(context);
            var user = await users.GetByIdAsync(1);
            var book = await books.GetByIdAsync(3);

            await users.AddBookToUserAsync(user, book);

            var reloaded = await users.GetByIdAsync(1);
            Assert.Contains(reloaded.Books, x => x.Id == 3);
        }
    }
}
