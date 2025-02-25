using Domain;
using Domain.Common;
using Serilog;
using System.Net;
using System.Text.Json;

namespace ECommerce.API.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this._next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await this._next(context);
            }
            catch (Exception e)
            {
                Log.Error("An unhandled exception: " + e.Message);
                await HandleException(context, e);
            }
        }

        private static Task HandleException(HttpContext context, Exception e)
        {

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new CustomErrorResponse(ErrorCodes.InternalServerError, "an internal server error has occured");

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
