using BookClub.DAL.Interface;
using BookClub.DTO.Interface;

namespace BookClub.DTO.Repository
{
    public class DataAccessRepository : IDataAccessRepository
    {
        public DataAccessRepository(IUserRepository userRepository, IBookRepository bookRepository)
        {
            UserRepository = userRepository;
            BookRepository = bookRepository;
        }

        public IUserRepository UserRepository { get; }
        public IBookRepository BookRepository { get; }
    }
}
