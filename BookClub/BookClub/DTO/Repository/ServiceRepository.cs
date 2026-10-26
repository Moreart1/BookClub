using BookClub.DTO.Interface;

namespace BookClub.DTO.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        public ServiceRepository(IClubService clubService, IAuthorizationService authorizationService)
        {
            ClubService = clubService;
            AuthorizationService = authorizationService;
        }

        public IClubService ClubService { get; }
        public IAuthorizationService AuthorizationService { get; }
    }
}
