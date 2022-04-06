using BookClub.DTO.Interface;
using BookClub.DTO.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookClub.Controllers
{
    [Route("api/BookClub")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {       
        [HttpGet("Login")]
        public async Task<ActionResult<ServiceResponse<UserViewModel>>> Login([FromQuery] string login)
        {           
            return Ok();
        }
        [HttpPost("Registration")]
        public async Task<ActionResult<ServiceResponse<UserViewModel>>> Registration([FromBody] UserViewModel userViewModel)
        {           
            return Ok();
        }
    }
}
