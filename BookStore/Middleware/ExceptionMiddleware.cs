namespace BookStore.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(
            RequestDelegate next,
            ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception");  
                context.Response.StatusCode = ex switch
                {
                    KeyNotFoundException _ => StatusCodes.Status404NotFound,
                    UnauthorizedAccessException _ => StatusCodes.Status401Unauthorized,
                    _ => StatusCodes.Status500InternalServerError
                };
                context.Response.ContentType = "application/json";
                var problem = new { error = ex.Message };
                await context.Response.WriteAsJsonAsync(problem);
            }
        }
    }
}
