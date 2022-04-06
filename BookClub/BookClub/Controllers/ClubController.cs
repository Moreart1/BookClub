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
        [HttpGet("AllBooks")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Book>>>> GetAllBooks()
        {           
            return Ok();
        }

        [HttpGet("AddBookToUser")]
        public async Task<ActionResult<ServiceResponse<IEnumerable<Book>>>> AddBookToUser([FromQuery,Required] int userId,[FromQuery,Required] int bookId)
        {           
            return Ok();
        }

        [HttpGet("AllUserBooks")]
        public async Task<ActionResult<ServiceResponse<List<Book>>>> GetAllUserBooks([FromQuery,Required] int userId)
        {           
            return Ok();
        }

        [HttpGet("DeleteUserBook")]
        public async Task<ActionResult<ServiceResponse<User>>> DeleteUserBook([FromQuery,Required] int userId,[FromQuery,Required] int bookId)
        {           
            return Ok();
        }
    }
}