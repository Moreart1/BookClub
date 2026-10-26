using AutoMapper;
using BookClub.DAL.Interface;
using BookClub.DTO.Interface;
using BookClub.DTO.Models;
using BookClub.DTO.Service;
using BookClub.Models;
using Moq;

namespace BookClub.Tests
{
    public class AuthorizationServiceTests
    {
        private readonly Mock<IDataAccessRepository> _dataAccess = new();
        private readonly Mock<IUserRepository> _users = new();
        private readonly Mock<IMapper> _mapper = new();
        private readonly AuthorizationService _sut;

        public AuthorizationServiceTests()
        {
            _dataAccess.Setup(x => x.UserRepository).Returns(_users.Object);
            _mapper
                .Setup(x => x.Map<UserViewModel>(It.IsAny<User>()))
                .Returns((User user) => new UserViewModel
                {
                    Id = user.Id,
                    Login = user.Login,
                    Name = user.Name
                });
            _sut = new AuthorizationService(_dataAccess.Object, _mapper.Object);
        }

        [Fact]
        public async Task Login_WhenUserMissing_ReturnsFailure()
        {
            _users.Setup(x => x.GetByLoginAsync("ghost")).ReturnsAsync((User)null!);

            var result = await _sut.Login("ghost");

            Assert.False(result.Success);
            Assert.Equal("User ghost not found", result.Message);
            _mapper.Verify(x => x.Map<UserViewModel>(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Login_WhenUserExists_ReturnsMappedViewModel()
        {
            var user = new User { Id = 1, Login = "morearti", Name = "Марк" };
            _users.Setup(x => x.GetByLoginAsync("morearti")).ReturnsAsync(user);

            var result = await _sut.Login("morearti");

            Assert.True(result.Success);
            Assert.Equal(1, result.Data.Id);
            Assert.Equal("morearti", result.Data.Login);
            Assert.Equal("Марк", result.Data.Name);
        }

        [Fact]
        public async Task Register_WhenLoginTaken_ReturnsFailure()
        {
            var existing = new User { Id = 1, Login = "morearti", Name = "Марк" };
            _users.Setup(x => x.GetByLoginAsync("morearti")).ReturnsAsync(existing);

            var result = await _sut.Register(new UserViewModel { Login = "morearti", Name = "Другой" });

            Assert.False(result.Success);
            Assert.Equal("User with this username morearti already exists", result.Message);
            _users.Verify(x => x.CreateAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Register_WhenLoginFree_CreatesUser()
        {
            _users.Setup(x => x.GetByLoginAsync("newbie")).ReturnsAsync((User)null!);
            User? created = null;
            _users.Setup(x => x.CreateAsync(It.IsAny<User>()))
                .Callback<User>(user =>
                {
                    user.Id = 10;
                    created = user;
                })
                .Returns(Task.CompletedTask);

            var result = await _sut.Register(new UserViewModel { Login = "newbie", Name = "Новый" });

            Assert.True(result.Success);
            Assert.NotNull(created);
            Assert.Equal("newbie", created!.Login);
            Assert.Equal("Новый", created.Name);
            Assert.Equal(10, result.Data.Id);
            Assert.Equal("newbie", result.Data.Login);
            _users.Verify(x => x.CreateAsync(It.Is<User>(u => u.Login == "newbie" && u.Name == "Новый")), Times.Once);
        }
    }
}
