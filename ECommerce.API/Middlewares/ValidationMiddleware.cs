using FluentValidation;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Text.Json;

namespace ECommerce.API.Middlewares
{
    public class ValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public ValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method == HttpMethods.Post || context.Request.Method == HttpMethods.Put)
            {
                var endpoint = context.GetEndpoint();
                var controllerActionDescriptor = endpoint?.Metadata.GetMetadata<ControllerActionDescriptor>();

                if (controllerActionDescriptor == null)
                {
                    await _next(context);
                    return;
                }

                // Get the DTO type from the action parameters
                var parameter = controllerActionDescriptor.Parameters.FirstOrDefault(p => p.ParameterType.Name.EndsWith("Dto"));
                if (parameter == null)
                {
                    await _next(context);
                    return;
                }

                var dtoType = parameter.ParameterType;

                context.Request.EnableBuffering();

                var body = await JsonSerializer.DeserializeAsync(context.Request.Body, dtoType, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                context.Request.Body.Position = 0;

                if (body == null)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new
                    {

                        errorCode = "ValidationError",
                        errorMessage = "Invalid request body"
                    }
                    );
                    return;
                }
                var validatorType = typeof(IValidator<>).MakeGenericType(body.GetType());

                var validator = context.RequestServices.GetRequiredService(validatorType) as IValidator;

                if (validator != null)
                {
                    var validationContext = new ValidationContext<object>(body);
                    var validationResult = await validator.ValidateAsync(validationContext);

                    if (!validationResult.IsValid)
                    {
                        context.Response.StatusCode = 400;
                        await context.Response.WriteAsJsonAsync(
                          new
                          {
                              errorCode = validationResult.Errors.First().ErrorCode,
                              errorMessage = validationResult.Errors.First().ErrorMessage,
                          });
                        return;
                    }
                }
            }

            await _next(context);


        }
    }
}