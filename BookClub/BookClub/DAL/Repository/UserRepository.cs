using BookClub.DAL.DBContext;
using BookClub.DAL.Interface;
using BookClub.Models;
using Microsoft.EntityFrameworkCore;

namespace BookClub.DAL.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DataBaseContext databaseContext) : base(databaseContext) { }

        public async Task CreateAsync(User user)
        {
            await AddAsync(user);
            await SaveAsync();
        }

        public async Task<User> GetByLoginAsync(string login)
        {
            return await dataBase.Users.Include(x => x.Books).Where(x => x.Login == login).FirstOrDefaultAsync();
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await dataBase.Users.Include(x => x.Books).Where(x =>x.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddBookToUserAsync(User user, Book book)
        {
            user.Books.Add(book);

            await UpdateAsync(user);
        }

        public async Task UpdateAsync(User user)
        {
            Update(user);
            await SaveAsync();
        }
    }
}
