using BookClub.DAL.DBContext;
using Microsoft.EntityFrameworkCore;

namespace BookClub.DAL.Repository
{
    public class BaseRepository<T> where T : class
    {
        protected DataBaseContext dataBase { get; set; }

        public BaseRepository(DataBaseContext _dataBase)
        {
            dataBase = _dataBase;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await dataBase.Set<T>().AsNoTracking().ToListAsync();
        }

        public async Task<T> Get(int id)
        {
            return await dataBase.Set<T>().FindAsync(id);
        }

        public async Task AddAsync(T item)
        {
            await dataBase.Set<T>().AddAsync(item);
        }

        public void Update(T item)
        {
            dataBase.Set<T>().Update(item);
        }

        public async Task SaveAsync()
        {
            await dataBase.SaveChangesAsync();
        }
    }
}
