using Microsoft.Data.SqlClient;
using OnlineShop.Api.Common;

namespace OnlineShop.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);  
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unexpected error occurred.");

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = StatusCodes.Status500InternalServerError;  
            var message = "An unexpected error occurred. Please try again later.";

            if (ex is SqlException)
            {
                statusCode = StatusCodes.Status500InternalServerError;
                message = "A database error occurred. Please try again later.";
            }
            else if (ex is TimeoutException)
            {
                statusCode = StatusCodes.Status408RequestTimeout;
                message = "The request timed out. Please try again later.";
            }


            var result = Result.FailureResult(message, statusCode);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            return context.Response.WriteAsJsonAsync(result);  
        }
    }

}
