using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace DbStructureEmployees.Middleware
{
    /// <summary>
    /// Global exception handling middleware that catches all unhandled exceptions
    /// and returns consistent error responses
    /// </summary>
    public class GlobalExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(RequestDelegate next, ILogger<GlobalExceptionHandlerMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred. RequestPath: {RequestPath}, Method: {Method}",
                    context.Request.Path, context.Request.Method);

                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            var response = new ErrorResponse();

            switch (exception)
            {
                case ArgumentNullException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Invalid request: required parameter is missing";
                    response.Details = ex.ParamName;
                    break;

                case ArgumentException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Invalid argument provided";
                    response.Details = ex.Message;
                    break;

                case InvalidOperationException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Invalid operation";
                    response.Details = ex.Message;
                    break;

                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = "An unexpected error occurred";
                    response.Details = "Please contact support if the problem persists";
                    break;
            }

            response.TraceId = context.TraceIdentifier;
            response.Timestamp = DateTime.UtcNow;

            return context.Response.WriteAsJsonAsync(response);
        }
    }

    /// <summary>
    /// Standard error response format
    /// </summary>
    public class ErrorResponse
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public string? Details { get; set; }
        public string TraceId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
    }
}