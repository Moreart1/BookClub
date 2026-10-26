using BookClub.DTO;
using BookClub.DTO.Extensions;
using BookClub.DTO.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace BookClub.Controllers
{
    [Route("api/BookClub")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;

        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpGet("Login")]
        public async Task<ActionResult<ServiceResponse<UserViewModel>>> Login([FromQuery, Required] string login)
        {
            return (await _authorizationService.Login(login)).ToActionResult();
        }

        [HttpPost("Registration")]
        public async Task<ActionResult<ServiceResponse<UserViewModel>>> Registration([FromBody] UserViewModel userViewModel)
        {
            return (await _authorizationService.Register(userViewModel)).ToActionResult();
        }
    }
}
