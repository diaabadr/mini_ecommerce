using Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseAPIController : ControllerBase
    {
        private IMediator _mediator;
        protected IMediator Mediator =>
        _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>()
        ?? throw new InvalidOperationException("IMediator service is not available");

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
