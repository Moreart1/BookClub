using AutoMapper;
using BookClub.DTO.Models;
using BookClub.Models;

namespace BookClub.Extensions
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserViewModel>();
        }
    }
}
