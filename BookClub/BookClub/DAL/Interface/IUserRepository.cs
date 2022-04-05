using BookClub.Models;

namespace BookClub.DAL.Interface
{
    public interface IUserRepository
    {
        Task CreateAsync(User user);
        Task UpdateAsync(User user);
        Task AddBookToUserAsync(User user,Book book);
        Task<User> GetByLoginAsync(string login);
        Task<User> GetByIdAsync(int  id);
    }
}
