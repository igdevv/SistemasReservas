using FluentValidation;
using System.Net;
using System.Text.Json;

namespace SistemasReservas.API.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                message = "Error de validación.",
                errors = ex.Errors.Select(error => new
                {
                    campo = error.PropertyName,
                    mensaje = error.ErrorMessage
                })
            });
        }
        catch (KeyNotFoundException ex)
        {
            await EscribirError(
                context,
                HttpStatusCode.NotFound,
                ex.Message);
        }
        catch (ArgumentException ex)
        {
            await EscribirError(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            await EscribirError(
                context,
                HttpStatusCode.BadRequest,
                ex.Message);
        }
        catch (Exception)
        {
            await EscribirError(
                context,
                HttpStatusCode.InternalServerError,
                "Ocurrió un error inesperado.");
        }
    }

    private static async Task EscribirError(
        HttpContext context,
        HttpStatusCode statusCode,
        string mensaje)
    {
        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";

        var response = new
        {
            status = (int)statusCode,
            message = mensaje
        };

        await context.Response.WriteAsync(
            JsonSerializer.Serialize(response));
    }
}