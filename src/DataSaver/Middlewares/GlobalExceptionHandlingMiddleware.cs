using JsonSerializer = System.Text.Json.JsonSerializer;

namespace DataSaver.Middlewares
{
    public sealed class GlobalExceptionHandlingMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

        public GlobalExceptionHandlingMiddleware(
            ILogger<GlobalExceptionHandlingMiddleware> logger)
        {
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex) when (ex is CategoryNotFoundException ||
                              ex is LinkNotFoundException ||
                              ex is TopicNotFoundException)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode =
                    (int)HttpStatusCode.NotFound;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.NotFound,
                    Title = "Not found error",
                    Detail = "A not found error has occured"
                };

                string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);

                context.Response.ContentType = "application/json";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode =
                    (int)HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int)HttpStatusCode.InternalServerError,
                    Title = "Server error",
                    Detail = "An internal server error has occured"
                };

                string json = JsonSerializer.Serialize(problem);

                await context.Response.WriteAsync(json);

                context.Response.ContentType = "application/json";
            }
        }
    }
}
