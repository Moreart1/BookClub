using BookClub.DTO.Models;

namespace BookClub.DTO
{
    public interface IAuthorizationService
    {
        Task<ServiceResponse<UserViewModel>> Login(string login);
        Task<ServiceResponse<UserViewModel>> Register(UserViewModel userViewModel);
    }
}
