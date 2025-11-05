namespace docker_sharp.Middleware;

using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next) => _next = next;

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (ArgumentException ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            httpContext.Response.ContentType = "application/json";
            var res = JsonSerializer.Serialize(new { error = ex.Message });
            await httpContext.Response.WriteAsync(res);
        }
        catch (Exception ex)
        {
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            var res = JsonSerializer.Serialize(new { error = "Error interno del servidor." });
            await httpContext.Response.WriteAsync(res);
        }
    }
}
