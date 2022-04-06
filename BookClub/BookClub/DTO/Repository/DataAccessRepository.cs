using BookClub.DAL.DBContext;
using BookClub.DAL.Interface;
using BookClub.DAL.Repository;
using BookClub.DTO.Interface;

namespace BookClub.DTO.Repository
{
    public class DataAccessRepository :IDataAccessRepository
    {
        private readonly DataBaseContext dataBaseContext;

        public DataAccessRepository(DataBaseContext _dataBaseContext)
        {
            dataBaseContext = _dataBaseContext;
        }

        IUserRepository userRepository;
        IBookRepository bookRepository;

        public IUserRepository UserRepository => userRepository ?? new UserRepository(dataBaseContext);
        public IBookRepository BookRepository => bookRepository ?? new BookRepository(dataBaseContext);
    }
}
