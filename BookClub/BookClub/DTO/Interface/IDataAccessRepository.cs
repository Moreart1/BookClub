using BookClub.DAL.Interface;

namespace BookClub.DTO.Interface
{
    public interface IDataAccessRepository
    {
        IUserRepository UserRepository { get; }
        IBookRepository BookRepository { get; }
    }
}
