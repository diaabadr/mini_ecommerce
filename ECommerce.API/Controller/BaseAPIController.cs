using Application.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseAPIController : ControllerBase
    {
        protected IActionResult HandleResponse<T>(ServiceResponse<T> response)
        {
            if (response.Success)
            {
                return StatusCode(response.StatusCode, response.Data);
            }

            return StatusCode(response.StatusCode, new
            {
                errorCode = response.ErrorCode,
                errorMessage = response.ErrorMessage,
            });
        }
    }
}
