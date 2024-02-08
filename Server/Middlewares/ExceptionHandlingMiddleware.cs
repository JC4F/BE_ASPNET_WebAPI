using BusinessObject.Commons;
using Newtonsoft.Json;
using System.Net;

namespace Server.Middlewares
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
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
                _logger.LogError(
                    ex, "Exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex.Message);
            }
            finally
            {
                var statusCode = context.Response.StatusCode;
                var msg = "";
                if (statusCode == 401)
                {

                    msg = "unauthorize";

                }
                else if (statusCode == 404)
                {
                    msg = "NotFound";
                }
                else if (statusCode == 400)
                {
                    msg = "BadRequest";
                }
                else if (statusCode != 200)
                {
                    msg = "Unkonwn";
                }
                if (!string.IsNullOrWhiteSpace(msg))
                {
                    await HandleExceptionAsync(context, msg);
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string msg)
        {
            var result = JsonConvert.SerializeObject(new { isSuccess=false, message=msg });
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
}
