using BookClub.DTO.Models;
using BookClub.Models;

namespace BookClub.DTO.Interface
{
    public interface IClubService
    {
        Task<ServiceResponse<IEnumerable<Book>>> GetAllBooks();
        Task<ServiceResponse<IEnumerable<Book>>> AddBookToUser(int userId, int bookId);
        Task<ServiceResponse<IEnumerable<Book>>> GetAllUserBooks(int userId);
        Task<ServiceResponse<User>> DeleteUserBook(int userId, int bookId);
    }
}
