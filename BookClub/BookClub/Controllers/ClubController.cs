using BookClub.DTO.Extensions;
using BookClub.DTO.Interface;
using BookClub.DTO.Models;
using BookClub.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookClub.Controllers
{
    [Route("api/Club")]
    [ApiController]
    public class ClubController : ControllerBase
    {
        private readonly IClubService _clubService;

        public ClubController(IClubService clubService)
        {
            _clubService = clubService;
        }

        [HttpGet("AllBooks")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Book>>>> GetAllBooks()
        {
            return (await _clubService.GetAllBooks()).ToActionResult();
        }

        [HttpGet("AddBookToUser")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Book>>>> AddBookToUser(
            [FromQuery, Required] int userId,
            [FromQuery, Required] int bookId)
        {
            return (await _clubService.AddBookToUser(userId, bookId)).ToActionResult();
        }

        [HttpGet("AllUserBooks")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Book>>>> GetAllUserBooks([FromQuery, Required] int userId)
        {
            return (await _clubService.GetAllUserBooks(userId)).ToActionResult();
        }

        [HttpGet("DeleteUserBook")]
        public async Task<ActionResult<ServiceResponse<User>>> DeleteUserBook(
            [FromQuery, Required] int userId,
            [FromQuery, Required] int bookId)
        {
            return (await _clubService.DeleteUserBook(userId, bookId)).ToActionResult();
        }
    }
}
