using AutoMapper;
using BookClub.DTO.Interface;
using BookClub.DTO.Service;

namespace BookClub.DTO.Repository
{
    public class ServiceRepository : IServiceRepository
    {
        IDataAccessRepository repository;
        IMapper mapper;

        public ServiceRepository(IDataAccessRepository _repository,IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }

        IClubService service;
        IAuthorizationService authorizationService;

        public IClubService ClubService => service ??= new ClubService(repository);
        public IAuthorizationService AuthorizationService => authorizationService ??= new AuthorizationService(repository,mapper);
    }
}
