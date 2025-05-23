using System.Net;
using System.Text.Json;

namespace RegistrationAPI.API.Middlewares
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate next;
        private readonly ILogger<GlobalExceptionMiddleware> logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            this.next = next;
            this.logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Something went wrong!");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

                var message = "An error occurred while processing your request. Please check the input values.";

                if (ex is JsonException || ex is InvalidOperationException)
                    message = "Invalid data sent. Please check the data types.";

                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    message
                }));
            }
        }
    }
} 
