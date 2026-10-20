using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace income_expense_tracker.Exception
{
    public sealed class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) 
        {
            _logger = logger;
        }


        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {
            var (status, title, detail) = exception switch
            {
                ValidationException ex => (
                    StatusCodes.Status400BadRequest,
                    "Validation Failed",
                    ex.Message
                ),

                DbUpdateConcurrencyException => (
                    StatusCodes.Status409Conflict,
                    "Database conflict",
                    "The record was modified or deleted by another operation."
                ),

                DbUpdateException => (
                    StatusCodes.Status500InternalServerError,
                    "Database operation failed",
                    "Unable to complete the database operation."
                ),
                _ => (
                    StatusCodes.Status500InternalServerError,
                    "Internal server error",
                    "An unexpected error occurred."
                )

            };

            if(status >= 500)
            {
                _logger.LogError(
                      exception,
                      "Request failed at {Path}",
                      httpContext.Request.Path
                    );
            }
            else
            {
                _logger.LogWarning(
                    exception,
                    "Request error at {Path}",
                    httpContext.Request.Path
                );
            }
            await Results.Problem(
                statusCode: status,
                title:title,
                detail:detail,
                extensions: new Dictionary<string,object?>
                {
                    ["traceId"] = httpContext.TraceIdentifier
                }
                ).ExecuteAsync(httpContext);

            return true;

        }
    }
}
