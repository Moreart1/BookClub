using BookClub.DAL.Interface;
using BookClub.DTO.Interface;
using BookClub.DTO.Service;
using BookClub.Models;
using Moq;

namespace BookClub.Tests
{
    public class ClubServiceTests
    {
        private readonly Mock<IDataAccessRepository> _dataAccess = new();
        private readonly Mock<IUserRepository> _users = new();
        private readonly Mock<IBookRepository> _books = new();
        private readonly ClubService _sut;

        public ClubServiceTests()
        {
            _dataAccess.Setup(x => x.UserRepository).Returns(_users.Object);
            _dataAccess.Setup(x => x.BookRepository).Returns(_books.Object);
            _sut = new ClubService(_dataAccess.Object);
        }

        [Fact]
        public async Task GetAllBooks_WhenRepositoryReturnsBooks_SetsDataAndSuccess()
        {
            var books = new List<Book>
            {
                new() { Id = 1, Name = "Война и мир.Том 1" },
                new() { Id = 2, Name = "Война и мир.Том 2" }
            };
            _books.Setup(x => x.GetAllAsync()).ReturnsAsync(books);

            var result = await _sut.GetAllBooks();

            Assert.True(result.Success);
            Assert.Equal(books, result.Data);
        }

        [Fact]
        public async Task GetAllBooks_WhenRepositoryThrows_ReturnsFailure()
        {
            _books.Setup(x => x.GetAllAsync()).ThrowsAsync(new InvalidOperationException("db"));

            var result = await _sut.GetAllBooks();

            Assert.False(result.Success);
            Assert.Equal("GetAllBooks Exception", result.Message);
        }

        [Fact]
        public async Task AddBookToUser_WhenUserMissing_ReturnsFailure()
        {
            _users.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((User)null!);

            var result = await _sut.AddBookToUser(1, 2);

            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
            _users.Verify(x => x.AddBookToUserAsync(It.IsAny<User>(), It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task AddBookToUser_WhenBookMissing_ReturnsFailure()
        {
            _users.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(new User { Id = 1, Login = "morearti", Name = "Марк" });
            _books.Setup(x => x.GetByIdAsync(2)).ReturnsAsync((Book)null!);

            var result = await _sut.AddBookToUser(1, 2);

            Assert.False(result.Success);
            Assert.Equal("Book not found", result.Message);
            _users.Verify(x => x.AddBookToUserAsync(It.IsAny<User>(), It.IsAny<Book>()), Times.Never);
        }

        [Fact]
        public async Task AddBookToUser_WhenUserAndBookExist_AddsBookAndReturnsList()
        {
            var book = new Book { Id = 2, Name = "Война и мир.Том 2" };
            var user = new User { Id = 1, Login = "morearti", Name = "Марк" };
            _users.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
            _books.Setup(x => x.GetByIdAsync(2)).ReturnsAsync(book);
            _users.Setup(x => x.AddBookToUserAsync(user, book))
                .Callback<User, Book>((u, b) => u.Books.Add(b))
                .Returns(Task.CompletedTask);

            var result = await _sut.AddBookToUser(1, 2);

            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal(2, result.Data.First().Id);
            _users.Verify(x => x.AddBookToUserAsync(user, book), Times.Once);
        }

        [Fact]
        public async Task AddBookToUser_WhenRepositoryThrows_ReturnsFailure()
        {
            _users.Setup(x => x.GetByIdAsync(1)).ThrowsAsync(new InvalidOperationException("db"));

            var result = await _sut.AddBookToUser(1, 2);

            Assert.False(result.Success);
            Assert.Equal("Add book to user Exception", result.Message);
        }

        [Fact]
        public async Task GetAllUserBooks_WhenUserMissing_ReturnsFailure()
        {
            _users.Setup(x => x.GetByIdAsync(9)).ReturnsAsync((User)null!);

            var result = await _sut.GetAllUserBooks(9);

            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
        }

        [Fact]
        public async Task GetAllUserBooks_WhenUserExists_ReturnsBooks()
        {
            var user = new User
            {
                Id = 1,
                Login = "morearti",
                Name = "Марк",
                Books = { new Book { Id = 1, Name = "Война и мир.Том 1" } }
            };
            _users.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _sut.GetAllUserBooks(1);

            Assert.True(result.Success);
            Assert.Single(result.Data);
            Assert.Equal(1, result.Data.First().Id);
        }

        [Fact]
        public async Task DeleteUserBook_WhenUserMissing_ReturnsFailure()
        {
            _users.Setup(x => x.GetByIdAsync(1)).ReturnsAsync((User)null!);

            var result = await _sut.DeleteUserBook(1, 2);

            Assert.False(result.Success);
            Assert.Equal("User not found", result.Message);
            _users.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserBook_WhenBookNotOnUser_ReturnsFailure()
        {
            var user = new User { Id = 1, Login = "morearti", Name = "Марк" };
            _users.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);

            var result = await _sut.DeleteUserBook(1, 2);

            Assert.False(result.Success);
            Assert.Equal("Book not found", result.Message);
            _users.Verify(x => x.UpdateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserBook_WhenBookOnUser_RemovesAndUpdates()
        {
            var book = new Book { Id = 2, Name = "Война и мир.Том 2" };
            var user = new User
            {
                Id = 1,
                Login = "morearti",
                Name = "Марк",
                Books = { book }
            };
            _users.Setup(x => x.GetByIdAsync(1)).ReturnsAsync(user);
            _users.Setup(x => x.UpdateAsync(user)).Returns(Task.CompletedTask);

            var result = await _sut.DeleteUserBook(1, 2);

            Assert.True(result.Success);
            Assert.Empty(result.Data.Books);
            _users.Verify(x => x.UpdateAsync(user), Times.Once);
        }
    }
}
