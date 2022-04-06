using AutoMapper;
using BookClub.DTO.Interface;
using BookClub.DTO.Models;
using BookClub.Models;

namespace BookClub.DTO.Service
{
    public class AuthorizationService : IAuthorizationService
    {
        IDataAccessRepository repository;
        IMapper mapper;
        public AuthorizationService(IDataAccessRepository _repository,IMapper _mapper)
        {
            repository = _repository;
            mapper = _mapper;
        }

        public async Task<ServiceResponse<UserViewModel>> Login(string login)
        {
            ServiceResponse<UserViewModel> result = new ServiceResponse<UserViewModel>();

            User user = await repository.UserRepository.GetByLoginAsync(login);

            if (user == null)
            {
                result.Success = false;
                result.Message = $"User {login} not found";
                return result;
            }
            result.Data = mapper.Map<UserViewModel>(user);
            return result; 
        }

        public async Task<ServiceResponse<UserViewModel>> Register(UserViewModel userViewModel)
        {
            ServiceResponse<UserViewModel> result = new ServiceResponse<UserViewModel>();

            User user = await repository.UserRepository.GetByLoginAsync(userViewModel.Login);
            if (user != null)
            {
                result.Success = false;
                result.Message = $"User with this username {userViewModel.Login} already exists";
                return result;
            }

            User newUser = new User()
            {
                Login = userViewModel.Login,
                Name = userViewModel.Name
            };

            await repository.UserRepository.CreateAsync(newUser);

            result.Data = mapper.Map<UserViewModel>(newUser);

            return result;
        }
    }
}
