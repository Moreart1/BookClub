using BookClub.DTO;
using BookClub.DTO.Interface;
using BookClub.DTO.Repository;
using Moq;

namespace BookClub.Tests
{
    public class ServiceRepositoryTests
    {
        [Fact]
        public void ClubService_ReturnsInjectedInstance()
        {
            var clubService = Mock.Of<IClubService>();
            var repository = new ServiceRepository(clubService, Mock.Of<IAuthorizationService>());

            Assert.Same(clubService, repository.ClubService);
        }

        [Fact]
        public void AuthorizationService_ReturnsInjectedInstance()
        {
            var authorizationService = Mock.Of<IAuthorizationService>();
            var repository = new ServiceRepository(Mock.Of<IClubService>(), authorizationService);

            Assert.Same(authorizationService, repository.AuthorizationService);
        }
    }
}
