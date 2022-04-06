using BookClub.DTO.Interface;
using BookClub.DTO.Models;
using BookClub.Models;

namespace BookClub.DTO.Service
{
    public class ClubService : IClubService
    {
        IDataAccessRepository repository;

        public ClubService(IDataAccessRepository _repository)
        {
            repository = _repository;
        }

        public async Task<ServiceResponse<IEnumerable<Book>>> GetAllBooks()
        {
            ServiceResponse<IEnumerable<Book>> result = new ServiceResponse<IEnumerable<Book>>();
            try
            {
                result.Data = await repository.BookRepository.GetAllAsync();
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "GetAllBooks Exception";
            }
            return result;
        }

        public async Task<ServiceResponse<IEnumerable<Book>>> AddBookToUser(int userId, int bookId)
        {
            ServiceResponse<IEnumerable<Book>> result = new ServiceResponse<IEnumerable<Book>>();
            try
            {
                User user = await repository.UserRepository.GetByIdAsync(userId);
                if (user == null)
                {
                    result.Success = false;
                    result.Message = "User not found";
                    return result;
                }
                Book book = await repository.BookRepository.GetByIdAsync(bookId);
                if (book == null)
                {
                    result.Success=false;
                    result.Message = "Book not found";
                    return result;
                }
                await repository.UserRepository.AddBookToUserAsync(user, book);

                result.Data = user.Books;
            }
            catch (Exception)
            {
                result.Success = false;
                result.Message = "Add book to user Exception";              
            }
            return result;
        }

        public async Task<ServiceResponse<IEnumerable<Book>>> GetAllUserBooks(int userId)
        {
            ServiceResponse<IEnumerable<Book>> result = new ServiceResponse<IEnumerable<Book>>();

            User user = await repository.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                result.Success = false;
                result.Message = "User not found";
                return result;
            }
            result.Data = user.Books;
            return result;
        }

        public async Task<ServiceResponse<User>> DeleteUserBook(int userId, int bookId)
        {
            ServiceResponse<User> result = new ServiceResponse<User>();

            User user = await repository.UserRepository.GetByIdAsync(userId);
            if (user == null)
            {
                result.Success = false;
                result.Message = "User not found";
                return result;
            }
            Book book = user.Books.Where(x => x.Id == bookId).FirstOrDefault();
            if (book == null)
            {
                result.Success = false;
                result.Message = "Book not found";
                return result;
            }
            user.Books.Remove(book);
            await repository.UserRepository.UpdateAsync(user);
            result.Data = user;

            return result;
        }

    }
}
