using BookClub.DAL.Repository;
using BookClub.Tests.Infrastructure;

namespace BookClub.Tests
{
    public class BookRepositoryTests
    {
        [Fact]
        public async Task GetAllAsync_ReturnsSeededBooks()
        {
            await using var context = TestDbFactory.Create();
            var repository = new BookRepository(context);

            var books = (await repository.GetAllAsync()).ToList();

            Assert.Equal(8, books.Count);
            Assert.Contains(books, book => book.Name == "Война и мир.Том 1");
        }

        [Fact]
        public async Task GetByIdAsync_WhenBookExists_ReturnsBook()
        {
            await using var context = TestDbFactory.Create();
            var repository = new BookRepository(context);

            var book = await repository.GetByIdAsync(4);

            Assert.NotNull(book);
            Assert.Equal("Война и мир.Том 4", book.Name);
        }

        [Fact]
        public async Task GetByIdAsync_WhenBookMissing_ReturnsNull()
        {
            await using var context = TestDbFactory.Create();
            var repository = new BookRepository(context);

            var book = await repository.GetByIdAsync(99);

            Assert.Null(book);
        }
    }
}
