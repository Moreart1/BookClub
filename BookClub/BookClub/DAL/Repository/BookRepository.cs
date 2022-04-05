using BookClub.DAL.DBContext;
using BookClub.DAL.Interface;

namespace BookClub.DAL.Repository
{
    public class BookRepository:BaseRepository<Book>,IBookRepository
    {
        public BookRepository(DataBaseContext databaseContext) : base(databaseContext) { }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            return await GetAll();
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            return await Get(id);
        }
    }
}
