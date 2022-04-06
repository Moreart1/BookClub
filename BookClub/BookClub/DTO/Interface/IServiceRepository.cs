namespace BookClub.DTO.Interface
{
    public interface IServiceRepository
    {
        IClubService ClubService { get; }
        IAuthorizationService AuthorizationService { get; }
    }
}
