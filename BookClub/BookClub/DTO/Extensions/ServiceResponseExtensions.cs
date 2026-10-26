using BookClub.DTO.Models;
using Microsoft.AspNetCore.Mvc;

namespace BookClub.DTO.Extensions
{
    public static class ServiceResponseExtensions
    {
        public static ActionResult<ServiceResponse<T>> ToActionResult<T>(this ServiceResponse<T> result)
        {
            if (result.Success)
            {
                return new OkObjectResult(result);
            }

            var message = result.Message ?? string.Empty;
            if (message.Contains("not found", StringComparison.OrdinalIgnoreCase))
            {
                return new NotFoundObjectResult(result);
            }

            if (message.Contains("already exists", StringComparison.OrdinalIgnoreCase))
            {
                return new ConflictObjectResult(result);
            }

            if (message.Contains("Exception", StringComparison.OrdinalIgnoreCase))
            {
                return new ObjectResult(result) { StatusCode = StatusCodes.Status500InternalServerError };
            }

            return new BadRequestObjectResult(result);
        }
    }
}
