using HelpDesk.Service.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace HelpDesk.WebApi.Handlers;

public class AppExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception ex, CancellationToken cancellationToken)
    {
        if (ex is AppException applicationException)
        {
            httpContext.Response.StatusCode = applicationException.Code switch
            {
                404 => StatusCodes.Status404NotFound,
                409 => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError,
            };

            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message = applicationException.Message,
                Type = ex.GetType().Name
            }, cancellationToken);
            return true;
        }
        else
        {
            await httpContext.Response.WriteAsJsonAsync(new
            {
                Message = "Internal server error.",
                Type = ex.GetType().Name
            }, cancellationToken);
            return true;
        }
    }
}
