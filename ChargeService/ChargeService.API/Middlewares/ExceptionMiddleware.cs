using ChargeService.Utility.Exceptions;
using ChargeService.Utility.Models;
using Newtonsoft.Json;
using System.Net;

namespace ChargeService.API.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment env, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _env = env;
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
                await HandleExceptionAsync(context, ex, _env);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, IWebHostEnvironment env)
        {
            var errors = new ApiErrorResponse();

            if (_env.IsDevelopment())
            {
                errors.Details = exception.Message;
                errors.StackTrace = exception?.StackTrace;
            }

            _logger.LogError(exception, $"Error handling");

            switch (exception)
            {
                case ApplicationValidationException e:
                    errors.Message = e.Message;
                    errors.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
                    break;
                default:
                    errors.Message = "Something is wrong in our system";
                    errors.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            var result = JsonConvert.SerializeObject(errors);
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = errors.StatusCode;
            await context.Response.WriteAsync(result);
        }
    }
}
